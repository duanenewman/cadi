using System;

namespace Cadi.UI
{
    public interface IGpioService : IDisposable
    {
        void ExportPin(Pin pin, GPIODirection direction, GPIOState initialValue);
        void UnexportPin(Pin pin);

        GPIOState GetPinState(Pin pin);
        void SetPinState(Pin pin, GPIOState state);
    }


}