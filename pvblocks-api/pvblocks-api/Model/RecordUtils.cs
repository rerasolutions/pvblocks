using System;
using System.Collections.Generic;
using System.Text.Json;

namespace pvblocks_api.Model
{
    public static class RecordUtils
    {
        
        public record CommandTriggerStatus(bool direct);
        public record SensorInfo(string description, bool enabled, string unit, JsonDocument calibration, JsonDocument options);

        public record PipelineUpdate(string description, bool daylightOnly, string[] cronTabs);

        public record AddCommandRecord(int PvBlockId, string CommandName, object? Parameters, bool WithTrigger = true);

        public record SunPosition(double elevation, double azimuth, double hour);
       
        public record SunPath(double solstice, double equinox, double today);

        public record TimeInfo(int uptimeDay, int uptimeHour, int uptimeMinute, DateTime currentDateTime, string timezone);

        public record VersionRecord(string Version, string UpdateLog, DateTime ReleaseDate);

        public record VersionListRecord(List<VersionRecord> AvailableVersions, string? CurrentVersion);

        public record FactoryResetMessage(string confirmMessage);

        public record ClaimRecord(string ClaimType, string Value);

        public record ChangePasswordRequest(string Username, string OldPassword, string NewPassword, string ConfirmNewPassword);

        public record BackupRecord(string Url);

        public record IvMppStateRecord(string guid, int state, double vbias);

        public record PvDeviceMetrics(int pvdeviceId, int rowCount, int attachedSensors, string name);

        public record TimeServerHostname(string hostname);

        public record EnableRecord(bool enable);

        public record OpenWeatherApiKey(string apikey);

        

    }
}
