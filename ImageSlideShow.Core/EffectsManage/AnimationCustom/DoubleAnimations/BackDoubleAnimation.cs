using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Animation;

namespace ImageSlideShow.Core.EffectsManage.AnimationCustom.DoubleAnimations
{
    internal class BackDoubleAnimation : DoubleAnimationBase
    {
        public enum EdgeBehaviorEnum
        {
            EaseIn, EaseOut, EaseInOut
        }

        public static readonly DependencyProperty EdgeBehaviorProperty =
            DependencyProperty.Register("EdgeBehavior", typeof(EdgeBehaviorEnum), typeof(BackDoubleAnimation), new PropertyMetadata(EdgeBehaviorEnum.EaseIn));

        public static readonly DependencyProperty AmplitudeProperty =
            DependencyProperty.Register("Amplitude", typeof(double), typeof(BackDoubleAnimation), new PropertyMetadata(4.0));

        public static readonly DependencyProperty SuppressionProperty =
            DependencyProperty.Register("Suppression", typeof(double), typeof(BackDoubleAnimation), new PropertyMetadata(2.0));

        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From",
                typeof(double?),
                typeof(BackDoubleAnimation),
                new PropertyMetadata(null));

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To",
                typeof(double?),
                typeof(BackDoubleAnimation),
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
        /// how much backwards motion is there in the effect
        /// </summary>
        public double Amplitude
        {
            get
            {
                return (double)GetValue(AmplitudeProperty);
            }
            set
            {
                SetValue(AmplitudeProperty, value);
            }
        }

        /// <summary>
        /// how quickly the effect drops off vs. the entire timeline
        /// </summary>
        public double Suppression
        {
            get
            {
                return (double)GetValue(SuppressionProperty);
            }
            set
            {
                SetValue(SuppressionProperty, value);
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
                    returnValue = easeIn(clock.CurrentProgress.Value, start, delta, Amplitude, Suppression);
                    break;
                case EdgeBehaviorEnum.EaseOut:
                    returnValue = easeOut(clock.CurrentProgress.Value, start, delta, Amplitude, Suppression);
                    break;
                case EdgeBehaviorEnum.EaseInOut:
                default:
                    returnValue = easeInOut(clock.CurrentProgress.Value, start, delta, Amplitude, Suppression);
                    break;
            }
            return returnValue;
        }

        protected override Freezable CreateInstanceCore()
        {

            return new BackDoubleAnimation();
        }


        private static double easeOut(double timeFraction, double start, double delta, double amplitude, double suppression)
        {
            double returnValue = 0.0;
            double frequency = 0.5;

            // math magic: The sine gives us the right wave, the timeFraction * 0.5 (frequency) gives us only half 
            // of the full wave, the amplitude gives us the relative height of the peak, and the exponent makes the 
            // effect drop off more quickly by the "suppression" factor. 
            returnValue = Math.Pow((timeFraction), suppression)
                          * amplitude * Math.Sin(2 * Math.PI * timeFraction * frequency) + timeFraction;
            returnValue = (returnValue * delta);
            returnValue += start;
            return returnValue;
        }

        private static double easeIn(double timeFraction, double start, double delta, double amplitude, double suppression)
        {
            double returnValue = 0.0;
            double frequency = 0.5;

            // math magic: The sine gives us the right wave, the timeFraction * 0.5 (frequency) gives us only half 
            // of the full wave (flipped by multiplying by -1 so that we go "backwards" first), the amplitude gives 
            // us the relative height of the peak, and the exponent makes the effect start later by the "suppression" 
            // factor. 
            returnValue = Math.Pow((1 - timeFraction), suppression)
                          * amplitude * Math.Sin(2 * Math.PI * timeFraction * frequency) * -1 + timeFraction;
            returnValue = (returnValue * delta);
            returnValue += start;
            return returnValue;
        }

        private static double easeInOut(double timeFraction, double start, double delta, double amplitude, double suppression)
        {
            double returnValue = 0.0;

            // we cut each effect in half by multiplying the time fraction by two and halving the distance.
            if (timeFraction <= 0.5)
            {
                return easeIn(timeFraction * 2, start, delta / 2, amplitude, suppression);
            }
            else
            {
                returnValue = easeOut((timeFraction - 0.5) * 2, start, delta / 2, amplitude, suppression);
                returnValue += (delta / 2);
            }
            return returnValue;
        }
    }
}
