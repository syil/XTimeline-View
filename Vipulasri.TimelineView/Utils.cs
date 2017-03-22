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
using Android.Content.Res;
using Android.Util;

namespace Vipulasri.TimelineView
{
    public static class Utils
    {
        public static int dpToPx(float dp, Context context)
        {
            return dpToPx(dp, context.Resources);
        }

        public static int dpToPx(float dp, Resources resources)
        {
            float px = TypedValue.ApplyDimension(ComplexUnitType.Dip, dp, resources.DisplayMetrics);

            return (int)px;
        }
    }
}