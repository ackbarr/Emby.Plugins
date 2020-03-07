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
using Emby.Notification.Slack.Configuration;
using MediaBrowser.Model.Serialization;

namespace Emby.Notification.Slack
{
    public class SlackNotifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public SlackNotifier(ILogManager logManager, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
        }

        private SlackPluginOptions GetOptions(User user)
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


            var slackMessage = new SlackMessage { channel = options.Channel, icon_emoji = options.Emoji, username = options.UserName };

            if (string.IsNullOrEmpty(request.Description))
            {
                slackMessage.text = request.Name;
            }
            else
            {
                slackMessage.text = request.Name + "\r\n" + request.Description;
            }

            slackMessage.text = slackMessage.text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");


            var parameters = new Dictionary<string, string> { };
            parameters.Add("payload", System.Net.WebUtility.UrlEncode(_jsonSerializer.SerializeToString(slackMessage)));

            _logger.Debug("Slack to {0} - {1} - {2}", options.Channel, request.Name, request.Description);
            _logger.Debug("Slack Payload: {0}", System.Net.WebUtility.UrlEncode(_jsonSerializer.SerializeToString(slackMessage)));
            var _httpRequest = new HttpRequestOptions
            {
                Url = options.SlackWebHookURI,
                CancellationToken = cancellationToken
            };

            _httpRequest.SetPostData(parameters);
            using (await _httpClient.Post(_httpRequest).ConfigureAwait(false))
            {

            }


        }


        private bool IsValid(SlackPluginOptions options)
        {
            return !string.IsNullOrEmpty(options.SlackWebHookURI) && !string.IsNullOrEmpty(options.Channel);
        }
    }
}

