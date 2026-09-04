using System.Collections.Generic;

namespace ETG.Data.Helpers
{
    public class CruiseType
    {
        public string CruiseTypeName { get; set; }
        public string BlackIconImage { get; set; }
        public string WhiteIconImage { get; set; }
    }
    public class CruiseMapper
    {
        public static Dictionary<int, CruiseType> CruiseTypeMapping = new Dictionary<int, CruiseType>
        {
            {
                1, new CruiseType
                {
                    CruiseTypeName = "Self-Drive Cruising",
                    BlackIconImage = "/getmedia/6318224b-3231-47a4-84bf-6b89d1ab8627/icon-self_drive_cruising.png",
                    WhiteIconImage = "/getmedia/512e7c1b-bbdb-44b1-b8e3-7e3e63e5836a/icon-self_drive_cruising-white.png"
                }
            },
            {
                2, new CruiseType
                {
                    CruiseTypeName = "Barge Cruising",
                    BlackIconImage = "/getmedia/984bc083-be8a-444c-817f-ec465ef4d1b4/icon-barge_cruising.png",
                    WhiteIconImage = "/getmedia/8cab3202-06b0-43f3-a6f1-e1fff9caa368/icon-barge_cruising-white.png"
                }
            },
            {
                3, new CruiseType
                {
                    CruiseTypeName = "River Cruising",
                    BlackIconImage = "/getmedia/73cad232-c4d7-4aca-879a-638b3c1fcdc9/icon-river_cruising.png",
                    WhiteIconImage = "/getmedia/1ec3ef21-9919-442b-824e-829f865cee9b/icon-river_cruising-white.png"
                }
            },{
                4, new CruiseType
                {
                    CruiseTypeName = "Ocean Cruising",
                    BlackIconImage = "/getmedia/16fbe5c3-b320-4eb6-843e-982ae944d095/icon-ocean_cruising.png",
                    WhiteIconImage = "/getmedia/90964a03-58fd-45ac-88a9-805f77eb104e/icon-ocean_cruising-white.png"
                }
            }

        };
    }
}
