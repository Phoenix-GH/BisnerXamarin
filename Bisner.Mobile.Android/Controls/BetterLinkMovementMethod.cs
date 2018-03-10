using Android.App;
using Android.Graphics;
using Android.Text;
using Android.Text.Method;
using Android.Text.Style;
using Android.Text.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace Bisner.Mobile.Droid.Controls
{
    /**
     * Handles URL clicks on TextViews. Unlike the default implementation, this:
     * <p>
     * <ul>
     * <li>Reliably applies a highlight color on links when they're touched.</li>
     * <li>Let's you handle URL clicks</li>
     * <li>Correctly identifies touched URLs (Unlike the default implementation where a click is registered even if it's
     * made outside of the URL's bounds if there is no more text in that direction.)</li>
     * </ul>
     */
    public class BetterLinkMovementMethod : LinkMovementMethod
    {
        private static readonly Class SpanClass = Class.FromType(typeof(ClickableSpan));

        private static BetterLinkMovementMethod _singleInstance;

        private const int Bleh = -2;
        private const MatchOptions LinkifyNone = (MatchOptions)Bleh;

        private IOnLinkClickListener _onLinkClickListener;
        private readonly RectF _touchedLineBounds = new RectF();
        private bool _isUrlHighlighted;
        private bool _touchStartedOverLink;
        private int _activeTextViewHashcode;

        public interface IOnLinkClickListener
        {
            /**
             * @param textView The TextView on which the URL was clicked.
             * @param url      The clicked URL.
             * @return True if this click was handled. False to let Android handle the URL.
             */
            bool OnClick(TextView textView, string url);
        }

        /**
         * Return a new instance of BetterLinkMovementMethod.
         */
        public static BetterLinkMovementMethod NewInstance()
        {
            return new BetterLinkMovementMethod();
        }

        /**
         * @param linkifyMask One of {@link Linkify#ALL}, {@link Linkify#PHONE_NUMBERS}, {@link Linkify#MAP_ADDRESSES},
         *                    {@link Linkify#WEB_URLS} and {@link Linkify#EMAIL_ADDRESSES}.
         * @param textViews   The TextViews on which a {@link BetterLinkMovementMethod} should be registered.
         * @return The registered {@link BetterLinkMovementMethod} on the TextViews.
         */
        public static BetterLinkMovementMethod Linkify(MatchOptions linkifyMask, params TextView[] textViews)
        {
            var movementMethod = NewInstance();

            foreach (var textView in textViews)
            {
                AddLinks(linkifyMask, movementMethod, textView);
            }

            return movementMethod;
        }

        /**
         * Like {@link #Linkify(int, TextView...)}, but can be used for TextViews with HTML links.
         *
         * @param textViews The TextViews on which a {@link BetterLinkMovementMethod} should be registered.
         * @return The registered {@link BetterLinkMovementMethod} on the TextViews.
         */
        public static BetterLinkMovementMethod LinkifyHtml(params TextView[] textViews)
        {
            return Linkify(LinkifyNone, textViews);
        }

        /**
         * Recursively register a {@link BetterLinkMovementMethod} on every TextView inside a layout.
         *
         * @param linkifyMask One of {@link Linkify#ALL}, {@link Linkify#PHONE_NUMBERS}, {@link Linkify#MAP_ADDRESSES},
         *                    {@link Linkify#WEB_URLS} and {@link Linkify#EMAIL_ADDRESSES}.
         * @return The registered {@link BetterLinkMovementMethod} on the TextViews.
         */
        public static BetterLinkMovementMethod Linkify(MatchOptions linkifyMask, ViewGroup viewGroup)
        {
            var movementMethod = NewInstance();
            RAddLinks(linkifyMask, viewGroup, movementMethod);
            return movementMethod;
        }

        /**
         * Like {@link #Linkify(int, TextView...)}, but can be used for TextViews with HTML links.
         *
         * @return The registered {@link BetterLinkMovementMethod} on the TextViews.
         */
        public static BetterLinkMovementMethod LinkifyHtml(ViewGroup viewGroup)
        {
            return Linkify(LinkifyNone, viewGroup);
        }

        /**
         * Recursively register a {@link BetterLinkMovementMethod} on every TextView inside a layout.
         *
         * @param linkifyMask One of {@link Linkify#ALL}, {@link Linkify#PHONE_NUMBERS}, {@link Linkify#MAP_ADDRESSES},
         *                    {@link Linkify#WEB_URLS} and {@link Linkify#EMAIL_ADDRESSES}.
         * @return The registered {@link BetterLinkMovementMethod} on the TextViews.
         */
        public static BetterLinkMovementMethod Linkify(MatchOptions linkifyMask, Activity activity)
        {
            // Find the layout passed to setContentView().
            var activityLayout = (ViewGroup)((ViewGroup)activity.FindViewById(Window.IdAndroidContent)).GetChildAt(0);

            var movementMethod = NewInstance();
            RAddLinks(linkifyMask, activityLayout, movementMethod);
            return movementMethod;
        }

        /**
         * Like {@link #Linkify(int, TextView...)}, but can be used for TextViews with HTML links.
         *
         * @return The registered {@link BetterLinkMovementMethod} on the TextViews.
         */
        public static BetterLinkMovementMethod LinkifyHtml(Activity activity)
        {
            return Linkify(LinkifyNone, activity);
        }

        /**
         * Get a static instance of BetterLinkMovementMethod. Do note that registering a click listener on the returned
         * instance is not supported because it will potentially be shared on multiple TextViews.
         */
        public static BetterLinkMovementMethod GetInstance()
        {
            if (_singleInstance == null)
            {
                _singleInstance = new BetterLinkMovementMethod();
            }
            return _singleInstance;
        }

        protected BetterLinkMovementMethod()
        {
        }

        /**
         * Set a listener that will get called whenever any link is clicked on the TextView.
         */
        public BetterLinkMovementMethod SetOnLinkClickListener(IOnLinkClickListener onLinkClickListener)
        {
            if (this == _singleInstance)
            {
                throw new UnsupportedOperationException(
                    "Setting a click listener on the instance returned by GetInstance() is not supported. " +
                    "Please use NewInstance() or any of the Linkify() methods instead.");
            }

            _onLinkClickListener = onLinkClickListener;
            return this;
        }

        // ======== PUBLIC APIs END ======== //

        private static void RAddLinks(MatchOptions linkifyMask, ViewGroup viewGroup, BetterLinkMovementMethod movementMethod)
        {
            for (var i = 0; i < viewGroup.ChildCount; i++)
            {
                var child = viewGroup.GetChildAt(i);

                if (child is ViewGroup)
                {
                    // Recursively find child TextViews.
                    RAddLinks(linkifyMask, child as ViewGroup, movementMethod);
                }
                else if (child is TextView)
                {
                    var textView = child as TextView;
                    AddLinks(linkifyMask, movementMethod, textView);
                }
            }
        }

        private static void AddLinks(MatchOptions linkifyMask, BetterLinkMovementMethod movementMethod, TextView textView)
        {
            textView.MovementMethod = movementMethod;
            if (linkifyMask != LinkifyNone)
            {
                Android.Text.Util.Linkify.AddLinks(textView, linkifyMask);
            }
        }


        public override bool OnTouchEvent(TextView view, ISpannable buffer, MotionEvent e)
        {
            if (_activeTextViewHashcode != view.GetHashCode())
            {
                // Bug workaround: TextView stops calling onTouchEvent() once any URL is highlighted.
                // A hacky solution is to reset any "autoLink" property set in XML. But we also want
                // to do this once per TextView.
                _activeTextViewHashcode = view.GetHashCode();
                view.AutoLinkMask = 0;
            }

            var touchedClickableSpan = FindClickableSpanUnderTouch(view, buffer, e);

            // Toggle highlight
            if (touchedClickableSpan != null)
            {
                HighlightUrl(view, touchedClickableSpan, buffer);
            }
            else
            {
                RemoveUrlHighlightColor(view);
            }

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    _touchStartedOverLink = touchedClickableSpan != null;
                    return _touchStartedOverLink;

                case MotionEventActions.Up:
                    // Register a click only if the touch started on an URL. That is, the touch did not start
                    // elsewhere and ended up on an URL.
                    if (touchedClickableSpan != null && _touchStartedOverLink)
                    {
                        DispatchUrlClick(view, touchedClickableSpan);
                        RemoveUrlHighlightColor(view);
                    }
                    var didTouchStartOverLink = _touchStartedOverLink;
                    _touchStartedOverLink = false;

                    // Consume this event even if we could not find any spans. Android's TextView implementation
                    // has a bug where links get clicked even when there is no more text next to the link and the
                    // touch lies outside its bounds in the same direction.
                    return didTouchStartOverLink;

                case MotionEventActions.Move:
                    return _touchStartedOverLink;

                default:
                    return false;
            }
        }

        /**
         * Determines the touched location inside the TextView's text and returns the ClickableSpan found under it (if any).
         *
         * @return The touched ClickableSpan or null.
         */
        protected ClickableSpanWithText FindClickableSpanUnderTouch(TextView textView, ISpannable text, MotionEvent e)
        {
            // So we need to find the location in text where touch was made, regardless of whether the TextView
            // has scrollable text. That is, not the entire text is currently visible.
            var touchX = (int)e.GetX();
            var touchY = (int)e.GetY();

            // Ignore padding.
            touchX -= textView.TotalPaddingLeft;
            touchY -= textView.TotalPaddingTop;

            // Account for scrollable text.
            touchX += textView.ScrollX;
            touchY += textView.ScrollY;

            var layout = textView.Layout;
            var touchedLine = layout.GetLineForVertical(touchY);
            var touchOffset = layout.GetOffsetForHorizontal(touchedLine, touchX);

            _touchedLineBounds.Left = layout.GetLineLeft(touchedLine);
            _touchedLineBounds.Top = layout.GetLineTop(touchedLine);
            _touchedLineBounds.Right = layout.GetLineWidth(touchedLine) + _touchedLineBounds.Left;
            _touchedLineBounds.Bottom = layout.GetLineBottom(touchedLine);

            if (_touchedLineBounds.Contains(touchX, touchY))
            {
                // Find any ClickableSpan that lies under the touched area
                var spans = text.GetSpans(touchOffset, touchOffset, SpanClass);
                foreach (var span in spans)
                {
                    if (span is ClickableSpan)
                    {
                        return ClickableSpanWithText.OfSpan(textView, span as ClickableSpan);
                    }
                }

                // No ClickableSpan found under the touched location.
                return null;
            }

            // Touch lies outside the line's horizontal bounds where no spans should exist.
            return null;
        }

        /**
         * Adds a highlight background color span to the TextView.
         */
        protected void HighlightUrl(TextView textView, ClickableSpanWithText spanWithText, ISpannable text)
        {
            if (_isUrlHighlighted)
            {
                return;
            }

            _isUrlHighlighted = true;

            var spanStart = text.GetSpanStart(spanWithText.Span);
            var spanEnd = text.GetSpanEnd(spanWithText.Span);
            text.SetSpan(new BackgroundColorSpan(textView.HighlightColor), spanStart, spanEnd, SpanTypes.InclusiveInclusive);
            textView.SetText(text, TextView.BufferType.Spannable);

            Selection.SetSelection(text, spanStart, spanEnd);
        }

        /**
         * Removes the highlight color under the Url.
         */
        protected void RemoveUrlHighlightColor(TextView textView)
        {
            if (!_isUrlHighlighted)
            {
                return;
            }
            _isUrlHighlighted = false;

            var text = (ISpannable)textView.TextFormatted;

            var highlightSpans = text.GetSpans(0, text.Length(), Class.FromType(typeof(BackgroundColorSpan)));

            foreach (var highlightSpan in highlightSpans)
            {
                text.RemoveSpan(highlightSpan);
            }

            textView.SetText(text, TextView.BufferType.Spannable);

            Selection.RemoveSelection(text);
        }

        protected void DispatchUrlClick(TextView textView, ClickableSpanWithText spanWithText)
        {
            var spanUrl = spanWithText.Text;
            var handled = _onLinkClickListener != null && _onLinkClickListener.OnClick(textView, spanUrl);
            if (!handled)
            {
                // Let Android handle this click.
                spanWithText.Span.OnClick(textView);
            }
        }
    }

    /**
     * A wrapper with a clickable span and its text.
     */
    public class ClickableSpanWithText
    {
        public static ClickableSpanWithText OfSpan(TextView textView, ClickableSpan span)
        {
            var s = (ISpanned)textView.TextFormatted;
            string text;
            if (span is URLSpan)
            {
                text = ((URLSpan)span).URL;
            }
            else
            {
                var start = s.GetSpanStart(span);
                var end = s.GetSpanEnd(span);
                text = s.SubSequence(start, end);
            }
            return new ClickableSpanWithText(span, text);
        }

        private ClickableSpanWithText(ClickableSpan span, string text)
        {
            Span = span;
            Text = text;
        }

        public ClickableSpan Span { get; }

        public string Text { get; }
    }
}