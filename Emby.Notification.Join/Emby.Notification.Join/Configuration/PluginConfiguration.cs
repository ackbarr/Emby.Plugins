using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Model.Plugins;

namespace Emby.Notification.Join.Configuration
{
    public class PluginConfiguration : BasePluginConfiguration
    {
        public JoinPluginOptions[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new JoinPluginOptions[] { };
        }
    }

    public class JoinPluginOptions
    {
        public Boolean Enabled { get; set; }
        public String ApiKey { get; set; }
        public String DeviceId { get; set; }
        public string MediaBrowserUserId { get; set; }

    }
}
