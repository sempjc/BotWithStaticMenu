using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace StaticMenuBot
{
    public static class Staticmenu
    {
        public static List<CardAction> options = new()
        {
            new CardAction
            {
                Title = "Menu",
                Value = "goto:menu",
                Type = ActionTypes.PostBack,
            },
            new CardAction
            {
                Title = "UserProfile",
                Value = "goto:userprofiledialog",
                Type = ActionTypes.PostBack,
            },
            new CardAction
            {
                Title = "weather",
                Value = "goto:weatherdialog",
                Type = ActionTypes.PostBack,
            },
            new CardAction
            {
                Title = "Cancel",
                Value = "goto:cancelconversation",
                Type = ActionTypes.PostBack,
            }
        };
    }
}
