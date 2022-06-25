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

        private Task<DialogTurnResult> Step01_ChooseDialog(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choices = new List<Choice>();
            foreach(var option in Staticmenu.options)
            {
                choices.Add(new Choice
                {
                    Action = option,
                    Value = option.Value.ToString(),
                });
            }

            return stepContext.PromptAsync(
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

        private Task<DialogTurnResult> Step02_FinalStep(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return stepContext.ReplaceDialogAsync(Id, null, cancellationToken);
        }

        protected override Task<DialogTurnResult> OnContinueDialogAsync(
            DialogContext innerDc, CancellationToken cancellationToken = default)
        {
            return innerDc.Context.Activity.Text switch
            {
                "goto:menu" => CallMenu(innerDc, cancellationToken),
                "goto:userprofiledialog" => CallUserProfile(innerDc, cancellationToken),
                "goto:weatherdialog" => CallWeatherDialog(innerDc, cancellationToken),
                "goto:cancelconversation" => CancelDialog(innerDc, cancellationToken),
                _ => base.OnContinueDialogAsync(innerDc, cancellationToken),
            };
        }

        private Task<DialogTurnResult> CancelDialog(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            innerDc.Context.SendActivityAsync(
                "Conversation is cancelled.",
                cancellationToken: cancellationToken);
            innerDc.CancelAllDialogsAsync(
                cancellationToken: cancellationToken);
            return innerDc.ReplaceDialogAsync(
                Id, 
                cancellationToken:cancellationToken);
        }

        private static Task<DialogTurnResult> CallWeatherDialog(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            return innerDc.ReplaceDialogAsync(
                nameof(WeatherDialog),
                cancellationToken: cancellationToken);
        }

        private static Task<DialogTurnResult> CallUserProfile(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            return innerDc.ReplaceDialogAsync(
                nameof(UserProfileDialog),
                cancellationToken: cancellationToken);
        }

        private static Task<DialogTurnResult> CallMenu(
            DialogContext innerDc, CancellationToken cancellationToken)
        {
            return innerDc.ReplaceDialogAsync(
                nameof(RootDialog),
                cancellationToken: cancellationToken);
        }
    }
}
