using Microsoft.AspNet.Identity.EntityFramework;
using SiteYonetimProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SiteYonetimProje.Identity
{
    public class ApplicationUser:IdentityUser
    {
        
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Seflink { get; set; }
        public int Gorev { get; set; }
        public int Cinsiyet { get; set; }
        

        public DateTime? DogumTarihi { get; set; }

        public DateTime? Create_Time { get; set; }
        public DateTime? Update_Time { get; set; }
        public string OtoSifre { get; set; }

       






        public ICollection<ApartmanModel.ApartmanBilgileri> ApartmanBilgileris { get; set; }
        public ICollection<ApartmanModel.MesajIslemleri> MesajIslemleris{ get; set; }
        public int Apartman { get; set; }
        public int DaireNo { get; set; }

        public ICollection<GorevModel.Gorev> Gorevs{ get; set; }

    }
}