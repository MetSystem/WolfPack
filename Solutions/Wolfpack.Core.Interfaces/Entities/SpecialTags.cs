using System.Collections.Generic;

namespace Wolfpack.Core.Interfaces.Entities
{
    public class SpecialTags
    {
        public const string Running = "Running";
        public const string Activity = "Activity";
        public const string HealthCheck = "HealthCheck";
        public const string Publisher = "Publisher";
        public const string Schedule = "Schedule";

        private static readonly List<string> NotForPersisting = new List<string>
                                                            {
                                                                Running
                                                            };

        public static bool ThatShouldNotBePersisted(string tag)
        {
            return NotForPersisting.Contains(tag);
        }
    }
}