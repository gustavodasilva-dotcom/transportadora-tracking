using System;
using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.CustomDataAnnotations
{
    public class CpfAttribute : ValidationAttribute
    {
        public CpfAttribute() { }

        public override bool IsValid(object value)
        {
            if (value.ToString() != "" && value.ToString() != "string")
            {
                var input = (string)value;

                if (input.Length != 11)
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
