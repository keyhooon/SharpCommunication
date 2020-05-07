using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SharpCommunication.Base.Transport
{
    public class DataTransportOption : INotifyPropertyChanged
    {
        public DataTransportOption(bool isOpenCheckAutomatically = true, int isOpenCheckAutomaticallyDelay = 500)
        {
            _isOpenCheckAutomatically = isOpenCheckAutomatically;
            _isOpenCheckAutomaticallyDelay = isOpenCheckAutomaticallyDelay;
        }
        private bool _isOpenCheckAutomatically;
        public bool IsOpenCheckAutomatically
        {
            get => _isOpenCheckAutomatically;
            set
            {
                if (_isOpenCheckAutomatically == value)
                    return;
                _isOpenCheckAutomatically = value;
                OnPropertyChanged();
            }
        }

        private int _isOpenCheckAutomaticallyDelay;
        public int IsOpenCheckAutomaticallyDelay
        {
            get => _isOpenCheckAutomaticallyDelay;
            set
            {
                if (_isOpenCheckAutomaticallyDelay == value)
                    return;
                _isOpenCheckAutomaticallyDelay = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
