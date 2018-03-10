using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace Bisner.Mobile.Droid.Controls
{
    public class CenteredButtonDrawable : Button
    {
        private static int LEFT = 0, TOP = 1, RIGHT = 2, BOTTOM = 3;

        // Pre-allocate objects for layout measuring
        private readonly Rect _textBounds = new Rect();
        private readonly Rect _drawableBounds = new Rect();

        public CenteredButtonDrawable(Context context) : base(context, null)
        {
        }

        public CenteredButtonDrawable(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public CenteredButtonDrawable(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (!changed) return;

            var text = Text;
            if (!string.IsNullOrEmpty(text))
            {
                var textPaint = Paint;
                textPaint.GetTextBounds(text, 0, text.Length, _textBounds);
            }
            else
            {
                _textBounds.SetEmpty();
            }

            var width = Width - (PaddingLeft + PaddingRight);
            var height = Height - (PaddingTop + PaddingBottom);

            var drawables = GetCompoundDrawables();

            if (drawables[LEFT] != null)
            {
                drawables[LEFT].CopyBounds(_drawableBounds);
                int leftOffset = (width - (_textBounds.Width() + _drawableBounds.Width()) + RightPaddingOffset) / 2 - CompoundDrawablePadding;
                _drawableBounds.Offset(leftOffset, 0);
                drawables[LEFT].Bounds = _drawableBounds;
            }

            if (drawables[RIGHT] != null)
            {
                drawables[RIGHT].CopyBounds(_drawableBounds);
                int rightOffset =
                    ((_textBounds.Width() + _drawableBounds.Width()) - width + LeftPaddingOffset) / 2 + CompoundDrawablePadding;
                _drawableBounds.Offset(rightOffset, 0);
                drawables[RIGHT].Bounds = _drawableBounds;
            }

            if (drawables[TOP] != null)
            {
                drawables[TOP].CopyBounds(_drawableBounds);
                int topOffset = (height - (_textBounds.Height() + _drawableBounds.Height()) + TopPaddingOffset) / 2 - CompoundDrawablePadding;
                _drawableBounds.Offset(0, topOffset);
                drawables[TOP].Bounds = _drawableBounds;
            }
        }
    }
}