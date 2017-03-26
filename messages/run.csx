#r "Newtonsoft.Json"
#load "BasicLuisDialog.csx"

using System;
using System.Net;
using System.Threading;
using Newtonsoft.Json;

using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    // Initialize the azure bot
    using (BotService.Initialize())
    {
        // Deserialize the incoming activity
        string jsonContent = await req.Content.ReadAsStringAsync();
        var activity = JsonConvert.DeserializeObject<Activity>(jsonContent);
        
        // authenticate incoming request and add activity.ServiceUrl to MicrosoftAppCredentials.TrustedHostNames
        // if request is authenticated
        if (!await BotService.Authenticator.TryAuthenticateAsync(req, new [] {activity}, CancellationToken.None))
        {
            return BotAuthenticator.GenerateUnauthorizedResponse(req);
        }
    
        if (activity != null)
        {
            // one of these will have an interface and process it
            switch (activity.GetActivityType())
          
            {
                case ActivityTypes.Message:
                    log.Info($"Message!");
               
                    activity.Attachments.Add(new Attachment()
                    {
                        ContentUrl = "http://aihelpwebsite.com/portals/0/Images/AIHelpWebsiteLogo_Large.png",
                        ContentType = "image/png",
                        Name = "AIHelpWebsiteLogo_Large.png"
                    });
                    var reply = activity.CreateReply();
                    await client.Conversations.ReplyToActivityAsync(reply);
                    //await Conversation.SendAsync(activity, () => new BasicLuisDialog());
                    break;
                case ActivityTypes.ConversationUpdate:
                    log.Info($"Message1!");
                    var client = new ConnectorClient(new Uri(activity.ServiceUrl));
                    log.Info($"{activity.ServiceUrl}");
                    IConversationUpdateActivity update = activity;
                    log.Info($"Message3!");
                    if (update.MembersAdded.Any())
                    {
                        log.Info($"Message4!");
                        var reply = activity.CreateReply(); log.Info($"Message1!");
                        var newMembers = update.MembersAdded?.Where(t => t.Id != activity.Recipient.Id);
                        log.Info($"Message5!");
                        foreach (var newMember in newMembers)
                        {
                            reply.Text = "Welcome";
                            if (!string.IsNullOrEmpty(newMember.Name))
                            {
                                reply.Text += $" {newMember.Name}";
                            }
                            reply.Text += "!";
                            log.Info($"Message6!");
                            await client.Conversations.ReplyToActivityAsync(reply);
                            log.Info($"Message7!");
                        }
                    }
                    break;
                case ActivityTypes.ContactRelationUpdate:
                case ActivityTypes.Typing:
                case ActivityTypes.DeleteUserData:
                case ActivityTypes.Ping:
                default:
                    log.Error($"Unknown activity type ignored: {activity.GetActivityType()}");
                    break;
            }
        }
        return req.CreateResponse(HttpStatusCode.Accepted);
    }    
}