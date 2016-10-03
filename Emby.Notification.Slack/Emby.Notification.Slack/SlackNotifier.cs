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
using ServiceStack.Text;

namespace Emby.Notification.Slack
{
    public class SlackNotifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;

        public SlackNotifier(ILogManager logManager, IHttpClient httpClient)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
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

        public Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {
            var options = GetOptions(request.User);

            
            var slackMessage = new SlackMessage { Channel = options.Channel, IconEmoji = options.Emoji, Username = options.UserName };

            if (string.IsNullOrEmpty(request.Description))
            {
                slackMessage.Text = request.Name;
            }
            else
            {
                slackMessage.Text = request.Name + "\r\n" + request.Description;
            }


            var parameters = new Dictionary<string, string> { };
            using (var scope = JsConfig.BeginScope())
            {
                scope.EmitLowercaseUnderscoreNames = true;
                scope.IncludeNullValues = false;

                parameters.Add("payload", JsonSerializer.SerializeToString(slackMessage));

            }


            _logger.Debug("Slack to {0} - {1} - {2}", options.Channel, request.Name, request.Description);

            return _httpClient.Post(options.SlackWebHookURI, parameters, cancellationToken);
        }


        private bool IsValid(SlackPluginOptions options)
        {
            return !string.IsNullOrEmpty(options.SlackWebHookURI) && !string.IsNullOrEmpty(options.Channel);
        }
    }
}

