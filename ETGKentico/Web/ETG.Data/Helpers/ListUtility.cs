using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace ETG.Data.Helpers
{
    public static class ListUtility
    {
        public static List<string> GetCombinedList(char separator, params string[] strs)
        {
            List<string> combinedList = new List<string>();
            foreach (var str in strs)
            {
                if (str.IsNullOrEmpty())
                {
                    continue;
                }
                combinedList.AddRange(str.Split(separator).ToList());
            }

            return combinedList;
        }
    }
}
