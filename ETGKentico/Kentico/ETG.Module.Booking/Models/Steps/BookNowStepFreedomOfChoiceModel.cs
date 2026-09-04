using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Devotion.Automapper.Common;

namespace ETG.Module.Booking.Models.Steps
{
    public class BookNowStepFreedomOfChoiceModel : BookNowStepModel, IDataModel
    {
        public bool? FreedomOfChoiceSelectNow { get; set; }
        public List<FreedomOfChoiceItem> SelectedFreedomOfChoices { get; set; }

        public string IsSelected(string dayCaption, string value)
        {
            if (SelectedFreedomOfChoices.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (SelectedFreedomOfChoices.Any(a => a.DayCaption == dayCaption && a.OptionLabel == value))
            {
                return "checked";
            }

            return string.Empty;

        }

        public bool DisableSelectNow { get; set; }
    }
}