using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteYonetimProje.Models
{
    public class ApartmanModel
    {
        public class ApartmanBilgileri
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [DisplayName("Apartman Blok")]
            public string ApartmanBlok { get; set; }

            [Required]
            [DisplayName("Apartman no")]
            public int ApartmanNo { get; set; }

            [Required]
            [DisplayName("Apartman Daire Sayısı")]
            public int DaireSayisi { get; set; }

            [Required]
            [DisplayName("Apartmandaki Dolu Daire Sayısı")]
            public int DoluDaireSayisi { get; set; }

            public string YoneticiAd { get; set; }
            
            public string YoneticiSoyad { get; set; }
            public string Yonetici_Id { get; set; }
        }


        public class MesajIslemleri
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public string Mesaj { get; set; }

            public string Cevap { get; set; }

            public int MesajTuru { get; set; }

            public int DaireNo { get; set; }
            

            public int ApartmanNo { get; set; }

            public int OkunduDurum { get; set; }

            public string Gonderen_Id { get; set; }

            public int Gonderen_DaireNo { get; set; }

        }
    }
}