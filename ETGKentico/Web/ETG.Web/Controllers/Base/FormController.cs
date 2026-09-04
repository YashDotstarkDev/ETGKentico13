using System.Web.Mvc;
using ETG.Data.Services;

namespace ETG.Web.Controllers.Base
{
    public class FormController : Controller
    {
        private readonly IKenticoContactService _contactService;
        public FormController(IKenticoContactService contactService)
        {
            _contactService = contactService;
        }

        protected void AddContact(bool subscribeToNewsletter, string firstName,
            string lastName, string email, string phone, string contactType)
        {
            _contactService.AddContactForWebsiteConsumer(firstName,
                lastName, email, phone, contactType, subscribeToNewsletter);
        }
    }
}
