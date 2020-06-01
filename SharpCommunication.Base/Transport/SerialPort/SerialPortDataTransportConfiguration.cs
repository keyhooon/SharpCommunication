﻿using Microsoft.Extensions.Configuration;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace SharpCommunication.Transport.SerialPort
{
    public class SerialPortDataTransportConfiguration : INotifyPropertyChanged
    {
        private Parity _parity;
        private int _baudRate;
        private string _portName;
        private StopBits _stopBits;
        private int _dataBits;

        public SerialPortDataTransportConfiguration(string portName, int baudRate, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits= StopBits.One)
        {
            _stopBits = stopBits;
            _dataBits = dataBits;
            _portName = portName;
            _baudRate = baudRate;
            _parity = parity;
        }

        public string PortName { get => _portName;
            set
            {
                if (_portName == value)
                    return;
                _portName = value;
                OnPropertyChanged();
            }
        }
        public int BaudRate { get => _baudRate;
            set
            {
                if (_baudRate == value)
                    return;
                _baudRate = value;
                OnPropertyChanged();
            }
        }
        public Parity Parity { get => _parity;
            set
            {
                if (_parity == value)
                    return;
                _parity = value;
                OnPropertyChanged();
            }
        }

        public StopBits StopBits
        {
            get => _stopBits;
            set
            {
                if (_stopBits == value)
                    return;
                _stopBits = value;
                OnPropertyChanged();
            }
        }

        public int DataBits
        {
            get => _dataBits;
            set
            {
                if (_dataBits == value)
                    return;
                _dataBits = value;
                OnPropertyChanged();
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public static class SerialPortDataTransportConfigurationExtension
    {
        public static SerialPortDataTransportConfiguration GetSerialPortConfiguration(this IConfiguration configurationRoot)
        {
            var section = configurationRoot.GetSection("SerialPort");
            SerialPortDataTransportConfiguration ret = null;
            if (section.Exists())
                ret = ConfigurationBinder.Get<SerialPortDataTransportConfiguration>(section);
            return ret;
        }
    }
}