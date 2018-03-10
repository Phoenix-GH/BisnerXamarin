using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Bisner.Mobile.Core.ViewModels.Feed.Items;
using Bisner.Mobile.iOS.Controls;
using Cirrious.FluentLayouts.Touch;
using Cirrious.MvvmCross.Binding.BindingContext;
using Foundation;
using UIKit;

namespace Bisner.Mobile.iOS.Views.Feed.Cells
{
    public abstract class TextPostCellBase<T> : FeedPostCellBase<T> where T : FeedTextPost
    {
        #region Constructor

        //private UITextView _postText;
        private FeedWebView _postText;
        private UIView _childContentContainer;
        private NSLayoutConstraint _heightConstraint;

        protected TextPostCellBase(IntPtr handle)
            : base(handle)
        {
        }

        #endregion Constructor

        #region LifeCycle

        #endregion LifeCycle

        #region Overrides

        protected sealed override List<UIView> ControllsToAdd()
        {
            _postText = new FeedWebView();
            _postText.ScrollView.ScrollEnabled = false;
            _postText.ScrollView.Bounces = false;
            _postText.AllowsLinkPreview = false;
            _postText.LoadFinished += WebViewOnLoadFinished;
            _heightConstraint = NSLayoutConstraint.Create(_postText, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 40);
            _postText.AddConstraint(_heightConstraint);

            _childContentContainer = new UIView { BackgroundColor = UIColor.Clear };

            foreach (var addChildControl in AddChildControls())
            {
                _childContentContainer.Add(addChildControl);
            }

            return new List<UIView> { _postText, _childContentContainer };
        }

        private void WebViewOnLoadFinished(object sender, EventArgs e)
        {
            // get the document height.
            var stringHeight = _postText.EvaluateJavascript(@"document.height");
            nfloat height = nfloat.Parse(stringHeight);

            if (_heightConstraint.Constant != height)
            {
                // update
                _heightConstraint.Constant = height;

                // ask the tableview to recalc heights
                var tableview = GetTableView(Superview);

                if (tableview != null)
                {
                    tableview.BeginUpdates();
                    tableview.EndUpdates();
                }
            }
        }

        private UITableView GetTableView(UIView superview)
        {
            if (superview is UITableView)
            {
                return superview as UITableView;
            }

            return GetTableView(superview.Superview);
        }

        /// <summary>
        /// Add child controls below the text area
        /// </summary>
        /// <returns></returns>
        protected virtual List<UIView> AddChildControls()
        {
            return new List<UIView>();
        }

        protected sealed override List<FluentLayout> AddContentConstraints(UIView contentContainer)
        {
            // Base does not set this property on child views
            _childContentContainer.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints();

            // Add the constraints to the child content container which is placed below the text in the parent content container
            _childContentContainer.AddConstraints(AddContentConstraintsBelowText(_childContentContainer));

            // Return constraints for the post text and child content container
            return new List<FluentLayout>
            {
                _postText.AtTopOf(contentContainer, 10),
                _postText.WithSameLeft(contentContainer),
                _postText.WithSameRight(contentContainer),

                _childContentContainer.Below(_postText, 5),
                _childContentContainer.WithSameLeft(contentContainer),
                _childContentContainer.WithSameRight(contentContainer),
                _childContentContainer.AtBottomOf(contentContainer, 10)
            };
        }

        /// <summary>
        /// Returns a list of constraints for the child content container
        /// </summary>
        /// <param name="contentContainer"></param>
        /// <returns></returns>
        protected virtual List<FluentLayout> AddContentConstraintsBelowText(UIView contentContainer)
        {
            return new List<FluentLayout>();
        }

        /// <summary>
        /// Add bindings accordingly
        /// </summary>
        /// <param name="set"></param>
        protected override void AddBindingsToSet(MvxFluentBindingDescriptionSet<FeedPostCellBase<T>, T> set)
        {
            set.Bind(_postText).For(p => p.HtmlString).To(x => x.Text);
            set.Bind(_postText).To(x => x.Text);
        }

        #endregion Overrides
    }
}