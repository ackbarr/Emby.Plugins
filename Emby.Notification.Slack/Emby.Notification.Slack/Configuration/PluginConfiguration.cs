using System;
using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Emby.Notification.Slack.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public SlackPluginOptions[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new SlackPluginOptions[] { };
        }
    }

    public class SlackPluginOptions
    {
        public string SlackWebHookURI { get; set; }
        public string Channel { get; set; }
        public string UserName { get; set; }
        public string Emoji { get; set; }
        public Boolean Enabled { get; set; }
        public string MediaBrowserUserId { get; set; }

    }
}
