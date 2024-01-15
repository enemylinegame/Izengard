using System.ComponentModel;
using System;

namespace Tools
{
    public static class EnumExtension
    {
        /// <summary>
        /// Get Description in Enum
        /// </summary>
        /// <param name="currentEnum">Enum</param>
        /// <returns></returns>
        public static string GetDescription(this Enum currentEnum)
        {
            var fi = currentEnum.GetType().GetField(currentEnum.ToString());

            if (fi != null)
            {
                var da = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

                return da != null ? da.Description : currentEnum.ToString();
            }
            return "";
        }
    }
}
