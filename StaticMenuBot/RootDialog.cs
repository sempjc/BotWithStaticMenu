using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StaticMenuBot
{
    public class RootDialog : ComponentDialog
    {
        private const string MenuChoicePromt = "MenuChoicePromptId";

        public RootDialog()
            :base(nameof(RootDialog))
        {
            InitialDialogId = Id + "_wsteps";

            AddDialog(new WaterfallDialog(InitialDialogId, new WaterfallStep[]
            {
                Step01_ChooseDialog,
                Step02_FinalStep,
            }));
            AddDialog(new UserProfileDialog());
            AddDialog(new WeatherDialog());
            AddDialog(new ChoicePrompt(MenuChoicePromt));
        }

        private async Task<DialogTurnResult> Step01_ChooseDialog(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choices = new List<Choice>();
            foreach(var option in Staticmenu.options)
                choices.Add(new Choice
                {
                    Action = option,
                    Value = option.Value.ToString(),
                });

            return await stepContext.PromptAsync(
                MenuChoicePromt,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(
                        "Please choose an options"),
                    RetryPrompt = MessageFactory.Text(
                        "Please choose an options from the list."),
                    Choices = choices,
                    Style = ListStyle.None
                }, cancellationToken);
        }

        private async Task<DialogTurnResult> Step02_FinalStep(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.ReplaceDialogAsync(Id, null, cancellationToken);
        }


        protected override async Task<DialogTurnResult> OnContinueDialogAsync(
            DialogContext innerDc, CancellationToken cancellationToken = default)
        {
            string action = innerDc.Context.Activity.Text ?? 
                innerDc.Context.Activity.Value.ToString() ?? ""; 

            return action switch
            {
                "goto:menu" => await CallMenu(innerDc, cancellationToken),
                "goto:userprofiledialog" => await CallUserProfile(innerDc, cancellationToken),
                "goto:weatherdialog" => await CallWeatherDialog(innerDc, cancellationToken),
                "goto:cancelconversation" => await CancelDialog(innerDc, cancellationToken),
                _ => await base.OnContinueDialogAsync(innerDc, cancellationToken),
            };
        }

        private async Task<DialogTurnResult> CancelDialog(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            await innerDc.Context.SendActivityAsync(
                "Conversation is cancelled.",
                cancellationToken: cancellationToken);
            await innerDc.CancelAllDialogsAsync(
                cancellationToken: cancellationToken);
            return await innerDc.ReplaceDialogAsync(
                Id, 
                cancellationToken:cancellationToken);
        }

        private static async Task<DialogTurnResult> CallWeatherDialog(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            return await innerDc.ReplaceDialogAsync(
                nameof(WeatherDialog),
                cancellationToken: cancellationToken);
        }

        private static async Task<DialogTurnResult> CallUserProfile(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            return await innerDc.ReplaceDialogAsync(
                nameof(UserProfileDialog),
                cancellationToken: cancellationToken);
        }

        private static async Task<DialogTurnResult> CallMenu(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            return await innerDc.ReplaceDialogAsync(
                nameof(RootDialog),
                cancellationToken: cancellationToken);
        }
    }
}
