using System;
using System.Collections.Generic;
using System.IO;

namespace Cadi.UI
{

    /// <summary>
    /// P1 GPIO pins
    /// </summary>
    public enum Pin
    {
        GPIO2,
        GPIO3,
        GPIO4,
        GPIO7,
        GPIO8,
        GPIO9,
        GPIO10,
        GPIO11,
        GPIO14,
        GPIO15,
        GPIO17,
        GPIO18,
        GPIO22,
        GPIO23,
        GPIO24,
        GPIO25,
        GPIO27
    };
    /// <summary>
    /// Direction of the GPIO Pin
    /// </summary>
    public enum GPIODirection
    {
        In,
        Out
    };
    /// <summary>
    /// The value (High or Low) of a GPIO pin.
    /// </summary>
    public enum GPIOState
    {
        Low = 0,
        High = 1
    };


    public interface IGpioService
    {
        void ExportPin(Pin pin, GPIODirection direction, GPIOState initialValue);
        void UnexportPin(Pin pin);

        GPIOState GetPinState(Pin pin);
        void SetPinState(Pin pin, GPIOState state);
    }


}