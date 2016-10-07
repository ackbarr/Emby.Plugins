using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller.Plugins;
using System.IO;

namespace Emby.Notification.Join.Configuration
{
    public class ConfigPage : IPluginConfigurationPage
    {

        public string Name
        {
            get { return Plugin.Name; }
        }

        /// <summary>
        /// Gets the HTML stream.
        /// </summary>
        /// <returns>Stream</returns>
        public Stream GetHtmlStream()
        {
            return GetType().Assembly.GetManifestResourceStream(GetType().Namespace + ".config.html");
        }

        /// <summary>
        /// Gets the type of the configuration page.
        /// </summary>
        /// <value>The type of the configuration page.</value>
        public ConfigurationPageType ConfigurationPageType
        {
            get { return ConfigurationPageType.PluginConfiguration; }
        }

        public IPlugin Plugin
        {
            get { return Emby.Notification.Join.Plugin.Instance; }
        }
    }
}