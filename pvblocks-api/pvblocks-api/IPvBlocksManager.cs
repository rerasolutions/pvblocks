using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pvblocks_api.Exceptions;
using pvblocks_api.Helpers;
using pvblocks_api.Model;

namespace pvblocks_api
{
    public interface IPvBlocksManager
    {

        /// <summary>
        /// Check is Api V! is available
        /// </summary>
        /// <returns>true when available</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<bool> IsV1ApiAvailable();

        Task<bool> IsLoggedIn();
        #region Sensor Manager

        /// <summary>
        /// Gets a list of all sensors
        /// </summary>
        /// <returns></returns>
        Task<List<Sensor>> GetSensors();
        Task<List<Sensor>> GetEnabledSensors(bool includeIvLoadSensors);

        /// <summary>
        /// Updates the sensor
        /// </summary>
        /// <param name="updatedSensor"></param>
        /// <returns></returns>
        Task UpdateSensor(int id, RecordUtils.SensorInfo updatedSensor);

        /// <summary>
        /// Gets the sensor
        /// </summary>
        /// <param name="sensorId">Id</param>
        /// <returns></returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<Sensor> GetSensor(int sensorId);

        Task DeleteAttachedSensors(int pvdeviceId);

        Task AttachSensor(int pvdeviceId, int sensorId);

        #endregion

        #region Site Manager      
        /// <summary>
        /// Get the Site data
        /// </summary>
        /// <returns>Site or null if it does not exist</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<Site> GetSite();


        /// <summary>
        /// Updates the Site 
        /// </summary>
        /// <param name="site">The Site to update</param>
        /// <exception cref="CouldNotUpdateSiteException">Thrown if the site could not be updated</exception>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task UpdateSite(Site site);

        /// <summary>
        /// Gets all timezones as a dictionary
        /// </summary>
        /// <returns>timezones</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<IDictionary<string, string>> GetTimezones();

        /// <summary>
        /// Gets the uptime and current local time
        /// </summary>
        /// <returns></returns>
        Task<RecordUtils.TimeInfo> GetTimeInfo();

        #endregion

        #region PvDevices Manager        
        /// <summary>
        /// Get all PvDevices
        /// </summary>
        /// <returns>List of PvDevices</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<List<PvDevice>> GetPvDevices();

        /// <summary>
        /// Get a PvDevice by id
        /// </summary>
        /// <param name="id">The id of the PvDevice</param>
        /// <returns>PvDevice or null if it does not exist</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<PvDevice> GetPvDevice(int id);

        /// <summary>
        /// Add a new PvDevice
        /// </summary>
        /// <param name="pvdevice">The PvDevice to add without id</param>
        /// <returns>The assigned id</returns>
        /// <exception cref="CouldNotAddPvDeviceException">Thrown if the pvdevice could not be added</exception>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<int> AddPvDevice(PvDevice pvdevice);

        /// <summary>
        /// Updates a PvDevice
        /// </summary>
        /// <param name="pvdevice">The PvDevice to update</param>
        /// <exception cref="CouldNotUpdateDeviceException">Thrown if the pvdevice could not be updated</exception>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task UpdatePvDevice(PvDevice pvdevice);

        /// <summary>
        /// Deletes a PvDevice.
        /// </summary>
        /// <param name="id">The id of the PvDevice to delete</param>
        /// <returns>True on success</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<bool> DeletePvDevice(int id);

        #endregion

        #region PvBlocks Manager
        /// <summary>
        /// Gets all PVBlocks (Deleted and Not deleted)
        /// </summary>
        /// <returns>A list of PvBlocks</returns>
        Task<List<PvBlock>> GetPvBlocks();

        /// <summary>
        /// Gets the PvBlock by its database id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The PvBlock or null</returns>
        Task<PvBlock> GetPvBlock(int id);

        Task EnableSensor(int id, bool enable);
        Task<(int,int)> GetIvCurveParameters(int pvblockId);

        Task SetIvCurveParameters(int pvblockId, int points, int delay);

        /// <summary>
        /// Updates the position of the PVBlock
        /// </summary>
        /// <param name="id">Database id of the PvBlock</param>
        /// <param name="position">Any position</param>
        /// <returns></returns>
        Task UpdatePvBlockPosition(int id, int position);


        /// <summary>
        /// Updates the label of the PVBlock
        /// </summary>
        /// <param name="id">Database id of the PvBlock</param>
        /// <param name="position">The label</param>
        /// <returns></returns>
        Task UpdatePvBlockLabel(int id, string label);

        #endregion

        #region Hardware Manager

        /// <summary>
        /// Blink the red led 3 times
        /// </summary>
        /// <param name="id">The hardware id</param>
        /// <returns></returns>
        Task BlinkLed(Guid id);

        /// <summary>
        /// Get the current status of a block
        /// </summary>
        /// <param name="id">The hardware id</param>
        /// <returns>The block status</returns>
        Task<BlockStatus> GetBlockStatus(Guid id);

        #endregion

        #region Direct measurement manager

        /// <summary>
        /// Return the four measured temperatures
        /// </summary>
        /// <param name="guid">The hardware id</param>
        /// <returns>Tuple of temperatures</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<(double, double, double, double)> RR1740_GetTemperatures(Guid guid);


        /// <summary>
        /// Return the measured data from PVMon
        /// </summary>
        /// <param name="guid">The hardware id</param>
        /// <returns>List of PVMon data</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<List<PvMonData>> RR1752_GetPvMons(Guid guid);


        Task<List<MsXxReading>> RR1751_GetReadings(Guid guid);

        Task<RsbReading> RR1754_GetReadings(Guid guid);

        Task<MsXxConfig> RR1751_GetConfiguration(Guid guid);


        /// <summary>
        /// Return the four measured irradiances
        /// </summary>
        /// <param name="guid">The hardware id</param>
        /// <returns>Tuple of irradiances</returns>
        /// <exception cref="HttpServiceException">Thrown if the API does not work as expected</exception>
        Task<(double, double, double, double)> RR1730_GetIrradiances(Guid guid);


        /// <summary>
        /// Returns the current voltage and current of the module.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>IvPoint</returns>
        Task<IvPoint> RR1720_GetIvPoint(Guid guid);
        
        public record IvCurve(double[] Voltages, double[] Currents, Int32 TimeStamp);

        /// <summary>
        /// Measures directly an IV-Curve
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task<IvCurve> RR1720_MeasureIvCurve(Guid guid, int points, int delay);



        #endregion

        #region Pipeline & Command manager

        Task<List<Pipeline>> GetPipelines();
        Task SavePipelines(List<Pipeline> pipelineUpdates);
        Task DeletePipeline(int id);
        Task<Pipeline> NewPipeline(Pipeline pipeline);
        Task<List<Command>> GetCommands();
        Task<Command> GetCommand(int pipelineId, int commandId);
        Task DeleteCommand(int pipelineId, int commandId);
        Task AddCommand(int pipelineId, RecordUtils.AddCommandRecord commandRecord);

        Task<bool> GetPipelinesRunning();
        Task SetPipelinesRunning(bool enablePipelines);

        #endregion

        #region Meteo Manager
        Task<MeteoAssignment> GetMeteoAssigments();
        Task<MeteoReading> GetLastMeteoReadings();

        Task SetMeteoAssigments(MeteoAssignment meteoAssignment);
        Task<int> GetMeteoDeviceId();
        #endregion

        #region Calculation manager

        Task<RecordUtils.SunPosition> GetSunPosition();

        Task<IDictionary<double, RecordUtils.SunPath>> GetSunPath();

        #endregion

        #region Logbook manager

        Task AddLogbookEntry(LogbookEntry logbookEntry);

        #endregion

        Task<RecordUtils.VersionListRecord> GetVersions();
        Task<IvParameters> CalculateIvParameters(IvData ivData);
        Task ShutdownHost();
        Task RebootHost();
        Task<bool> FactoryReset(RecordUtils.FactoryResetMessage factoryResetMessage);
        Task<IvMeasurement> GetSampleCurve();
        Task RR1720_SetState(Guid guid, Rr1720State state, double voltageBias);
        Task RR1720_StoreState(Guid guid, int state, double voltageBias);
        Task<RecordUtils.IvMppStateRecord> RR1720_GetState(Guid guid);
        Task<IvPointCalibration> RR1720_GetCalibrationValues(Guid guid);
        Task RR1720_SetCalibrationValues(Guid guid, IvPointCalibration calibration);

        Task UpdateSystem();

        Task RefreshBlocks();
        Task<List<string>> GetIpAddresses();
        Task<string> GetApiKey();
        Task<string> RecreateApiKey();

        Task<bool> ChangePassword(string username, string oldPassword, string newPassword, string confirmNewPassword);
        Task<List<LogbookEntry>> GetLogbookEntries();
        Task<RecordUtils.BackupRecord> CreateBackup();
        
        Task<string> GetCalibratedMeasurementsCsv(DateTime? startDate, DateTime? stopDate, bool addFullCurves, int pvdeviceId);

        Task<List<RecordUtils.PvDeviceMetrics>> GetPvDevicesDataStorage();
        Task<List<CalibratedMeasurement>> GetCalibratedMeasurements(int pvdeviceId, DateTime? startDate, DateTime? stopDate);
        Task DeleteCommands();
        Task<string> GetTimeServer();
        Task SetTimeServer(RecordUtils.TimeServerHostname timeserver);
        Task<CalibratedMeasurement> GetLastCalibratedMeasurements(int pvdeviceId);
        Task<CalibratedMeasurement> GetLastCalibratedMeasurementIvCurve(int pvdeviceId);

        Task<int> GetGbFreeSpace();
        Task<InfluxData> GetInfluxData();
        Task SetInfluxData(InfluxData influxData);
        Task<string> GetInfluxDataVersion(InfluxData influxData);
        Task EnableOpenWeatherApi(bool enable);
        Task<bool> GetOpenWeatherApiEnabled();
        Task<string> GetOpenWeatherApiKey();
        Task SetOpenWeatherApiKey(string apikey);

        Task RR1751_SetConfiguration(Guid guid, MsXxConfig config);
    }
}
