using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;
using System;

namespace TempleAdvisor_Bot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Dialogs.QnADialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
                var heroCard = new ThumbnailCard
                {
                    Title = "Temple Advisor",
                    Subtitle = "I am your virtual assistant to help you on your queries related to Temples accross India",
                    //Text = "I maintain all data related to Temples",
                    Images = new List<CardImage> { new CardImage("https://templeadvisor.azurewebsites.net/images/talogo.png") },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.ImBack, "Get Started", value: "Get Started") }
                };


                //Activity replyToConversation = message.CreateReply("Welcome **" + message.From.Name +"!!**");
                Activity replyToConversation = message.CreateReply("Welcome **Devotee!!**");

                Attachment plAttachment = heroCard.ToAttachment();
                replyToConversation.Attachments.Add(plAttachment);

                if (message.MembersAdded.Any(o => o.Id == message.Recipient.Id))
                {
                    //var reply = message.CreateReply("Welcome to cBOT. You can have all your financial info here.");

                    ConnectorClient connector = new ConnectorClient(new Uri(message.ServiceUrl));

                    await connector.Conversations.ReplyToActivityAsync(replyToConversation);
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            //return null;
        }
    }
}