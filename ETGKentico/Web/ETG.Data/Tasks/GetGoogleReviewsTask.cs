using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Castle.Core.Internal;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.Helpers;
using CMS.Scheduler;
using ETG.Core.CustomTables;
using Newtonsoft.Json;

namespace ETG.Data.Tasks
{
    public class GetGoogleReviewsTask : ITask
    {
        public string Execute(TaskInfo task)
        {
            try
            {
                string pathApi = "https://wextractor.com/api/v1/reviews?";
                var placeId = SettingsKeyInfoProvider.GetValue("GooglePlaceId");
                var authToken = SettingsKeyInfoProvider.GetValue("WextractorAuthToken");

                if (placeId.IsNullOrEmpty())
                {
                    return $"Place id is missing on the settings application.";
                }

                if (authToken.IsNullOrEmpty())
                {
                    return "Wextractor authentication token is missing on the settings application.";
                }

                pathApi = $"{pathApi}id={placeId}&auth_token={authToken}&offset=0&sort=highest_rating";


                WebRequest request = WebRequest.Create(pathApi);

                string resultString = "";
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        resultString = reader.ReadToEnd();
                    }
                }


                var googleReviewItem =
                    CustomTableItemProvider.GetItems<GoogleReviewsItem>().FirstOrDefault();

                if (googleReviewItem != null)
                {
                    googleReviewItem.Reviews = resultString;
                    googleReviewItem.DateExtracted = DateTime.Now;
                    googleReviewItem.Update();
                }
                else
                {
                    var reviewItem = new GoogleReviewsItem
                    {
                        Reviews = resultString,
                        DateExtracted = DateTime.Now
                    };
                    reviewItem.Insert();
                }


                return $"Last run at {DateTime.Now}. ";
            }
            catch (Exception exception)
            {
                return $"Failed to run the task. The exception is {exception.Message}";
            }
        }
    }
}