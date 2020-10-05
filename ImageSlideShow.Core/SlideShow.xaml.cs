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
using System.Threading;
using System.IO;
using ImageSlideShow.Core.EffectsManage.TransitionPresenter;
using System.Windows.Media.Animation;
using ImageSlideShow.Core.EffectsManage.AnimationCustom;
using ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models;
 

namespace ImageSlideShow.Core
{
    /// <summary>
    /// SlidShow.xaml 的交互逻辑
    /// </summary>
    public partial class SlideShow : UserControl
    {
        public static readonly RoutedEvent EnableScrollEvent = EventManager.RegisterRoutedEvent("EnableScroll", RoutingStrategy.Bubble, typeof(ScrollEnabledChangedRoutedHandler), typeof(SlideShow));

        public static readonly RoutedEvent DisableScrollEvent = EventManager.RegisterRoutedEvent("DisableScroll", RoutingStrategy.Bubble, typeof(ScrollEnabledChangedRoutedHandler), typeof(SlideShow));

        private static bool m_EnableZoomMode = true;
        public static bool EnableZoomMode
        {
            get
            {
                return m_EnableZoomMode;
            }
            set 
            {
                m_EnableZoomMode = value;
            }
        }
        

        #region Private Parameters

        private bool m_IsEnableScroll = true;

        /// <summary>
        /// get or set Effect type list 
        /// </summary>
        private List<EffectType> EffectList = new List<EffectType>();

        /// <summary>
        /// get or set IsAutoPlayImage 
        /// </summary>
        private bool IsAutoPlayImage
        {
            get;
            set;
        }

        /// <summary>
        /// touch down count
        /// </summary>
        private int m_TouchDownCount = 0;

        /// <summary>
        /// when manipulation,stop drag the item
        /// </summary>
        private bool m_ManipulationFlag = false;

        /// <summary>
        /// this flag said the SlidShow control is been inited or not
        /// </summary>
        private bool m_IsSlidShowInited = false;

        /// <summary>
        /// this flag said the items container is moving or not
        /// </summary>
        private bool m_IsContainerMoving = false;

        /// <summary>
        /// use touch event
        /// </summary>
        private bool m_IsTouchEvent = false;

        /// <summary>
        /// define the event PreviewDragEvent is be handled
        /// </summary>
        private bool m_PreDragFlag = false;

        /// <summary>
        /// define notify DragedEvent when the index changed
        /// </summary>
        private bool m_IsNotifyIndexChanged = false;

        /// <summary>
        /// auto play direction
        /// </summary>
        private ScrollDirection m_AutoPlayDirection = ScrollDirection.None;

        /// <summary>
        /// get the point when mouse left button down
        /// </summary>
        private Point m_DownPoint;

        /// <summary>
        /// get the point when mouse left button up
        /// </summary>
        private Point m_UpPoint;

        /// <summary>
        /// get the new mouse point when mouse move
        /// </summary>
        private Point m_DynamicPoint;

        /// <summary>
        /// set this flag true when mouse down,and set this flag false when mouse up
        /// </summary>
        private bool m_MouseDownFlag = false;

        /// <summary>
        /// difine the image scroll direction
        /// </summary>
        private ScrollDirection m_MoveDirection = ScrollDirection.None;

        /// <summary>
        /// Define image scroll layout
        /// </summary>
        private Orientation m_Orientation = Orientation.Horizontal;

        /// <summary>
        /// current Image index
        /// </summary>
        private int m_CurrentIndex = -1;

        /// <summary>
        /// auto play stop time
        /// </summary>
        private int m_AutoPlayTime = 5000;

        /// <summary>
        /// if true auto play,else stop play
        /// </summary>
        private bool m_IsAutoPlay = false;

        /// <summary>
        /// get or set auto play flag
        /// </summary>
        private bool IsAutoPlay
        {
            get
            {
                return m_IsAutoPlay;
            }
            set
            {
                if (m_IsAutoPlay && !value)
                {
                    if (AutoPlayStopedHadeler != null)
                    {
                        AutoPlayStopedHadeler();
                    }
                    if (m_AutoPlayThread != null)
                    {
                        m_AutoPlayThread.Abort();
                        m_AutoPlayThread = null;
                    }
                }
                m_IsAutoPlay = value;
                if (!value && IsAutoPlayImage)
                {
                    if (ItemsContainer.Children.Count > 0)
                    {
                        ItemsContainer.Children.RemoveAt(0);
                        SlideShowItem obj = new SlideShowItem(null);
                        if (m_CurrentIndex > 0 && m_CurrentIndex < m_ItemsList.Count)
                        {
                            obj = m_ItemsList[m_CurrentIndex - 1];
                        }
                        ItemsContainer.Children.Insert(0, GetScrollItem(obj));
                    }
                    if (ItemsContainer.Children.Count > 2)
                    {
                        ItemsContainer.Children.RemoveAt(2);
                        SlideShowItem obj = new SlideShowItem(null); ;
                        if (m_CurrentIndex < m_ItemsList.Count - 1 && m_CurrentIndex >= 0)
                        {
                            obj = m_ItemsList[m_CurrentIndex + 1];
                        }
                        ItemsContainer.Children.Add(GetScrollItem(obj));
                    }
                    IsAutoPlayImage = false;
                }
            }
        }

        /// <summary>
        /// auto play thread
        /// </summary>
        private Thread m_AutoPlayThread = null;

        /// <summary>
        /// scroll control
        /// </summary>
        private List<SlideShowItem> m_ItemsList = new List<SlideShowItem>();
        #endregion

        #region Public Attribute

        /// <summary>
        /// set or get auto play image type
        /// </summary>
        public EffectType AutoPlayImageType
        {
            get;
            set;
        }
        /// <summary>
        /// Get Items count
        /// </summary>
        public int ItemsCount
        {
            get
            {
                return m_ItemsList.Count;
            }
        }
        /// <summary>
        /// get or set AutoPlay time
        /// </summary>
        public int AutoPlayTime
        {
            get
            {
                return m_AutoPlayTime;
            }
            set
            {
                m_AutoPlayTime = value;
            }
        }

        /// <summary>
        /// Get the Current item is the first or not
        /// </summary>
        public bool IsFirst
        {
            get
            {
                return m_CurrentIndex == 0;
            }
        }

        /// <summary>
        /// Get the current item is the last item or not
        /// </summary>
        public bool IsLast
        {
            get
            {
                return m_CurrentIndex == m_ItemsList.Count - 1;
            }
        }

        /// <summary>
        /// get or set if stay at the current page
        /// </summary>
        public bool IsStayAfterClick
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the auto play direction
        /// </summary>
        public ScrollDirection AutoPlayDirection
        {
            get
            {
                return m_AutoPlayDirection;
            }
            set
            {
                m_AutoPlayDirection = value;
            }
        }

        /// <summary>
        /// Get or Set current Image index
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                return m_CurrentIndex;
            }
            set
            {
                if (m_ItemsList.Count > 0)
                {
                    int _Index = value;
                    _Index = _Index > m_ItemsList.Count - 1 ? m_ItemsList.Count - 1 : (_Index < 0 ? 0 : _Index);
                    if (ItemsContainer.Children.Count > 1 && _Index != m_CurrentIndex)
                    {
                        TransitionElement TE = (TransitionElement)ItemsContainer.Children[1];
                        if (TE != null)
                        {
                            if (EffectList.Count > 0)
                            {
                                string _EffictType = "";
                                if (AutoPlayImageType == EffectType.Random)
                                {
                                    Random _Random = new Random();
                                    int _EffectIndex = _Random.Next(0, EffectList.Count - 1);
                                    _EffictType = EffectList[_EffectIndex].ToString();
                                }
                                else
                                {
                                    _EffictType = AutoPlayImageType.ToString();
                                }
                                Transition _Trans = FindResource(_EffictType) as Transition;
                                TE.Transition = _Trans;
                            }
                            TE.Content = m_ItemsList[_Index].ControlObject;
                            m_CurrentIndex = _Index;
                            NotifyCurrentItemChanged();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get current item
        /// </summary>
        public SlideShowItem CurrentItem
        {
            get
            {
                if (m_CurrentIndex >= 0 && m_CurrentIndex < m_ItemsList.Count)
                {
                    return m_ItemsList[m_CurrentIndex];
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Get or Set the Image layout in this control
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return m_Orientation;
            }
            set
            {
                m_Orientation = value;
                ItemsContainer.Orientation = m_Orientation;
            }
        }

        /// <summary>
        /// cause this event when the auto play stoped
        /// </summary>
        public SlideShowEventHandler AutoPlayStopedHadeler;

        /// <summary>
        /// User click event
        /// </summary>
        public SlideShowEventHandler SlidShowClickEvent;

        /// <summary>
        /// when drag slidshow control fire this event 
        /// </summary>
        public event SlideShowEventHandler PreviewDragEvent;

        public event RoutedEventHandler MultiModeEvent;

        public event RoutedEventHandler MultiExitModeEvent;

        /// <summary>
        /// when draged slidshow control and the current index changed fire this event
        /// </summary>
        public event CurrentItemChangedEventHandler CurrentItemChangedEvent;
        #endregion

        #region Private Method

        /// <summary>
        /// get the scroll item by the UIElement
        /// </summary>
        /// <param name="Item">UIElment</param>
        /// <returns>return scroll item</returns>
        private TransitionElement GetScrollItem(SlideShowItem Item)
        {
            TransitionElement TE = new TransitionElement();
            if (Item.ControlObject != null)
            {
                TE.Content = Item.ControlObject;
            }
            TE.Width = this.ActualWidth;
            TE.Height = this.ActualHeight;
            return TE;
        }

        /// <summary>
        /// Check the count of the item in the container
        /// </summary>
        private void CheckItems()
        {
            if (ItemsContainer.Children.Count >= 3)
            {
                TransitionElement TE = ItemsContainer.Children[2] as TransitionElement;
                if (TE != null && TE.Content == null && m_CurrentIndex < m_ItemsList.Count - 1)
                {
                    ItemsContainer.Children.RemoveAt(2);
                    ItemsContainer.Children.Add(GetScrollItem(m_ItemsList[m_CurrentIndex + 1]));
                }
            }
        }

        /// <summary>
        /// Set the grid clip
        /// </summary>
        private void ChangClip()
        {
            Grid BackContainer = base.FindName("BackContainer") as Grid;
            if (BackContainer != null)
            {
                double _X = BackContainer.ActualWidth;
                double _Y = BackContainer.ActualHeight;
                string str = string.Format("M1,0 L{0},0 L{1},{2} L1,{3}", _X - 1, _X - 1, _Y, _Y);
                BackContainer.Clip = Geometry.Parse(str);
            }
        }

        /// <summary>
        /// initialize this control
        /// </summary>
        private void InitControls()
        {
            AutoPlayImageType = EffectType.Random;
            EffectList.Add(EffectType.Checkerboard);
            EffectList.Add(EffectType.Cloth);
            EffectList.Add(EffectType.DiagonalWipe);
            EffectList.Add(EffectType.Diamonds);
            EffectList.Add(EffectType.Door3D);
            EffectList.Add(EffectType.Dots);
            EffectList.Add(EffectType.DoubleRotateWipe);
            EffectList.Add(EffectType.Explosion3D);
            EffectList.Add(EffectType.Fade);
            EffectList.Add(EffectType.FadeAndGrow);
            EffectList.Add(EffectType.FadeWipe2);
            EffectList.Add(EffectType.Flip3D);
            EffectList.Add(EffectType.HorizontalBlinds);
            EffectList.Add(EffectType.HorizontalWipe);
            EffectList.Add(EffectType.Melt);
            EffectList.Add(EffectType.Page);
            EffectList.Add(EffectType.Roll);
            EffectList.Add(EffectType.Rotate3D);
            EffectList.Add(EffectType.RotateWipe);
            EffectList.Add(EffectType.Spin3D);
            EffectList.Add(EffectType.TranslateTransition2D);
            EffectList.Add(EffectType.VerticalBlinds);
            EffectList.Add(EffectType.VerticalWipe);
            ItemsContainer.Orientation = m_Orientation;
            ItemsContainer.PreviewMouseDown += new MouseButtonEventHandler(ItemsContainer_PreviewMouseDown);
            ItemsContainer.PreviewMouseMove += new MouseEventHandler(ItemsContainer_PreviewMouseMove);
            ItemsContainer.PreviewMouseUp += new MouseButtonEventHandler(ItemsContainer_PreviewMouseUp);
            ItemsContainer.MouseLeave += new MouseEventHandler(ItemsContainer_MouseLeave);
            ItemsContainer.PreviewTouchDown += new EventHandler<TouchEventArgs>(ItemsContainer_PreviewTouchDown);
            ItemsContainer.PreviewTouchMove += new EventHandler<TouchEventArgs>(ItemsContainer_PreviewTouchMove);
            ItemsContainer.PreviewTouchUp += new EventHandler<TouchEventArgs>(ItemsContainer_PreviewTouchUp);

            EventManager.RegisterClassHandler(typeof(SlideShow), SlideShow.EnableScrollEvent, new ScrollEnabledChangedRoutedHandler(EnableScrollHandler));
            EventManager.RegisterClassHandler(typeof(SlideShow), SlideShow.DisableScrollEvent, new ScrollEnabledChangedRoutedHandler(DisableScrollHandler));

            this.Unloaded += delegate
            {
                Clear(false);
            };
            this.SizeChanged += delegate
            {
                ChangClip();
                foreach (UIElement fe in ItemsContainer.Children)
                {
                    TransitionElement TE = fe as TransitionElement;
                    if (TE != null)
                    {
                        TE.Width = this.ActualWidth;
                        TE.Height =  this.ActualHeight;
                    }
                }
                if (ItemsContainer != null)
                {
                    if (m_Orientation == System.Windows.Controls.Orientation.Horizontal)
                        ItemsContainer.Margin = new Thickness(-this.ActualWidth, 0, 0, 0);
                    else
                        ItemsContainer.Margin = new Thickness(0, -this.ActualHeight, 0, 0);
                }
            };
            this.Loaded += delegate
            {
                ChangClip();
            };
        }

        /// <summary>
        /// Set flag and the mouse point when mouse down or mouse up
        /// </summary>
        /// <param name="IsMouseDown">when mouse down this value is true,mouse up this value is false</param>
        /// <param name="MousePoint">mouse point when mouse down or up</param>
        private void SetMouseStatus(bool IsMouseDown, Point MousePoint)
        {
            if (IsMouseDown)
            {
                if (m_IsEnableScroll)
                {
                    m_MouseDownFlag = IsMouseDown;
                    m_DynamicPoint = MousePoint;
                    m_DownPoint = MousePoint;
                    IsAutoPlay = false;
                    m_PreDragFlag = false;
                }
            }
            else
            {
                m_UpPoint = MousePoint;
                if (m_MouseDownFlag)
                {
                    m_MouseDownFlag = IsMouseDown;
                    ScrollByDrag();
                }
            }
        }

        /// <summary>
        /// Set the scroll direction
        /// </summary>
        /// <param name="p_NewPoint"></param>
        private void GetDirection(Point p_NewPoint)
        {
            if (!m_ManipulationFlag)
            {
                double _NewX = 0;
                double _NewY = 0;
                if (Orientation == System.Windows.Controls.Orientation.Horizontal)
                {
                    if (p_NewPoint.X == m_DynamicPoint.X)
                        return;
                    double _X = p_NewPoint.X - m_DynamicPoint.X;
                    if (_X == 0)
                        m_MoveDirection = ScrollDirection.None;
                    if (_X > 0)
                        m_MoveDirection = ScrollDirection.Right;
                    if (_X < 0)
                        m_MoveDirection = ScrollDirection.Left;

                    double t = 1;
                    if (m_CurrentIndex == 0 && m_MoveDirection == ScrollDirection.Right)
                    {
                        t = (this.ActualWidth - (p_NewPoint.X - m_DownPoint.X)) / this.ActualWidth;
                    }
                    if (m_CurrentIndex == m_ItemsList.Count - 1 && m_MoveDirection == ScrollDirection.Left)
                    {
                        t = (this.ActualWidth - (m_DownPoint.X - p_NewPoint.X)) / this.ActualWidth;
                    }
                    t = t > 1 ? 1 : t;
                    t = t < 0.02 ? 0.02 : t;
                    _NewX = ((TransitionElement)ItemsContainer.Children[0]).Margin.Left + (p_NewPoint.X - m_DynamicPoint.X) * t;
                }
                if (Orientation == System.Windows.Controls.Orientation.Vertical)
                {
                    if (p_NewPoint.Y == m_DynamicPoint.Y)
                        return;
                    double _Y = p_NewPoint.Y - m_DynamicPoint.Y;
                    if (_Y == 0)
                        m_MoveDirection = ScrollDirection.None;
                    if (_Y > 0)
                        m_MoveDirection = ScrollDirection.Down;
                    if (_Y < 0)
                        m_MoveDirection = ScrollDirection.Up;
                    double t = 1;
                    if (m_CurrentIndex == 0 && m_MoveDirection == ScrollDirection.Down)
                    {
                        t = (this.ActualHeight - (p_NewPoint.Y - m_DownPoint.Y)) / this.ActualHeight;
                    }
                    if (m_CurrentIndex == m_ItemsList.Count - 1 && m_MoveDirection == ScrollDirection.Up)
                    {
                        t = (this.ActualHeight - (m_DownPoint.Y - p_NewPoint.Y)) / this.ActualHeight;
                    }
                    t = t > 1 ? 1 : t;
                    t = t < 0.02 ? 0.02 : t;
                    _NewY = ((TransitionElement)ItemsContainer.Children[0]).Margin.Top + (p_NewPoint.Y - m_DynamicPoint.Y) * t;
                }
                ((TransitionElement)ItemsContainer.Children[0]).Margin = new Thickness(_NewX, _NewY, 0, 0);
                m_DynamicPoint = p_NewPoint;

                if (!m_PreDragFlag && PreviewDragEvent != null)
                {
                    m_PreDragFlag = true;
                    PreviewDragEvent();
                }
            }
            else
            {
                m_MoveDirection = ScrollDirection.None;
            }
        }

        /// <summary>
        /// Scroll the image when mouse button up or mouse leave
        /// </summary>
        private void ScrollByDrag()
        {
            if (Orientation == System.Windows.Controls.Orientation.Horizontal)
            {
                if (m_MoveDirection == ScrollDirection.Right)
                {
                    if (m_UpPoint.X > m_DownPoint.X)
                    {
                        Animation(AnimationType.Previous);
                    }
                    else
                    {
                        Animation(AnimationType.None);
                    }
                }
                if (m_MoveDirection == ScrollDirection.Left)
                {
                    if (m_UpPoint.X < m_DownPoint.X)
                    {
                        Animation(AnimationType.Next);
                    }
                    else
                    {
                        Animation(AnimationType.None);
                    }
                }
                if (m_MoveDirection == ScrollDirection.None)
                {
                    Animation(AnimationType.None);
                }
            }
        }

        /// <summary>
        /// scroll to the next or previous item
        /// </summary>
        /// <param name="p_IsNext">true scroll to the next item,false scroll to the previous item</param>
        private void Scroll(bool p_IsNext)
        {
            if (p_IsNext)
                Animation(AnimationType.Next);
            else
                Animation(AnimationType.Previous);
        }

        enum AnimationType
        {
            None,
            Next,
            Previous
        }
        /// <summary>
        /// begin animation when mouse or tuch up
        /// </summary>
        /// <param name="p_Type">animation type</param>
        private void Animation(AnimationType p_Type)
        {
            if (ItemsContainer.Children.Count <= 0)
                return;
            Thickness _To = new Thickness();
            switch (p_Type)
            {
                case AnimationType.None:
                    _To = new Thickness(0, 0, 0, 0);
                    break;
                case AnimationType.Next:
                    if (m_CurrentIndex >= m_ItemsList.Count - 1)
                    {
                        _To = new Thickness(0, 0, 0, 0);
                        p_Type = AnimationType.None;
                    }
                    else
                    {
                        _To = new Thickness(-this.ActualWidth, 0, 0, 0);
                    }
                    break;
                case AnimationType.Previous:
                    if (m_CurrentIndex <= 0)
                    {
                        _To = new Thickness(0, 0, 0, 0);
                        p_Type = AnimationType.None;
                    }
                    else
                    {
                        _To = new Thickness(this.ActualWidth, 0, 0, 0);
                    }
                    break;
            }
            FrameworkElement Ele = (TransitionElement)ItemsContainer.Children[0];
            if (IsStayAfterClick)
            {
                if (Ele.Margin.Left >= -30 && Ele.Margin.Left <= 30)
                {
                    p_Type = AnimationType.None;
                    _To = new Thickness(0, 0, 0, 0);
                }
                IsStayAfterClick = false;
            }

            if (SlidShowClickEvent != null)
            {
                if (Ele.Margin.Left >= -10 && Ele.Margin.Left <= 10)
                {
                    p_Type = AnimationType.None;
                    _To = new Thickness(0, 0, 0, 0);
                    SlidShowClickEvent();
                }
            }

            ThicknessAnimation TA = new ThicknessAnimation();
            TA.To = _To;
            CuxAnimation.CuxThicknessAnimation(Ele, FrameworkElement.MarginProperty, _To, 0.25,
                delegate()
                {
                    AnimationCompletet(p_Type);
                },
                delegate(AnimationTimeline CurrentObject)
                {
                    CurrentObject.DecelerationRatio = 1;
                });
        }

        /// <summary>
        /// animation complete method
        /// </summary>
        /// <param name="p_Type"></param>
        private void AnimationCompletet(AnimationType p_Type)
        {
            switch (p_Type)
            {
                case AnimationType.Next:
                    if (m_CurrentIndex < m_ItemsList.Count - 1)
                    {
                        m_CurrentIndex++;
                        m_IsNotifyIndexChanged = true;
                        SlideShowItem Item = m_CurrentIndex < m_ItemsList.Count - 1 ? m_ItemsList[m_CurrentIndex + 1] : new SlideShowItem(null);
                        TransitionElement TE = GetScrollItem(Item);
                        ItemsContainer.Children.Add(TE);

                        TransitionElement TE0 = (TransitionElement)ItemsContainer.Children[0];
                        ItemsContainer.Children.Remove(TE0);
                    }
                    break;
                case AnimationType.Previous:
                    if (m_CurrentIndex > 0)
                    {
                        m_CurrentIndex--;
                        m_IsNotifyIndexChanged = true;
                        SlideShowItem Item = m_CurrentIndex > 0 ? m_ItemsList[m_CurrentIndex - 1] : new SlideShowItem(null);
                        TransitionElement TE = GetScrollItem(Item);

                        TransitionElement TE0 = (TransitionElement)ItemsContainer.Children[0];
                        TransitionElement TE2 = (TransitionElement)ItemsContainer.Children[2];
                        ItemsContainer.Children.RemoveAt(2);
                        ItemsContainer.Children.Insert(0, TE);
                        TE0.Margin = new Thickness(0, 0, 0, 0);
                    }
                    break;
            }
            m_TouchDownCount = 0;
            m_ManipulationFlag = false;
            if (m_IsNotifyIndexChanged)
            {
                m_IsNotifyIndexChanged = false;
                NotifyCurrentItemChanged();
            }
        }

        private void NotifyCurrentItemChanged()
        {
            if (CurrentItemChangedEvent != null)
            {
                if (m_CurrentIndex >= 0 && m_CurrentIndex < m_ItemsList.Count)
                {
                    CurrentItemChangedEventArgs e = new CurrentItemChangedEventArgs();
                    e.CurrentIndex = m_CurrentIndex;
                    e.CurrentItem = m_ItemsList[m_CurrentIndex];
                    e.Direction = m_MoveDirection;
                    CurrentItemChangedEvent(e);
                }
            }
        }

        /// <summary>
        /// Animation to the container when the current item is the end or start item
        /// </summary>
        /// <param name="p_ToValue"></param>
        /// <param name="p_IsToFromValue">Back to the old value or not</param>
        private void AnimationContainer(Thickness p_ToValue, bool p_IsToFromValue)
        {
            if (!m_IsContainerMoving)
            {
                m_IsContainerMoving = true;
                Thickness _OldThickness = ItemsContainer.Margin;
                Thickness tk = p_ToValue;

                ThicknessAnimation TA = new ThicknessAnimation();
                TA.To = tk;
                CuxThicknessAnimationUsingKeyFrames.CreateSplineAnimation(ItemsContainer, FrameworkElement.MarginProperty, tk, 0.2,
                    delegate()
                    {
                        m_IsContainerMoving = false;
                        if (p_IsToFromValue)
                            AnimationContainer(_OldThickness, false);
                    });
            }
        }

        /// <summary>
        /// when the item scroll to the first or the last item use this animation
        /// </summary>
        private void ElasticityAnimation(bool p_IsEnd)
        {
            double _ToX = this.ActualWidth / 4;
            double _ToY = this.ActualHeight / 4;
            Thickness _To = new Thickness();
            if (m_Orientation == System.Windows.Controls.Orientation.Horizontal)
            {
                if (p_IsEnd)
                    _To = new Thickness(ItemsContainer.Margin.Left - _ToX, 0, 0, 0);
                else
                    _To = new Thickness(ItemsContainer.Margin.Left + _ToX, 0, 0, 0);
            }
            else
            {
                if (p_IsEnd)
                    _To = new Thickness(0, ItemsContainer.Margin.Top - _ToY, 0, 0);
                else
                    _To = new Thickness(0, ItemsContainer.Margin.Top + _ToY, 0, 0);
            }
            AnimationContainer(_To, true);
        }

        #endregion

        #region Public Method

        public void EnableZoomModeFlag(bool flag)
        {
            EnableZoomMode = flag;
        }

        public void ExitZoomMode()
        {
            if (CurrentItem != null && CurrentItem.ControlObject!=null)
            {
                Picture pic = CurrentItem.ControlObject as Picture;
                pic.IsZoomMode = true;
            }
        }


        /// <summary>
        /// this method is for picture(if the item has FancyType)
        /// </summary>
        public void InitSlidShowForAutoPlay()
        {
            int i = 0;
            bool _IsInit = false;
            while (i < m_ItemsList.Count)
            {
                if (m_ItemsList[i].FancyType != FancyType.DisLike)
                {
                    InitSlidShow(i);
                    _IsInit = true;
                    break;
                }
                i++;
            }
            if (!_IsInit && m_ItemsList.Count > 0)
            {
                InitSlidShow(0);
            }
        }

        /// <summary>
        /// Init SlidShow and display it from first item
        /// </summary>
        public void InitSlidShow()
        {
            InitSlidShow(0);
        }

        /// <summary>
        /// Get the item by p_Index
        /// </summary>
        /// <param name="p_Index"></param>
        /// <returns></returns>
        public SlideShowItem GetItem(int p_Index)
        {
            if (p_Index < 0 || p_Index > m_ItemsList.Count - 1)
            {
                return null;
            }
            else
            {
                return m_ItemsList[p_Index];
            }
        }

        /// <summary>
        /// Init SlidShow and display it from the StartIndex item
        /// </summary>
        /// <param name="StartIndex">specify the start index</param>
        public void InitSlidShow(int StartIndex)
        {
            m_IsSlidShowInited = true;
            StartIndex = StartIndex >= 0 ? StartIndex : 0;
            StartIndex = StartIndex < m_ItemsList.Count ? StartIndex : m_ItemsList.Count - 1;
            ItemsContainer.Children.Clear();
            if (StartIndex - 1 < 0)
            {
                ItemsContainer.Children.Add(GetScrollItem(new SlideShowItem(null)));
            }
            else
            {
                ItemsContainer.Children.Add(GetScrollItem(m_ItemsList[StartIndex - 1]));
            }
            if (StartIndex >= 0)
            {
                m_CurrentIndex = StartIndex;
                ItemsContainer.Children.Add(GetScrollItem(m_ItemsList[StartIndex]));
                if (StartIndex + 1 < m_ItemsList.Count)
                {
                    ItemsContainer.Children.Add(GetScrollItem(m_ItemsList[StartIndex + 1]));
                }
                else
                {
                    ItemsContainer.Children.Add(GetScrollItem(new SlideShowItem(null)));
                }
                NotifyCurrentItemChanged();
            }
        }

        /// <summary>
        /// clear the items
        /// </summary>
        /// <param name="isClearDataSource">if true clear the items source,otherwise do not clear the items source</param>
        public void Clear(bool isClearDataSource)
        {
            m_CurrentIndex = -1;
            m_IsSlidShowInited = false;
            if (m_AutoPlayThread != null)
            {
                m_AutoPlayThread.Abort();
                m_AutoPlayThread = null;
            }
            ItemsContainer.Children.Clear();
            if (isClearDataSource)
            {
                m_ItemsList.Clear();
            }
        }

        /// <summary>
        /// scroll to the next item
        /// </summary>
        public void Next()
        {
            if (m_IsEnableScroll)
            {
                IsAutoPlay = false;
                if (m_CurrentIndex == m_ItemsList.Count - 1)
                {
                    ElasticityAnimation(true);
                }
                else
                {
                    Scroll(true);
                }
            }
        }

        /// <summary>
        /// Scroll to the previous item
        /// </summary>
        public void Previous()
        {
            if (m_IsEnableScroll)
            {
                IsAutoPlay = false;
                if (m_CurrentIndex == 0)
                {
                    ElasticityAnimation(false);
                }
                else
                {
                    Scroll(false);
                }
            }
        }

        /// <summary>
        /// auto play method
        /// </summary>
        public void AutoPaly()
        {
            if (!m_IsSlidShowInited && m_ItemsList.Count > 0)
            {
                InitSlidShow(0);
            }
            if (m_CurrentIndex == -1 && m_ItemsList.Count > 0)
            {
                m_CurrentIndex = 0;
            }
            if (!IsAutoPlay && m_IsEnableScroll)
            {
                IsAutoPlay = true;
                IsAutoPlayImage = true;
                bool _IsFirstFlag = true;
                m_AutoPlayThread = new Thread(new ThreadStart(
                    delegate
                    {
                        while (IsAutoPlay)
                        {
                            if (_IsFirstFlag)
                            {
                                if (CurrentItem.FancyType == FancyType.Favorite)
                                    Thread.Sleep(m_AutoPlayTime * 2);
                                else
                                    Thread.Sleep(m_AutoPlayTime);
                                _IsFirstFlag = false;
                            }
                            Action ac = delegate
                            {
                                int _Start = m_CurrentIndex;
                                while (true)
                                {
                                    if ((m_CurrentIndex + 1) >= m_ItemsList.Count)
                                    {
                                        if (m_ItemsList[0].FancyType == FancyType.DisLike)
                                        {
                                            if (_Start == 0)
                                                break;
                                            m_CurrentIndex = 1;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (m_ItemsList[m_CurrentIndex + 1].FancyType == FancyType.DisLike)
                                        {
                                            m_CurrentIndex++;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    if (_Start == m_CurrentIndex)
                                        break;
                                }
                                if (CurrentIndex == m_ItemsList.Count - 1)
                                {
                                    CurrentIndex = 0;
                                }
                                else
                                {
                                    if (CurrentIndex < m_ItemsList.Count - 1)
                                    {
                                        CurrentIndex += 1;
                                    }
                                }
                            };
                            this.Dispatcher.BeginInvoke(ac, null);
                            int _Count = 1;
                            if (CurrentItem.FancyType == FancyType.Favorite)
                                _Count = 2;
                            Thread.Sleep(m_AutoPlayTime * _Count + 150);
                        }
                    }));
                m_AutoPlayThread.IsBackground = true;
                m_AutoPlayThread.Start();
            }
        }

        /// <summary>
        /// User stop auto play
        /// </summary>
        public void StopPlay()
        {
            IsAutoPlay = false;
        }

        public ImageOrientation GetOrientation(string orientationName)
        {
            switch (orientationName.ToLower())
            {
                case "normal":
                    return ImageOrientation.Normal;
                case "rotatedrightandmirroredvertically":
                    return ImageOrientation.RightAndLeft;
                case "rotated180":
                    return ImageOrientation.Rotated180;
                case "rotatedright":
                    return ImageOrientation.RotatedRight;
                default:
                    return ImageOrientation.None;
            }
        }

        public void AddItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj, FancyType fancyType)
        {
            SlideShowItem Item = new SlideShowItem(Obj, CuxObj, SlideShowItemResetObj, fancyType);
            try
            {
                ImageSlideShow.Core.ExifLibrary.ExifFile data = ImageSlideShow.Core.ExifLibrary.ExifFile.Read(CuxObj.ToString());
                if (data != null && Item != null)
                {
                    ImageSlideShow.Core.ExifLibrary.ExifProperty property = data[ExifLibrary.ExifTag.Orientation];
                    switch (GetOrientation(property.Value.ToString()))
                    {
                        case ImageOrientation.RightAndLeft:
                            ((Picture)Item.ControlObject).RotateAngle = -90;
                            break;
                        case ImageOrientation.Rotated180:
                            ((Picture)Item.ControlObject).RotateAngle = 180;
                            break;
                        case ImageOrientation.RotatedRight:
                            ((Picture)Item.ControlObject).RotateAngle = 90;
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                string i = ex.Message;
            };

            //BitmapFrame bf = BitmapFrame.Create(new Uri(CuxObj.ToString(), UriKind.RelativeOrAbsolute), BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
            //bf.Freeze();
            //((Picture)Item.ControlObject).ImageStretch = bf.PixelWidth >= 1920 ? Stretch.Uniform : Stretch.None;

            m_ItemsList.Add(Item);
            if (m_IsSlidShowInited)
            {
                if (ItemsContainer.Children.Count < 3)
                {
                    TransitionElement _Item = GetScrollItem(Item);
                    ItemsContainer.Children.Add(_Item);
                }
                else
                {
                    CheckItems();
                }
            }
        }

        /// <summary>
        /// Add item
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="CuxObj"></param>
        /// <param name="SlideShowItemResetObj"></param>
        public void AddItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj)
        {
            this.AddItem(Obj, CuxObj, SlideShowItemResetObj, FancyType.Normal);
        }

        public void AddItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj,string fullName)
        {
            SlideShowItem Item = new SlideShowItem(Obj, CuxObj, SlideShowItemResetObj, FancyType.Normal);
            try
            {
                ImageSlideShow.Core.ExifLibrary.ExifFile data = ImageSlideShow.Core.ExifLibrary.ExifFile.Read(CuxObj.ToString());
                if (data != null && Item != null)
                {
                    ImageSlideShow.Core.ExifLibrary.ExifProperty property = data[ExifLibrary.ExifTag.Orientation];
                    switch (GetOrientation(property.Value.ToString()))
                    {
                        case ImageOrientation.RightAndLeft:
                            ((Picture)Item.ControlObject).RotateAngle = -90;
                            break;
                        case ImageOrientation.Rotated180:
                            ((Picture)Item.ControlObject).RotateAngle = 180;
                            break;
                        case ImageOrientation.RotatedRight:
                            ((Picture)Item.ControlObject).RotateAngle = 90;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string i = ex.Message;
            };

            BitmapFrame bf = BitmapFrame.Create(new Uri(fullName, UriKind.RelativeOrAbsolute), BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.None);
            bf.Freeze();
            ((Picture)Item.ControlObject).ImageStretch = bf.PixelWidth >= 1920 ? Stretch.Uniform : Stretch.None;

            m_ItemsList.Add(Item);
            if (m_IsSlidShowInited)
            {
                if (ItemsContainer.Children.Count < 3)
                {
                    TransitionElement _Item = GetScrollItem(Item);
                    ItemsContainer.Children.Add(_Item);
                }
                else
                {
                    CheckItems();
                }
            }
        }

        /// <summary>
        /// Add Item
        /// </summary>
        /// <param name="Item"></param>
        public void AddItem(object Obj)
        {
            this.AddItem(Obj, null, null, FancyType.Normal);
        }

        public void InsertItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj, FancyType fancyType)
        {
            SlideShowItem Item = new SlideShowItem(Obj, CuxObj, SlideShowItemResetObj, fancyType);
            try
            {
                ImageSlideShow.Core.ExifLibrary.ExifFile data = ImageSlideShow.Core.ExifLibrary.ExifFile.Read(CuxObj.ToString());
                if (data != null && Item != null)
                {
                    ImageSlideShow.Core.ExifLibrary.ExifProperty property = data[ExifLibrary.ExifTag.Orientation];
                    switch (GetOrientation(property.Value.ToString()))
                    {
                        case ImageOrientation.RightAndLeft:
                            ((Picture)Item.ControlObject).RotateAngle = -90;
                            break;
                        case ImageOrientation.Rotated180:
                            ((Picture)Item.ControlObject).RotateAngle = 180;
                            break;
                        case ImageOrientation.RotatedRight:
                            ((Picture)Item.ControlObject).RotateAngle = 90;
                            break;
                    }
                }
            }
            catch { };
            m_ItemsList.Insert(0, Item);
            if (m_IsSlidShowInited)
            {
                m_CurrentIndex++;
                if (ItemsContainer.Children.Count > 0)
                {
                    TransitionElement TE = ItemsContainer.Children[0] as TransitionElement;
                    if (TE != null && TE.Content == null)
                    {
                        ItemsContainer.Children.RemoveAt(0);
                        ItemsContainer.Children.Insert(0, GetScrollItem(Item));
                    }
                }
            }
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="CuxObj"></param>
        /// <param name="SlideShowItemResetObj"></param>
        public void InsertItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj)
        {
            this.InsertItem(Obj, CuxObj, SlideShowItemResetObj, FancyType.Normal);
        }
        /// <summary>
        /// Insert Item at the first
        /// </summary>
        /// <param name="Item">object</param>
        public void InsertItem(object Obj)
        {
            this.InsertItem(Obj, null, null, FancyType.Normal);
        }
        #endregion

        #region Events

        void EnableScrollHandler(object sender, RoutedEventArgs e)
        {
            m_IsEnableScroll = true;
            m_TouchDownCount = 0;
            m_ManipulationFlag = false;
            if (MultiExitModeEvent != null)
                MultiExitModeEvent(sender, e);
        }

        void DisableScrollHandler(object sender, RoutedEventArgs e)
        {
            m_IsEnableScroll = false;
            if (MultiModeEvent != null)
                MultiModeEvent(sender, e);
        }

        void ItemsContainer_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            if (m_TouchDownCount < 0)
                m_TouchDownCount = 0;
            if (m_TouchDownCount == 0)
                SetMouseStatus(true, e.GetTouchPoint(null).Position);
            m_IsTouchEvent = true;
            m_TouchDownCount += 1;
            if (m_TouchDownCount == 2)
                m_ManipulationFlag = true;
        }

        void ItemsContainer_PreviewTouchMove(object sender, TouchEventArgs e)
        {
            if (m_IsTouchEvent)
            {
                if (m_MouseDownFlag)
                {
                    GetDirection(e.GetTouchPoint(null).Position);
                }
            }
        }

        void ItemsContainer_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            if (m_IsTouchEvent)
            {
                if (m_MouseDownFlag)
                {
                    SetMouseStatus(false, e.GetTouchPoint(null).Position);
                }
                m_IsTouchEvent = false;
            }
        }

        /// <summary>
        /// Prepare Move Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemsContainer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!m_IsTouchEvent)
                SetMouseStatus(true, e.GetPosition(null));
        }

        /// <summary>
        /// Move Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemsContainer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!m_IsTouchEvent)
            {
                if (m_MouseDownFlag)
                {
                    GetDirection(e.GetPosition(null));
                }
            }
        }

        /// <summary>
        /// Move to the define image when mouse left button up  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemsContainer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!m_IsTouchEvent)
            {
                if (m_MouseDownFlag)
                {
                    SetMouseStatus(false, e.GetPosition(null));
                }
            }
        }

        /// <summary>
        /// Move to the define image when mouse leave  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ItemsContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!m_IsTouchEvent)
            {
                if (m_MouseDownFlag)
                {
                    SetMouseStatus(false, e.GetPosition(null));
                }
            }
        }
        #endregion

        #region Delegate
        /// <summary>
        /// Delegate class
        /// </summary>
        public delegate void SlideShowEventHandler();

        /// <summary>
        /// drag eventagrs
        /// </summary>
        /// <param name="e"></param>
        public delegate void CurrentItemChangedEventHandler(CurrentItemChangedEventArgs e);
        #endregion

        public SlideShow()
        {
            InitializeComponent();
            InitControls();
         
        }
    }

    /// <summary>
    /// Scroll Enable changed event handeler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ScrollEnabledChangedRoutedHandler(object sender, RoutedEventArgs e);

    /// <summary>
    /// Define the scroll direction
    /// </summary>
    public enum ScrollDirection
    {
        Up,
        Down,
        Left,
        Right,
        None
    }

    /// <summary>
    /// Drag event args struct
    /// </summary>
    public class CurrentItemChangedEventArgs : EventArgs
    {
        public ScrollDirection Direction { get; set; }
        public bool IsCancel = false;
        public int CurrentIndex { get; set; }
        public SlideShowItem CurrentItem { get; set; }
        public CurrentItemChangedEventArgs()
        {
            Direction = ScrollDirection.None;
            CurrentIndex = -1;
        }
    }

    /// <summary>
    /// reset the datasouce delegate
    /// </summary>
    /// <param name="Obj">original object</param>
    /// <returns>return the source which has been reseted</returns>
    public delegate object DeleSlideShowItemResetObj(object Obj);

    /// <summary>
    /// Slide show item object
    /// </summary>
    public class SlideShowItem
    {
        public SlideShowItem(object Obj)
            : this(Obj, null, null, FancyType.Normal)
        {
        }

        public SlideShowItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj)
            : this(Obj, CuxObj, SlideShowItemResetObj, FancyType.Normal)
        {
        }

        public SlideShowItem(object Obj, object CuxObj, DeleSlideShowItemResetObj SlideShowItemResetObj, FancyType itemType)
        {
            this.Obj = Obj;
            this.CuxObj = CuxObj;
            this.SlideShowItemResetObj = SlideShowItemResetObj;
            this.FancyType = itemType;
        }

        private object m_Obj = null;
        private object m_TmpObj = null;
        private FancyType m_FancyType = FancyType.Normal;

        public FancyType FancyType
        {
            get
            {
                return m_FancyType;
            }
            set
            {
                m_FancyType = value;
            }
        }

        public object Obj
        {
            get
            {
                if (SlideShowItemResetObj != null)
                {
                    if (m_TmpObj == null)
                    {
                        m_TmpObj = SlideShowItemResetObj(m_Obj);
                    }
                    if (m_TmpObj == null)
                    {
                        m_TmpObj = new Grid();
                    }
                    return m_TmpObj;
                }
                else
                {
                    return m_Obj;
                }
            }
            set
            {
                m_TmpObj = null;
                m_Obj = value;

                if (m_ControlObject != null)
                {
                    if (m_ControlObject is Picture)
                    {
                        Picture p = (Picture)m_ControlObject;
                        p.Source = m_Obj;
                    }
                    if (m_ControlObject is ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models.Control)
                    {
                        ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models.Control p = (ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models.Control)m_ControlObject;
                        p.Source = m_Obj;
                    }
                }
            }
        }

        public object CuxObj
        {
            get;
            set;
        }

        public DeleSlideShowItemResetObj SlideShowItemResetObj
        {
            get;
            set;
        }

        private object m_ControlObject = null;
        public object ControlObject
        {
            get
            {
                if (m_ControlObject == null)
                {
                    if (Obj != null && (Obj is string || Obj is MemoryStream))
                    {
                        m_ControlObject = new Picture(Obj);
                    }
                    if (Obj != null && Obj is FrameworkElement)
                    {
                        m_ControlObject = new ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models.Control(m_Obj);
                    }
                }
                return m_ControlObject;
            }
        }

        public double Angle
        {
            get;
            set;
        }
    }

    public enum EffectType
    {
        Random,
        Fade,
        FadeWipe2,
        Melt,
        HorizontalWipe,
        VerticalWipe,
        DiagonalWipe,
        RotateWipe,
        DoubleRotateWipe,
        VerticalBlinds,
        HorizontalBlinds,
        Diamonds,
        Checkerboard,
        Roll,
        Dots,
        FadeAndGrow,
        TranslateTransition2D,
        Flip3D,
        Door3D,
        Rotate3D,
        Spin3D,
        Explosion3D,
        Cloth,
        Page
    }

    public enum FancyType
    {
        Favorite,
        DisLike,
        Normal
    }

    public enum ImageOrientation
    {
        Normal,
        RightAndLeft,
        Rotated180,
        RotatedRight,
        None
    }

}
