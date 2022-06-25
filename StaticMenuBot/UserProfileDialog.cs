using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StaticMenuBot
{
    public class UserProfileDialog
        : ComponentDialog
    {
        private const string AgePromptId = "AgePrompt";
        private const string NamePromptId = "UserNamePrompt";
        private const string TransportPromptId = "TrasnportPrompt";
        private const string UserProfileConfirmationPromptId = "UserProfileConfirmationPrompt";
        private readonly List<string> TransportChoices = new(4) { "Car", "Bicycle", "Bus", "Motorcycle" };

        public UserProfileDialog()
            : base(dialogId: nameof(UserProfileDialog))
        {
            InitialDialogId = nameof(Id) + "_wsteps";
            var wfDialog = new WaterfallDialog(
                InitialDialogId, new WaterfallStep[]
                {
                    Step01_AskUserName,
                    Step02_AskAge,
                    Step03_AskTransportPreference,
                    Step04_UserProfileSummary,
                    Step05_FinalStep
                });

            AddDialog(wfDialog);
            AddDialog(new TextPrompt(NamePromptId));
            AddDialog(new NumberPrompt<int>(AgePromptId));
            AddDialog(new ChoicePrompt(TransportPromptId));
            AddDialog(new ConfirmPrompt(UserProfileConfirmationPromptId));
        }

        private Task<DialogTurnResult> Step01_AskUserName(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return stepContext.PromptAsync(
                NamePromptId,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What is your name?")
                }, cancellationToken);
        }

        private Task<DialogTurnResult> Step02_AskAge(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["name"] = stepContext.Result as string;
            return stepContext.PromptAsync(
                AgePromptId,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text("What is your age?"),
                    RetryPrompt = MessageFactory.Text("Please enter your age")
                }, cancellationToken);
        }

        private Task<DialogTurnResult> Step03_AskTransportPreference(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["age"] = stepContext.Result.ToString();
            return stepContext.PromptAsync(
                TransportPromptId,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(
                        "Please choose your favorite method of transportation."),
                    RetryPrompt = MessageFactory.Text(
                        "Please choose your favorite method of transportation from the list."),
                    Choices = ChoiceFactory.ToChoices(TransportChoices),
                    Style = ListStyle.HeroCard
                }, cancellationToken);
        }
        private Task<DialogTurnResult> Step04_UserProfileSummary(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var transport = ((FoundChoice)stepContext.Result).Value;
            var userProfile =
                $"**Name**:             {stepContext.Values["name"]}\n\n" +
                $"**Age**:              {stepContext.Values["age"]}\n\n" +
                $"**Transportation**:   {transport}";

            return stepContext.PromptAsync(
                UserProfileConfirmationPromptId,
                new PromptOptions
                {
                    Prompt = MessageFactory.Text(
                        "## Is this information correct?\n\n" + userProfile),
                    RetryPrompt = MessageFactory.Text(
                        "## Please indicate if this information is correct\n\n" + userProfile),
                    Style = ListStyle.HeroCard
                }, cancellationToken);
        }

        private Task<DialogTurnResult> Step05_FinalStep(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            bool confirmationResutl = (bool)stepContext.Result;
            if (confirmationResutl)
            {
                stepContext.Context.SendActivityAsync(
                    $"Thank your the information {stepContext.Values["name"]}",
                    cancellationToken: cancellationToken);
                return stepContext.EndDialogAsync(null, cancellationToken);
            }
            else
            {
                stepContext.Context.SendActivityAsync(
                    "Ok let try again.", cancellationToken: cancellationToken);
                return stepContext.ReplaceDialogAsync(
                    Id, cancellationToken: cancellationToken);
            }
        }
    }
}
