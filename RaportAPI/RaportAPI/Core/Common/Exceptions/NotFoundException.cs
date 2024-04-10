using System.Globalization;

namespace RaportAPI.Core.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not Found")
        {
        }

        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
