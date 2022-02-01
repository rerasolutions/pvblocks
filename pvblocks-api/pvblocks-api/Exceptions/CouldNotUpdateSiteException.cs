using System;

namespace pvblocks_api.Exceptions
{
    public class CouldNotUpdateSiteException : Exception
    {
        public CouldNotUpdateSiteException()
            : base("It is not possible to update the site")
        {
        }
    }
}