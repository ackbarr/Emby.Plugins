using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emby.Notification.Slack
{
    /// <summary>
    /// Slack Message
    /// </summary>
    public class SlackMessage
    {
        /// <summary>
        /// This is the text that will be posted to the channel
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// Optional override of destination channel
        /// </summary>
        public string channel { get; set; }
        /// <summary>
        /// Optional override of the username that is displayed
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// Optional emoji displayed with the message
        /// </summary>
        public string icon_emoji { get; set; }
        /// <summary>
        /// Optional url for icon displayed with the message
        /// </summary>

        public SlackMessage Clone(string newChannel = null)
        {
            return new SlackMessage()
            {

                text = text,
                icon_emoji = icon_emoji,
                username = username,
                channel = newChannel ?? channel
            };
        }
    }
}