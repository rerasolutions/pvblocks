using System;

namespace pvblocks_api.Exceptions
{
    public class CouldNotUpdateDeviceException : Exception
    {
        public CouldNotUpdateDeviceException()
            : base("It is not possible to update the pvdevice")
        {
        }
    }
}