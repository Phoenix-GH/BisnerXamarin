using System;
using System.Diagnostics;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using MvvmCross.Binding.Droid.Views;
using Math = Java.Lang.Math;

namespace Bisner.Mobile.Droid.Controls
{
    [Register("bisner.mobile.droid.controls.TouchImageView")]
    public class TouchImageView : MvxImageView, View.IOnTouchListener, ScaleGestureDetector.IOnScaleGestureListener
    {
        #region Variables

        private const float Tolerance = 0.0000001f;

        private Matrix _matrix;
        // We can be in one of these 3 states  
        private const int None = 0;
        private const int Dragging = 1;
        private const int Zoom = 2;

        private int _mode = None;

        // Remember some things for zooming  
        private readonly PointF _last = new PointF();
        private readonly PointF _start = new PointF();
        private const float MinScale = 1f;
        private float _maxScale = 3f;
        private float[] _m;
        private int _viewWidth;
        private int _viewHeight;

        private const int Clicking = 3;

        private float _saveScale = 1f;

        protected float OrigWidth, OrigHeight;

        private int _oldMeasuredWidth;
        private int _oldMeasuredHeight;

        private ScaleGestureDetector _scaleDetector;

        #endregion Variables

        #region Constructor

        public TouchImageView(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
            SharedConstructing();
        }

        public TouchImageView(Context context) : base(context)
        {
            SharedConstructing();
        }

        public TouchImageView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SharedConstructing();
        }

        public TouchImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            SharedConstructing();
        }

        #endregion Constructor

        #region Setup

        private void SharedConstructing()
        {
            Clickable = true;

            _scaleDetector = new ScaleGestureDetector(Context, this);

            _matrix = new Matrix();

            _m = new float[9];

            ImageMatrix = _matrix;

            SetScaleType(ScaleType.Matrix);

            SetOnTouchListener(this);
        }

        #endregion Setup

        #region Touch

        public bool OnTouch(View v, MotionEvent e)
        {
            _scaleDetector.OnTouchEvent(e);

            PointF curr = new PointF(e.GetX(), e.GetY());

            switch (e.Action)
            {

                case MotionEventActions.Down:

                    _last.Set(curr);

                    _start.Set(_last);

                    _mode = Dragging;

                    break;

                case MotionEventActions.Move:
                    if (_mode == Dragging)
                    {
                        float deltaX = curr.X - _last.X;
                        float deltaY = curr.Y - _last.Y;
                        float fixTransX = GetFixDragTrans(deltaX, _viewWidth, OrigWidth * _saveScale);
                        float fixTransY = GetFixDragTrans(deltaY, _viewHeight, OrigHeight * _saveScale);
                        Matrix.PostTranslate(fixTransX, fixTransY);
                        FixTrans();
                        _last.Set(curr.X, curr.Y);
                    }

                    break;

                case MotionEventActions.Up:
                    _mode = None;
                    var xDiff = (int)Math.Abs(curr.X - _start.X);
                    var yDiff = (int)Math.Abs(curr.Y - _start.Y);
                    if (xDiff < Clicking && yDiff < Clicking)
                        PerformClick();
                    break;
                case MotionEventActions.PointerUp:
                    _mode = None;
                    break;
            }

            ImageMatrix = _matrix;

            Invalidate();

            return true; // indicate event was handled  
        }

        #endregion Touch

        #region Measure

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

            _viewWidth = MeasureSpec.GetSize(widthMeasureSpec);
            _viewHeight = MeasureSpec.GetSize(heightMeasureSpec);

            // Rescales image on rotation  
            if (_oldMeasuredHeight == _viewWidth && _oldMeasuredHeight == _viewHeight || _viewWidth == 0 || _viewHeight == 0)
                return;

            _oldMeasuredHeight = _viewHeight;
            _oldMeasuredWidth = _viewWidth;

            if (System.Math.Abs(_saveScale - 1) < Tolerance)
            {

                //Fit to screen.  

                if (Drawable == null || Drawable.IntrinsicWidth == 0 || Drawable.IntrinsicHeight == 0)
                    return;

                var bmWidth = Drawable.IntrinsicWidth;
                var bmHeight = Drawable.IntrinsicHeight;

                Debug.WriteLine($"bmWidth: {bmWidth } bmHeight : {bmHeight}");


                var scaleX = _viewWidth / (float)bmWidth;
                var scaleY = _viewHeight / (float)bmHeight;

                var scale = Math.Min(scaleX, scaleY);
                _matrix.SetScale(scale, scale);

                // Center the image  
                var xspacedinges = scale * bmWidth;

                var redundantYSpace = _viewHeight - scale * bmHeight;
                var redundantXSpace = _viewWidth - xspacedinges;

                redundantYSpace /= 2;
                redundantXSpace /= 2;

                _matrix.PostTranslate(redundantXSpace, redundantYSpace);

                OrigWidth = _viewWidth - 2 * redundantXSpace;

                OrigHeight = _viewHeight - 2 * redundantYSpace;

                ImageMatrix = _matrix;
            }

            FixTrans();
        }

        #endregion Measure

        #region Scale

        public void SetMaxZoom(float x)
        {
            _maxScale = x;
        }

        private void FixTrans()
        {
            _matrix.GetValues(_m);

            var transX = _m[Matrix.MtransX];

            var transY = _m[Matrix.MtransY];

            var fixTransX = GetFixTrans(transX, _viewWidth, OrigWidth * _saveScale);

            var fixTransY = GetFixTrans(transY, _viewHeight, OrigHeight * _saveScale);

            if (System.Math.Abs(fixTransX) > Tolerance || System.Math.Abs(fixTransY) > Tolerance)

                _matrix.PostTranslate(fixTransX, fixTransY);
        }

        private static float GetFixTrans(float trans, float viewSize, float contentSize)
        {
            float minTrans, maxTrans;

            if (contentSize <= viewSize)
            {
                minTrans = 0;
                maxTrans = viewSize - contentSize;
            }
            else
            {
                minTrans = viewSize - contentSize;
                maxTrans = 0;
            }

            if (trans < minTrans)
                return -trans + minTrans;
            if (trans > maxTrans)
                return -trans + maxTrans;
            return 0;
        }

        public float GetFixDragTrans(float delta, float viewSize, float contentSize)
        {
            return contentSize <= viewSize ? 0 : delta;
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            var mScaleFactor = detector.ScaleFactor;

            var origScale = _saveScale;

            _saveScale *= mScaleFactor;

            if (_saveScale > _maxScale)
            {
                _saveScale = _maxScale;
                mScaleFactor = _maxScale / origScale;
            }
            else if (_saveScale < MinScale)
            {
                _saveScale = MinScale;
                mScaleFactor = MinScale / origScale;
            }

            if (OrigWidth * _saveScale <= _viewWidth || OrigHeight * _saveScale <= _viewHeight)
                _matrix.PostScale(mScaleFactor, mScaleFactor, _viewWidth / 2, _viewHeight / 2);
            else
                _matrix.PostScale(mScaleFactor, mScaleFactor, detector.FocusX, detector.FocusY);

            FixTrans();

            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            _mode = Zoom;

            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
            // TODO : WAT DAN?
        }

        #endregion Scale
    }
}