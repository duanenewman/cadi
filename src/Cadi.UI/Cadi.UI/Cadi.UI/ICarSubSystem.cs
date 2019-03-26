using Xamarin.Forms.Xaml;
using System;

namespace Cadi.UI
{
    public interface ICarSubSystem: IDisposable
    {
        void Start();
        void Stop();

        bool IsAcOn { get; set; }
    }
}
