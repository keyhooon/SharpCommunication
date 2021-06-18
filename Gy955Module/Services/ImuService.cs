using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.GY955.Codec;
using SharpCommunication.Module.Services;
using SharpCommunication.Transport.SerialPort;

namespace ImuModule.Services
{
    public class ImuService : TransportService<Gy955>
    {
        public ImuService(SerialPortDataTransport<Gy955> dataTransport, Codec<Gy955> codec) : base(dataTransport, codec)
        {
            
            ((INotifyCollectionChanged)dataTransport.Channels).CollectionChanged += (sender, e) =>
            {
                if (e.NewItems != null)
                    foreach (IChannel<Gy955> eNewItem in e.NewItems)
                    {
                        eNewItem.DataReceived += ENewItem_DataReceived;
                    }

                if (e.OldItems != null)
                    foreach (IChannel<Gy955> eOldItem in e.OldItems)
                    {
                        eOldItem.DataReceived -= ENewItem_DataReceived;
                    }
            };


        }

        private void ENewItem_DataReceived(object sender, DataReceivedEventArg<Gy955> e)
        {
            Q4 = e.Data.Output.Q4;
            Yrp = e.Data.Output.Yrp;
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Yrp)));
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Q4)));
        }


        public Vector3? Yrp { get; private set; }

        public Vector4? Q4 { get; private set; }
    }
}
