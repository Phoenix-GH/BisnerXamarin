using Bisner.Mobile.Core.Models.Chat;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace Bisner.Mobile.Droid.Views.Chat
{
    public class ChatConversationTemplateSelector : MvxTemplateSelector<IChatItem>
    {
        public override int GetItemLayoutId(int fromViewType)
        {
            return fromViewType == 0 ? Resource.Layout.chat_label : Resource.Layout.chat_message;
        }

        protected override int SelectItemViewType(IChatItem forItemObject)
        {
            if (forItemObject is ChatLabel)
                return 0;
            return 1;
        }
    }
}