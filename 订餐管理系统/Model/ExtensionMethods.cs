using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 订餐管理系统.Model
{
    public static class ExtensionMethods
    {
        public static string SplitArrayToString(this string[] strArray, string delimiter)
        {
            return strArray.SplitArrayToString(string.Empty, delimiter);
        }

        public static string SplitArrayToString(this string[] strArray, string adornment, string delimiter)
        {
            StringBuilder splitStrb = new StringBuilder();
            if (strArray == null || delimiter == null)
            {
                throw new NullReferenceException("参数不能为null");
            }
            else
            {
                for (int i = 0; i < strArray.Length - 1; i++)
                {
                    splitStrb.Append(adornment); 
                    splitStrb.Append(strArray[i]); 
                    splitStrb.Append(adornment); 
                    splitStrb.Append(delimiter);
                }
                splitStrb.Append(adornment); 
                splitStrb.Append(strArray[strArray.Length - 1]); 
                splitStrb.Append(adornment);
            }
            return splitStrb.ToString();
        }

        
    }
}
