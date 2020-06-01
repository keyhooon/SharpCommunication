using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharpCommunication.Transport
{
    public class DataTransportOption : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DataTransportOption(bool autoCheckIsOpen = true, int autoCheckIsOpenTime = 500 )
        {
            _autoCheckIsOpen = autoCheckIsOpen;
            _autoCheckIsOpenTime = autoCheckIsOpenTime;
        }

        private bool _autoCheckIsOpen;
        private int _autoCheckIsOpenTime;


        public bool AutoCheckIsOpen  
        { 
            get => _autoCheckIsOpen;
            set
            {
                if (_autoCheckIsOpen == value)
                    return;
                _autoCheckIsOpen = value;
                OnPropertyChanged();
            }
        }

        public int AutoCheckIsOpenTime 
        { 
            get => _autoCheckIsOpenTime;
            set
            {
                if (_autoCheckIsOpenTime == value)
                    return;
                _autoCheckIsOpenTime = value;
                OnPropertyChanged();
            }
        }
    }
}
