using System;

namespace SharpCommunication.Base.Channels.ChannelTools
{
    public class IoMonitor
    {

        public Channel MonitoredChannel { get; }

        public IoMonitor(Channel monitoredChannel)
        {
            MonitoredChannel = monitoredChannel;
            MonitorBeginTime = DateTime.Now;
            monitoredChannel.DataReceived += (sender, arg) => { DataReceivedCount++; };
        }
        public DateTime MonitorBeginTime { get; set; }
        public int DataReceivedCount { get; set; }
    }
}
