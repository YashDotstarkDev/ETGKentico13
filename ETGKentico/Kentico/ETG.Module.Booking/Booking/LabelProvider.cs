using ETG.Booking.Pricing.Enums;

namespace ETG.Module.Booking.Booking
{
    public static class LabelProvider
    {
        public static string GetRoomOptionTypeLabel(RoomOptionTypeEnum type)
        {
            switch (type)
            {
                case RoomOptionTypeEnum.TwinShareRoomOption:
                    return "Twin share room";
                case RoomOptionTypeEnum.SingleRoomOption:
                    return "Single room";
                case RoomOptionTypeEnum.Extras:
                    return "Extras";
            }

            return string.Empty;
        }
    }
}
