using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Services;
using Emby.Notification.Slack.Configuration;
using System.Threading;
using MediaBrowser.Model.Serialization;

namespace Emby.Notification.Slack.Api
{
    [Route("/Notification/Slack/Test/{UserID}", "POST", Summary = "Tests Slack Notification")]
    public class TestNotification : IReturnVoid
    {
        [ApiMember(Name = "UserID", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string UserID { get; set; }
    }

    public class ServerApiEndpoints : IService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IJsonSerializer _jsonSerializer;

        public ServerApiEndpoints(ILogManager logManager, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }

        private SlackPluginOptions GetOptions(String userID)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, userID, StringComparison.OrdinalIgnoreCase));
        }

        public void Post(TestNotification request)
        {
            var task = PostAsync(request);
            Task.WaitAll(task);
        }

        public async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserID);

            var slackMessage = new SlackMessage { channel = options.Channel, icon_emoji = options.Emoji, username = options.UserName, text = "This is a test notification from Emby" };

            var parameters = new Dictionary<string, string> { };
            parameters.Add("payload", _jsonSerializer.SerializeToString(slackMessage));

            _logger.Debug("Slack <TEST> to {0}", options.Channel);

            var httpRequestOptions = new HttpRequestOptions
            {
                Url = options.SlackWebHookURI,
                CancellationToken = CancellationToken.None
            };

            httpRequestOptions.SetPostData(parameters);

            using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
            {

            }
        }
    }
}

