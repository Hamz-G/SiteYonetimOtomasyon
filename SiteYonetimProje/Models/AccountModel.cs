using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteYonetimProje.Models
{
    public class AccountModel
    {
        public class KayitOlustur
        {
            [Required]
            [DisplayName("Kullanıcı Adınız")]
            public string UserName { get; set; }

            [Required]
            [EmailAddress(ErrorMessage = "Eposta adresiniz hatalıdır.")]
            [DisplayName("Email Adresiniz")]
            public string Email { get; set; }


            //[MaxLength(11,ErrorMessage = "Telefon numaranız en fazla 11 haneli olmalıdır")]
            [Required]
            [DisplayName("Şifreniz")]
            public string Sifre { get; set; }
        }

        public class Giris
        {
            [Required]
            [DisplayName("Kullanıcı Adınız")]
            public string UserName { get; set; }

            [Required]
            [DisplayName("Şifreniz")]
            public string Sifre { get; set; }
        }

        public class KisiselBilgiler
        {
            [Required]
            [DisplayName("Adınız")]
            public string Ad { get; set; }

            [Required]
            [DisplayName("Soyadınız")]
            public string Soyad { get; set; }

            [Required]
            [DisplayName("Telefon Numaranız")]
            public string Telefon { get; set; }

            [Required]
            [DisplayName("Göreviniz")]
            public int Gorev { get; set; }

            [Required]
            [DisplayName("Cinsiyetiniz")]
            public int Cinsiyet { get; set; }

            [Required]
            [DisplayName("Apartman Blok")]
            public string ApartmanBlok { get; set; }
            [Required]
            [DisplayName("Apartman no")]
            public int ApartmanNo { get; set; }
            [Required]
            [DisplayName("Daire no")]
            public int DaireNo { get; set; }


        }
    }
}