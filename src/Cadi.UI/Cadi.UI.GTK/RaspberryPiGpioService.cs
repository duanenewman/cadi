using System;
using System.Collections.Generic;

namespace Cadi.UI.GTK
{
	public class RaspberryPiGpioService : IGpioService, IDisposable
    {
        private Dictionary<Pin, GPIOPinDriver> PinDrivers { get; set; } = new Dictionary<Pin, GPIOPinDriver>();

        public void ExportPin(Pin pin, GPIODirection direction, GPIOState initialValue)
        {
            if (!PinDrivers.ContainsKey(pin))
            {
                PinDrivers.Add(pin, new GPIOPinDriver(pin, direction, initialValue));
            }
        }

        public GPIOState GetPinState(Pin pin)
        {
            if (!PinDrivers.ContainsKey(pin)) throw new ArgumentOutOfRangeException(nameof(pin));

            var pinDriver = PinDrivers[pin];
            return pinDriver.State;
        }

        public void SetPinState(Pin pin, GPIOState state)
        {
            if (!PinDrivers.ContainsKey(pin)) throw new ArgumentOutOfRangeException(nameof(pin));

            var pinDriver = PinDrivers[pin];
            pinDriver.State = state;
            return;
        }

        public void UnexportPin(Pin pin)
        {
            if (PinDrivers.ContainsKey(pin))
            {
                var pinDriver = PinDrivers[pin];
                PinDrivers.Remove(pin);
                pinDriver.Dispose();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var pinDriver in PinDrivers)
                    {
                        pinDriver.Value.Dispose();
                    }

                    PinDrivers.Clear();
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RaspberryPiGpioService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
