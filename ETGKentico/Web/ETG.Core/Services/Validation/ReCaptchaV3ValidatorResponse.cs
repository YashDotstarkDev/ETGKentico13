using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ETG.Core.Services.Validation
{
    public class ReCaptchaV3ValidatorResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("score")]
        public decimal Score { get; set; }
        [JsonProperty("action")]
        public string Action { get; set; }
        [JsonProperty("challenge_ts")]
        public DateTime ChallengeDateTime { get; set; }
        [JsonProperty("hostname")]
        public string HostName { get; set; }
        [JsonProperty("error-codes")]
        public IEnumerable<string> ErrorCodes { get; set; } = Enumerable.Empty<string>();
    }
}