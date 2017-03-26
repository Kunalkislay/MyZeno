using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;


// For more information about this template visit http://aka.ms/azurebots-csharp-luis
[Serializable]
public class BasicLuisDialog : LuisDialog<object>
{
     
    public BasicLuisDialog( ) : base(new LuisService(new LuisModelAttribute(Utils.GetAppSetting("LuisAppId"), Utils.GetAppSetting("LuisAPIKey"))))
    {
        
     
    }

    [LuisIntent("None")]
    public async Task NoneIntent(IDialogContext context, LuisResult result)
    {
         
        await context.PostAsync($"You have reached the none intent. You said: {result.Query}"); //
        context.Wait(MessageReceived);
    }

    // Go to https://luis.ai and create a new intent, then train/publish your luis app.
    // Finally replace "MyIntent" with the name of your newly created intent in the following handler
    [LuisIntent("GetActivities")]
    public async Task MyIntent(IDialogContext context, LuisResult result)
    {

      
        Dictionary<string, string> cardContentList = new Dictionary<string, string>();
        cardContentList.Add("SendCommunication", "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png");
        cardContentList.Add("AdminTask", "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png");
        cardContentList.Add("AddAttendee", "https://upload.wikimedia.org/wikipedia/en/a/a6/Bender_Rodriguez.png");
        foreach (KeyValuePair<string, string> cardContent in cardContentList)
        {
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: cardContent.Value));
            List<CardAction> cardButtons = new List<CardAction>();
            CardAction plButton = new CardAction()
            {
                Value = $"https://en.wikipedia.org/wiki/{cardContent.Key}",
                Type = "openUrl",
                Title = "WikiPedia Page"
            };
            cardButtons.Add(plButton);
            HeroCard plCard = new HeroCard()
            {
                Title = $"I'm a hero card about {cardContent.Key}",
                Subtitle = $"{cardContent.Key} Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };
            Attachment plAttachment = plCard.ToAttachment();
            message.Attachments.Add.Add(plAttachment);
        }
       
        await context.PostAsync(message); //
        
        
        context.Wait(MessageReceived);
    }
}