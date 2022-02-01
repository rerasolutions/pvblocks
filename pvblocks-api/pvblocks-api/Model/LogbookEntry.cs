using System;

namespace pvblocks_api.Model
{
    public class LogbookEntry
    {
        public int Id { get; set; }

        /// <summary>
        /// Entry title
        /// </summary>
        public string Title { get; set; } = "";
        /// <summary>
        /// Entry content
        /// </summary>
        public string Content { get; set; } = "";
        /// <summary>
        /// Entry timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}