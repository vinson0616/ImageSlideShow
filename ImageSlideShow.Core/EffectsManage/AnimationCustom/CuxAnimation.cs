using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom
{
    internal class CuxAnimation
    {
        #region Double

        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double Second)
        {
            CuxAnimation.CuxDoubleAnimation(Item, dp, ToValue, Second, null, null);
        }
        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double Second, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimation.CuxDoubleAnimation(Item, dp, ToValue, Second, null, SetExtentValue);
        }
        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation.CuxDoubleAnimation(Item, dp, ToValue, Second, OnCompleted, null);
        }
        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)));
            CuxExtentCommon.BeginAnimation(Item, dp, Ani, null, ToValue, OnCompleted, SetExtentValue);
        }

        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double Second)
        {
            CuxAnimation.CuxDoubleAnimation(Item, dp, FromValue, ToValue, Second, null, null);
        }
        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double Second, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimation.CuxDoubleAnimation(Item, dp, FromValue, ToValue, Second, null, SetExtentValue);
        }
        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation.CuxDoubleAnimation(Item, dp, FromValue, ToValue, Second, OnCompleted, null);
        }
        public static void CuxDoubleAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)));
            CuxExtentCommon.BeginAnimation(Item, dp, Ani, FromValue, ToValue, OnCompleted, SetExtentValue);
        }

        #endregion

        #region Thickness

        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double Second)
        {
            CuxAnimation.CuxThicknessAnimation(Item, dp, ToValue, Second, null, null);
        }
        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double Second, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimation.CuxThicknessAnimation(Item, dp, ToValue, Second, null, SetExtentValue);
        }
        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation.CuxThicknessAnimation(Item, dp, ToValue, Second, OnCompleted, null);
        }
        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            ThicknessAnimation Ani = new ThicknessAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)));
            CuxExtentCommon.BeginAnimation(Item, dp, Ani, null, ToValue, OnCompleted, SetExtentValue);
        }

        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double Second)
        {
            CuxAnimation.CuxThicknessAnimation(Item, dp, FromValue, ToValue, Second, null, null);
        }
        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double Second, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimation.CuxThicknessAnimation(Item, dp, FromValue, ToValue, Second, null, SetExtentValue);
        }
        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation.CuxThicknessAnimation(Item, dp, FromValue, ToValue, Second, OnCompleted, null);
        }
        public static void CuxThicknessAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            ThicknessAnimation Ani = new ThicknessAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)));
            CuxExtentCommon.BeginAnimation(Item, dp, Ani, FromValue, ToValue, OnCompleted, SetExtentValue);
        }

        #endregion

        #region Transform

        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double ToValue, double Second) where T : Transform, new()
        {
            CuxAnimation.CuxTransformAnimation<T>(Item, dp, ToValue, Second, null, null);
        }
        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double ToValue, double Second, SetExtentAnimationTimelineDelegate SetExtentValue) where T : Transform, new()
        {
            CuxAnimation.CuxTransformAnimation<T>(Item, dp, ToValue, Second, null, SetExtentValue);
        }
        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double ToValue, double Second, Action OnCompleted) where T : Transform, new()
        {
            CuxAnimation.CuxTransformAnimation<T>(Item, dp, ToValue, Second, OnCompleted, null);
        }
        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where T : Transform, new()
        {
            T Transform = CuxExtentCommon.GetRTInstance<T>(Item);
            if (Transform != null)
            {
                CuxAnimation.CuxDoubleAnimation(Transform, dp, ToValue, Second, OnCompleted, SetExtentValue);
            }
        }

        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double Second) where T : Transform, new()
        {
            CuxAnimation.CuxTransformAnimation<T>(Item, dp, FromValue, ToValue, Second, null, null);
        }
        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double Second, SetExtentAnimationTimelineDelegate SetExtentValue) where T : Transform, new()
        {
            CuxAnimation.CuxTransformAnimation<T>(Item, dp, FromValue, ToValue, Second, null, SetExtentValue);
        }
        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double Second, Action OnCompleted) where T : Transform, new()
        {
            CuxAnimation.CuxTransformAnimation<T>(Item, dp, FromValue, ToValue, Second, OnCompleted, null);
        }
        public static void CuxTransformAnimation<T>(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where T : Transform, new()
        {
            T Transform = CuxExtentCommon.GetRTInstance<T>(Item);
            if (Transform != null)
            {
                CuxAnimation.CuxDoubleAnimation(Transform, dp, FromValue, ToValue, Second, OnCompleted, SetExtentValue);
            }
        }

        #endregion
    }
}