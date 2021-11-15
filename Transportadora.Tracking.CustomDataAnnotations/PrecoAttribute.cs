using System;
using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.CustomDataAnnotations
{
    public class PrecoAttribute : ValidationAttribute
    {
        public PrecoAttribute() { }

        public override bool IsValid(object value)
        {
            var preco = (string)value;

            try
            {
                Convert.ToDouble(preco);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
