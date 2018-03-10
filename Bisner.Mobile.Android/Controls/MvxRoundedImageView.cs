using System;
using System.Linq;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Util;
using Bisner.Mobile.Droid.Controls.Bindings;
using MvvmCross.Binding.Droid.Views;

namespace Bisner.Mobile.Droid.Controls
{
    public class MvxRoundedImageView : MvxImageView
    {
        private const int TileModeUndefined = -2;
        private const int TileModeClamp = 0;
        private const int TileModeRepeat = 1;
        private const int TileModeMirror = 2;

        public new const string Tag = "RoundedImageView";
        public const float DefaultRadius = 0f;
        public const float DefaultBorderWidth = 0f;
        public static readonly Shader.TileMode DefaultTileMode = Shader.TileMode.Clamp;

        private static readonly ScaleType[] ScaleTypes =
        {
            ScaleType.Matrix,
            ScaleType.FitXy,
            ScaleType.FitStart,
            ScaleType.FitCenter,
            ScaleType.FitEnd,
            ScaleType.Center,
            ScaleType.CenterCrop,
            ScaleType.CenterInside
        };

        private readonly float[] _cornerRadii = { DefaultRadius, DefaultRadius, DefaultRadius, DefaultRadius };

        private Drawable _backgroundDrawable;
        private ColorStateList _borderColor = ColorStateList.ValueOf(RoundedDrawable.DEFAULT_BORDER_COLOR);
        private float _borderWidth = DefaultBorderWidth;
        private ColorFilter _colorFilter;
        private bool _colorMod;
        private Drawable _drawable;
        private bool _hasColorFilter;
        private bool _isOval;
        private bool _mutateBackground;
        private int _resource;
        private ScaleType _scaleType = ScaleType.FitCenter;
        private Shader.TileMode _tileModeX = DefaultTileMode;
        private Shader.TileMode _tileModeY = DefaultTileMode;

        public MvxRoundedImageView(Context context) : base(context)
        {
        }

        public MvxRoundedImageView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            var a = context.ObtainStyledAttributes(attrs, Resource.Styleable.RoundedImageView, 0, 0);

            var index = a.GetInt(Resource.Styleable.RoundedImageView_android_scaleType, -1);

            SetScaleType(index >= 0 ? ScaleTypes[index] : ScaleType.FitCenter);

            float cornerRadiusOverride = a.GetDimensionPixelSize(Resource.Styleable.RoundedImageView_CornerRadius, -1);

            _cornerRadii[Corner.TopLeft] = a.GetDimensionPixelSize(Resource.Styleable.RoundedImageView_CornerRadiusTopLeft, -1);
            _cornerRadii[Corner.TopRight] = a.GetDimensionPixelSize(Resource.Styleable.RoundedImageView_CornerRadiusTopRight, -1);
            _cornerRadii[Corner.BottomLeft] = a.GetDimensionPixelSize(Resource.Styleable.RoundedImageView_CornerRadiusBottomLeft, -1);
            _cornerRadii[Corner.BottomRight] = a.GetDimensionPixelSize(Resource.Styleable.RoundedImageView_CornerRadiusBottomRight, -1);

            var any = false;

            for (int i = 0, len = _cornerRadii.Length; i < len; i++)
            {
                if (_cornerRadii[i] < 0)
                {
                    _cornerRadii[i] = 0f;
                }
                else
                {
                    any = true;
                }
            }

            if (!any)
            {
                if (cornerRadiusOverride < 0)
                {
                    cornerRadiusOverride = DefaultRadius;
                }
                for (int i = 0, len = _cornerRadii.Length; i < len; i++)
                {
                    _cornerRadii[i] = cornerRadiusOverride;
                }
            }

            _borderWidth = a.GetDimensionPixelSize(Resource.Styleable.RoundedImageView_BorderWidth, -1);
            if (_borderWidth < 0)
            {
                _borderWidth = DefaultBorderWidth;
            }

            _borderColor = a.GetColorStateList(Resource.Styleable.RoundedImageView_BorderColor) ?? ColorStateList.ValueOf(RoundedDrawable.DEFAULT_BORDER_COLOR);

            _mutateBackground = a.GetBoolean(Resource.Styleable.RoundedImageView_MutateBackground, false);
            _isOval = a.GetBoolean(Resource.Styleable.RoundedImageView_Oval, false);

            var tileMode = a.GetInt(Resource.Styleable.RoundedImageView_TileMode, TileModeUndefined);
            if (tileMode != TileModeUndefined)
            {
                TileModeX = ParseTileMode(tileMode);
                TileModeY = ParseTileMode(tileMode);
            }

            var tileModeX = a.GetInt(Resource.Styleable.RoundedImageView_TileModeX, TileModeUndefined);
            if (tileModeX != TileModeUndefined)
            {
                TileModeX = ParseTileMode(tileModeX);
            }

            var tileModeY = a.GetInt(Resource.Styleable.RoundedImageView_TileModeY, TileModeUndefined);
            if (tileModeY != TileModeUndefined)
            {
                TileModeY = ParseTileMode(tileModeY);
            }

            UpdateDrawableAttrs();

            UpdateBackgroundDrawableAttrs(true);

            a.Recycle();
        }

        protected MvxRoundedImageView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private static Shader.TileMode ParseTileMode(int tileMode)
        {
            switch (tileMode)
            {
                case TileModeClamp:
                    return Shader.TileMode.Clamp;
                case TileModeRepeat:
                    return Shader.TileMode.Repeat;
                case TileModeMirror:
                    return Shader.TileMode.Mirror;
                default:
                    return null;
            }
        }

        protected override void DrawableStateChanged()
        {
            base.DrawableStateChanged();

            Invalidate();
        }

        public override ScaleType GetScaleType()
        {
            return _scaleType;
        }

        public override void SetScaleType(ScaleType scaleType)
        {
            if (scaleType == null) return;

            if (_scaleType == scaleType) return;

            _scaleType = scaleType;

            if (scaleType == ScaleType.FitXy ||
                scaleType == ScaleType.Center ||
                scaleType == ScaleType.FitEnd ||
                scaleType == ScaleType.FitStart ||
                scaleType == ScaleType.FitCenter ||
                scaleType == ScaleType.CenterCrop ||
                scaleType == ScaleType.CenterInside)
            {
                base.SetScaleType(ScaleType.FitXy);
            }
            else
            {
                base.SetScaleType(scaleType);
            }

            UpdateDrawableAttrs();
            UpdateBackgroundDrawableAttrs(false);
            Invalidate();
        }

        public override void SetImageDrawable(Drawable drawable)
        {
            _resource = 0;
            _drawable = RoundedDrawable.FromDrawable(drawable);
            UpdateDrawableAttrs();

            base.SetImageDrawable(drawable);
        }

        public override void SetImageBitmap(Bitmap bm)
        {
            _resource = 0;
            _drawable = RoundedDrawable.FromBitmap(bm);
            UpdateDrawableAttrs();

            base.SetImageDrawable(_drawable);
        }

        public override void SetImageResource(int resId)
        {
            if (_resource == resId) return;

            _resource = resId;
            _drawable = ResolveResource();
            UpdateDrawableAttrs();

            base.SetImageDrawable(_drawable);
        }

        public override void SetImageURI(Android.Net.Uri uri)
        {
            base.SetImageURI(uri);

            SetImageDrawable(Drawable);
        }

        private Drawable ResolveResource()
        {
            var rsrc = Resources;

            if (rsrc == null) return null;

            if (_resource == 0) return null;

            try
            {
                var d = ContextCompat.GetDrawable(Context, _resource);
                return RoundedDrawable.FromDrawable(d);
            }
            catch (Exception e)
            {
                Log.Warn(Tag, "Unable to find resource: " + _resource, e);
                // Don't try again.
                _resource = 0;
            }

            return null;
        }

        private void UpdateDrawableAttrs()
        {
            UpdateAttrs(_drawable);
        }

        private void UpdateBackgroundDrawableAttrs(bool convert)
        {
            if (!_mutateBackground) return;

            if (convert)
            {
                _backgroundDrawable = RoundedDrawable.FromDrawable(_backgroundDrawable);
            }

            UpdateAttrs(_backgroundDrawable);
        }

        public override void SetColorFilter(ColorFilter cf)
        {
            if (_colorFilter == cf) return;

            _colorFilter = cf;
            _hasColorFilter = true;
            _colorMod = true;
            ApplyColorMod();
            Invalidate();
        }

        private void ApplyColorMod()
        {
            // Only mutate and apply when modifications have occurred. This should
            // not reset the _colorMod flag, since these filters need to be
            // re-applied if the Drawable is changed.
            if (_drawable == null || !_colorMod) return;

            _drawable = _drawable.Mutate();

            if (_hasColorFilter)
            {
                _drawable.SetColorFilter(_colorFilter);
            }
        }

        private void UpdateAttrs(Drawable drawable)
        {
            if (drawable == null) { return; }

            if (drawable is RoundedDrawable)
            {
                var roundedDrawable = drawable as RoundedDrawable;
                roundedDrawable.ScaleType = _scaleType;
                roundedDrawable.BorderWidth = _borderWidth;
                roundedDrawable.BorderColors = _borderColor;
                roundedDrawable.IsOval = _isOval;
                roundedDrawable.TileModeX = _tileModeX;
                roundedDrawable.TileModeY = _tileModeY;

                if (_cornerRadii != null)
                {
                    roundedDrawable.SetCornerRadius(
                        _cornerRadii[Corner.TopLeft],
                        _cornerRadii[Corner.TopRight],
                        _cornerRadii[Corner.BottomRight],
                        _cornerRadii[Corner.BottomLeft]);
                }

                ApplyColorMod();
            }
            else if (drawable is LayerDrawable)
            {
                // loop through layers to and set drawable attrs
                var ld = drawable as LayerDrawable;
                for (int i = 0, layers = ld.NumberOfLayers; i < layers; i++)
                {
                    UpdateAttrs(ld.GetDrawable(i));
                }
            }
        }

        [System.Obsolete]
        public override void SetBackgroundDrawable(Drawable background)
        {
            _backgroundDrawable = background;
            UpdateBackgroundDrawableAttrs(true);
            base.SetBackgroundDrawable(_backgroundDrawable);
        }

        /**
         * @return the largest corner radius.
         */
        public float CornerRadius
        {
            get
            {
                return MaxCornerRadius;
            }
        }

        /**
         * @return the largest corner radius.
         */
        public float MaxCornerRadius => _cornerRadii.Concat(new[] { 0f }).Max();

        /**
         * Get the corner radius of a specified corner.
         *
         * @param corner the corner.
         * @return the radius.
         */
        public float GetCornerRadius(int corner)
        {
            return _cornerRadii[corner];
        }

        /**
         * Set all the corner radii from a dimension resource id.
         *
         * @param resId dimension resource id of radii.
         */
        public void SetCornerRadiusDimen(int resId)
        {
            var radius = Resources.GetDimension(resId);
            SetCornerRadius(radius, radius, radius, radius);
        }

        /**
         * Set the corner radius of a specific corner from a dimension resource id.
         *
         * @param corner the corner to set.
         * @param resId the dimension resource id of the corner radius.
         */
        public void SetCornerRadiusDimen(int corner, int resId)
        {
            SetCornerRadius(corner, Resources.GetDimensionPixelSize(resId));
        }

        /**
         * Set the corner radii of all corners in px.
         *
         * @param radius the radius to set.
         */
        public void SetCornerRadius(float radius)
        {
            SetCornerRadius(radius, radius, radius, radius);
        }

        /**
         * Set the corner radius of a specific corner in px.
         *
         * @param corner the corner to set.
         * @param radius the corner radius to set in px.
         */
        public void SetCornerRadius(int corner, float radius)
        {
            if (_cornerRadii[corner] != radius)
            {
                _cornerRadii[corner] = radius;

                UpdateDrawableAttrs();
                UpdateBackgroundDrawableAttrs(false);
                Invalidate();
            }
        }

        /**
         * Set the corner radii of each corner individually. Currently only one unique nonzero value is
         * supported.
         *
         * @param topLeft radius of the top left corner in px.
         * @param topRight radius of the top right corner in px.
         * @param bottomRight radius of the bottom right corner in px.
         * @param bottomLeft radius of the bottom left corner in px.
         */
        public void SetCornerRadius(float topLeft, float topRight, float bottomLeft, float bottomRight)
        {
            if (_cornerRadii[Corner.TopLeft] == topLeft
                && _cornerRadii[Corner.TopRight] == topRight
                && _cornerRadii[Corner.BottomRight] == bottomRight
                && _cornerRadii[Corner.BottomLeft] == bottomLeft)
            {
                return;
            }

            _cornerRadii[Corner.TopLeft] = topLeft;
            _cornerRadii[Corner.TopRight] = topRight;
            _cornerRadii[Corner.BottomLeft] = bottomLeft;
            _cornerRadii[Corner.BottomRight] = bottomRight;

            UpdateDrawableAttrs();
            UpdateBackgroundDrawableAttrs(false);
            Invalidate();
        }

        public float BorderWidth
        {
            get
            {
                return _borderWidth;
            }
            set
            {
                if (_borderWidth != value)
                {
                    _borderWidth = value;
                    UpdateDrawableAttrs();
                    UpdateBackgroundDrawableAttrs(false);
                    Invalidate();
                }
            }
        }

        public void SetBorderWidth(int resId)
        {
            BorderWidth = Resources.GetDimension(resId);
        }

        public int BorderColor
        {
            get
            {
                return _borderColor.DefaultColor;
            }
            set
            {
                BorderColors = ColorStateList.ValueOf(new Color(value));
            }
        }

        public ColorStateList BorderColors
        {
            get
            {
                return _borderColor;
            }
            set
            {
                if (_borderColor.Equals(value)) return;

                _borderColor = value ?? ColorStateList.ValueOf(RoundedDrawable.DEFAULT_BORDER_COLOR);

                UpdateDrawableAttrs();

                UpdateBackgroundDrawableAttrs(false);

                if (_borderWidth > 0)
                {
                    Invalidate();
                }
            }
        }

        public bool IsOval
        {
            get
            {
                return _isOval;
            }
            set
            {
                _isOval = value;
                UpdateDrawableAttrs();
                UpdateBackgroundDrawableAttrs(false);
                Invalidate();
            }
        }

        public Shader.TileMode TileModeX
        {
            get
            {
                return _tileModeX;
            }
            set
            {
                if (_tileModeX == value) return;

                _tileModeX = value;
                UpdateDrawableAttrs();
                UpdateBackgroundDrawableAttrs(false);
                Invalidate();
            }
        }

        public Shader.TileMode TileModeY
        {
            get
            {
                return _tileModeY;
            }
            set
            {
                if (_tileModeY == value) return;

                _tileModeY = value;
                UpdateDrawableAttrs();
                UpdateBackgroundDrawableAttrs(false);
                Invalidate();
            }
        }

        public bool MutatesBackground
        {
            get
            {
                return _mutateBackground;
            }
            set
            {
                if (_mutateBackground == value) return;

                _mutateBackground = value;
                UpdateBackgroundDrawableAttrs(true);
                Invalidate();
            }
        }
    }
}