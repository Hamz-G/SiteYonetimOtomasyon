using SiteYonetimProje.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SiteYonetimProje.Identity
{
    public class DataContext : DbContext
    {
        public DataContext() : base("dbConnection")
        {

        }

        public DbSet<ApartmanModel.ApartmanBilgileri> Apartman { get; set; }
        public DbSet<ApartmanModel.MesajIslemleri> MesajIslemleri { get; set; }
        public DbSet<GorevModel.Gorev> Gorev{ get; set; }

    }
}