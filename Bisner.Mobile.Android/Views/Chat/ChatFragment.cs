using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Bisner.Mobile.Core.ViewModels;
using Bisner.Mobile.Core.ViewModels.Chat;
using Bisner.Mobile.Droid.Views.Base;
using Java.Lang;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Shared.Attributes;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace Bisner.Mobile.Droid.Views.Chat
{
    [Register("bisner.mobile.droid.views.chat.ChatFragment")]
    [MvxFragment(typeof(MainViewModel), Resource.Id.content_frame)]
    public class ChatFragment : BaseToolbarFragment<ChatViewModel>
    {
        #region Constructor

        #endregion Constructor

        #region Fragment

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HasOptionsMenu = true;
        }

        protected override int FragmentId => Resource.Layout.chat_fragment;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            SetupRecyclerView(view);

            return view;
        }

        protected override string ScreenName => "ChatView";

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);

            inflater.Inflate(Resource.Menu.chat_toolbar, menu);

            // Add icon
            TintMenuItem(menu.GetItem(0), Resource.Color.unselectedtabbarcolor);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.create_chat_item)
            {
                ViewModel.CreateChatCommand.Execute(null);

                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        #endregion Fragment

        #region RecyclerView

        private void SetupRecyclerView(View view)
        {
            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.chat_recycler_view);

            if (recyclerView != null)
            {
                // Chat items have a fixed size
                recyclerView.HasFixedSize = true;
                recyclerView.Adapter = new ChatAdapter((IMvxAndroidBindingContext)BindingContext);
            }
        }

        private class ChatAdapter : MvxRecyclerAdapter
        {
            public ChatAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
            {

            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position, IList<Object> payloads)
            {
                base.OnBindViewHolder(holder, position, payloads);

                // Get bottom ruler
                var bottomRuler = holder.ItemView.FindViewById<View>(Resource.Id.chat_item_bottom_ruler);

                bottomRuler.Visibility = position == ItemCount - 1 ? ViewStates.Visible : ViewStates.Invisible;
            }
        }

        #endregion RecyclerView
    }
}