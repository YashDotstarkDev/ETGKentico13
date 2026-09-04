using ETG.Data.Models.Common;

namespace ETG.Data.Services
{
    public interface IKenticoContactService
    {
        bool ContactHasTags(string email, string[] tags);
        bool AddContact(string email, bool isAgent);
        bool AddContact(ContactInfoModel contactModel, bool subscribeToNewsletter);

        bool Unsubscribe(string Email, string ReasonId = "");
        bool AddContactForWebsiteConsumer(string firstName, string lastName, string email, string phoneNumber,
            string preferredContactMethod, bool subscribeToNewsletter);
        
    }
}
