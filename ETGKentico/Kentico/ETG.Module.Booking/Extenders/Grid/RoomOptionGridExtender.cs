using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CMS.Base;
using CMS.Base.Web.UI;
using CMS.Base.Web.UI.ActionsConfig;
using CMS.Helpers;
using CMS.UIControls;

namespace ETG.Module.Booking.Extenders.Grid
{
    public class RoomOptionGridExtender : ControlExtender<UniGrid>
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
                default:
                    return parameter;
                case "roomoptiontype":

                    var type = parameter.ToInteger(0);

                    switch (type)
                    {
                        case 1:
                            return "Twin Share";
                        case 2:
                            return "Single Room";
                        case 3:
                            return "Extras";
                    }
                    
                    return string.Empty;
            }
        }
    }
}
