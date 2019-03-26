using System;

namespace Cadi.UI.GTK
{
    class PinAlreadyExportedException : Exception
    {
        public PinAlreadyExportedException()
            : base()
        { }

        public PinAlreadyExportedException(string message)
            : base(message)
        { }
    }
}
