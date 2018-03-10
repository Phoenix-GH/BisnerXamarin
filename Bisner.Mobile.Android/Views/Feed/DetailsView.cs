using System;
using System.Linq;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Bisner.ApiModels.Security.Roles;
using Bisner.Mobile.Core.Helpers;
using Bisner.Mobile.Core.ViewModels.Feed;
using Bisner.Mobile.Droid.Views.Base;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace Bisner.Mobile.Droid.Views.Feed
{
    [Activity(NoHistory = false, WindowSoftInputMode = SoftInput.StateHidden)]
    public class DetailsView : BaseToolbarActivity<DetailsViewModel>, View.IOnLayoutChangeListener
    {
        #region Variables

        private MvxRecyclerView _recyclerView;

        private MvxSubscriptionToken _mvxSubscriptionToken;

        #endregion Variables

        #region Constructor

        public DetailsView()
        {
            SupportBackButton = true;
        }

        #endregion Constructor

        #region Activity

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.details_recycler_view);

            if (_recyclerView != null)
            {
                _recyclerView.AddOnLayoutChangeListener(this);

                _recyclerView.Adapter = new DetailsAdapter((IMvxAndroidBindingContext)BindingContext);
            }

            _mvxSubscriptionToken = Mvx.Resolve<IMvxMessenger>().SubscribeOnMainThread<NewCommentMessage>(message =>
            {
                var detailAdapter = _recyclerView.Adapter as DetailsAdapter;

                if (ViewModel.Items.Count > 1)
                    detailAdapter?.NotifyItemChanged(ViewModel.Items.Count - 1);

                _recyclerView.ScrollToPosition(ViewModel.Items.Count - 1);
            });

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            // Check comment role
            if (Settings.UserRoles.All(r => r != Home.Feed.Comment.ToLower()))
            {
                var inputContainer = FindViewById<LinearLayout>(Resource.Id.details_input_container);

                if (inputContainer != null)
                {
                    var layoutParams = inputContainer.LayoutParameters;
                    layoutParams.Height = 0;
                    inputContainer.LayoutParameters = layoutParams;
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _mvxSubscriptionToken?.Dispose();
            _mvxSubscriptionToken = null;
        }

        protected override int LayoutId => Resource.Layout.details_view;

        protected override string ScreenName => "DetailsView postId = " + ViewModel.PostId;

        #endregion Activity

        #region LayoutChangeListener

        public void OnLayoutChange(View v, int left, int top, int right, int bottom, int oldLeft, int oldTop, int oldRight,
            int oldBottom)
        {
            // If the bottom changed scroll to bottom
            if (bottom < oldBottom)
            {
                _recyclerView.PostDelayed(() =>
                {
                    _recyclerView.SmoothScrollToPosition(ViewModel.Items?.Count ?? 0);
                }, 100);
            }
        }

        #endregion LayoutChangeListener

        #region RecyclerView

        private class DetailsAdapter : MvxRecyclerAdapter
        {
            #region Variabels

            private readonly int _marginPixels;

            #endregion Variables

            #region Contstructor

            public DetailsAdapter(IMvxAndroidBindingContext bindingContext) : base(bindingContext)
            {
                // Get the pixels for 15 dp margins on left and right
                var applyDimension = TypedValue.ApplyDimension(ComplexUnitType.Dip, 15, Application.Context.Resources.DisplayMetrics);
                _marginPixels = (int)Math.Round(applyDimension);
            }

            #endregion Constructor

            #region Adapter

            protected override View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
            {
                var view = base.InflateViewForHolder(parent, viewType, bindingContext);

                var bottomRuler = view.FindViewById<View>(Resource.Id.feed_item_footer_border);

                if (bottomRuler != null)
                {
                    // If more then 1 items, contains comments, set bottom post border invisible
                    bottomRuler.Visibility = ItemCount > 1 ? ViewStates.Invisible : ViewStates.Visible;
                }

                return view;
            }

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                base.OnBindViewHolder(holder, position);

                var commentBottomBorder = holder.ItemView.FindViewById<View>(Resource.Id.comment_item_bottom_ruler);

                if (commentBottomBorder != null)
                {
                    // Get linearlayout params of bottom border
                    var layoutParams = commentBottomBorder.LayoutParameters;

                    var relativeLayoutParams = layoutParams as RelativeLayout.LayoutParams;

                    // If this is the last comment item set bottom border to full width, otherwise set 15dp margin on both sides
                    if (position == ItemCount - 1)
                    {
                        relativeLayoutParams?.SetMargins(0, 0, 0, 0);
                    }
                    else
                    {
                        relativeLayoutParams?.SetMargins(_marginPixels, 0, _marginPixels, 0);
                    }

                    commentBottomBorder.LayoutParameters = relativeLayoutParams;
                }
            }

            #endregion Adapter
        }

        private class FooterDecoration : RecyclerView.ItemDecoration
        {
            private View _layout;

            public override void OnDraw(Canvas cValue, RecyclerView parent, RecyclerView.State state)
            {
                base.OnDraw(cValue, parent, state);

                _layout.Layout(parent.Left, 0, parent.Right, _layout.MeasuredHeight);

                for (int i = 0; i < parent.ChildCount; i++)
                {
                    var view = parent.GetChildAt(i);

                    if (parent.GetChildAdapterPosition(view) == parent.ChildCount - 1)
                    {

                    }
                }
            }
        }

        #endregion RecyclerView
    }
}