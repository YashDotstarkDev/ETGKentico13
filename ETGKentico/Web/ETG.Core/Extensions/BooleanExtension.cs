using Castle.Core.Internal;

namespace ETG.Core.Extensions
{
    public static class BooleanExtension
    {
        public static string YesOrNo(this bool b, string NoCaption = "", bool EmptyYesCaption = false)
        {
            if (b)
            {
                if (EmptyYesCaption)
                {
                    return string.Empty;
                }
                return "Yes";
            }

            if (NoCaption.IsNullOrEmpty())
            {
                return "No";
            }

            return NoCaption;
        }
    }
}
