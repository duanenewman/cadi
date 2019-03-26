using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cadi.UI.ViewModels;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Platform.GTK;

namespace Cadi.UI.GTK
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Gtk.Application.Init();
            Forms.Init();

            var gtke = new GtkAppExitor();

            MessagingCenter.Subscribe<MainPageViewModel>(gtke, "exit", OnExit);
            
            var app = new App(new GtkInitializer());
            var window = new FormsWindow();
            window.LoadApplication(app);
            window.SetApplicationTitle("CADI");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                window.Fullscreen();
            }

            window.Show();

            Gtk.Application.Run();
        }

        static public void OnExit(MainPageViewModel obj)
        {
            Gtk.Application.Quit();
        }

        public class GtkAppExitor
        {


        }
    }


    public class GtkInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterSingleton<IGpioService, RaspberryPiGpioService>();
        }
    }



    class PinAlreadyExportedException : Exception
    {
        public PinAlreadyExportedException()
            : base()
        { }

        public PinAlreadyExportedException(string message)
            : base(message)
        { }
    }

    public class GPIOPinDriver : IDisposable
    {
        private const string GPIO_ROOT_DIR = "/sys/class/gpio/";
        private static List<Pin> _exported_pins = new List<Pin>();
        private bool _disposed;


        private Pin _gpioPin;
        /// <summary>
        /// Gets the GPIO pin number.
        /// </summary>
        public Pin GPIOPin
        {
            get { return _gpioPin; }
            private set
            {
                lock (_exported_pins)
                {
                    if (_disposed)
                        throw new ObjectDisposedException("Selected pin has been disposed.");
                    else if (_exported_pins.IndexOf(value) != -1)
                        throw new PinAlreadyExportedException("Requested pin is already exported.");
                    else
                    {                                                                                     //  0  1  2  3  4  5
                        File.WriteAllText(GPIO_ROOT_DIR + "export", value.ToString().Substring(4));       //  G  P  I  O  8
                        _exported_pins.Add(value);
                        _gpioPin = value;
                    }
                }
            }
        }

        private GPIODirection _gpioDirection;
        /// <summary>
        /// Gets or sets the direction of of an output GPIO pin.
        /// </summary>
        public GPIODirection Direction
        {
            get { return _gpioDirection; }
            set
            {
                if (_disposed)
                    throw new ObjectDisposedException("Selected pin has been disposed.");
                else
                {
                    File.WriteAllText(String.Format("{0}gpio{1}/direction", GPIO_ROOT_DIR, GPIOPin.ToString().Substring(4)), (value == GPIODirection.In ? "in" : "out"));
                    _gpioDirection = value;
                }
            }
        }

        /// <summary>
        /// The current value of a GPIO pin.
        /// </summary>
        public GPIOState State
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException("Selected pin has been disposed.");
                else
                {
                    string state = File.ReadAllText(String.Format("{0}gpio{1}/value", GPIO_ROOT_DIR, GPIOPin.ToString().Substring(4)));
                    return (state[0] == '1' ? GPIOState.High : GPIOState.Low);
                }
            }
            set
            {
                if (_disposed)
                    throw new ObjectDisposedException("Selected pin has been disposed.");
                else if (this.Direction == GPIODirection.In)
                    throw new InvalidOperationException("State of an input pin can only be read.");
                else
                {
                    File.WriteAllText(String.Format("{0}gpio{1}/value", GPIO_ROOT_DIR, GPIOPin.ToString().Substring(4)), (value == GPIOState.High ? "1" : "0"));
                }
            }
        }

        /// <summary>
        /// Sets up an interface for accessing the specified GPIO pin with direction set to OUT and initial value to LOW.
        /// </summary>
        /// <param name="gpioPin">The GPIO pin to be accessed</param>
        public GPIOPinDriver(Pin gpioPin)
            : this(gpioPin, GPIODirection.Out, GPIOState.Low) { }

        /// <summary>
        /// Sets up an interface for accessing the specified GPIO pin with the given direction and initial value set to LOW.
        /// </summary>
        /// <param name="gpioPin">The GPIO pin to be accessed.</param>
        /// <param name="direction">The direction the GPIO pin should have.</param>
        public GPIOPinDriver(Pin gpioPin, GPIODirection direction)
                   : this(gpioPin, direction, GPIOState.Low) { }

        /// <summary>
        /// Sets up an interface for accessing the specified GPIO pin with the given direction and given initial value.
        /// </summary>
        /// <param name="gpioPin">The GPIO pin to be accessed.</param>
        /// <param name="direction">The direction the GPIO pin should have.</param>
        /// <param name="initialValue">The initial value the GPIO pin should have.</param>
        public GPIOPinDriver(Pin gpioPin, GPIODirection direction, GPIOState initialValue)
        {
            this._disposed = false;
            this.GPIOPin = gpioPin;
            this.Direction = direction;
            if (this.Direction == GPIODirection.Out)
            {
                this.State = initialValue;
            }
        }

        /// <summary>
        /// Unexports the GPIO interface.
        /// </summary>
        public void Unexport()
        {
            if (!_disposed)
                Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
                throw new ObjectDisposedException("Selected pin has already been disposed.");
            File.WriteAllText(GPIO_ROOT_DIR + "unexport", GPIOPin.ToString().Substring(4));
            _exported_pins.Remove(this.GPIOPin);
            _disposed = true;
        }

        ~GPIOPinDriver()
        {
            if (!_disposed)
                Dispose();
        }
    }


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
