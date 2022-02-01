using System;
using System.Collections.Generic;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Physical Pv Block in the system
    /// </summary>
    public class PvBlock
    {
        public int Id { get; set; }
        /// <summary>
        /// Label to be used for the block
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Fixed model name
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Block type
        /// </summary>
        public string Type { get; set; } = null!;
        /// <summary>
        /// Unique identifier for the block
        /// </summary>
        public Guid UniqueIdentifier { get; set; }
        /// <summary>
        /// Position of the block in the system. This number can be completely arbitrary and has no actual meaning.
        /// </summary>
        public int Position { get; set; } = 0;
        /// <summary>
        /// Commands that are available for this block
        /// </summary>
        public List<CommandJson> AvailableCommands { get; set; } = null!;
        /// <summary>
        /// Flag that is true when the block is 'online' in a live system. False if the block could not be found
        /// in the system.
        /// </summary>
        public bool Online { get; set; }
        
        public List<MeasurementDevice> MeasurementDevices { get; set; } = null!;

        public bool IsDeleted { get; set; }
    }
}