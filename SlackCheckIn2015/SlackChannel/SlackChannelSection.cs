using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Client;
using OregonStateUniversity.SlackCheckIn.Models;
using SlackCheckIn2015.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.VersionControl.Controls.Extensibility;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.Diagnostics;
using Cass.Slack;
using System.Net;
using System.Windows;

namespace OregonStateUniversity.SlackCheckIn.SlackChannel
{
    [TeamExplorerSection(SlackChannelSection.SectionId, TeamExplorerPageIds.PendingChanges, 900)]
    public class SlackChannelSection : ITeamExplorerSection
    {
        public const string SectionId = "b7e00d7b-be45-482a-882d-35755c38d43d";
        private object m_sectionContent = null;
        private bool m_isBusy = false;
        private bool m_isExpanded = true;
        private bool m_isVisible = true;
        private string m_title = null;
        private SlackChannelViewModel m_viewModel = null;
        private IServiceProvider m_provider = null;
        private VersionControlServer m_vcServer = null;


        public SlackChannelSection()
        {
            Title = "Post Check-In to Slack";
            //create the new model class and view to populate the section content
            //get recent valuse for model from user settings
            m_viewModel = new SlackChannelViewModel()
            {
                PostToSlack = Settings.Default.RecentSlackPostToSlack,
                BotName = Settings.Default.RecentSlackBotName,
                Channel = Settings.Default.RecentSlackChannel,
                WebhookUrl = Settings.Default.RecentSlackWebhookUrl
            };
            SlackChannelView view = new SlackChannelView() { Model = m_viewModel };
            SectionContent = view;
        }

        #region ITeamExplorerSection

        public virtual void Cancel()
        {
        }

        public virtual object GetExtensibilityService(Type serviceType)
        {
            return null;
        }

        public virtual void Initialize(object sender, SectionInitializeEventArgs e)
        {
            m_provider = e.ServiceProvider;
            ITeamFoundationContextManager tfContextManager = GetService<ITeamFoundationContextManager>();
            
            m_vcServer = tfContextManager.CurrentContext.TeamProjectCollection.GetService<VersionControlServer>();
            m_vcServer.CommitCheckin += vcServer_CommitCheckin;
        }

        async void vcServer_CommitCheckin(object sender, CommitCheckinEventArgs e)
        {
            // Only post to Slack if user specified we should post to Slack.
            if (!m_viewModel.PostToSlack)
            {
                return;
            }

            IPendingChangesExt pendingChanges = GetService<IPendingChangesExt>();
            var hyperlinkService = GetService<ITeamFoundationContextManager>()
                .CurrentContext.TeamProjectCollection
                .GetService<TswaClientHyperlinkService>();
            try
            {
                var response = await SlackServiceAdapter.PostToSlack(requestUri: m_viewModel.WebhookUrl,
                    channelName: m_viewModel.Channel,
                    userName: e.Workspace.OwnerDisplayName,
                    changesetID: e.ChangesetId.ToString(),
                    fileChangedCount: pendingChanges.IncludedChanges.Length,
                    changesetComment: pendingChanges.CheckinComment,
                    changesetUrl: hyperlinkService.GetChangesetDetailsUrl(e.ChangesetId).ToString());

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ShowNotification("Successfully posted check-in message to Slack.", NotificationType.Information);
                }
                else
                {
                    var errorMessage = "Error posting to Slack:\n" + response.StatusCode +
                        " - " + await response.Content.ReadAsStringAsync();
                    ShowNotification(errorMessage, NotificationType.Error);
                }
            }
            catch
            {
                // FIXME: For some reason, ShowErrorNotification doesn't work in the context of this catch block.
                var errorMessage = "Error posting to Slack. The check-in still occurred. Look for an issue with your Post Check-In to Slack parameters before checking in again.";
                ShowNotification(errorMessage, NotificationType.Error);
            }
        }

        public bool IsBusy
        {
            get
            {
                return m_isBusy;
            }
            private set
            {
                m_isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        public bool IsExpanded
        {
            get
            {
                return m_isExpanded;
            }
            set
            {
                m_isExpanded = true;
                OnPropertyChanged("IsExpanded");
            }
        }

        public bool IsVisible
        {
            get
            {
                return m_isVisible;
            }
            set
            {
                m_isVisible = true;
                OnPropertyChanged("IsVisible");
            }
        }

        public void Loaded(object sender, SectionLoadedEventArgs e)
        {
        }

        public void Refresh()
        {
        }

        public void SaveContext(object sender, SectionSaveContextEventArgs e)
        {
            SlackChannelViewModel model = m_viewModel ?? new SlackChannelViewModel();

            Settings.Default.RecentSlackBotName = model.BotName;
            Settings.Default.RecentSlackChannel = model.Channel;
            Settings.Default.RecentSlackPostToSlack = model.PostToSlack;
            Settings.Default.RecentSlackWebhookUrl = model.WebhookUrl;
            Settings.Default.Save();
        }

        public object SectionContent
        {
            get
            {
                return m_sectionContent;
            }
            private set
            {
                m_sectionContent = value;
                OnPropertyChanged("SectionContent");
            }
        }

        public string Title
        {
            get 
            {
                return m_title;
            }
            private set
            {
                m_title = value;
                OnPropertyChanged("Title");
            }
        }

        public void Dispose()
        {
            if (m_vcServer != null)
            {
                m_vcServer.CommitCheckin -= vcServer_CommitCheckin;
                m_vcServer = null;
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void ShowNotification(string message, NotificationType type)
        {
            ITeamExplorer teamExplorer = GetService<ITeamExplorer>();
            if (teamExplorer != null)
            {
                Guid guid = Guid.NewGuid();
                teamExplorer.ShowNotification(message, type, NotificationFlags.None, null, guid);
            }
        }

        private T GetService<T>()
        {
            if (m_provider != null)
            {
                return (T)m_provider.GetService(typeof(T));
            }

            return default(T);
        }
    }
}
