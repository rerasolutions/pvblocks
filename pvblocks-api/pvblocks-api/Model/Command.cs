using System.Text.Json;

namespace pvblocks_api.Model
{
    /// <summary>
    /// Command to send to a measurement device in a pipeline.
    ///
    /// Note: the order of commands cannot be set and after all commands in a pipeline have been executed a trigger will
    /// be called 
    /// </summary>
    public class Command
    {
        public int Id { get; set; }
        
        /// <summary>
        /// Pv Block this command is sent to
        /// </summary>
        public int PvBlockId { get; set; }
        /// <summary>
        /// Pipeline this command will be run on
        /// </summary>
        public int PipelineId { get; set; }
        /// <summary>
        /// Command to execute
        /// </summary>
        public string CommandName { get; set; } = null!;
        /// <summary>
        /// Parameters for the command
        /// </summary>
        public JsonDocument CommandParameters { get; set; } = null!;
        /// <summary>
        /// True if the command should be run with a trigger. False if the command should be run immediately.
        /// </summary>
        public bool WithTrigger { get; set; }

        public PvBlock PvBlock { get; set; } = null!;
        public Pipeline Pipeline { get; set; } = null!;
    }
}