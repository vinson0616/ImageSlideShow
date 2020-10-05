using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models
{
    internal class Control : INotifyPropertyChanged
    {
        public Control(object source)
        {
            m_Source = source;
        }

        private object m_Source = null;
        public object Source
        {
            get
            {
                return m_Source;
            }
            set
            {
                m_Source = value;
                NotifyPropertyChanged("Source");
            }
        }

        // Declare the PropertyChanged event.
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
