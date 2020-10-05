using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;
using System.ComponentModel;

namespace ImageSlideShow.Core.EffectsManage.TransitionPresenter.Models
{
    public class Picture : INotifyPropertyChanged
    {
        public Picture(object source)
        {
            m_Source = source;
        }

        private bool m_EnableZoomMode = true;

        public bool EnableZoomMode
        {
            get
            {
                return m_EnableZoomMode;
            }
            set
            {
                m_EnableZoomMode = value;
                NotifyPropertyChanged("EnableZoomMode");
            }
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

        private Stretch m_ImageStretch = Stretch.Uniform;
        public Stretch ImageStretch
        {
            get { return m_ImageStretch; }
            set
            {
                m_ImageStretch = value;
                NotifyPropertyChanged("ImageStretch");
            }
        }

        private double m_RotateAngle = 0.0;
        private double m_ScaleX = 1;
        private double m_ScaleY = 1;

        public double ScaleX
        {
            get
            {
                return m_ScaleX;
            }
            set
            {
                m_ScaleX = value;
                NotifyPropertyChanged("ScaleX");
            }
        }

        private bool m_IsZoomMode = false;
        public bool IsZoomMode
        {
            get
            {
                return m_IsZoomMode;
            }
            set
            {
                m_IsZoomMode = value;
                NotifyPropertyChanged("IsZoomMode");
            }
        }

        public double ScaleY
        {
            get
            {
                return m_ScaleY;
            }
            set
            {
                m_ScaleY = value;
                NotifyPropertyChanged("ScaleY");
            }
        }

        public double RotateAngle
        {
            get
            {
                return m_RotateAngle;
            }
            set
            {
                m_RotateAngle = value;
                NotifyPropertyChanged("RotateAngle");
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
