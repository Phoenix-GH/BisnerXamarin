using System;
using Android.Graphics;
using Square.Picasso;

namespace Bisner.Mobile.Droid.Controls.PicassoTransformations
{
    public class RoundedCornersTransformation : Java.Lang.Object, ITransformation
    {
        #region Variables

        private readonly int _radius;
        private readonly int _diameter;
        private readonly int _margin;
        private readonly CornerType _cornerType;

        #endregion Variables

        #region Constructor

        public RoundedCornersTransformation(int radius, int margin) : this(radius, margin, CornerType.ALL)
        {
        }

        public RoundedCornersTransformation(int radius, int margin, CornerType cornerType)
        {
            _radius = radius;
            _diameter = radius * 2;
            _margin = margin;
            _cornerType = cornerType;
        }

        public Bitmap Transform(Bitmap source)
        {
            var width = source.Width;
            var height = source.Height;

            var bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);

            var canvas = new Canvas(bitmap);
            var paint = new Paint { AntiAlias = true };
            paint.SetShader(new BitmapShader(source, Shader.TileMode.Clamp, Shader.TileMode.Clamp));
            DrawRoundRect(canvas, paint, width, height);
            source.Recycle();

            return bitmap;
        }

        #endregion Constructor

        #region Helpers 
        private void DrawRoundRect(Canvas canvas, Paint paint, float width, float height)
        {
            var right = width - _margin;
            var bottom = height - _margin;

            switch (_cornerType)
            {
                case CornerType.ALL:
                    canvas.DrawRoundRect(new RectF(_margin, _margin, right, bottom), _radius, _radius, paint);
                    break;
                case CornerType.TOP_LEFT:
                    DrawTopLeftRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.TOP_RIGHT:
                    DrawTopRightRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.BOTTOM_LEFT:
                    DrawBottomLeftRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.BOTTOM_RIGHT:
                    DrawBottomRightRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.TOP:
                    DrawTopRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.BOTTOM:
                    DrawBottomRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.LEFT:
                    DrawLeftRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.RIGHT:
                    DrawRightRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.OTHER_TOP_LEFT:
                    DrawOtherTopLeftRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.OTHER_TOP_RIGHT:
                    DrawOtherTopRightRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.OTHER_BOTTOM_LEFT:
                    DrawOtherBottomLeftRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.OTHER_BOTTOM_RIGHT:
                    DrawOtherBottomRightRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.DIAGONAL_FROM_TOP_LEFT:
                    DrawDiagonalFromTopLeftRoundRect(canvas, paint, right, bottom);
                    break;
                case CornerType.DIAGONAL_FROM_TOP_RIGHT:
                    DrawDiagonalFromTopRightRoundRect(canvas, paint, right, bottom);
                    break;
                default:
                    canvas.DrawRoundRect(new RectF(_margin, _margin, right, bottom), _radius, _radius, paint);
                    break;
            }
        }

        private void DrawTopLeftRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, _margin + _diameter, _margin + _diameter),
                _radius, _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin + _radius, _margin + _radius, bottom), paint);
            canvas.DrawRect(new RectF(_margin + _radius, _margin, right, bottom), paint);
        }

        private void DrawTopRightRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(right - _diameter, _margin, right, _margin + _diameter), _radius,
                _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin, right - _radius, bottom), paint);
            canvas.DrawRect(new RectF(right - _radius, _margin + _radius, right, bottom), paint);
        }

        private void DrawBottomLeftRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, bottom - _diameter, _margin + _diameter, bottom), _radius, _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin, _margin + _diameter, bottom - _radius), paint);
            canvas.DrawRect(new RectF(_margin + _radius, _margin, right, bottom), paint);
        }

        private void DrawBottomRightRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(right - _diameter, bottom - _diameter, right, bottom), _radius, _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin, right - _radius, bottom), paint);
            canvas.DrawRect(new RectF(right - _radius, _margin, right, bottom - _radius), paint);
        }

        private void DrawTopRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, right, _margin + _diameter), _radius, _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin + _radius, right, bottom), paint);
        }

        private void DrawBottomRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, bottom - _diameter, right, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin, _margin, right, bottom - _radius), paint);
        }

        private void DrawLeftRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, _margin + _diameter, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin + _radius, _margin, right, bottom), paint);
        }

        private void DrawRightRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(right - _diameter, _margin, right, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin, _margin, right - _radius, bottom), paint);
        }

        private void DrawOtherTopLeftRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, bottom - _diameter, right, bottom), _radius, _radius,
                paint);
            canvas.DrawRoundRect(new RectF(right - _diameter, _margin, right, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin, _margin, right - _radius, bottom - _radius), paint);
        }

        private void DrawOtherTopRightRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, _margin + _diameter, bottom), _radius, _radius,
                paint);
            canvas.DrawRoundRect(new RectF(_margin, bottom - _diameter, right, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin + _radius, _margin, right, bottom - _radius), paint);
        }

        private void DrawOtherBottomLeftRoundRect(Canvas canvas, Paint paint, float right, float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, right, _margin + _diameter), _radius, _radius,
                paint);
            canvas.DrawRoundRect(new RectF(right - _diameter, _margin, right, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin, _margin + _radius, right - _radius, bottom), paint);
        }

        private void DrawOtherBottomRightRoundRect(Canvas canvas, Paint paint, float right,
            float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, right, _margin + _diameter), _radius, _radius,
                paint);
            canvas.DrawRoundRect(new RectF(_margin, _margin, _margin + _diameter, bottom), _radius, _radius,
                paint);
            canvas.DrawRect(new RectF(_margin + _radius, _margin + _radius, right, bottom), paint);
        }

        private void DrawDiagonalFromTopLeftRoundRect(Canvas canvas, Paint paint, float right,
            float bottom)
        {
            canvas.DrawRoundRect(new RectF(_margin, _margin, _margin + _diameter, _margin + _diameter),
                _radius, _radius, paint);
            canvas.DrawRoundRect(new RectF(right - _diameter, bottom - _diameter, right, bottom), _radius,
                _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin + _radius, right - _diameter, bottom), paint);
            canvas.DrawRect(new RectF(_margin + _diameter, _margin, right, bottom - _radius), paint);
        }

        private void DrawDiagonalFromTopRightRoundRect(Canvas canvas, Paint paint, float right,
            float bottom)
        {
            canvas.DrawRoundRect(new RectF(right - _diameter, _margin, right, _margin + _diameter), _radius,
                _radius, paint);
            canvas.DrawRoundRect(new RectF(_margin, bottom - _diameter, _margin + _diameter, bottom),
                _radius, _radius, paint);
            canvas.DrawRect(new RectF(_margin, _margin, right - _radius, bottom - _radius), paint);
            canvas.DrawRect(new RectF(_margin + _radius, _margin + _radius, right, bottom), paint);
        }

        #endregion Helpers

        #region Interface

        public string Key => $"RoundedTransformation(radius={_radius}, margin={_margin }, diameter={_diameter }, cornerType={_cornerType})";

        #endregion Interface

        #region Properties

        [Flags]
        public enum CornerType
        {
            ALL,
            TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT,
            TOP, BOTTOM, LEFT, RIGHT,
            OTHER_TOP_LEFT, OTHER_TOP_RIGHT, OTHER_BOTTOM_LEFT, OTHER_BOTTOM_RIGHT,
            DIAGONAL_FROM_TOP_LEFT, DIAGONAL_FROM_TOP_RIGHT
        }

        #endregion Properties
    }
}