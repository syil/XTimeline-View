using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Util;
using Android.Content.Res;

namespace Vipulasri.TimelineView
{
    public class TimelineView : View
    {

        private Drawable mMarker;
        private Drawable mStartLine;
        private Drawable mEndLine;
        private int mMarkerSize;
        private int mLineSize;
        private int mLineOrientation;
        private int mLinePadding;
        private bool mMarkerInCenter;

        private Rect mBounds;
        private Context mContext;

        public TimelineView(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            mContext = context;

            init(attrs);
        }

        private void init(IAttributeSet attrs)
        {
            TypedArray typedArray = Context.ObtainStyledAttributes(attrs, Resource.Styleable.timeline_style);
            mMarker = typedArray.GetDrawable(Resource.Styleable.timeline_style_marker);
            mStartLine = typedArray.GetDrawable(Resource.Styleable.timeline_style_line);
            mEndLine = typedArray.GetDrawable(Resource.Styleable.timeline_style_line);
            mMarkerSize = typedArray.GetDimensionPixelSize(Resource.Styleable.timeline_style_markerSize, Utils.dpToPx(20, mContext));
            mLineSize = typedArray.GetDimensionPixelSize(Resource.Styleable.timeline_style_lineSize, Utils.dpToPx(2, mContext));
            mLineOrientation = typedArray.GetInt(Resource.Styleable.timeline_style_lineOrientation, 1);
            mLinePadding = typedArray.GetDimensionPixelSize(Resource.Styleable.timeline_style_linePadding, 0);
            mMarkerInCenter = typedArray.GetBoolean(Resource.Styleable.timeline_style_markerInCenter, true);
            typedArray.Recycle();

            if (mMarker == null)
            {
                mMarker = mContext.Resources.GetDrawable(Resource.Drawable.marker);
            }

            if (mStartLine == null && mEndLine == null)
            {
                mStartLine = new ColorDrawable(mContext.Resources.GetColor(Android.Resource.Color.DarkerGray));
                mEndLine = new ColorDrawable(mContext.Resources.GetColor(Android.Resource.Color.DarkerGray));
            }
        }
        
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            //Width measurements of the width and height and the inside view of child controls
            int w = mMarkerSize + PaddingLeft + PaddingRight;
            int h = mMarkerSize + PaddingTop + PaddingBottom;

            // Width and height to determine the final view through a systematic approach to decision-making
            int widthSize = ResolveSizeAndState(w, widthMeasureSpec, 0);
            int heightSize = ResolveSizeAndState(h, heightMeasureSpec, 0);

            SetMeasuredDimension(widthSize, heightSize);
            initDrawable();
        }
        
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            // When the view is displayed when the callback
            // Positioning Drawable coordinates, then draw
            initDrawable();
        }

        private void initDrawable()
        {
            int pLeft = PaddingLeft;
            int pRight = PaddingRight;
            int pTop = PaddingTop;
            int pBottom = PaddingBottom;

            int width = Width; // Width of current custom view
            int height = Height;

            int cWidth = width - pLeft - pRight;// Circle width
            int cHeight = height - pTop - pBottom;

            int markSize = Math.Min(mMarkerSize, Math.Min(cWidth, cHeight));

            if (mMarkerInCenter)
            { //Marker in center is true

                if (mMarker != null)
                {
                    mMarker.SetBounds((width / 2) - (markSize / 2), (height / 2) - (markSize / 2), (width / 2) + (markSize / 2), (height / 2) + (markSize / 2));
                    mBounds = mMarker.Bounds;
                }

            }
            else
            { //Marker in center is false

                if (mMarker != null)
                {
                    mMarker.SetBounds(pLeft, pTop, pLeft + markSize, pTop + markSize);
                    mBounds = mMarker.Bounds;
                }
            }

            int centerX = mBounds.CenterX();
            int lineLeft = centerX - (mLineSize >> 1);

            if (mLineOrientation == 0)
            {

                //Horizontal Line
                if (mStartLine != null)
                {
                    mStartLine.SetBounds(0, pTop + (mBounds.Height() / 2), mBounds.Left - mLinePadding, (mBounds.Height() / 2) + pTop + mLineSize);
                }

                if (mEndLine != null)
                {
                    mEndLine.SetBounds(mBounds.Right + mLinePadding, pTop + (mBounds.Height() / 2), width, (mBounds.Height() / 2) + pTop + mLineSize);
                }
            }
            else
            {

                //Vertical Line
                if (mStartLine != null)
                {
                    mStartLine.SetBounds(lineLeft, 0, mLineSize + lineLeft, mBounds.Top - mLinePadding);
                }

                if (mEndLine != null)
                {
                    mEndLine.SetBounds(lineLeft, mBounds.Bottom + mLinePadding, mLineSize + lineLeft, height);
                }
            }
        }
        
        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (mMarker != null)
            {
                mMarker.Draw(canvas);
            }

            if (mStartLine != null)
            {
                mStartLine.Draw(canvas);
            }

            if (mEndLine != null)
            {
                mEndLine.Draw(canvas);
            }
        }

        /// <summary>
        /// Sets marker.
        /// </summary>
        /// <param name="marker">will set marker drawable to timeline</param>
        public void setMarker(Drawable marker)
        {
            mMarker = marker;
            initDrawable();
        }
        
        /// <summary>
        /// Sets marker.
        /// </summary>
        /// <param name="marker">will set marker drawable to timeline</param>
        /// <param name="color">with a color</param>
        public void setMarker(Drawable marker, Color color)
        {
            mMarker = marker;
            mMarker.SetColorFilter(color, PorterDuff.Mode.Src);
            initDrawable();
        }
        
        /// <summary>
        /// Sets marker color.
        /// </summary>
        /// <param name="color">the color</param>
        public void setMarkerColor(Color color)
        {
            mMarker.SetColorFilter(color, PorterDuff.Mode.Src);
            initDrawable();
        }
        
        /// <summary>
        /// Sets start line.
        /// </summary>
        /// <param name="color">the color</param>
        /// <param name="viewType">the view type</param>
        public void setStartLine(Color color, LineType viewType)
        {
            mStartLine = new ColorDrawable(color);
            initLine(viewType);
        }
        
        /// <summary>
        /// Sets end line.
        /// </summary>
        /// <param name="color">the color</param>
        /// <param name="viewType"> the view type</param>
        public void setEndLine(Color color, LineType viewType)
        {
            mEndLine = new ColorDrawable(color);
            initLine(viewType);
        }
        
        /// <summary>
        /// Sets marker size.
        /// </summary>
        /// <param name="markerSize">the marker size</param>
        public void setMarkerSize(int markerSize)
        {
            mMarkerSize = markerSize;
            initDrawable();
        }
        
        /// <summary>
        /// Sets line size.
        /// </summary>
        /// <param name="lineSize">the line size</param>
        public void setLineSize(int lineSize)
        {
            mLineSize = lineSize;
            initDrawable();
        }
        
        /// <summary>
        /// Sets line padding
        /// </summary>
        /// <param name="padding">the line padding</param>
        public void setLinePadding(int padding)
        {
            mLinePadding = padding;
            initDrawable();
        }

        private void setStartLine(Drawable startLine)
        {
            mStartLine = startLine;
            initDrawable();
        }

        private void setEndLine(Drawable endLine)
        {
            mEndLine = endLine;
            initDrawable();
        }
        
        /// <summary>
        /// Init line.
        /// </summary>
        /// <param name="viewType">the view type</param>
        public void initLine(LineType viewType)
        {
            if (viewType == LineType.BEGIN)
            {
                setStartLine(null);
            }
            else if (viewType == LineType.END)
            {
                setEndLine(null);
            }
            else if (viewType == LineType.ONLYONE)
            {
                setStartLine(null);
                setEndLine(null);
            }
            initDrawable();
        }
        /// <summary>
        /// Gets timeline view type.
        /// </summary>
        /// <param name="position">the position</param>
        /// <param name="total_size">the total size</param>
        /// <returns>the time line view type</returns>
        public static LineType getTimeLineViewType(int position, int total_size)
        {
            if (total_size == 1)
            {
                return LineType.ONLYONE;
            }
            else if (position == 0)
            {
                return LineType.BEGIN;
            }
            else if (position == total_size - 1)
            {
                return LineType.END;
            }
            else
            {
                return LineType.NORMAL;
            }
        }
    }

}