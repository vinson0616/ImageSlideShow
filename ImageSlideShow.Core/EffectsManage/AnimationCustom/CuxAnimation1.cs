using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom
{
    internal class CuxAnimation1
    {
        public static void DefaultSetExtentValue(AnimationTimeline CurrentObject)
        {
            CurrentObject.AccelerationRatio = 0.1;
            CurrentObject.DecelerationRatio = 0.5;
            CurrentObject.SpeedRatio = 1;
        }

        #region WidthAnimation

        public static void WidthAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.WidthAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void WidthAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.WidthAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void WidthAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.WidthAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void WidthAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.WidthAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void WidthAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Item.Width = Ani.To.Value;
                        if (OnCompleted != null && Item.Width == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(FrameworkElement.WidthProperty, Ani);
        }

        #endregion

        #region HeightAnimation

        public static void HeightAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.HeightAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void HeightAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.HeightAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void HeightAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.HeightAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void HeightAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.HeightAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void HeightAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Item.Height = Ani.To.Value;
                        if (OnCompleted != null && Item.Height == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(FrameworkElement.HeightProperty, Ani);
        }

        #endregion

        #region MarginAnimation

        public static void MarginAnimation(FrameworkElement Item, Thickness ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.MarginAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void MarginAnimation(FrameworkElement Item, Thickness FromValue, Thickness ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.MarginAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void MarginAnimation(FrameworkElement Item, Thickness ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            ThicknessAnimation Ani = new ThicknessAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.MarginAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void MarginAnimation(FrameworkElement Item, Thickness FromValue, Thickness ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            ThicknessAnimation Ani = new ThicknessAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.MarginAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void MarginAnimation(FrameworkElement Item, ThicknessAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Item.Margin = Ani.To.Value;
                        if (OnCompleted != null && Item.Margin == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(FrameworkElement.MarginProperty, Ani);
        }

        #endregion

        #region OpacityAnimation

        public static void OpacityAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.OpacityAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void OpacityAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.OpacityAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void OpacityAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.OpacityAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void OpacityAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.OpacityAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void OpacityAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Item.Opacity = Ani.To.Value;
                        if (OnCompleted != null && Item.Opacity == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(FrameworkElement.OpacityProperty, Ani);
        }

        #endregion

        #region CanvasPositionAnimation

        #region CanvasLeftAnimation

        public static void CanvasLeftAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasLeftAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasLeftAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasLeftAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasLeftAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasLeftAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void CanvasLeftAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasLeftAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void CanvasLeftAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Canvas.SetLeft(Item, Ani.To.Value);
                        if (OnCompleted != null && Canvas.GetLeft(Item) == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(Canvas.LeftProperty, Ani);
        }

        #endregion

        #region CanvasTopAnimation

        public static void CanvasTopAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasTopAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasTopAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasTopAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasTopAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasTopAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void CanvasTopAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasTopAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void CanvasTopAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Canvas.SetTop(Item, Ani.To.Value);
                        if (OnCompleted != null && Canvas.GetTop(Item) == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(Canvas.TopProperty, Ani);
        }

        #endregion

        #region CanvasRightAnimation

        public static void CanvasRightAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasRightAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasRightAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasRightAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasRightAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasRightAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void CanvasRightAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasRightAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void CanvasRightAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Canvas.SetRight(Item, Ani.To.Value);
                        if (OnCompleted != null && Canvas.GetRight(Item) == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(Canvas.RightProperty, Ani);
        }

        #endregion

        #region CanvasBottomAnimation

        public static void CanvasBottomAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasBottomAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasBottomAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.CanvasBottomAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void CanvasBottomAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasBottomAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void CanvasBottomAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.CanvasBottomAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void CanvasBottomAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            if (SetExtentValue != null)
            {
                SetExtentValue(Ani);
            }
            Ani.Completed += new EventHandler(
                delegate(object sender, EventArgs e)
                {
                    if (!Ani.AutoReverse)
                    {
                        Canvas.SetBottom(Item, Ani.To.Value);
                        if (OnCompleted != null && Canvas.GetBottom(Item) == Ani.To.Value)
                        {
                            OnCompleted();
                        }
                    }
                    else
                    {
                        if (OnCompleted != null)
                        {
                            OnCompleted();
                        }
                    }
                });
            Item.BeginAnimation(Canvas.BottomProperty, Ani);
        }

        #endregion

        #endregion

        #region RenderTransform

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

        #region RTScaleXAnimation

        public static void RTScaleXAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTScaleXAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTScaleXAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTScaleXAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTScaleXAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTScaleXAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void RTScaleXAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTScaleXAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTScaleXAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            ScaleTransform Transform = CuxAnimation1.GetRTInstance<ScaleTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.ScaleX = Ani.To.Value;
                            if (OnCompleted != null && Transform.ScaleX == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(ScaleTransform.ScaleXProperty, Ani);
            }
        }

        #endregion

        #region RTScaleYAnimation

        public static void RTScaleYAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTScaleYAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTScaleYAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTScaleYAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTScaleYAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTScaleYAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void RTScaleYAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTScaleYAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTScaleYAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            ScaleTransform Transform = CuxAnimation1.GetRTInstance<ScaleTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.ScaleY = Ani.To.Value;
                            if (OnCompleted != null && Transform.ScaleY == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(ScaleTransform.ScaleYProperty, Ani);
            }
        }

        #endregion

        #region RTSkewXAnimation

        public static void RTSkewXAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTSkewXAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTSkewXAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTSkewXAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTSkewXAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTSkewXAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }

        public static void RTSkewXAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTSkewXAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTSkewXAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            SkewTransform Transform = CuxAnimation1.GetRTInstance<SkewTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.AngleX = Ani.To.Value;
                            if (OnCompleted != null && Transform.AngleX == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(SkewTransform.AngleXProperty, Ani);
            }
        }

        #endregion

        #region RTSkewYAnimation

        public static void RTSkewYAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTSkewYAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTSkewYAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTSkewYAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTSkewYAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTSkewYAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void RTSkewYAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTSkewYAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTSkewYAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            SkewTransform Transform = CuxAnimation1.GetRTInstance<SkewTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.AngleY = Ani.To.Value;
                            if (OnCompleted != null && Transform.AngleY == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(SkewTransform.AngleYProperty, Ani);
            }
        }

        #endregion

        #region RTRotateAnimation

        public static void RTRotateAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTRotateAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTRotateAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTRotateAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTRotateAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTRotateAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void RTRotateAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTRotateAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTRotateAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            RotateTransform Transform = CuxAnimation1.GetRTInstance<RotateTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.Angle = Ani.To.Value;
                            if (OnCompleted != null && Transform.Angle == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(RotateTransform.AngleProperty, Ani);
            }
        }

        #endregion

        #region RTTranslateXAnimation

        public static void RTTranslateXAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTTranslateXAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTTranslateXAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTTranslateXAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTTranslateXAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTTranslateXAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void RTTranslateXAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTTranslateXAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTTranslateXAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            TranslateTransform Transform = CuxAnimation1.GetRTInstance<TranslateTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.X = Ani.To.Value;
                            if (OnCompleted != null && Transform.X == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(TranslateTransform.XProperty, Ani);
            }
        }

        #endregion

        #region RTTranslateYAnimation

        public static void RTTranslateYAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTTranslateYAnimation(Item, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTTranslateYAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted)
        {
            CuxAnimation1.RTTranslateYAnimation(Item, FromValue, ToValue, Second, OnCompleted, DefaultSetExtentValue);
        }
        public static void RTTranslateYAnimation(FrameworkElement Item, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTTranslateYAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        public static void RTTranslateYAnimation(FrameworkElement Item, double FromValue, double ToValue, double Second, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            DoubleAnimation Ani = new DoubleAnimation(FromValue, ToValue, new Duration(TimeSpan.FromSeconds(Second)), FillBehavior.Stop);
            CuxAnimation1.RTTranslateYAnimation(Item, Ani, OnCompleted, SetExtentValue);
        }
        private static void RTTranslateYAnimation(FrameworkElement Item, DoubleAnimation Ani, Action OnCompleted, SetExtentAnimationTimelineDelegate SetExtentValue)
        {
            TranslateTransform Transform = CuxAnimation1.GetRTInstance<TranslateTransform>(Item);
            if (Transform != null)
            {
                if (SetExtentValue != null)
                {
                    SetExtentValue(Ani);
                }
                Ani.Completed += new EventHandler(
                    delegate(object sender, EventArgs e)
                    {
                        if (!Ani.AutoReverse)
                        {
                            Transform.Y = Ani.To.Value;
                            if (OnCompleted != null && Transform.Y == Ani.To.Value)
                            {
                                OnCompleted();
                            }
                        }
                        else
                        {
                            if (OnCompleted != null)
                            {
                                OnCompleted();
                            }
                        }
                    });
                Transform.BeginAnimation(TranslateTransform.YProperty, Ani);
            }
        }

        #endregion

        #endregion
    }
}
