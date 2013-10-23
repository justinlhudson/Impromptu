using System;

namespace Impromptu
{
    public static class EnumStringAttribute
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            System.Type type = value.GetType();

            var fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(StringValue), false) as StringValue[];
            if(attrs.Length > 0)
                output = attrs[0].Value;

            return output;
        }

        public class StringValue : System.Attribute
        {
            private string m_value;

            public StringValue(string value)
            {
                m_value = value;
            }

            public string Value {
                get { return m_value; }
            }
        }
    }
}
