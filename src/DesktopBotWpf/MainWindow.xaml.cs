﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DesktopBotWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MessageCollection messages = new MessageCollection();
        private Storyboard scrollViewerStoryboard;
        private DoubleAnimation scrollViewerScrollToEndAnim;
        ChatHistoryViewModel vm { set; get; }
        private MessageSide curside;

        #region VerticalOffset DP

        /// <summary>
        /// VerticalOffset, a private DP used to animate the scrollviewer
        /// </summary>
        private DependencyProperty VerticalOffsetProperty = DependencyProperty.Register("VerticalOffset",
          typeof(double), typeof(MainWindow), new PropertyMetadata(0.0, OnVerticalOffsetChanged));

        private static void OnVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow app = d as MainWindow;
            app.OnVerticalOffsetChanged(e);
        }

        private void OnVerticalOffsetChanged(DependencyPropertyChangedEventArgs e)
        {
            ConversationScrollViewer.ScrollToVerticalOffset((double)e.NewValue);
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            var chatMgr = ObjectContainer.Get<ChatManager>();
            vm = new(chatMgr);
            // FOLLOWING CODEBLOCK IS ONLY FOR DEMONSTRATION PURPOSES
            messages.Add(new Message()
            {
                Side = MessageSide.Bot,
                Text = "Hello sir. How may I help you?"
            });

            curside = MessageSide.Bot;
            // END OF DEMO BLOCK
            
            this.DataContext = messages;

            scrollViewerScrollToEndAnim = new DoubleAnimation()
            {
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new SineEase()
            };
            Storyboard.SetTarget(scrollViewerScrollToEndAnim, this);
            Storyboard.SetTargetProperty(scrollViewerScrollToEndAnim, new PropertyPath(VerticalOffsetProperty));

            scrollViewerStoryboard = new Storyboard();
            scrollViewerStoryboard.Children.Add(scrollViewerScrollToEndAnim);
            this.Resources.Add("foo", scrollViewerStoryboard);

            TextInput.Focus();
        }

        private void TextInput_GotFocus(object sender, RoutedEventArgs e)
        {
            ScrollConversationToEnd();
        }

        private void ScrollConversationToEnd()
        {
            scrollViewerScrollToEndAnim.From = ConversationScrollViewer.VerticalOffset;
            scrollViewerScrollToEndAnim.To = ConversationContentContainer.ActualHeight;
            scrollViewerStoryboard.Begin();
        }

        private void TextInput_LostFocus(object sender, RoutedEventArgs e)
        {
            ScrollConversationToEnd();
        }

        private async void TextInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await addTextMe(TextInput.Text);

                e.Handled = true;
            }
        }

        private async Task addTextMe(string text)
        {
            if (vm.CanSubmit)
            {
                messages.Add(new Message()
                {
                    Side = MessageSide.Me,
                    Text = text,
                    PrevSide = curside
                });

                curside = MessageSide.Me;

                ScrollConversationToEnd();

                TextInput.Text = "";
                TextInput.Focus();

                vm.Text = text;
                await vm.Submit();
                var response = vm.GetLastBotMessage();
                addTextYou(response.Text);
            }
            // FOLLOWING CODEBLOCK IS ONLY FOR DEMONSTRATION PURPOSES
            // DELETE FOR NORMAL USE!
            /*
            addTextYou(text);

            addTextYou(text);

            messages.Add(new Message()
            {
                Side = MessageSide.Me, 
                Text = text,
                PrevSide = curside
            });

            curside = MessageSide.Me;

            ScrollConversationToEnd();

            TextInput.Text = "";
            TextInput.Focus();

            messages.Add(new Message()
            {
                Side = MessageSide.Me,
                Text = text,
                PrevSide = curside
            });

            curside = MessageSide.Me;

            ScrollConversationToEnd();

            TextInput.Text = "";
            TextInput.Focus();
            */
            // END OF DEMO BLOCK
        }

        private void addTextYou(string text)
        {
            messages.Add(new Message()
            {
                Side = MessageSide.Bot,
                Text = text,
                PrevSide = curside
            });

            curside = MessageSide.Bot;

            ScrollConversationToEnd();

            TextInput.Text = "";
            TextInput.Focus();
        }
    }
}
