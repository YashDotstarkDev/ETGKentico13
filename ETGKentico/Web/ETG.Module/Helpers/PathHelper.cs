using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETG.Module.Helpers
{
    public static class PathHelper
    {
        public static string Get2LevelUpAliasPath(string nodeAliasPath)
        {
            if (nodeAliasPath != null || nodeAliasPath.Length > 0)
            {
                var index = nodeAliasPath.LastIndexOf("/");

                if (index > 0)
                {
                    nodeAliasPath = nodeAliasPath.Substring(0, index);

                    index = nodeAliasPath.LastIndexOf("/");

                    if (index > 0)
                    {
                        return nodeAliasPath.Substring(0, index);


                    }

                }
            }

            return string.Empty;
        }
    }
}