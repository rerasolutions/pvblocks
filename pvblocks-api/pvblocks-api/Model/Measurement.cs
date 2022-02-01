using System;
using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public class Measurement
    {
        public int Id { get; set; }
        
        /// <summary>
        /// The pipeline that this measurement was made for
        /// </summary>
        public int PipelineId { get; set; }
        /// <summary>
        /// Time the measurement was made at
        /// </summary>
        public DateTime Timestamp { get; set; }

        public Pipeline Pipeline { get; set; } = null!;

        public List<MeasuredValue> MeasuredValues { get; set; } = null!;
        public List<CalibratedMeasurement> CalibratedMeasurements { get; set; } = null!;
    }
}