using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Serialization;
using Emby.Notification.Join.Configuration;

namespace Emby.Notification.Join
{
    public class Plugin : MediaBrowser.Common.Plugins.BasePlugin<PluginConfiguration>
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return "Join Notifications"; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "Sends notifications to Join - a notification framework by joaoapps.";
            }
        }

        public Uri IconURL
        {
            get { return new Uri("https://raw.githubusercontent.com/MediaBrowser/Emby.Resources/master/images/Logos/logoicon.png");  }
        }

        public string ApiV1Endpoint
        {
            get { return "https://joinjoaomgcd.appspot.com/_ah/api/messaging/v1/sendPush";  }
        }


        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Plugin Instance { get; private set; }
    }
}
