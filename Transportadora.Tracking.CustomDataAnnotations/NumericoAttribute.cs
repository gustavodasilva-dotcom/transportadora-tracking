using System;
using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.CustomDataAnnotations
{
    public class NumericoAttribute : ValidationAttribute
    {
        public NumericoAttribute() { }

        public override bool IsValid(object value)
        {
            var entrada = (string)value;

            try
            {
                Convert.ToInt32(entrada);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
