using System;

namespace pvblocks_api.Exceptions
{
    public class CouldNotAddPvDeviceException : Exception
    {
        public CouldNotAddPvDeviceException()
            : base("It is not possible to add the pvdevice")
        {
        }
    }
}