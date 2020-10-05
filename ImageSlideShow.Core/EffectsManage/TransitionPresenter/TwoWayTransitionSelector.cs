using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ImageSlideShow.Core.EffectsManage.TransitionPresenter
{
    internal enum TransitionDirection
    {
        Forward,
        Backward,
    }

    //Choose between a forward and backward transition based on the Direction property
    internal class TwoWayTransitionSelector : TransitionSelector
    {
        public TwoWayTransitionSelector() { }

        public Transition Forward
        {
            get { return (Transition)GetValue(ForwardProperty); }
            set { SetValue(ForwardProperty, value); }
        }

        public static readonly DependencyProperty ForwardProperty =
            DependencyProperty.Register("Forward", typeof(Transition), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));

        public Transition Backward
        {
            get { return (Transition)GetValue(BackwardProperty); }
            set { SetValue(BackwardProperty, value); }
        }

        public static readonly DependencyProperty BackwardProperty =
            DependencyProperty.Register("Backward", typeof(Transition), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(null));


        public TransitionDirection Direction
        {
            get { return (TransitionDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(TransitionDirection), typeof(TwoWayTransitionSelector), new UIPropertyMetadata(TransitionDirection.Forward));


        public override Transition SelectTransition(object oldContent, object newContent)
        {
            return Direction == TransitionDirection.Forward ? Forward : Backward;
        }
    }
}
