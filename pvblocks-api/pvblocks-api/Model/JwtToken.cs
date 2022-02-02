using System.Collections.Generic;

namespace pvblocks_api.Model
{
    public class JwtToken
    {
        public string Bearer { get; set; }

        public string ValidTo { get; set; }
        public List<ClaimRecord> Claims { get; set; }

        public record ClaimRecord(string ClaimType, string Value);
    }
}