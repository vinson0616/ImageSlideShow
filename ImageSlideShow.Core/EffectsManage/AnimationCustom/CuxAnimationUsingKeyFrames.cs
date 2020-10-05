using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom
{
    #region Base Class

    internal abstract class CuxAnimationUsingKeyFramesBase<T, S> where T : AnimationTimeline, IKeyFrameAnimation, new()
    {
        private T m_AniUKF = null;
        public CuxAnimationUsingKeyFramesBase()
        {
            m_AniUKF = new T();
        }

        #region BeginAnimation

        public void BeginAnimation(IAnimatable Item, DependencyProperty dp)
        {
            this.BeginAnimation(Item, dp, null, null);
        }
        public void BeginAnimation(IAnimatable Item, DependencyProperty dp, Action OnCompleted)
        {
            this.BeginAnimation(Item, dp, OnCompleted, null);
        }
        public void BeginAnimation(IAnimatable Item, DependencyProperty dp, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            this.BeginAnimation(Item, dp, null, SetExtentValue);
        }
        public void BeginAnimation(IAnimatable Item, DependencyProperty dp, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            object FromValue, ToValue;
            CuxExtentCommon.GetValues(m_AniUKF, out FromValue, out ToValue);
            CuxExtentCommon.BeginAnimation(Item, dp, m_AniUKF, FromValue, ToValue, OnCompleted, SetExtentValue);
        }

        #endregion

        #region AddKeyFrame

        protected void AddKeyFrame(object KeyFrame)
        {
            if (m_AniUKF != null && KeyFrame != null)
            {
                m_AniUKF.KeyFrames.Add(KeyFrame);
            }
        }
        public abstract void AddLinearKeyFrame(S Value, double SecondTime);
        public abstract void AddDiscreteKeyFrame(S Value, double SecondTime);
        public virtual void AddSplineKeyFrame(S Value, double SecondTime)
        {
            //x1是先慢后快，y1是先快后慢
            this.AddSplineKeyFrame(Value, SecondTime, 0, 0.7, 1, 1);
        }
        public abstract void AddSplineKeyFrame(S Value, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2);

        #endregion

        #region Static CreateAnimation

        #region CreateLinearAnimation

        protected static void CreateLinearAnimation<K>(IAnimatable Item, DependencyProperty dp, S ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddLinearKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        protected static void CreateLinearAnimation<K>(IAnimatable Item, DependencyProperty dp, S FromValue, S ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddLinearKeyFrame(FromValue, 0);
            animation.AddLinearKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateDiscreteAnimation

        protected static void CreateDiscreteAnimation<K>(IAnimatable Item, DependencyProperty dp, S ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddDiscreteKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        protected static void CreateDiscreteAnimation<K>(IAnimatable Item, DependencyProperty dp, S FromValue, S ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddDiscreteKeyFrame(FromValue, 0);
            animation.AddDiscreteKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateSplineAnimation

        protected static void CreateSplineAnimation<K>(IAnimatable Item, DependencyProperty dp, S ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddSplineKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        protected static void CreateSplineAnimation<K>(IAnimatable Item, DependencyProperty dp, S FromValue, S ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddSplineKeyFrame(FromValue, 0);
            animation.AddSplineKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        protected static void CreateSplineAnimation<K>(IAnimatable Item, DependencyProperty dp, S ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddSplineKeyFrame(ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        protected static void CreateSplineAnimation<K>(IAnimatable Item, DependencyProperty dp, S FromValue, S ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue) where K : CuxAnimationUsingKeyFramesBase<T, S>, new()
        {
            K animation = new K();
            animation.AddSplineKeyFrame(FromValue, 0, SplineX1, SplineY1, SplineX2, SplineY2);
            animation.AddSplineKeyFrame(ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        #endregion

        #endregion
    }

    #endregion

    #region Double

    internal class CuxDoubleAnimationUsingKeyFrames : CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>
    {
        #region AddKeyFrame

        public override void AddLinearKeyFrame(double Value, double SecondTime)
        {
            base.AddKeyFrame(new LinearDoubleKeyFrame(Value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SecondTime))));
        }

        public override void AddDiscreteKeyFrame(double Value, double SecondTime)
        {
            base.AddKeyFrame(new DiscreteDoubleKeyFrame(Value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SecondTime))));
        }

        public override void AddSplineKeyFrame(double Value, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2)
        {
            base.AddKeyFrame(new SplineDoubleKeyFrame(Value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SecondTime)), new KeySpline(SplineX1, SplineY1, SplineX2, SplineY2)));
        }

        #endregion

        #region CreateLinearAnimation

        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateLinearAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateLinearAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateDiscreteAnimation

        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateDiscreteAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateDiscreteAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateSplineAnimation

        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateSplineAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateSplineAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2, OnCompleted, SetExtentValue);
        }

        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateSplineAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<DoubleAnimationUsingKeyFrames, double>.CreateSplineAnimation<CuxDoubleAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2, OnCompleted, SetExtentValue);
        }

        #endregion
    }

    #endregion

    #region Thickness

    internal class CuxThicknessAnimationUsingKeyFrames : CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>
    {
        #region AddKeyFrame

        public override void AddLinearKeyFrame(Thickness Value, double SecondTime)
        {
            base.AddKeyFrame(new LinearThicknessKeyFrame(Value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SecondTime))));
        }

        public override void AddDiscreteKeyFrame(Thickness Value, double SecondTime)
        {
            base.AddKeyFrame(new DiscreteThicknessKeyFrame(Value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SecondTime))));
        }

        public override void AddSplineKeyFrame(Thickness Value, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2)
        {
            base.AddKeyFrame(new SplineThicknessKeyFrame(Value, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(SecondTime)), new KeySpline(SplineX1, SplineY1, SplineX2, SplineY2)));
        }

        #endregion

        #region CreateLinearAnimation

        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, Action OnCompleted)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateLinearAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, Action OnCompleted)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateLinearAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateLinearAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateDiscreteAnimation

        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, Action OnCompleted)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateDiscreteAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, Action OnCompleted)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateDiscreteAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateDiscreteAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateSplineAnimation

        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, Action OnCompleted)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateSplineAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateSplineAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2, OnCompleted, SetExtentValue);
        }

        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, Action OnCompleted)
        {
            CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateSplineAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, SetExtentValue);
        }
        public static void CreateSplineAnimation(IAnimatable Item, DependencyProperty dp, Thickness FromValue, Thickness ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxAnimationUsingKeyFramesBase<ThicknessAnimationUsingKeyFrames, Thickness>.CreateSplineAnimation<CuxThicknessAnimationUsingKeyFrames>(Item, dp, FromValue, ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2, OnCompleted, SetExtentValue);
        }

        #endregion
    }

    #endregion

    #region Transform

    internal class CuxTransformAnimationUsingKeyFrames<T> : CuxDoubleAnimationUsingKeyFrames where T : Transform, new()
    {
        #region BeginAnimation

        public void BeginAnimation(FrameworkElement Item, DependencyProperty dp)
        {
            this.BeginAnimation(Item, dp, null, null);
        }
        public void BeginAnimation(FrameworkElement Item, DependencyProperty dp, Action OnCompleted)
        {
            this.BeginAnimation(Item, dp, OnCompleted, null);
        }
        public void BeginAnimation(FrameworkElement Item, DependencyProperty dp, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            this.BeginAnimation(Item, dp, null, SetExtentValue);
        }
        public void BeginAnimation(FrameworkElement Item, DependencyProperty dp, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            T Transform = CuxExtentCommon.GetRTInstance<T>(Item);
            if (Transform != null)
            {
                base.BeginAnimation(Transform, dp, OnCompleted, SetExtentValue);
            }
        }

        #endregion

        #region CreateLinearAnimation

        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateLinearAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateLinearAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateLinearAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddLinearKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateLinearAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateLinearAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddLinearKeyFrame(FromValue, 0);
            animation.AddLinearKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateDiscreteAnimation

        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateDiscreteAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddDiscreteKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateDiscreteAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateDiscreteAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddDiscreteKeyFrame(FromValue, 0);
            animation.AddDiscreteKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        #endregion

        #region CreateDiscreteAnimation

        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateSplineAnimation(Item, dp, ToValue, SecondTime, null, null);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateSplineAnimation(Item, dp, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateSplineAnimation(Item, dp, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddSplineKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddSplineKeyFrame(ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, null, null);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, null, SetExtentValue);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted)
        {
            CuxTransformAnimationUsingKeyFrames<T>.CreateSplineAnimation(Item, dp, FromValue, ToValue, SecondTime, OnCompleted, null);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddSplineKeyFrame(FromValue, 0);
            animation.AddSplineKeyFrame(ToValue, SecondTime);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }
        public static void CreateSplineAnimation(FrameworkElement Item, DependencyProperty dp, double FromValue, double ToValue, double SecondTime, double SplineX1, double SplineY1, double SplineX2, double SplineY2, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            CuxTransformAnimationUsingKeyFrames<T> animation = new CuxTransformAnimationUsingKeyFrames<T>();
            animation.AddSplineKeyFrame(FromValue, 0, SplineX1, SplineY1, SplineX2, SplineY2);
            animation.AddSplineKeyFrame(ToValue, SecondTime, SplineX1, SplineY1, SplineX2, SplineY2);
            animation.BeginAnimation(Item, dp, OnCompleted, SetExtentValue);
        }

        #endregion
    }

    #endregion
}