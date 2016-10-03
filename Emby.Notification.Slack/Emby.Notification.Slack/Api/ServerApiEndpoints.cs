using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Logging;
using Emby.Notification.Slack.Configuration;
using ServiceStack;
using ServiceStack.Text;

namespace Emby.Notification.Slack.Api
{
    [Route("/Notification/Slack/Test/{UserID}", "POST", Summary = "Tests Slack Notification")]
    public class TestNotification : IReturnVoid
    {
        [ApiMember(Name = "UserID", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string UserID { get; set; }
    }

    public class ServerApiEndpoints : IRestfulService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;

        public ServerApiEndpoints(ILogManager logManager, IHttpClient httpClient)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
        }

        private SlackPluginOptions GetOptions(String userID)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, userID, StringComparison.OrdinalIgnoreCase));
        }

        public object Post(TestNotification request)
        {
            var options = GetOptions(request.UserID);

            var slackMessage = new SlackMessage { Channel = options.Channel, IconEmoji = options.Emoji, Username = options.UserName, Text = "This is a test notification from Emby" };

            var parameters = new Dictionary<string, string> { };
            using (var scope = JsConfig.BeginScope())
            {
                scope.EmitLowercaseUnderscoreNames = true;
                scope.IncludeNullValues = false;

                parameters.Add("payload", JsonSerializer.SerializeToString(slackMessage));

            }


            _logger.Debug("Slack <TEST> to {0}", options.Channel);

            return _httpClient.Post(new HttpRequestOptions { Url = options.SlackWebHookURI }, parameters);
        }
    }
}

