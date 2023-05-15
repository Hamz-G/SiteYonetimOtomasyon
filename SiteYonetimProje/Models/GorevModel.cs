using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiteYonetimProje.Models
{
    public class GorevModel
    {
        public class Gorev 
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public DateTime Tarih { get; set; }
            
            [Required]

            public string Saat { get; set; }

            [Required]
            public int? BakimYeri { get; set; }
            [Required]
            [DisplayName("Apartman Blok")]
            public string ApartmanBlok { get; set; }

            [Required]
            [DisplayName("Apartman no")]
            public int? ApartmanNo { get; set; }

            [Required]
            [DisplayName("Apartman no")]
            public int? DaireNo { get; set; }
            public int? Aktiflik { get; set; }
            public int? Tamamlandi { get; set; }

            public string Gorevli_Id { get; set; }
        }
    }
}