﻿using OregonStateUniversity.SlackCheckIn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OregonStateUniversity.SlackCheckIn.SlackChannel
{
    /// <summary>
    /// Interaction logic for SlackChannelView.xaml
    /// </summary>
    public partial class SlackChannelView : UserControl
    {
        public SlackChannelView()
        {
            InitializeComponent();
        }

        public SlackChannelViewModel Model
        {
            get
            {
                return (SlackChannelViewModel)GetValue(ModelProperty);
            }
            set
            {
                SetValue(ModelProperty, value);
            }
        }

        public static DependencyProperty ModelProperty = 
            DependencyProperty.Register(
                "Model", 
                typeof(SlackChannelViewModel), 
                typeof(SlackChannelView));
    }
}
