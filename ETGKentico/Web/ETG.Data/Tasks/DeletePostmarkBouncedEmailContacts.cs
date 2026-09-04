using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.EventLog;
using CMS.Newsletters;
using CMS.SalesForce.WebServiceClient;
using CMS.Scheduler;
using ETG.Core.PageTypes.Providers;
using ETG.Data.Models.Api.Postmark;
using ETG.Data.Settings;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ETG.Data.Tasks
{
    public class DeletePostmarkBouncedEmailContacts : ITask
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly IEventLogService _eventLogService;

        public DeletePostmarkBouncedEmailContacts()
        {
            _eventLogService = Service.Resolve<IEventLogService>();
        }
        public string Execute(TaskInfo task)
        {
            try
            {
                string baseUrl = SettingsKeyInfoProvider.GetValue(ETGSettingsKey.EtgPostmarkBaseUrl);
                var serverToken = SettingsKeyInfoProvider.GetValue(ETGSettingsKey.EtgPostmarkServerToken);
                var count = SettingsKeyInfoProvider.GetIntValue(ETGSettingsKey.EtgPostmarkResponseCount, 100);

                List<Bounce> bouncedList = Task.Run(() => GetAllBouncesAsync(baseUrl, serverToken, count)).Result;
                List<string> bouncedEmailList = new List<string>();

                foreach (Bounce bouncedItem in bouncedList)
                {
                    if (bouncedItem.RecordType.ToLowerInvariant() == "bounce" && bouncedItem.Type.ToLowerInvariant() == "hardbounce" && bouncedItem.Inactive)
                    {
                        bouncedEmailList.Add(bouncedItem.Email);
                    }
                }

                int numberOfdeletedContacts = RemoveBouncedContacts(bouncedEmailList);

                return $"Removed {numberOfdeletedContacts} contacts";
            }
            catch (System.Exception ex)
            {
                _eventLogService.LogException(nameof(DeletePostmarkBouncedEmailContacts), "POSTMARK API", ex);
                return $"An error has occurred, please check the event log for more details.";
            }
        }

        private async Task<List<Bounce>> GetAllBouncesAsync(string BaseUrl, string ServerToken, int Count = 100)
        {
            return await GetBouncesRecursiveAsync(BaseUrl, ServerToken, 0, Count, new List<Bounce>());
        }

        private async Task<List<Bounce>> GetBouncesRecursiveAsync(string BaseUrl, string ServerToken, int Offset, int Count, List<Bounce> AllBounces)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/bounces?type=HardBounce&inactive=true&count={Count}&offset={Offset}");
            request.Headers.Add("X-Postmark-Server-Token", ServerToken);
            request.Headers.Add("Accept", "application/json");

            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<BounceApiResponse>(responseContent);

            if (apiResponse != null)
            {
                AllBounces.AddRange(apiResponse.Bounces);

                if (apiResponse.Bounces.Count == Count)
                {
                    return await GetBouncesRecursiveAsync(BaseUrl, ServerToken, Offset + Count, Count, AllBounces);
                }
            }

            return AllBounces;
        }

        private int RemoveBouncedContacts(List<string> BouncedEmails)
        {
            int removedCount = 0;
            foreach (string email in BouncedEmails)
                if (!string.IsNullOrEmpty(email))
                {
                    ContactInfo deleteContact = ContactInfo.Provider.Get()
                                    .WhereEquals("ContactEmail", email)
                                    .TopN(1)
                                    .FirstOrDefault();
                    if (deleteContact != null)
                    {
                        ContactInfo.Provider.Delete(deleteContact);
                        removedCount++;
                    }
                }
            return removedCount;
        }
    }
}
