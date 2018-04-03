using System;
using System.ComponentModel;
using System.Text;

namespace WaveBasedMethodModel
{
    /// <summary>
    /// Provides a utility class with static methods
    /// </summary>
    public static class Utility
    {
        public static string ToString<T>(T obj)
        {
            PropertyDescriptorCollection coll = TypeDescriptor.GetProperties(obj);
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (PropertyDescriptor pd in coll)
            {
                builder.Append(string.Format("{0}:{1} ", pd.Name, pd.GetValue(obj).ToString()));
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
