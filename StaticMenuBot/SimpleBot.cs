// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StaticMenuBot
{
    public class SimpleBot : ActivityHandler
    {
        private readonly Dialog _dialog;
        private readonly ConversationState _conversationState;

        public SimpleBot(Dialog dialog, ConversationState conversationState)
        {
            _dialog = dialog 
                ?? throw new System.ArgumentNullException(nameof(dialog));
            _conversationState = conversationState;
        }

        public override Task OnTurnAsync(
            ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            if(turnContext.Activity.Type != ActivityTypes.ConversationUpdate)
                RunDialog(turnContext, cancellationToken);
            else
                base.OnTurnAsync(turnContext, cancellationToken);

            return _conversationState.SaveChangesAsync(
                turnContext, false, cancellationToken);
        }

        private void RunDialog(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var dialogState = _conversationState
                .CreateProperty<DialogState>(nameof(DialogState));
            _dialog.RunAsync(turnContext, dialogState, cancellationToken);
        }

        protected override Task OnMembersAddedAsync(
            IList<ChannelAccount> membersAdded, 
            ITurnContext<IConversationUpdateActivity> turnContext, 
            CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    turnContext.SendActivityAsync(
                        MessageFactory.Text($"Hello world!."), cancellationToken);
                }
            }

            RunDialog(turnContext, cancellationToken);
            return Task.CompletedTask;
        }
    }
}
