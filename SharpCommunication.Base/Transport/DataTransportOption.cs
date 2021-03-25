using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SharpCommunication.Transport
{
    public class DataTransportOption 
    {


        public DataTransportOption()
        {
            AutoCheckIsOpen = true;
            AutoCheckIsOpenTime = 500;
        }

        public bool AutoCheckIsOpen { get; set; }

        public int AutoCheckIsOpenTime { get; set; }
    }
}
