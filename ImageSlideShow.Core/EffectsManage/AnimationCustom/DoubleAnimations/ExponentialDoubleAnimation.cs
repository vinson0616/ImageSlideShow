using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom.DoubleAnimations
{
    internal class ExponentialDoubleAnimation : DoubleAnimationBase
    {
        public enum EdgeBehaviorEnum
        {
            EaseIn, EaseOut, EaseInOut
        }

        public static readonly DependencyProperty EdgeBehaviorProperty =
            DependencyProperty.Register("EdgeBehavior", typeof(EdgeBehaviorEnum), typeof(ExponentialDoubleAnimation), new PropertyMetadata(EdgeBehaviorEnum.EaseIn));

        public static readonly DependencyProperty PowerProperty =
            DependencyProperty.Register("Power", typeof(double), typeof(ExponentialDoubleAnimation), new PropertyMetadata(2.0));


        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From",
                typeof(double?),
                typeof(ExponentialDoubleAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To",
                typeof(double?),
                typeof(ExponentialDoubleAnimation),
                new PropertyMetadata(null));

        /// <summary>
        /// which side gets the effect
        /// </summary>
        public EdgeBehaviorEnum EdgeBehavior
        {
            get
            {
                return (EdgeBehaviorEnum)GetValue(EdgeBehaviorProperty);
            }
            set
            {
                SetValue(EdgeBehaviorProperty, value);
            }
        }

        /// <summary>
        /// exponential rate of growth
        /// </summary>
        public double Power
        {
            get
            {
                return (double)GetValue(PowerProperty);
            }
            set
            {
                if (value > 0.0)
                {
                    SetValue(PowerProperty, value);
                }
                else
                {
                    throw new ArgumentException("cannot set power to less than 0.0. Value: " + value);
                }
            }
        }

        /// <summary>
        /// Specifies the starting value of the animation.
        /// </summary>
        public double? From
        {
            get
            {
                return (double?)GetValue(FromProperty);
            }
            set
            {

                SetValue(FromProperty, value);

            }
        }

        /// <summary>
        /// Specifies the ending value of the animation.
        /// </summary>
        public double? To
        {
            get
            {
                return (double?)GetValue(ToProperty);
            }
            set
            {

                SetValue(ToProperty, value);

            }
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock clock)
        {
            double returnValue;
            double start = From != null ? (double)From : defaultOriginValue;
            double delta = To != null ? (double)To - start : defaultOriginValue - start;

            switch (this.EdgeBehavior)
            {
                case EdgeBehaviorEnum.EaseIn:
                    returnValue = easeIn(clock.CurrentProgress.Value, start, delta, Power);
                    break;
                case EdgeBehaviorEnum.EaseOut:
                    returnValue = easeOut(clock.CurrentProgress.Value, start, delta, Power);
                    break;
                case EdgeBehaviorEnum.EaseInOut:
                default:
                    returnValue = easeInOut(clock.CurrentProgress.Value, start, delta, Power);
                    break;
            }
            return returnValue;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new ExponentialDoubleAnimation();
        }

        private static double easeIn(double timeFraction, double start, double delta, double power)
        {
            double returnValue = 0.0;

            // math magic: simple exponential growth
            returnValue = Math.Pow(timeFraction, power);
            returnValue *= delta;
            returnValue = returnValue + start;
            return returnValue;
        }
        private static double easeOut(double timeFraction, double start, double delta, double power)
        {
            double returnValue = 0.0;

            // math magic: simple exponential decay
            returnValue = Math.Pow(timeFraction, 1 / power);
            returnValue *= delta;
            returnValue = returnValue + start;
            return returnValue;
        }
        private static double easeInOut(double timeFraction, double start, double delta, double power)
        {
            double returnValue = 0.0;

            // we cut each effect in half by multiplying the time fraction by two and halving the distance.
            if (timeFraction <= 0.5)
            {
                returnValue = easeOut(timeFraction * 2, start, delta / 2, power);
            }
            else
            {
                returnValue = easeIn((timeFraction - 0.5) * 2, start, delta / 2, power);
                returnValue += (delta / 2);
            }
            return returnValue;
        }
    }
}
