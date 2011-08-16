using System;
using System.IO.Ports;
using System.Text;
using Wolfpack.Core.Interfaces;
using Wolfpack.Core.Interfaces.Entities;

namespace Wolfpack.Contrib.HomeAutomation
{
    public class SerialPortReceiverActivityConfig : ConfigBase
    {
        public string ComPort { get; set; }
    }

    public class SerialPortReceiverActivity : IActivityPlugin
    {
        protected readonly SerialPortReceiverActivityConfig myConfig;
        protected readonly PluginDescriptor myIdentity;
        protected SerialPort mySerialPort;

        public bool Enabled { get; set; }
        public Status Status { get; set;}

        public SerialPortReceiverActivity(SerialPortReceiverActivityConfig config)
        {
            myConfig = config;
            myIdentity = new PluginDescriptor
                             {
                                 Description = "Receives COM port traffic from an RFXCOM receiver",
                                 Name = "RFXCOMReceiver",
                                 TypeId = new Guid("EA77AF56-8193-4B4E-9E6D-F78F5EA4FD0F")
                             };

            Enabled = myConfig.Enabled;
        }

        public void Initialise()
        {
            mySerialPort = new RFXComPort(myConfig.ComPort);
            mySerialPort.DataReceived += DataReceived;            
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var port = sender as SerialPort;
            var buffer = new byte[port.BytesToRead];
            port.Read(buffer, 0, port.BytesToRead);

            Console.WriteLine("RECEIVED ------------->");
            Console.WriteLine(ByteArrayToString(buffer));
        }

        public static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static byte[] StringToByteArray(String hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (var i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        public PluginDescriptor Identity
        {
            get { return myIdentity; }
        }

        public void Start()
        {
            mySerialPort.Open();
            if (!mySerialPort.IsOpen)
                throw new InvalidOperationException("Port not open");

            mySerialPort.DtrEnable = true;
            mySerialPort.RtsEnable = true;
            mySerialPort.DiscardInBuffer();


            var data = StringToByteArray("21810182C000");
            mySerialPort.Write(data, 0, data.Length);

        }

        public void Stop()
        {
            if (mySerialPort.IsOpen)
                mySerialPort.Close();
        }

        public void Pause()
        {
            
        }

        public void Continue()
        {
            
        }
    }
}