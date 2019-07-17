using System.Collections.Generic;

namespace Cadi.UI.GTK
{
	public class DesktopGpioService : IGpioService
	{
		Dictionary<Pin, GPIOState> pinStates = new Dictionary<Pin, GPIOState>();

		public void Dispose()
		{
		}

		public void ExportPin(Pin pin, GPIODirection direction, GPIOState initialValue)
		{
			if (!pinStates.ContainsKey(pin))
			{
				pinStates.Add(pin, initialValue);
			}
		}

		public GPIOState GetPinState(Pin pin)
		{
			return pinStates[pin];
		}

		public void SetPinState(Pin pin, GPIOState state)
		{
			pinStates[pin] = state;
		}

		public void UnexportPin(Pin pin)
		{
			if (!pinStates.ContainsKey(pin))
			{
				pinStates.Remove(pin);
			}
		}
	}
}
