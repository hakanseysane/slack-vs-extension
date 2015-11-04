using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OregonStateUniversity.SlackCheckIn.Models
{
    public class SlackChannelViewModel : INotifyPropertyChanged
    {
        private string m_webhookUrl;
        private string m_botName;
        private string m_channel;
        private string m_notificationMessage;
        private bool m_postToSlack;

        public virtual string WebhookUrl
        {
            get
            {
                return m_webhookUrl;
            }
            set
            {
                m_webhookUrl = value;
                OnPropertyChanged("WebhookUrl");
            }
        }

        public virtual string BotName
        {
            get
            {
                return m_botName;
            }
            set
            {
                m_botName = value;
                OnPropertyChanged("BotName");
            }
        }

        public virtual string Channel
        {
            get
            {
                return m_channel;
            }
            set
            {
                m_channel = value;
                OnPropertyChanged("Channel");
            }
        }

        public virtual bool PostToSlack
        {
            get
            {
                return m_postToSlack;
            }
            set
            {
                m_postToSlack = value;
                OnPropertyChanged("PostToSlack");
            }
        }

        public string NotificationMessage
        {
            get
            {
                return m_notificationMessage;
            }
            set
            {
                m_notificationMessage = value;
                OnPropertyChanged("NotificationMessage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
