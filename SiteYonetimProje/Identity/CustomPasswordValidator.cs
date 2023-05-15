using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SiteYonetimProje.Identity
{
    public class CustomPasswordValidator:PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            var result = await base.ValidateAsync(password);
            if(password.Contains("1234"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Şifre ardışık sayı içeremez");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}