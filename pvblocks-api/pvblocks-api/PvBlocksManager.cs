using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using pvblocks_api.Exceptions;
using pvblocks_api.Helpers;
using pvblocks_api.Model;
using ReRa.PVBlocks.Blazor.Client.Helpers;

namespace pvblocks_api
{
    public class PvBlocksManager : IPvBlocksManager
    {
        private readonly IHttpService _httpService;
       

        public PvBlocksManager(IHttpService httpService)
        {
            _httpService = httpService;
            
        }

        public async Task<bool> IsV1ApiAvailable()
        {
            try
            {
                var response = await _httpService.Get<ApiInfo>("Info");
                var isAvailable = response.Version.Contains("v1");
                return isAvailable;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> IsLoggedIn()
        {
            try
            {
                var response = await _httpService.Get<ApiInfo>("Info/version");
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

       


        #region Sensor Management

        public async Task<List<Sensor>> GetSensors()
        {
            var sensors = await _httpService.Get<List<Sensor>>($"Sensor");
            return sensors.OrderBy(p => p.Name).ToList();
        }

        public async Task<List<Sensor>> GetEnabledSensors(bool includeIvLoadSensors)
        {
            var sensors = await GetSensors();
            if (includeIvLoadSensors) return sensors.Where(p => p.Enabled == true).ToList();

            return sensors.Where(p => p.Enabled && p.Name != "ivpoint" && p.Name != "ivcurve").ToList();
        }

        public async Task UpdateSensor(int id, RecordUtils.SensorInfo updatedSensor)
        {

            var options = JsonSerializer.Serialize(updatedSensor.options);
            await _httpService.Put<RecordUtils.SensorInfo>($"Sensor/{id}", updatedSensor);
        }

        public async Task<Sensor> GetSensor(int sensorId)
        {
            return await _httpService.Get<Sensor>($"Sensor/{sensorId}");
        }

        public async Task DeleteSensor(int id)
        {
            await _httpService.Delete<int>($"Sensor/{id}");
        }


        public async Task DeleteAttachedSensors(int pvdeviceId)
        {
            await _httpService.Delete<int>($"pvdevice/{pvdeviceId}/attachedSensors");
        }

        public async Task AttachSensor(int pvdeviceId, int sensorId)
        {
            await _httpService.Post<Sensor>($"Sensor/{sensorId}/attach/{pvdeviceId}", sensorId);
        }

        #endregion

        #region Site Management

        public async Task<Site> GetSite()
        {
            return await _httpService.Get<Site>($"Info/site");
        }

        public async Task UpdateSite(Site site)
        {
            await _httpService.Post<Site>("Info/site", site);
        }

        public async Task<IDictionary<string, string>> GetTimezones()
        { 
            return await _httpService.Get<Dictionary<string, string>>($"Info/timezones");
        }

        public async Task<RecordUtils.TimeInfo> GetTimeInfo()
        {
            var ti = await _httpService.Get<RecordUtils.TimeInfo>($"Info/timeinfo");
            return ti;
        }


        public async Task<string> GetTimeServer()
        {
            return await _httpService.Get<string>($"Host/timeserver");
        }

        public async Task SetTimeServer(RecordUtils.TimeServerHostname timeserver)
        {
#if !DEBUG
            await _httpService.Post<RecordUtils.TimeServerHostname>($"Host/timeserver", timeserver);
#endif
        }


#endregion

#region PvDevice Management

        public async Task<List<PvDevice>> GetPvDevices()
        {
            return await _httpService.Get<List<PvDevice>>("pvdevice");
        }

        public async Task<PvDevice> GetPvDevice(int id)
        {
            var device = await _httpService.Get<PvDevice>($"pvdevice/{id}");
            return device;
        }

        public async Task<int> AddPvDevice(PvDevice pvdevice)
        {
            var dev = await _httpService.Post<PvDevice>($"pvdevice", pvdevice);
            return dev.Id;
        }

        public async Task UpdatePvDevice(PvDevice pvdevice)
        {
            await _httpService.Put<PvDevice>($"pvdevice/{pvdevice.Id}", pvdevice);
        }

        public async Task<bool> DeletePvDevice(int id)
        {
            await _httpService.Delete<int>($"pvdevice/{id}");
            return true;
        }

#endregion

#region PvBlocks Manager

        public async Task<List<PvBlock>> GetPvBlocks()
        {
            var pvblocks = await _httpService.Get<List<PvBlock>>("Block");
            return pvblocks.OrderBy(p => p.Position).ToList();
        }

        public async Task<PvBlock> GetPvBlock(int id)
        {
            var pvblock = await _httpService.Get<PvBlock>($"Block/{id}");
            return pvblock;
        }

        public async Task DeletePvBlock(int id)
        {
            await _httpService.Delete<int>($"Block/{id}");
        }


       
        public async Task<(int, int)> GetIvCurveParameters(int pvblockId)
        {
            var json = await _httpService.Get<JsonDocument>($"Command/ivCurveParameters/{pvblockId}");

            var r = json.RootElement;
            var points = r.GetProperty("points").GetInt32();
            var delay = r.GetProperty("delay").GetInt32();

            return (points, delay);

        }

        public async Task SetIvCurveParameters(int pvblockId, int points, int delay)
        {
            await _httpService.Post<IvCurveData>($"Command/updateIvCurveParameters/{pvblockId}", new IvCurveData() { Points = points, Delay = delay });
        }



        public async Task UpdatePvBlockPosition(int id, int position)
        {
            await _httpService.Post<BlockUserInfo>($"Block/Position/{id}",
                new BlockUserInfo() {Position = position, Label = "none"});
        }


        public async Task UpdatePvBlockLabel(int id, string label)
        {
            await _httpService.Post<BlockUserInfo>($"Block/Label/{id}",
                new BlockUserInfo() {Label = label, Position = 0});
        }

#endregion

#region Hardware Manager

        public async Task BlinkLed(Guid id)
        {
            await _httpService.Get<int>($"Hardware/{id}/blink/3");
        }

        public async Task<BlockStatus> GetBlockStatus(Guid id)
        {
            return await _httpService.Get<BlockStatus>($"Hardware/{id}/status");
        }

#endregion

#region Direct measurement manager

        public async Task<(double, double, double, double)> RR1740_GetTemperatures(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() {CommandName = "GetTemperatures", Parameters = new RecordUtils.CommandTriggerStatus(true) });
            var r = jdoc.RootElement;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            var list = new List<double>();
            for (var i = 1; i < 5; i++)
            {
                if (r.TryGetProperty(i.ToString(), out var degreeProp))
                {
                    var x = r.GetProperty(i.ToString()).GetProperty("temperature").GetDouble();
                    list.Add(x);
                }
            }

            return (list[0], list[1], list[2], list[3]);
        }


        public async Task<List<PvMonData>> RR1752_GetPvMons(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() { CommandName = "GetPvMons", Parameters = new RecordUtils.CommandTriggerStatus(true) });
            var r = jdoc.RootElement;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            var list = new List<PvMonData>();
            for (var i = 1; i < 5; i++)
            {
                if (r.TryGetProperty(i.ToString(), out var degreeProp))
                {
                    var x = r.GetProperty(i.ToString()).GetProperty("temperature").GetDouble();
                   // list.Add(x);
                }
            }

            return list;
        }

        public async Task<List<MsXxReading>> RR1751_GetReadings(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() { CommandName = "GetReadings", Parameters = new RecordUtils.CommandTriggerStatus(true) });
            var r = jdoc.RootElement;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            var list = new List<MsXxReading>();
            for (var i = 1; i < 5; i++)
            {
                if (r.TryGetProperty(i.ToString(), out var degreeProp))
                {
                    var reading = new MsXxReading();
                    reading.Temperature = r.GetProperty(i.ToString()).GetProperty("temperature").GetDouble();
                    reading.Humidity = r.GetProperty(i.ToString()).GetProperty("humidity").GetDouble();
                    reading.Irradiance = r.GetProperty(i.ToString()).GetProperty("irradiance").GetDouble();
                    reading.Xtilt = r.GetProperty(i.ToString()).GetProperty("xtilt").GetDouble();
                    reading.Ytilt = r.GetProperty(i.ToString()).GetProperty("ytilt").GetDouble();
                    reading.NodeId = r.GetProperty(i.ToString()).GetProperty("nodeid").GetInt16();
                    list.Add(reading);
                }
            }
            return list;
        }


        public async Task<RsbReading> RR1754_GetReadings(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() { CommandName = "GetReadings", Parameters = new RecordUtils.CommandTriggerStatus(true) });
            var r = jdoc.RootElement;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            var reading = new RsbReading();
            if (r.TryGetProperty("1", out var degreeProp))
            {
                
                reading.Temperature = r.GetProperty("1").GetProperty("temperature").GetDouble();
                reading.GHI = r.GetProperty("1").GetProperty("ghi").GetDouble();
                reading.DHI = r.GetProperty("1").GetProperty("dhi").GetDouble();
                reading.Xtilt = r.GetProperty("1").GetProperty("xtilt").GetDouble();
                reading.Ytilt = r.GetProperty("1").GetProperty("ytilt").GetDouble();
                reading.DNI = r.GetProperty("1").GetProperty("dni").GetDouble();
            }
           
            return reading;
        }


        public async Task RR1751_SetConfiguration(Guid guid, MsXxConfig config)
        {
            await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() { CommandName = "ConfigureDevices", Parameters = JsonUtil.ToJsonDocument(config)});
        }

        public async Task<MsXxConfig> RR1751_GetConfiguration(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() { CommandName = "GetConfiguration"});
            var r = jdoc.RootElement;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            var count = r.GetProperty("1").GetProperty("MsXxCount").GetByte();
            var addr_str = r.GetProperty("1").GetProperty("MsXxNodeAddress").GetString();
            var delay = r.GetProperty("1").GetProperty("Delay_10ms").GetByte();
            var sensorType = r.GetProperty("1").GetProperty("SensorType").GetByte();
            var config = new MsXxConfig() {Delay_10ms = delay, MsXxCount = count, MsXxNodeAddress = addr_str, SensorType = sensorType};
            return config;
        }



        public async Task<(double, double, double, double)> RR1730_GetIrradiances(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() {CommandName = "GetIrradiances", Parameters = new RecordUtils.CommandTriggerStatus(true)});
            var r = jdoc.RootElement;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            var list = new List<double>();
            for (var i = 1; i < 5; i++)
            {
                if (r.TryGetProperty(i.ToString(), out var degreeProp))
                {
                    var x = r.GetProperty(i.ToString()).GetProperty("irradiance").GetDouble();
                    list.Add(x);
                }
            }

            return (list[0], list[1], list[2], list[3]);
        }

        public async Task<IvPoint> RR1720_GetIvPoint(Guid guid)
        {
            var jdoc = await _httpService.Post<JsonDocument>($"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject() {CommandName = "MeasureDirectIvPoint" });
            var r = jdoc.RootElement;
            double current = 0;
            double voltage = 0;
            if (r.ValueKind != JsonValueKind.Object)
            {
                throw new HttpServiceException();
            }

            if (r.TryGetProperty("1", out var ivpointProp))
            {
                current = r.GetProperty("1").GetProperty("ivpoint").GetProperty("i").GetDouble();
                voltage = r.GetProperty("1").GetProperty("ivpoint").GetProperty("v").GetDouble();
            }


            return new IvPoint(voltage, current);
        }

        public async Task<IPvBlocksManager.IvCurve> RR1720_MeasureIvCurve(Guid guid, int points, int delay)
        {
            var result = await _httpService.Post<Dictionary<string, IPvBlocksManager.IvCurve>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "StartIvCurve",
                    Parameters = new
                    {
                        points,
                        delay
                    }
                });


            if (result.Values.Count > 0)
                return result.Values.First();

            return null;
        }

#endregion

#region Pipeline & Command manager

        public async Task<List<Pipeline>> GetPipelines()
        {
            return await _httpService.Get<List<Pipeline>>("Pipeline");
        }

        public async Task SavePipelines(List<Pipeline> pipelines)
        {
            foreach (var pipeline in pipelines)
            {
                var pipelineUpdate = new RecordUtils.PipelineUpdate(pipeline.Description, pipeline.DaylightOnly,
                    pipeline.CronTabs.ToArray());
                await _httpService.Put<Pipeline>($"Pipeline/{pipeline.Id}", pipelineUpdate);
            }
        }

        public async Task DeletePipeline(int id)
        {
            await _httpService.Delete<Pipeline>($"Pipeline/{id}");
        }

        public async Task<Pipeline> NewPipeline(Pipeline pipeline)
        {
            return await _httpService.Post<Pipeline>($"Pipeline", pipeline);
        }

        public async Task<List<Command>> GetCommands()
        {
            return await _httpService.Get<List<Command>>("Command");
        }

        public async Task<Command> GetCommand(int pipelineId, int commandId)
        {
            return await _httpService.Get<Command>($"Pipeline/{pipelineId}/command/{commandId}");
        }

        public async Task DeleteCommand(int pipelineId, int commandId)
        {
            await _httpService.Delete<Command>($"Pipeline/{pipelineId}/command/{commandId}");
        }


        public async Task DeleteCommands()
        {
            await _httpService.Delete<Command>($"Pipeline/AllCommands");
        }


        public async Task AddCommand(int pipelineId, RecordUtils.AddCommandRecord commandRecord)
        {
            await _httpService.Post<RecordUtils.AddCommandRecord>($"Pipeline/{pipelineId}/command", commandRecord);
        }

        public async Task<bool> GetPipelinesRunning()
        {
            return await _httpService.Get<bool>("Pipeline/enabled");
        }

        public async Task SetPipelinesRunning(bool enablePipelines)
        {
            var enabled = "enable";
            if (!enablePipelines)
                enabled = "disable";
            await _httpService.Post<bool>($"Pipeline/{enabled}", true);

           

        }

#endregion

#region Meteo Manager

        public async Task<MeteoAssignment> GetMeteoAssigments()
        {
            return await _httpService.Get<MeteoAssignment>("Meteo");
        }

        public async Task<MeteoReading> GetLastMeteoReadings()
        {
            return await _httpService.Get<MeteoReading>("Meteo/last");
        }

        public async Task SetMeteoAssigments(MeteoAssignment meteoAssignment)
        {
            await _httpService.Post<MeteoAssignment>("Meteo", meteoAssignment);
        }

        public async Task<int> GetMeteoDeviceId()
        {
            return await _httpService.Get<int>("Meteo/MeteoPvDeviceId");
        }

        public async Task<RecordUtils.SunPosition> GetSunPosition()
        {
            return await _httpService.Get<RecordUtils.SunPosition>(@"Calculation/SunPosition");
        }

        public async Task<IDictionary<double, RecordUtils.SunPath>> GetSunPath()
        {
            return await _httpService.Get<IDictionary<double, RecordUtils.SunPath>>(@"Calculation/SolarPath");
        }

        public async Task AddLogbookEntry(LogbookEntry logbookEntry)
        {
            await _httpService.Post<LogbookEntry>("Logbook", logbookEntry);
        }

#endregion

        public async Task ShutdownHost()
        {
            await _httpService.Post<bool>("Host/poweroff", true);
        }

        public async Task RebootHost()
        {
            await _httpService.Post<bool>("Host/reboot", true);
        }

        public async Task<IvMeasurement> GetSampleCurve()
        {
            return await _httpService.Get<IvMeasurement>("Calculation/SampleCurve");
        }

        public async Task<RecordUtils.VersionListRecord> GetVersions()
        {
            return await _httpService.Get<RecordUtils.VersionListRecord>("Host/versions");
        }

        public async Task<bool> FactoryReset(RecordUtils.FactoryResetMessage factoryResetMessage)
        {
            await _httpService.Post<RecordUtils.FactoryResetMessage>("Host/cleardb", factoryResetMessage);

            return true;
        }

        public async Task<IvParameters> CalculateIvParameters(IvData ivData)
        {
            return await _httpService.Post<IvParameters>("Calculation/ProcessIvCurve", ivData);
        }

        public async Task RR1720_SetState(Guid guid, Rr1720State state, double voltageBias)
            => await _httpService.Post<Dictionary<string, IPvBlocksManager.IvCurve>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "ApplyState",
                    Parameters = new
                    {
                        state,
                        voltageBias
                    }
                });

        public async Task RR1720_StoreState(Guid guid, int state, double voltageBias)
        {
            await _httpService.Put<RecordUtils.IvMppStateRecord>($"Hardware/{guid.ToString()}/storeIvMppState",
                new RecordUtils.IvMppStateRecord(guid.ToString(), (int) state, voltageBias));
        }

        public async Task<RecordUtils.IvMppStateRecord> RR1720_GetState(Guid guid)
        {
            return await _httpService.Get<RecordUtils.IvMppStateRecord>($"Hardware/{guid.ToString()}/IvMppState");
        }


        public async Task<IvPointCalibration> RR1720_GetCalibrationValues(Guid guid)
        {
            var floats = (await _httpService.Post<Dictionary<string, List<double>>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "ReadFloatEeprom",
                    Parameters = new
                    {
                        count = 4,
                        address = 4
                    }
                })).Values.First();

            var ivpCal = new IvPointCalibration();
            ivpCal.voltage = new CalibrationOrder1();
            ivpCal.current = new CalibrationOrder1();
            ivpCal.voltage.n_1 = floats[0];
            ivpCal.voltage.c = floats[1];
            ivpCal.current.n_1 = floats[2];
            ivpCal.current.c = floats[3];

            return ivpCal;

        }

        public async Task RR1720_SetCalibrationValues(Guid guid, IvPointCalibration calibration)
        {
            await _httpService.Post<Dictionary<string, IPvBlocksManager.IvCurve>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "WriteFloatEeprom",
                    Parameters = new
                    {
                        flt = calibration.voltage.n_1,
                        address = 4
                    }
                });
            await _httpService.Post<Dictionary<string, IPvBlocksManager.IvCurve>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "WriteFloatEeprom",
                    Parameters = new
                    {
                        flt = calibration.voltage.c,
                        address = 8
                    }
                });
            await _httpService.Post<Dictionary<string, IPvBlocksManager.IvCurve>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "WriteFloatEeprom",
                    Parameters = new
                    {
                        flt = calibration.current.n_1,
                        address = 12
                    }
                });
            await _httpService.Post<Dictionary<string, IPvBlocksManager.IvCurve>>(
                $"Hardware/{guid.ToString()}/sendCommand",
                new CommandNameObject
                {
                    CommandName = "WriteFloatEeprom",
                    Parameters = new
                    {
                        flt = calibration.current.c,
                        address = 16
                    }
                });

            await _httpService.Get<string>($"Hardware/{guid.ToString()}/reset");

        }

        public async Task UpdateSystem()
        {
            await _httpService.Post<string>("Host/update", "");
        }

        public async Task RefreshBlocks()
        {
            await _httpService.Post<string>("Block", "");
        }

        public async Task<List<string>> GetIpAddresses()
        {

#if DEBUG
            await Task.CompletedTask;
            return new List<string>() {"debug"};
#endif

            try
            {


                var ips = await _httpService.Get<string>("Host/hostname");

                var cols = ips.Split(' ');
                var list = new List<string>();
                foreach (var ip in cols)
                {
                    if (ip.Contains("."))
                        if (!ip.StartsWith("172"))
                            list.Add(ip);
                }

                await Task.CompletedTask;
                return list;

            }
            catch (Exception e)
            {
                Console.WriteLine("pvblocks: could not load ip addresses");
                return new List<string>();
            }
        }

        public async Task<string> GetApiKey()
        {
            var result =  await _httpService.Get<string>("/v1/authentication/ApiKey/activeKey");
            return result;
        }

        //TODO: This is really ugly
        public async Task<string> RecreateApiKey()
        {
            var key = await _httpService.Get<string>("/v1/authentication/ApiKey/activeKey");
            await _httpService.Delete<string>($"/v1/authentication/ApiKey/{key}");

            var claims = new ClaimStorage()
            {
                Claims = new List<RecordUtils.ClaimRecord>()
                {
                    new RecordUtils.ClaimRecord("CanReadData", "true"),
                    new RecordUtils.ClaimRecord("CanManagePvDevices", "true")
                }
            };
            var a = await _httpService.Post<ClaimStorage>("/v1/authentication/ApiKey", claims);
            key = await _httpService.Get<string>("/v1/authentication/ApiKey/activeKey");
            return key;
        }


        public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword,
            string confirmNewPassword)
        {
            try
            {
                var a = await _httpService.Put<RecordUtils.ChangePasswordRequest>("/v1/authentication/User/password",
                    new RecordUtils.ChangePasswordRequest(username,
                        oldPassword, newPassword, confirmNewPassword));
            }
            catch (Exception e)
            {

                return false;
            }

            return true;
        }

        public async Task<List<LogbookEntry>> GetLogbookEntries()
        {
            return await _httpService.Get<List<LogbookEntry>>("Logbook");
        }

        public async Task<RecordUtils.BackupRecord> CreateBackup()
        {
            return await _httpService.Post<RecordUtils.BackupRecord>("Host/backup",
                new RecordUtils.BackupRecord("empty"));
        }

        
        public async Task<string> GetCalibratedMeasurementsCsv(DateTime? startDate, DateTime? stopDate,
            bool addFullCurve, int pvdeviceId)
        {
            if (startDate == null || stopDate == null)
            {
                return "No data";
            }
            var start = ((DateTime) startDate).ToString("yyyy-MM-ddTHH:mm:ss");
            var stop = ((DateTime) stopDate).ToString("yyyy-MM-ddTHH:mm:ss");

            return await _httpService.Get<string>(
                $"CalibratedMeasurement/{start}/{stop}/device/{pvdeviceId}?addFullCurve={addFullCurve}&json=false");
        }

        public async Task<List<RecordUtils.PvDeviceMetrics>> GetPvDevicesDataStorage()
        {
            return await _httpService.Get<List<RecordUtils.PvDeviceMetrics>>(
                $"CalibratedMeasurement/pvdevicesDatastorage");
        }

        public async Task<List<CalibratedMeasurement>> GetCalibratedMeasurements(int pvdeviceId, DateTime? startDate,
            DateTime? stopDate)
        {
            if (startDate == null || stopDate == null)
            {
                return new List<CalibratedMeasurement>();
            }

            var start = ((DateTime) startDate).ToString("yyyy-MM-ddTHH:mm:ss");
            var stop = ((DateTime) stopDate).ToString("yyyy-MM-ddTHH:mm:ss");

            return await _httpService.Get<List<CalibratedMeasurement>>(
                $"CalibratedMeasurement/{start}/{stop}/device/{pvdeviceId}?addFullCurve=false&json=true");
        }

        public async Task<CalibratedMeasurement> GetLastCalibratedMeasurements(int pvdeviceId)
        {
            return await _httpService.Get<CalibratedMeasurement>(
                $"CalibratedMeasurement/last/device/{pvdeviceId}?lastCurve={false}");
        }

        public async Task<CalibratedMeasurement> GetLastCalibratedMeasurementIvCurve(int pvdeviceId)
        {
            return await _httpService.Get<CalibratedMeasurement>(
                $"CalibratedMeasurement/last/device/{pvdeviceId}?lastCurve=true");
        }

        public async Task<int> GetGbFreeSpace()
        {
            try
            {
                var freeSpace = await _httpService.Get<string>($"Host/freespace");
                if (freeSpace.Contains('G'))
                {
                    var s = int.Parse(freeSpace.Substring(0, freeSpace.Length - 1));
                    return s;
                }
            }
            catch (Exception)
            {
               
            }
            Console.WriteLine("Unable to retrieve free disk space.");
            return 0;

        }

        public async Task<InfluxData> GetInfluxData()
        {
            var rec =  await _httpService.Get<InfluxData>("CalibratedMeasurement/influxdata");

            return rec;
        }

        public async Task SetInfluxData(InfluxData influxData)
        {
            await _httpService.Post<InfluxData>("CalibratedMeasurement/influxdata", influxData);
        }

        public async Task<string> GetInfluxDataVersion(InfluxData influxData)
        {
            var version = await _httpService.Get<string>("CalibratedMeasurement/influxdataserverversion");

            return version;
        }

        public async Task EnableSensor(int id, bool enable)
        {
            var str = "disable";
            if (enable) str = "enable";

            await _httpService.Post<bool>($"Sensor/{id}/{str}", true);
        }



        public async Task EnableOpenWeatherApi(bool enable)
        {
            var str = "disable";
            if (enable) str = "enable";

            await _httpService.Post<bool>($"Hardware/openweathermapblock/{str}", true);
        }

        public async Task<bool> GetOpenWeatherApiEnabled()
        {
            return await _httpService.Get<bool>($"Hardware/openweathermapblock");
        }

        public async Task<string> GetOpenWeatherApiKey()
        {
            var result = await _httpService.Get<string>($"Hardware/openweathermapkey");

            return result;
        }

        public async Task SetOpenWeatherApiKey(string apikey)
        {
            await _httpService.Post<RecordUtils.OpenWeatherApiKey>($"Hardware/openweathermapkey", new RecordUtils.OpenWeatherApiKey(apikey));
        }

       
    }
}
