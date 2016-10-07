using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Logging;
using Emby.Notification.Join.Configuration;
using ServiceStack;
using ServiceStack.Text;

namespace Emby.Notification.Join.Api
{
    [Route("/Notification/Join/Test/{UserID}", "POST", Summary = "Tests Join Notification")]
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

        private JoinPluginOptions GetOptions(String userID)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, userID, StringComparison.OrdinalIgnoreCase));
        }

        public object Post(TestNotification request)
        {
            var options = GetOptions(request.UserID);

            var message = new Dictionary<string, string> {
                { "apikey", options.ApiKey },
                { "deviceId", options.DeviceId },
                { "icon", Plugin.Instance.IconURL.ToString() },
                { "title", "Test Message" },
                { "text", "This is a test from Emby." }
            };
            


            _logger.Debug("Join Notification <TEST> to {0}", options.DeviceId);

            return _httpClient.Get(new HttpRequestOptions { Url = Plugin.Instance.ApiV1Endpoint + Plugin.Instance.ToQueryString(message) });
        }
    }
}

