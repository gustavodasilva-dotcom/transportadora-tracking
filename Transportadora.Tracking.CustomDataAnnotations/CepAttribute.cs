using System;
using System.ComponentModel.DataAnnotations;

namespace Transportadora.Tracking.CustomDataAnnotations
{
    public class CepAttribute : ValidationAttribute
    {
        public CepAttribute() { }

        public override bool IsValid(object value)
        {
            var cep = (string)value;

            if (cep.Length == 8)
            {
                try
                {
                    Convert.ToInt32(cep);

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
