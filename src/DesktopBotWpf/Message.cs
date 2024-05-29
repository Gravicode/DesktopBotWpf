using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopBotWpf
{
    public enum MessageSide
    {
        Me,
        Bot
    }

    public partial class Message : ObservableObject
    {
        public Message()
        {
            Timestamp = DateTime.Now;
        }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public MessageSide Side { get; set; }
        public MessageSide PrevSide { get; set; }

        [ObservableProperty]
        string? content;

        [ObservableProperty]
        string? role;

        [ObservableProperty]
        bool pluginInvoked = false;

        [ObservableProperty]
        string? pluginUsage;
    }
}
