using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace Impromptu.IO
{
    public class SerialPort : System.IO.Ports.SerialPort, IDisposable
    {

        public new event EventHandler<string> DataReceived;

        public SerialPort(string portName, int baudRate, Parity parity= Parity.None, int dataBits=8, StopBits stopBits=StopBits.One) : base(portName,baudRate,parity,dataBits,stopBits)
        {
            this.Open();

            this.DiscardOutBuffer();
            this.DiscardInBuffer();

            Task.Run(() => DataCheck());
        }

        private void DataCheck()
        {
            while (true)
            {
                if (this.IsOpen && this.BytesToRead > 0)
                    DataReceived(this, this.ReadExisting());
                else
                    System.Threading.Thread.Sleep(25); // min.
            }
        }

        #region IDisposable implementation

        void IDisposable.Dispose()
        {
            if(this.IsOpen)
              this.Close();
        }

        #endregion
    }
}
