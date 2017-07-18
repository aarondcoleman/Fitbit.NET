using System;
using System.Reflection;

namespace Fitbit.Api.Portable
{
    public static class StringAttributeExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...

            //Look for our 'StringValueAttribute' 

            //in the field's custom attributes

#if NETSTANDARD_13
            FieldInfo fi = type.GetTypeInfo().GetDeclaredField(value.ToString());
#else
            FieldInfo fi = type.GetField(value.ToString());
#endif
            StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}