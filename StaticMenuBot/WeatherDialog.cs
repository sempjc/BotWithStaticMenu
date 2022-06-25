using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace StaticMenuBot
{
    public class WeatherDialog : ComponentDialog
    {
        public WeatherDialog()
            :base(nameof(WeatherDialog))
        {
            InitialDialogId = Id + "_wsteps";
            var wfDialog = new WaterfallDialog(InitialDialogId, new WaterfallStep[]
            {
                Step01_CommingSoon,
            });
            AddDialog(wfDialog);
        }

        private Task<DialogTurnResult> Step01_CommingSoon(
            WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Context.SendActivityAsync(
                "Commning soon!", cancellationToken: cancellationToken);
            return stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
