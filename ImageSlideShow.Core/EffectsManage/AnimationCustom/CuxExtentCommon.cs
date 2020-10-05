using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom
{
    internal delegate void SetExtentAnimationTimelineDelegate(AnimationTimeline CurrentObject);

    internal class CuxExtentCommon
    {
        public static void BeginAnimation(IAnimatable Item, DependencyProperty dp, AnimationTimeline ATL, object FromValue, object ToValue, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (ToValue == null)
            {
                return;
            }

            if (Item != null && Item is DependencyObject)
            {
                ATL.FillBehavior = FillBehavior.HoldEnd;

                if (SetExtentValue != null)
                {
                    SetExtentValue(ATL);
                }

                FillBehavior StatusValue = ATL.FillBehavior;
                ATL.FillBehavior = FillBehavior.Stop;

                DependencyObject ConvertItem = Item as DependencyObject;
                object TargetValue = null;
                if (StatusValue == FillBehavior.Stop || ATL.AutoReverse)
                {
                    if (FromValue == null)
                    {
                        TargetValue = ConvertItem.GetValue(dp);
                    }
                    else
                    {
                        TargetValue = FromValue;
                    }
                }
                else
                {
                    TargetValue = ToValue;
                }

                ATL.Completed += new EventHandler(
                        delegate(object sender, EventArgs e)
                        {
                            ConvertItem.SetValue(dp, TargetValue);
                            object CurValue = ConvertItem.GetValue(dp);
                            if (TargetValue != null && !TargetValue.Equals(CurValue))
                            {
                                return;
                            }

                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        });
                Item.BeginAnimation(dp, ATL, HandoffBehavior.SnapshotAndReplace);
            }
        }

        public static T GetRTInstance<T>(FrameworkElement Item) where T : Transform, new()
        {
            TransformGroup TG = null;
            if (Item.RenderTransform is TransformGroup)
            {
                TG = (TransformGroup)Item.RenderTransform;
            }
            else
            {
                TG = new TransformGroup();
                Item.RenderTransform = TG;
            }

            T Result = null;
            foreach (Transform Obj in TG.Children)
            {
                if (Obj is T)
                {
                    Result = (T)Obj;
                }
            }
            if (Result == null)
            {
                Result = new T();
                TG.Children.Add(Result);
            }

            return Result;
        }

        public static void GetValues(IKeyFrameAnimation KeyFrameAnimation, out object FromValue, out object ToValue)
        {
            FromValue = ToValue = null;

            if (KeyFrameAnimation != null)
            {
                double OldTimeSpan = 0;
                foreach (IKeyFrame KeyFrame in KeyFrameAnimation.KeyFrames)
                {
                    double CurTimeSpan = KeyFrame.KeyTime.TimeSpan.TotalSeconds;
                    if (CurTimeSpan <= 0)
                    {
                        FromValue = KeyFrame.Value;
                    }

                    if (CurTimeSpan >= OldTimeSpan)
                    {
                        ToValue = KeyFrame.Value;
                        OldTimeSpan = CurTimeSpan;
                    }
                }
            }
        }
    }
}
