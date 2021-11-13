using System;
using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.CustomDataAnnotations
{
    public class CnpjAttribute : ValidationAttribute
    {
        public CnpjAttribute() { }

        public override bool IsValid(object value)
        {
            if (value.ToString() != "" && value.ToString() != "string")
            {
                var input = (string)value;

                if (input.Length != 14)
                    return false;

                try
                {
                    Convert.ToInt64(input.Trim());
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
