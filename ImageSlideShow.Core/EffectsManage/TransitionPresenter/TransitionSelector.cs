using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace ImageSlideShow.Core.EffectsManage.TransitionPresenter
{
    // Allows different transitions to run based on the old and new contents
    // Override the SelectTransition method to return the transition to apply
    internal class TransitionSelector : DependencyObject
    {
        public virtual Transition SelectTransition(object oldContent, object newContent)
        {
            return null;
        }
    }
}
