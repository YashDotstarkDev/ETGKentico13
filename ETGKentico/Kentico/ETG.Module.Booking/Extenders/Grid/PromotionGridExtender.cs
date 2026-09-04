using CMS.Base.Web.UI;
using CMS.UIControls;

namespace ETG.Module.Booking.Extenders.Grid
{
    public class PromotionGridExtender: ControlExtender<UniGrid>
    {
        public override void OnInit()
        {
            // Registers a method that handles the functionality of header actions
            Control.OnExternalDataBound += Control_OnExternalDataBound;
        }

        private object Control_OnExternalDataBound(object sender, string sourceName, object parameter)
        {
            switch (sourceName)
            {
            }

            return parameter;
        }
    }
}