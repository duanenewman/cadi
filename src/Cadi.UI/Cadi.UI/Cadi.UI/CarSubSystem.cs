using Xamarin.Forms.Xaml;
using System;
using System.Threading.Tasks;

namespace Cadi.UI
{
    public class CarSubSystem : ICarSubSystem
    {
        public bool IsAcOn {
            get => GpioService.GetPinState(Pin.GPIO18) == GPIOState.High;
            set => GpioService.SetPinState(Pin.GPIO18, value ? GPIOState.High : GPIOState.Low);
        }

        public IGpioService GpioService { get; }

        private bool IsRunning { get; set; }
        private bool WasPin17Closed { get; set; }

        public CarSubSystem(IGpioService gpioService)
        {
            GpioService = gpioService;
        }

        public void Start()
        {
            GpioService.ExportPin(Pin.GPIO18, GPIODirection.Out, GPIOState.Low);
            GpioService.ExportPin(Pin.GPIO17, GPIODirection.In, GPIOState.Low);
            IsRunning = true;
            
            Task.Run(async () =>
            {
                while (IsRunning)
                {
                    var isPin17Closed = GpioService.GetPinState(Pin.GPIO17) == GPIOState.High;

                    var changed = (!WasPin17Closed && isPin17Closed) || (WasPin17Closed && !isPin17Closed);
                    if (changed)
                    {
                        if (isPin17Closed)
                        {
                            var currentState = GpioService.GetPinState(Pin.GPIO18);

                            GpioService.SetPinState(Pin.GPIO18,
                                currentState == GPIOState.Low ? GPIOState.High : GPIOState.Low);
                        }

                        WasPin17Closed = isPin17Closed;
                    }

                    await Task.Delay(100);
                }
            });
        }

        public void Stop()
        {
            IsRunning = false;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                IsRunning = false;

                if (disposing)
                {
                    GpioService.Dispose();
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CarSubSystem() {
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
