using System;
using System.Linq;
using pvblocks_api.Model;

namespace pvblocks_api.Helpers
{
    public class Utils
    {
        public static string GuidToPvBlockId(Guid guid) => guid.ToString().Substring(0, 18);

        public static (bool, Version, RecordUtils.VersionRecord?) CheckIfUpdateRequired(RecordUtils.VersionListRecord versionList)
        {
            Version currentVersion = new Version(versionList.CurrentVersion.Substring(1));
            var latest = versionList.AvailableVersions.Last();
            Version latestVersion = new Version(versionList.AvailableVersions.Last().Version.Substring(1));
            return (latestVersion > currentVersion, latestVersion, latest);
        }
    }

    public enum Rr1720State
    {
        Idle = 0,
        Isc = 1,
        Mpp = 2,
        Vbias = 3
    }
}
