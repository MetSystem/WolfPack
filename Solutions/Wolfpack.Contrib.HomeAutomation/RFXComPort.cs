using System.IO.Ports;
using System.Text;

namespace Wolfpack.Contrib.HomeAutomation
{
    public class RFXComPort : SerialPort
    {
        public RFXComPort(string portName) : base(portName)
        {
            BaudRate = 4800;
            Parity = Parity.None;
            DataBits = 8;
            Encoding = Encoding.GetEncoding(1252);
            StopBits = StopBits.One;
            Handshake = Handshake.None;
            ReadBufferSize = 4096;
            ReadTimeout = 100;
            WriteTimeout = 500;
        }
    }
}