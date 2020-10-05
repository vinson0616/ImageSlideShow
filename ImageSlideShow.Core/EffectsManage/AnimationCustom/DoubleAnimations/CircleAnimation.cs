using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom.DoubleAnimations
{
    internal class CircleAnimation : DoubleAnimationBase
    {
        public enum DirectionEnum
        {
            XDirection, YDirection
        }

        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(DirectionEnum), typeof(CircleAnimation), new PropertyMetadata(DirectionEnum.XDirection));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(CircleAnimation), new PropertyMetadata((double)10));

        /// <summary>
        /// distance from origin to polar coordinate
        /// </summary>
        public double Radius
        {
            get
            {
                return (double)GetValue(RadiusProperty);
            }
            set
            {
                if (value > 0.0)
                {
                    SetValue(RadiusProperty, value);
                }
                else
                {
                    throw new ArgumentException("a radius of " + value + " is not allowed!");
                }
            }
        }

        /// <summary>
        /// are we measuring in the X or Y direction?
        /// </summary>
        public DirectionEnum Direction
        {
            get
            {
                return (DirectionEnum)GetValue(DirectionProperty);
            }
            set
            {
                SetValue(DirectionProperty, value);
            }
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock clock)
        {
            double returnValue;
            double time = clock.CurrentProgress.Value;

            // math magic: calculate new coordinates using polar coordinate equations. This requires two 
            // animations to be wired up in order to move in a circle, since we don't make any assumptions
            // about what we're animating (e.g. a TranslateTransform). 
            if (Direction == DirectionEnum.XDirection)
            {
                returnValue = Math.Cos(2 * Math.PI * time);
            }
            else
            {
                returnValue = Math.Sin(2 * Math.PI * time);
            }

            // Need to add the defaultOriginValue so that composition works.
            return returnValue * Radius + defaultOriginValue;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new CircleAnimation();
        }
    }
}
