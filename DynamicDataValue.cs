using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csv_viewer
{
    using System;
    using System.Globalization;

    public class DynamicDataValue
    {
        public object? Value { get; private set; }
        public Type? DataType { get; private set; }

        public DynamicDataValue(string value)
        {
            InferType(value);
        }

        private void InferType(string value)
        {
            // Check for Boolean
            if (bool.TryParse(value, out bool boolVal))
            {
                Value = boolVal;
                DataType = typeof(bool);
                return;
            }

            // Check for Integer
            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intVal))
            {
                Value = intVal;
                DataType = typeof(int);
                return;
            }

            // Check for Float
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                Value = floatVal;
                DataType = typeof(float);
                return;
            }

            // Check for DateTime
            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeVal))
            {
                Value = dateTimeVal;
                DataType = typeof(DateTime);
                return;
            }

            //Check for empty value
            if (!string.IsNullOrEmpty(value))
            {
                Value = null;
                DataType = null;
                return;
            }

            // Default to String
            Value = value;
            DataType = typeof(string);
        }

        public override string ToString()
        {
            return $"Value: {Value}, Type: {DataType}";
        }
    }
}
