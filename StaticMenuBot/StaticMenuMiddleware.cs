using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StaticMenuBot
{
    public class StaticMenuMiddleware
        : IMiddleware
    {
        public async Task OnTurnAsync(
            ITurnContext turnContext,
            NextDelegate next,
            CancellationToken cancellationToken = default)
        {
            turnContext.OnSendActivities(AppendStaticMenuToLastActivity);
            await next(cancellationToken).ConfigureAwait(false);
        }

        private async Task<ResourceResponse[]> AppendStaticMenuToLastActivity(
            ITurnContext turnContext,
            List<Activity> activities,
            Func<Task<ResourceResponse[]>> next)
        {
            var lastActivity = activities.LastOrDefault();

            if (lastActivity.SuggestedActions is null)
                lastActivity.SuggestedActions =
                    new SuggestedActions(actions: new List<CardAction>());

            foreach (var option in Staticmenu.options)
                lastActivity.SuggestedActions.Actions.Add(option);

            return await next();
        }
    }
}
