using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Serialization;
using Emby.Notification.Join.Configuration;

namespace Emby.Notification.Join
{
    public class JoinNotifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;

        public JoinNotifier(ILogManager logManager, IHttpClient httpClient)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private JoinPluginOptions GetOptions(User user)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, user.Id.ToString("N"), StringComparison.OrdinalIgnoreCase));
        }

        public string Name
        {
            get { return Plugin.Instance.Name; }
        }

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {

            var options = GetOptions(request.User);

            var message = new Dictionary<string, string> {
                { "deviceId", options.DeviceId },
                { "icon", Plugin.Instance.IconURL.ToString() },
                { "apikey", options.ApiKey }
            };

            if (string.IsNullOrEmpty(request.Description)) {
                message.Add("title", "Emby");
                message.Add("text", request.Name);
            } else {
                message.Add("title", request.Name);
                message.Add("text", request.Description);
            }

            _logger.Debug("Join Notification to {0} - {1} - {2}", options.DeviceId, request.Name, request.Description);

            var _httpRequest = new HttpRequestOptions
            {
                Url = Plugin.Instance.ApiV1Endpoint + Plugin.Instance.ToQueryString(message),
                CancellationToken = cancellationToken
            };

            using (await _httpClient.Get(_httpRequest).ConfigureAwait(false))
            {

            }

        }


        private bool IsValid(JoinPluginOptions options)
        {
            return !string.IsNullOrEmpty(options.ApiKey) && !string.IsNullOrEmpty(options.DeviceId);
        }
    }
}

