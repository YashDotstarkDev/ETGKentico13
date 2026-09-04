using Castle.Core.Internal;
using ETG.Web.Models.Base;
using System.Collections.Generic;

namespace ETG.Web.DestinationExpertTeam.Models
{
    public class DestinationExpertTeamViewModel : PageNodeViewModel
    {
        public DestinationExpertTeamSummaryViewModel SummaryInfo { get; set; }
       
        public string ObjectiveStatement { get; set; }
        public string Profile { get; set; }
        public string GoalsInText { get; set; }

        public string FavouriteTourCode { get; set; }
        public string FeatureTourCodes { get; set; }

        public List<KeyValuePair<string, string>> GoalList
        {
            get
            {
                if (GoalsInText.IsNullOrEmpty())
                {
                    return null;
                }

                var goalList = new List<KeyValuePair<string, string>>();
                var lines = GoalsInText.Split('\n');

                foreach (var line in lines)
                {
                    var arr = line.Split('|');

                    if (arr.Length == 2)
                    {
                        goalList.Add(new KeyValuePair<string, string>(arr[0], arr[1]));
                    }
                }

                return goalList;
            }
        }
    }
}
