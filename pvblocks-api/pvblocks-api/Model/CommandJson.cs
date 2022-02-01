using System.Text.Json;

namespace pvblocks_api.Model
{
    public class CommandJson
    {
        /// <summary>
        /// Name of the command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Description of the command
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Default parameters for this command. Note that these are also all the parameters that can be set
        /// </summary>
        public JsonDocument DefaultParameters { get; }

        /// <summary>
        /// Whether the command should have a trigger by default. This is used in the UI to preconfigure the trigger
        /// setting
        /// </summary>
        public bool DefaultWithTrigger { get; }

        public CommandJson(string name, string description, JsonDocument? defaultParameters = null, bool defaultWithTrigger = true)
        {
            Name = name;
            Description = description;
            DefaultParameters = defaultParameters ?? JsonDocument.Parse("{}");
            DefaultWithTrigger = defaultWithTrigger;
        }
    }
}