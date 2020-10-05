using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Media.Animation;
using System.IO;
using ImageSlideShow.Core.EffectsManage.AnimationCustom;
using ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models;
using System.Windows.Threading;
 

namespace ImageSlideShow.Core
{
    /// <summary>
    /// Interaction logic for CuxImage.xaml
    /// </summary>
    public partial class CuxImage : UserControl
    {
        #region Private parameters

        private double m_SizePositionX = 0d;
        private double m_SizePositionY = 0d;
        private DateTime m_SelectTime = DateTime.Now;
        private Point m_SelectPoint = new Point();

        /// <summary>
        /// when the manipulation rotate less than m_RotateOffset*m_RotateAngle and more than -m_RotateOffset*m_RotateAngle,
        /// the rotate of the RotateTransform back to the oragin place;
        /// when the manipulation rotate more than m_RotateOffset*m_RotateAngle the rotate of the RotateTransform add m_RotateAngle;
        /// when the manipulation rotate less than -m_RotateOffset*m_RotateAngle the rotate of the RotateTransform subtract m_RotateAngle;
        /// </summary>
        private double m_RotateOffset = 1 / 3.0;

        /// <summary>
        /// rotate angle when manipulation completed
        /// </summary>
        private double m_RotateAngle = 90;

        /// <summary>
        /// Max scale
        /// </summary>
        private double m_ScaleMax = 2;

      

        /// <summary>
        /// Min scale
        /// </summary>
        private double m_ScaleMin = 0.5;

        /// <summary>
        /// Scale to m_ScaleMax when Scale more than m_ScaleMaxOffset
        /// Scale to 1 when Scale less than m_ScaleMaxOffset and more than 0
        /// </summary>
        private double m_ScaleMaxOffset = 1.5;

        /// <summary>
        /// Scale to 1 when Scale less than m_ScaleMinOffset and more than 0
        /// Scale to m_ScaleMin when Scale more than m_ScaleMinOffset
        /// </summary>
        private double m_ScaleMinOffset = 0.25;

        private double m_MaxX = 0.0;

        private double m_MinX = 0.0;

        private double m_MaxY = 0.0;

        private double m_MinY = 0.0;

        private Timer m_ScaleTimer = null;
        private Timer m_ZoomModeTimer = null;

        private int TouchCount = 0;

        private bool m_ScaleFlag = false;

        public bool ScaleFlag
        {
            get
            {
                return m_ScaleFlag;
            }
            set
            {
                m_ScaleFlag = value;
                if (m_ScaleFlag)
                {
                    //gd.Visibility = System.Windows.Visibility.Visible;
                    RoutedEventArgs evnt = new RoutedEventArgs { RoutedEvent = SlideShow.DisableScrollEvent, Source = this };
                    this.RaiseEvent(evnt);
                }
                else
                {
                   // gd.Visibility = System.Windows.Visibility.Collapsed;
                    RoutedEventArgs evnt = new RoutedEventArgs { RoutedEvent = SlideShow.EnableScrollEvent, Source = this };
                    this.RaiseEvent(evnt);
                }
            }
        }

        private bool RotateFlag = false;

        private ManipulationDeltaEventArgs modeEventArgs;
        private object modeEventObject;

        //public RotateTransform RotateTrans
        //{
        //    get { return this.rotateTrans; }
        //}
        #endregion

        public CuxImage()
        {
            InitializeComponent();
            
           

            gridContainer.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
            m_ScaleTimer = new Timer(200);
            m_ScaleTimer.Elapsed += new ElapsedEventHandler(m_ScaleTimer_Elapsed);

            m_ZoomModeTimer = new Timer(200);
            m_ZoomModeTimer.Elapsed += new ElapsedEventHandler(m_ZoomModeTimer_Tick);
            m_ZoomModeTimer.Start();

            //image.SizeChanged += delegate
            //{
            //    rotateTrans.CenterY = image.ActualHeight / 2;
            //    rotateTrans.CenterX = image.ActualWidth / 2;
            //    scaleTrans.CenterX = image.ActualWidth / 2;
            //    scaleTrans.CenterY = image.ActualHeight / 2;
            //};
        }


        void m_ScaleTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action ac = delegate
            {
                if (!ScaleFlag)
                {
                    ScaleFlag = true;
                }
            };
            this.Dispatcher.Invoke(ac, null);
        }

        #region Events


        private void gridContainer_TouchUp(object sender, TouchEventArgs e)
        {
            if (TouchCount > 0)
            {
                TouchCount -= 1;
            }
        }

        private void touchPad_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            TouchCount += 1;
            DateTime dt = DateTime.Now;
            Point p = e.GetTouchPoint(this).Position;
            bool doubleClick = Math.Abs(p.X - m_SelectPoint.X) < 50 && Math.Abs(p.Y - m_SelectPoint.Y) < 50;
            TimeSpan ts = dt - m_SelectTime;
            if (ts.TotalMilliseconds <= 500 & ts.TotalMilliseconds >= 50 && doubleClick)
            {
                ////double click
                //Matrix matrix = ((MatrixTransform)this.image.RenderTransform).Matrix;
                //matrix.M11 = 1.0;
                //matrix.M12 = 0;
                //matrix.M21 = 0;
                //matrix.M22 = 1.0;
                //matrix.OffsetX = 0;
                //matrix.OffsetY = 0;
                //((MatrixTransform)this.image.RenderTransform).Matrix = matrix;

                if (this.image.RenderTransform.Value.M11 != 1 && this.image.RenderTransform.Value.M22 != 1)
                {
                    FrameworkElement element = (FrameworkElement)this.image;
                    double oldWidth = element.ActualWidth;
                    double oldHeight = element.ActualHeight;

                    double ratioY = oldHeight / oldWidth;
                    if (m_CuxAngle == -180 || m_CuxAngle == -360 || m_CuxAngle == 0)
                        ratioY = 1;
                    ScaleTransform s = new ScaleTransform(ratioY, ratioY, element.ActualWidth / 2, element.ActualHeight / 2);
                    RotateTransform rm = new RotateTransform(m_CuxAngle, element.ActualWidth / 2, element.ActualHeight / 2);
                    TransformGroup tg = new TransformGroup();
                    tg.Children.Add(s);
                    tg.Children.Add(rm);
                    element.RenderTransform = tg;
                   
                }
                ScaleFlag = false;
            }
            else
            {
                m_SelectTime = dt;
                m_SelectPoint = p;
            }
        }

        void m_ZoomModeTimer_Tick(object sender, ElapsedEventArgs e)
        {
            if (TouchCount >= 2)
            {
                if (modeEventArgs != null)
                {
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = dt - m_SelectTime;
                    if (ts.TotalMilliseconds >= 1000)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                            {
                                FrameworkElement element = (FrameworkElement)modeEventArgs.Source;
                                Matrix matrix = ((Transform)element.RenderTransform).Value;
                                ScaleFlag = true;
                                m_SizePositionX = (modeEventArgs.Manipulators.First().GetPosition(this.image).X + modeEventArgs.Manipulators.Last().GetPosition(this.image).X) / 2;
                                m_SizePositionY = (modeEventArgs.Manipulators.First().GetPosition(this.image).Y + modeEventArgs.Manipulators.Last().GetPosition(this.image).Y) / 2;
                                var deltaManipulation = modeEventArgs.DeltaManipulation;
                                Point center = new Point(m_SizePositionX, m_SizePositionY);
                                center = matrix.Transform(center);
                                matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                            }));
                    }
                }
            }
        }

        private void Image_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            e.ManipulationContainer = gridContainer;
            e.Handled = true;
        }

        private void Image_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            //if (e.Manipulators.Count() > 1)
            //{
                //if (!RotateFlag && e.DeltaManipulation.Rotation != 0)
                //{
                //    RotateFlag = true;
                //}

                //if (!ScaleFlag)
                //{
                //    //rotateTrans.Angle += e.DeltaManipulation.Rotation;
                //}
                //else
                //{
                //    double _NewScaleX = scaleTrans.ScaleX * e.DeltaManipulation.Scale.X;
                //    double _NewScaleY = scaleTrans.ScaleY * e.DeltaManipulation.Scale.Y;
                //    scaleTrans.ScaleX = _NewScaleX;
                //    scaleTrans.ScaleY = _NewScaleY;
                //}
                FrameworkElement element = (FrameworkElement)e.Source;
                Matrix matrix = ((Transform)element.RenderTransform).Value;
                modeEventArgs = e;
                if (e.Manipulators.Count() > 1)
                {
                    if (ScaleFlag)
                    {
                        m_SizePositionX = (e.Manipulators.First().GetPosition(this.image).X + e.Manipulators.Last().GetPosition(this.image).X) / 2;
                        m_SizePositionY = (e.Manipulators.First().GetPosition(this.image).Y + e.Manipulators.Last().GetPosition(this.image).Y) / 2;
                        var deltaManipulation = e.DeltaManipulation;
                        Point center = new Point(m_SizePositionX, m_SizePositionY);
                        center = matrix.Transform(center);
                        matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                    }
                    //DateTime dt = DateTime.Now;
                    //TimeSpan ts = dt - m_SelectTime;

                    //if (ScaleFlag || ts.TotalMilliseconds > 1000)
                    //{
                    //    ScaleFlag = true;
                    //    m_SizePositionX = (e.Manipulators.First().GetPosition(this.image).X + e.Manipulators.Last().GetPosition(this.image).X) / 2;
                    //    m_SizePositionY = (e.Manipulators.First().GetPosition(this.image).Y + e.Manipulators.Last().GetPosition(this.image).Y) / 2;
                    //    var deltaManipulation = e.DeltaManipulation;
                    //    Point center = new Point(m_SizePositionX, m_SizePositionY);
                    //    center = matrix.Transform(center);
                    //    matrix.ScaleAt(deltaManipulation.Scale.X, deltaManipulation.Scale.Y, center.X, center.Y);
                    //}
                }
               
                if (ScaleFlag)
                {
                    matrix.Translate(e.DeltaManipulation.Translation.X, e.DeltaManipulation.Translation.Y);
                    //((MatrixTransform)element.RenderTransform).Matrix = matrix;
                    element.RenderTransform = new MatrixTransform(matrix);
                }
            //}
            //if (e.Manipulators.Count() == 1 && m_ScaleFlag)
            //{
       
            //    //if (scaleTrans.ScaleX > 1)
            //    //{
            //    //    if ((translateTrans.X < m_MaxX && translateTrans.X > m_MinX) ||
            //    //        (e.DeltaManipulation.Translation.X < 0 && translateTrans.X >= m_MaxX) ||
            //    //        (e.DeltaManipulation.Translation.X > 0 && translateTrans.X <= m_MinX))
            //    //    {
            //    //        translateTrans.X += e.DeltaManipulation.Translation.X;
            //    //    }
            //    //    else
            //    //    {
            //    //        double _X = e.DeltaManipulation.Translation.X;
            //    //        if (_X < 0)
            //    //        {
            //    //            _X = Math.Abs(_X);
            //    //            translateTrans.X -= ElasticHelper.ApplyLagBehind(_X, ref _X);
            //    //        }
            //    //        else
            //    //            translateTrans.X += ElasticHelper.ApplyLagBehind(_X, ref _X);
            //    //    }
            //    //    if ((translateTrans.Y < m_MaxY && translateTrans.Y > m_MinY) ||
            //    //        (e.DeltaManipulation.Translation.Y < 0 && translateTrans.Y >= m_MaxY) ||
            //    //        (e.DeltaManipulation.Translation.Y > 0 && translateTrans.Y <= m_MaxY))
            //    //    {
            //    //        translateTrans.Y += e.DeltaManipulation.Translation.Y;
            //    //    }
            //    //    else
            //    //    {
            //    //        double _Y = e.DeltaManipulation.Translation.Y;
            //    //        if (_Y < 0)
            //    //        {
            //    //            _Y = Math.Abs(_Y);
            //    //            translateTrans.Y -= ElasticHelper.ApplyLagBehind(_Y, ref _Y);
            //    //        }
            //    //        else
            //    //            translateTrans.Y += ElasticHelper.ApplyLagBehind(_Y, ref _Y);
            //    //    }
            //    //}
            //}
            e.Handled = true;
        }

        private void Image_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            //TouchCount = 0;
            //if (m_RotateOffset * m_RotateAngle > 0 && !ScaleFlag)
            //{
            //    double _TotalRotation = e.TotalManipulation.Rotation;
            //    double _To = 0;
            //    //if (_TotalRotation > 0)
            //    //    _To = _TotalRotation > m_RotateOffset * m_RotateAngle ? rotateTrans.Angle + m_RotateAngle - _TotalRotation : rotateTrans.Angle - _TotalRotation;
            //    //else
            //    //    _To = _TotalRotation > -m_RotateOffset * m_RotateAngle ? rotateTrans.Angle - _TotalRotation : rotateTrans.Angle - m_RotateAngle - _TotalRotation;
            //    //if (_To > 0)
            //    //    _To = _To % m_RotateAngle > (m_RotateAngle / 2) ? _To - _To % m_RotateAngle + m_RotateAngle : _To - _To % m_RotateAngle;
            //    //else
            //    //    _To = _To % m_RotateAngle > -(m_RotateAngle / 2) ? _To - _To % m_RotateAngle : _To - _To % m_RotateAngle - m_RotateAngle;
            //    //CuxTransformAnimationUsingKeyFrames<RotateTransform>.CreateSplineAnimation(rotateTrans, RotateTransform.AngleProperty, _To, 0.2, 1, 1.1, 1, 1.1, delegate()
            //    //{
            //    //    Completed();
            //    //    RotateFlag = false;
            //    //}, null);
            //    if (_TotalRotation != 0)
            //    {
            //        if (_To % 180 == 90 || _To % 180 == -90)
            //        {
            //            if (image.ActualWidth > this.ActualHeight)
            //            {
            //                CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, this.ActualHeight / image.ActualWidth, 0.4,
            //                    delegate()
            //                    {
            //                        if (image.DataContext is Picture)
            //                        {
            //                            ((Picture)image.DataContext).ScaleX = scaleTrans.ScaleX;
            //                        }
            //                    },
            //                    delegate(AnimationTimeline currentTimeline)
            //                    {
            //                        currentTimeline.DecelerationRatio = 1;
            //                    });
            //                CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, this.ActualHeight / image.ActualWidth, 0.4,
            //                   delegate()
            //                   {
            //                       if (image.DataContext is Picture)
            //                       {
            //                           ((Picture)image.DataContext).ScaleY = scaleTrans.ScaleY;
            //                       }
            //                   },
            //                   delegate(AnimationTimeline currentTimeline)
            //                   {
            //                       currentTimeline.DecelerationRatio = 1;
            //                   });
            //            }
            //        }
            //        if (_To % 180 == 0)
            //        {
            //            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, 1, 0.4,
            //                    null,
            //                    delegate(AnimationTimeline currentTimeline)
            //                    {
            //                        currentTimeline.DecelerationRatio = 1;
            //                    });
            //            CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, 1, 0.4,
            //               null,
            //               delegate(AnimationTimeline currentTimeline)
            //               {
            //                   currentTimeline.DecelerationRatio = 1;
            //               });
            //        }
            //    }
            //}
            //if (ScaleFlag)
            //{
            //    m_MaxX = (this.ActualWidth * scaleTrans.ScaleX - this.ActualWidth) / 2;
            //    m_MaxY = (this.ActualHeight * scaleTrans.ScaleY - this.ActualHeight) / 2;

            //    m_MaxX = m_MaxX < 0 ? 0 : m_MaxX;
            //    m_MaxY = m_MaxY < 0 ? 0 : m_MaxY;

            //    m_MinX = -m_MaxX;
            //    m_MinY = -m_MaxY;
            //}
            ElasticTranslate();
            Completed();
            e.Handled = true;
        }

        private void ElasticTranslate()
        {

            //if (translateTrans.X > m_MaxX)
            //{
            //    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(translateTrans, TranslateTransform.XProperty, m_MaxX, 0.4,
            //    null,
            //    delegate(AnimationTimeline currentTimeline)
            //    {
            //        currentTimeline.DecelerationRatio = 1;
            //    });
            //}
            //if (translateTrans.Y > m_MaxY)
            //{
            //    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(translateTrans, TranslateTransform.YProperty, m_MaxY, 0.4,
            //    null,
            //    delegate(AnimationTimeline currentTimeline)
            //    {
            //        currentTimeline.DecelerationRatio = 1;
            //    });
            //}
            //if (translateTrans.X < m_MinX)
            //{
            //    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(translateTrans, TranslateTransform.XProperty, m_MinX, 0.4,
            //    null,
            //    delegate(AnimationTimeline currentTimeline)
            //    {
            //        currentTimeline.DecelerationRatio = 1;
            //    });
            //}
            //if (translateTrans.Y < m_MinY)
            //{
            //    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(translateTrans, TranslateTransform.YProperty, m_MinY, 0.4,
            //    null,
            //    delegate(AnimationTimeline currentTimeline)
            //    {
            //        currentTimeline.DecelerationRatio = 1;
            //    });
            //}

            ////if (scaleTrans.ScaleX > m_ScaleMax)
            ////{
            ////    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, m_ScaleMax, 0.4,
            ////    null,
            ////    delegate(AnimationTimeline currentTimeline)
            ////    {
            ////        currentTimeline.DecelerationRatio = 1;
            ////    });
            ////}
            ////if (scaleTrans.ScaleX < m_ScaleMin)
            ////{
            ////    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, m_ScaleMin, 0.4,
            ////    null,
            ////    delegate(AnimationTimeline currentTimeline)
            ////    {
            ////        currentTimeline.DecelerationRatio = 1;
            ////    });
            ////}
            ////if (scaleTrans.ScaleY > m_ScaleMax)
            ////{
            ////    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, m_ScaleMax, 0.4,
            ////    null,
            ////    delegate(AnimationTimeline currentTimeline)
            ////    {
            ////        currentTimeline.DecelerationRatio = 1;
            ////    });
            ////}
            ////if (scaleTrans.ScaleY < m_ScaleMin)
            ////{
            ////    CuxDoubleAnimationUsingKeyFrames.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, m_ScaleMin, 0.4,
            ////    null,
            ////    delegate(AnimationTimeline currentTimeline)
            ////    {
            ////        currentTimeline.DecelerationRatio = 1;
            ////    });
            ////}
        }

        /// <summary>
        /// animation completed method
        /// </summary>
        private void Completed()
        {
            image.IsManipulationEnabled = true;
            m_ScaleTimer.Stop();
            //if (this.image.RenderTransform.Value.M11 != 1 && this.image.RenderTransform.Value.M22 != 1)
            //{
            //    ScaleFlag = true;
            //}
            //else
            //{
            //    ScaleFlag = false;
            //}
            //if (image.DataContext is Picture)
            //{
            //    rotateTrans.Angle = rotateTrans.Angle % 360;
            //    ((Picture)image.DataContext).RotateAngle = rotateTrans.Angle;
            //}
        }

        DateTime dt = DateTime.MinValue;
        DateTime dtNow = DateTime.MinValue;

        //private void gridContainer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ClickCount == 2 && ScaleFlag)
        //    {
        //        dtNow = DateTime.Now;

        //        image.IsManipulationEnabled = false;

        //        if (this.image.RenderTransform.Value.M11 != 1 && this.image.RenderTransform.Value.M22 != 1)
        //        {
        //            FrameworkElement element = (FrameworkElement)this.image;
        //            double oldWidth = element.ActualWidth;
        //            double oldHeight = element.ActualHeight;

        //            double ratioY = oldHeight / oldWidth;
        //            if (m_CuxAngle == -180 || m_CuxAngle == -360)
        //                ratioY = 1;
        //            ScaleTransform s = new ScaleTransform(ratioY, ratioY, element.ActualWidth / 2, element.ActualHeight / 2);
        //            RotateTransform rm = new RotateTransform(m_CuxAngle, element.ActualWidth / 2, element.ActualHeight / 2);
        //            TransformGroup tg = new TransformGroup();
        //            tg.Children.Add(s);
        //            tg.Children.Add(rm);
        //            element.RenderTransform = tg;
        //        }

        //        //CuxTransformAnimationUsingKeyFrames<ScaleTransform>.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, 1, 0.2, 1, 1.1, 1, 1.1, delegate()
        //        //{
        //        //    Completed();
        //        //}, null);
        //        //CuxTransformAnimationUsingKeyFrames<ScaleTransform>.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, 1, 0.2, 1, 1.1, 1, 1.1, delegate()
        //        //{
        //        //    Completed();
        //        //}, null);

        //        //CuxTransformAnimationUsingKeyFrames<TranslateTransform>.CreateSplineAnimation(translateTrans, TranslateTransform.XProperty, 0, 0.2, 1, 1.1, 1, 1.1, delegate()
        //        //{
        //        //}, null);
        //        //CuxTransformAnimationUsingKeyFrames<TranslateTransform>.CreateSplineAnimation(translateTrans, TranslateTransform.YProperty, 0, 0.2, 1, 1.1, 1, 1.1, delegate()
        //        //{
        //        //}, null);

        //        ScaleFlag = false;
        //        RotateFlag = false;
        //        dt = DateTime.MinValue;

        //        dt = dtNow;
        //    }
        //    TouchCount =0;
        //}

        private void gridContainer_TouchDown(object sender, TouchEventArgs e)
        {
            if (TouchCount == 0)
            {
                dtNow = DateTime.Now;
                int secon = (dtNow - dt).Milliseconds;
                TimeSpan ts1 = new TimeSpan(dtNow.Ticks);
                TimeSpan ts2 = new TimeSpan(dt.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                if (ts.TotalMilliseconds < 150)
                {
                    image.IsManipulationEnabled = false;

                    if (this.image.RenderTransform.Value.M11 != 1 && this.image.RenderTransform.Value.M22 != 1)
                    {
                        FrameworkElement element = (FrameworkElement)this.image;
                        double oldWidth = element.ActualWidth;
                        double oldHeight = element.ActualHeight;

                        double ratioY = oldHeight / oldWidth;
                        if (m_CuxAngle == -180 || m_CuxAngle == -360 || m_CuxAngle ==0)
                            ratioY = 1;
                        ScaleTransform s = new ScaleTransform(ratioY, ratioY, element.ActualWidth / 2, element.ActualHeight / 2);
                        RotateTransform rm = new RotateTransform(m_CuxAngle, element.ActualWidth / 2, element.ActualHeight / 2);
                        TransformGroup tg = new TransformGroup();
                        tg.Children.Add(s);
                        tg.Children.Add(rm);
                        element.RenderTransform = tg;
                    }

                    Completed();
                    //CuxTransformAnimationUsingKeyFrames<ScaleTransform>.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, 1, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //    Completed();
                    //}, null);
                    //CuxTransformAnimationUsingKeyFrames<ScaleTransform>.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, 1, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //    Completed();
                    //}, null);

                    //CuxTransformAnimationUsingKeyFrames<TranslateTransform>.CreateSplineAnimation(translateTrans, TranslateTransform.XProperty, 0, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //}, null);
                    //CuxTransformAnimationUsingKeyFrames<TranslateTransform>.CreateSplineAnimation(translateTrans, TranslateTransform.YProperty, 0, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //}, null);

                    ScaleFlag = false;
                    RotateFlag = false;
                    dt = DateTime.MinValue;
                }
                dt = dtNow;
            }
            TouchCount += 1;
            if (TouchCount == 2)
            {
                StartTimer();
            }
        }

       

        #endregion


        private void StartTimer()
        {
            if (SlideShow.EnableZoomMode)
            {
                m_ScaleTimer.Stop();
                if (!ScaleFlag)
                    m_ScaleTimer.Start();
            }
        }

        private void Grid_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.NewValue.ToString().ToLower().Equals("true"))
            {
                TouchCount = 0;
                if (TouchCount == 0)
                {
                    dtNow = DateTime.Now;
                    int secon = (dtNow - dt).Milliseconds;
                    TimeSpan ts1 = new TimeSpan(dtNow.Ticks);
                    TimeSpan ts2 = new TimeSpan(dt.Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    image.IsManipulationEnabled = false;

                    if (this.image.RenderTransform.Value.M11 != 1 && this.image.RenderTransform.Value.M22 != 1)
                    {
                        FrameworkElement element = (FrameworkElement)this.image;
                        double oldWidth = element.ActualWidth;
                        double oldHeight = element.ActualHeight;

                        double ratioY = oldHeight / oldWidth;
                        if (m_CuxAngle == -180 || m_CuxAngle == -360 || m_CuxAngle == 0)
                            ratioY = 1;
                        ScaleTransform s = new ScaleTransform(ratioY, ratioY, element.ActualWidth / 2, element.ActualHeight / 2);
                        RotateTransform rm = new RotateTransform(m_CuxAngle, element.ActualWidth / 2, element.ActualHeight / 2);
                        TransformGroup tg = new TransformGroup();
                        tg.Children.Add(s);
                        tg.Children.Add(rm);
                        element.RenderTransform = tg;
                    }
                    //CuxTransformAnimationUsingKeyFrames<ScaleTransform>.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleXProperty, 1, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //    Completed();
                    //}, null);
                    //CuxTransformAnimationUsingKeyFrames<ScaleTransform>.CreateSplineAnimation(scaleTrans, ScaleTransform.ScaleYProperty, 1, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //    Completed();
                    //}, null);

                    //CuxTransformAnimationUsingKeyFrames<TranslateTransform>.CreateSplineAnimation(translateTrans, TranslateTransform.XProperty, 0, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //}, null);
                    //CuxTransformAnimationUsingKeyFrames<TranslateTransform>.CreateSplineAnimation(translateTrans, TranslateTransform.YProperty, 0, 0.2, 1, 1.1, 1, 1.1, delegate()
                    //{
                    //}, null);
                    Completed();
                    ScaleFlag = false;
                    RotateFlag = false;
                    dt = DateTime.MinValue;
                }
            }
        }

    

        public double m_CuxAngle = 0;
        private double m_CuxSancle = 0;
        private void RotateAngle_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            double value = Convert.ToInt32(e.NewValue);


            FrameworkElement element = (FrameworkElement)this.image;
            if (element.ActualWidth != 0)
            {
                double oldWidth = element.ActualWidth;
                double oldHeight = element.ActualHeight;

                double ratioY = oldHeight / oldWidth;
                if (Math.Abs(value) == 180 || Math.Abs(value) == 360 || value == 0)
                    ratioY = 1;
                ScaleTransform s = new ScaleTransform(ratioY, ratioY, element.ActualWidth / 2, element.ActualHeight / 2);
                RotateTransform rm = new RotateTransform(value, element.ActualWidth / 2, element.ActualHeight / 2);
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(s);
                tg.Children.Add(rm);
                element.RenderTransform = tg;
                m_CuxAngle = value;
            }

        }

    
      

       
    }
    public class CuxManipulationEventArgs
    {
        public bool IsScaled
        {
            get;
            set;
        }
    }

    /// <summary>
    ///  The ElasticHelper logic class.
    /// </summary>
    internal static class ElasticHelper
    {
        private const int MaximumElasticPaddingThreshold = 4;

        public static double ApplyLagBehind(double maxElasticPadding, ref double offset)
        {
            if (double.IsNaN(offset) || offset == 0)
            {
                return 0.0;
            }
            double maximumElasticBorderOffset = GetMaximumElasticBorderOffset(maxElasticPadding);
            if (maximumElasticBorderOffset == 0.0)
            {
                offset = 0.0;
                return 0.0;
            }
            if (offset > maximumElasticBorderOffset)
            {
                offset = maximumElasticBorderOffset;
                return (maxElasticPadding - 4.0);
            }
            return Math.Round((double)((1.0 - (1.0 / Math.Pow(2.0, offset / maxElasticPadding))) * maxElasticPadding), 4);

        }

        public static double GetMaximumElasticBorderOffset(double maxElasticPadding)
        {
            if (maxElasticPadding <= 4.0)
            {
                return 0.0;
            }
            return Math.Round((double)(maxElasticPadding * Math.Log(maxElasticPadding / 4.0, 2.0)), 4);
        }

        public static double GetElasticPadding(double offset)
        {
            if (offset < 0.0)
            {
                return -offset;
            }
            return offset;
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is MemoryStream)
            {
                //using (MemoryStream ms = value as MemoryStream)
                //{
                //    ms.Seek(0, SeekOrigin.Begin);
                //    return DataConverter.GetImgSourceByStream(ms);
                //}
                MemoryStream ms = value as MemoryStream;
                ms.Seek(0, SeekOrigin.Begin);
                return DataConverter.GetImgSourceByStream(ms);
            }
            else
                return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

    }
 
}
