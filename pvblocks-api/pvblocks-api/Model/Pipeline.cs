using System.Collections.Generic;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Pipeline that runs commands in no particular order at a specific time
    /// </summary>
    public class Pipeline
    {
        public int Id { get; set; }
        /// <summary>
        /// Userfriendly description of the pipeline
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Only run this pipeline between sunrise and sunset
        /// </summary>
        public bool DaylightOnly { get; set; }
        /// <summary>
        /// CronTabs on which this Pipeline needs to run
        /// </summary>
        public List<string> CronTabs { get; set; } = new();
        /// <summary>
        /// Flag to set if the pipeline is deleted
        /// </summary>
        public bool Deleted { get; set; } = false;
        
        /// <summary>
        /// Commands to run for this pipeline
        /// </summary>
        public List<Command> Commands { get; set; } = null!;
        /// <summary>
        /// Measurements made for this pipeline
        /// </summary>
        public List<Measurement> Measurements { get; set; } = null!;
    }
}