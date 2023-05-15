using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using SiteYonetimProje.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteYonetimProje.Models;
using Microsoft.Ajax.Utilities;

namespace SiteYonetimProje.Controllers
{
    public class GorevliController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public GorevliController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityContext()));

        }
        DataContext db = new DataContext();
        public ActionResult Index()
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            if (kullanici != null)
            {
                if (kullanici.Gorev == 4)
                {
                    TempData["Gorev"] = 4;
                }
                else if (kullanici.Gorev == 5)
                {
                    TempData["Gorev"] = 5;
                }
                else if (kullanici.Gorev == 6)
                {
                    TempData["Gorev"] = 6;
                }
            }
            return View();
        }

        public ActionResult GorevliListesi()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TemizlikRandevulari()
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            var gorevBilgileri = db.Gorev.Where(x => x.Gorevli_Id == kullanici.Id).ToList();
            TempData["Gorev"] = "4";
            return View(gorevBilgileri);
        }

        public ActionResult GorevOnay(int id)
        {
            var gorevKontrol = db.Gorev.FirstOrDefault(x => x.Id == id);
            if (gorevKontrol != null)
            {
                gorevKontrol.Aktiflik = 0;
                gorevKontrol.Tamamlandi = 0;
                db.SaveChanges();
                TempData["onayIslemiBasarili"] = "Temizlik randevusu kabul edilmiştir. Temizlikten sonra temizlediğiniz daireye onay yapmaları gerektiğini söyleyiniz..";
                return RedirectToAction("TemizlikRandevulari");
            }
            return View();
        }


        [HttpGet]
        public ActionResult HavuzBakimRaporu()
        {

            var kullanici = userManager.FindByName(User.Identity.Name);
            var tarihtekiSaatler = db.Gorev.Where(x => x.Tarih == DateTime.Today && x.ApartmanBlok == "0" && x.ApartmanNo == 0 && x.DaireNo == 0 && x.BakimYeri == 5).ToList();
            string saat = "9";
            TempData["Gorev"] = "5";
            if (tarihtekiSaatler.Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    var havuzKontrolSaat = new GorevModel.Gorev();
                    havuzKontrolSaat.Aktiflik = 1;
                    havuzKontrolSaat.Tamamlandi = 0;
                    havuzKontrolSaat.ApartmanBlok = "0";
                    havuzKontrolSaat.ApartmanNo = 0;
                    havuzKontrolSaat.DaireNo = 0;
                    havuzKontrolSaat.BakimYeri = 5;
                    havuzKontrolSaat.Saat = saat + ".00";

                    int havuzArttir = Convert.ToInt32(saat);
                    havuzArttir = havuzArttir + 1;
                    saat = havuzArttir.ToString();
                    havuzKontrolSaat.Tarih = DateTime.Today;
                    db.Gorev.Add(havuzKontrolSaat);
                    db.SaveChanges();


                }
                var tarihtekiSaatler2 = db.Gorev.Where(x => x.Tarih == DateTime.Today && x.ApartmanBlok == "0" && x.ApartmanNo == 0 && x.DaireNo == 0 && x.BakimYeri == 5).ToList();
                return View(tarihtekiSaatler2);
            }
            else
            {
                return View(tarihtekiSaatler);
            }

        }

        [HttpGet]
        public ActionResult HavuzBakimOnay(int id)
        {
            var kullanici = userManager.FindByName(User.Identity.Name);

            var gorevKontrol = db.Gorev.FirstOrDefault(x => x.Id == id);
            if (gorevKontrol != null)
            {
                gorevKontrol.Aktiflik = 1;
                gorevKontrol.Tamamlandi = 1;
                gorevKontrol.Gorevli_Id = kullanici.Id;
                db.SaveChanges();

                TempData["onayIslemiBasarili"] = "Havuz bakımı onaylanmıştır..";
                return RedirectToAction("HavuzBakimRaporu");



            }
            return View();
        }

        [HttpGet]
        public ActionResult SporSalonuBilgileri()
        {

            var kullanici = userManager.FindByName(User.Identity.Name);
            var tarihtekiSaatler = db.Gorev.Where(x => x.Tarih == DateTime.Today && x.ApartmanBlok == "0" && x.ApartmanNo == 0 && x.DaireNo == 0 && x.BakimYeri == 6).ToList();
            string saat = "10";
            TempData["Gorev"] = "6";
            if (tarihtekiSaatler.Count() == 0)
            {
                for (int i = 0; i < 10; i++)
                {

                    var havuzKontrolSaat = new GorevModel.Gorev();
                    havuzKontrolSaat.Aktiflik = 1;
                    havuzKontrolSaat.Tamamlandi = 0;
                    havuzKontrolSaat.ApartmanBlok = "0";
                    havuzKontrolSaat.ApartmanNo = 0;
                    havuzKontrolSaat.DaireNo = 0;
                    havuzKontrolSaat.BakimYeri = 6;
                    havuzKontrolSaat.Saat = saat + ".00";

                    int havuzArttir = Convert.ToInt32(saat);
                    havuzArttir = havuzArttir + 1;
                    saat = havuzArttir.ToString();
                    havuzKontrolSaat.Tarih = DateTime.Today;
                    db.Gorev.Add(havuzKontrolSaat);
                    db.SaveChanges();


                }
                var tarihtekiSaatler2 = db.Gorev.Where(x => x.Tarih == DateTime.Today && x.ApartmanBlok == "0" && x.ApartmanNo == 0 && x.DaireNo == 0 && x.BakimYeri == 6).ToList();
                return View(tarihtekiSaatler2);
            }
            else
            {
                return View(tarihtekiSaatler);
            }

        }

        [HttpGet]
        public ActionResult SporSalonuBakimOnay(int id)
        {
            var kullanici = userManager.FindByName(User.Identity.Name);

            var gorevKontrol = db.Gorev.FirstOrDefault(x => x.Id == id);
            if (gorevKontrol != null)
            {
                gorevKontrol.Aktiflik = 1;
                gorevKontrol.Tamamlandi = 1;
                gorevKontrol.Gorevli_Id = kullanici.Id;
                db.SaveChanges();

                TempData["onayIslemiBasarili"] = "Spor salonu bakımı onaylanmıştır..";
                return RedirectToAction("SporSalonuBilgileri");



            }
            return View();
        }

        [HttpGet]
        public ActionResult GelenMesajlar()
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            if(kullanici!=null)
            {
                if(kullanici.Gorev==4)
                {
                    
                    var gelenMesajlar = db.MesajIslemleri.Where(x => x.Cevap == kullanici.Id && x.OkunduDurum == 1).ToList();
                    ViewBag.TemizlikKontrol = "Temizlik";
                    TempData["Gorev"] = "4";
                    return View(gelenMesajlar);
                }
                else if(kullanici.Gorev==5)
                {
                    var gelenMesajlar = db.MesajIslemleri.Where(x => x.Cevap== "havuz" && x.OkunduDurum==1).ToList(); // && x.MesajTuru==21
                    TempData["Gorev"] = "5";

                    return View(gelenMesajlar);

                }
                else if(kullanici.Gorev==6)
                {
                    var gelenMesajlar = db.MesajIslemleri.Where(x => x.Cevap == "sporSalonu" && x.OkunduDurum == 1).ToList();
                    TempData["Gorev"] = "6";

                    return View(gelenMesajlar);
                }
            }
            return View();
        }
    }
}