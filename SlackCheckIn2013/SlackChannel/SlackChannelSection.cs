using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Client;
using OregonStateUniversity.SlackCheckIn.Models;
using OregonStateUniversity.SlackCheckIn.Properties;
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
using Microsoft.VisualStudio.PlatformUI;
using System.Net;
using System.Windows;

namespace OregonStateUniversity.SlackCheckIn.SlackChannel
{
    /// <summary>
    /// Code-behind for the section of the TFS Pending Changes view that contains the Slack Check-In UI.
    /// </summary>
    [TeamExplorerSection(SlackChannelSection.SECTION_ID, TeamExplorerPageIds.PendingChanges, 900)]
    public class SlackChannelSection : ITeamExplorerSection
    {
        public const string SECTION_ID = "b7e00d7b-be45-482a-882d-35755c38d43d";
        private object m_sectionContent = null;
        private bool m_isBusy = false;
        private bool m_isExpanded = true;
        private bool m_isVisible = true;
        private bool m_isInitialized = false;
        private string m_title = null;
        private SlackChannelViewModel m_viewModel = null;
        private IServiceProvider m_provider = null;
        private VersionControlServer m_vcServer = null;
        private ITeamFoundationContextManager m_contextManager = null;



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

        /// <summary>
        /// Initializes the section, obtaining service dependencies as available,
        /// and subscribing to changes in the current team project collection.
        /// </summary>
        public virtual void Initialize(object sender, SectionInitializeEventArgs e)
        {
            if (!m_isInitialized)
            {
                m_provider = e.ServiceProvider;
                m_contextManager = GetService<ITeamFoundationContextManager>();
                m_contextManager.ContextChanged += ContextManager_ContextChanged;
                SetVersionControlServer(m_contextManager.CurrentContext);
                m_isInitialized = true;
            }
        }

        /// <summary>
        /// Associates a new version control server (child of a TeamProjectCollection)
        /// with this instance. Should be called when the TeamProjectCollection changes.
        /// </summary>
        private void SetVersionControlServer(ITeamFoundationContext tfContext)
        {
            RemoveVersionControlReferences();

            if (tfContext != null && tfContext.TeamProjectCollection != null)
            {
                m_vcServer = tfContext.TeamProjectCollection.GetService<VersionControlServer>();
                m_vcServer.CommitCheckin += VersionControlServer_CommitCheckin;
            }
        }

        private void ContextManager_ContextChanged(object sender, ContextChangedEventArgs e)
        {
            SetVersionControlServer(e.NewContext);
        }

        /// <summary>
        /// Called when the user checks in a changeset. Posts a summary of
        /// the most recent check-in to Slack based on user preferences.
        /// </summary>
        async void VersionControlServer_CommitCheckin(object sender, CommitCheckinEventArgs e)
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
                await SlackServiceAdapter.PostToSlack(requestUri: m_viewModel.WebhookUrl,
                    channelName: m_viewModel.Channel,
                    userName: e.Workspace.OwnerDisplayName,
                    changesetID: e.ChangesetId.ToString(),
                    fileChangedCount: pendingChanges.IncludedChanges.Length,
                    changesetComment: pendingChanges.CheckinComment,
                    changesetUrl: hyperlinkService.GetChangesetDetailsUrl(e.ChangesetId).ToString());

                m_viewModel.NotificationMessage = string.Format("Successfully posted changeset {0} to Slack.", e.ChangesetId);
            }
            catch (WebException ex)
            {
                m_viewModel.NotificationMessage = ex.ToString();
                
            }
            catch
            {
                m_viewModel.NotificationMessage = "Error posting to Slack. The check-in still occurred. Look for an issue with your Post Check-In to Slack parameters before checking in again.";
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (m_contextManager != null)
                {
                    m_contextManager.ContextChanged -= ContextManager_ContextChanged;
                }
                RemoveVersionControlReferences();
                m_contextManager = null;
                m_provider = null;
            }
        }

        /// <summary>
        /// Removes the check-in event handler and version control server reference for
        /// the version control server associated with this instance.
        /// </summary>
        private void RemoveVersionControlReferences()
        {
            if (m_vcServer != null)
            {
                m_vcServer.CommitCheckin -= VersionControlServer_CommitCheckin;
            }
            m_vcServer = null;
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
