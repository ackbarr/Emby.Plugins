using System;
using System.Collections.Generic;
using System.Linq;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Serialization;
using Emby.Notification.Join.Configuration;
using MediaBrowser.Model.Plugins;

namespace Emby.Notification.Join
{
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
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

        private Guid _id = new Guid("dae89dfe-a910-4eb4-8b3e-1d642a8ce075");
        public override Guid Id
        {
            get { return _id; }
        }

        public Uri IconURL
        {
            get { return new Uri("https://raw.githubusercontent.com/MediaBrowser/Emby.Resources/master/images/Logos/logoicon.png");  }
        }

        public string ApiV1Endpoint
        {
            get { return "https://joinjoaomgcd.appspot.com/_ah/api/messaging/v1/sendPush";  }
        }

        public string ToQueryString(Dictionary<string, string> variables)
        {
            var array = (from KeyValuePair<string,string> pair in variables
                         select string.Format("{0}={1}", Uri.EscapeUriString(pair.Key), Uri.EscapeUriString(pair.Value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        public IEnumerable<PluginPageInfo> GetPages()
        {

            return new[]
            {
                new PluginPageInfo
                {
                    Name = Name,
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.config.html"

                }
            };
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Plugin Instance { get; private set; }
    }
}
