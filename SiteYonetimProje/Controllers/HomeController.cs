using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using SiteYonetimProje.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteYonetimProje.Models;

namespace SiteYonetimProje.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public HomeController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityContext()));
        }

        DataContext db = new DataContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult ApartmanaGelenMesajlar()
        {
            var apartmanSakiniBilgi = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == apartmanSakiniBilgi.Apartman);
            if (apartmanBilgi != null)
            {
                return View(db.MesajIslemleri.Where(x => x.ApartmanNo == apartmanBilgi.ApartmanNo && x.MesajTuru == 2).ToList());
            }
            return View();
        }

        [HttpGet]
        public ActionResult MesajOnay(int id)
        {
            var mesajBilgi = db.MesajIslemleri.FirstOrDefault(x => x.Id == id);
            if (mesajBilgi != null)
            {

                mesajBilgi.OkunduDurum = 1;
                db.SaveChanges();
                if (mesajBilgi.MesajTuru == 1)
                {
                    TempData["onayIslemiBasarili"] = "Mesaj onaylama işleminiz başarılı";
                    return RedirectToAction("DaireyeGelenMesajlar");
                }
                else if (mesajBilgi.MesajTuru == 2)
                {
                    TempData["onayIslemiBasarili"] = "Mesaj onaylama işleminiz başarılı";
                    return RedirectToAction("ApartmanaGelenMesjalar");
                }
                else if (mesajBilgi.MesajTuru == 3)
                {
                    //yöneticiye gelen mesajlarda genelyöneticiden gelen mesajın onayı
                    TempData["onayIslemiBasarili"] = "Mesaj onaylama işleminiz başarılı";
                    return RedirectToAction("GelenMesajlar", "Yonetici");
                }
                else if (mesajBilgi.MesajTuru == 4)
                {
                    //genelyöneticiye gelen yönetici mesajlarının onayı
                    TempData["onayIslemiBasarili"] = "Mesaj onaylama işleminiz başarılı";
                    return RedirectToAction("GelenMesajlar", "GenelYonetici");
                }
                else if (mesajBilgi.MesajTuru == 5)
                {
                    //apartmansakininden yöneticiye
                    TempData["onayIslemiBasarili"] = "Mesaj onaylama işleminiz başarılı";
                    return RedirectToAction("GelenMesajlar", "Yonetici");
                }
                else if (mesajBilgi.MesajTuru == 10)
                {
                    //apartmansakininden yöneticiye
                    TempData["onayIslemiBasarili"] = "Şikayet tamamlama işleminiz başarılı";
                    return RedirectToAction("GelenMesajlar", "Yonetici");

                }
                else if (mesajBilgi.MesajTuru == 11)
                {
                    //apartmansakininden yöneticiye
                    TempData["onayIslemiBasarili"] = "Şikayet tamamlama işleminiz başarılı";
                    return RedirectToAction("GelenMesajlar", "Yonetici");
                }
                else if (mesajBilgi.MesajTuru == 12)
                {
                    //apartmansakininden yöneticiye
                    TempData["onayIslemiBasarili"] = "Şikayet tamamlama işleminiz başarılı";
                    return RedirectToAction("GelenMesajlar", "Yonetici");
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult DaireyeGelenMesajlar()
        {
            var apartmanSakiniBilgi = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == apartmanSakiniBilgi.Apartman);
            if (apartmanBilgi != null)
            {
                return View(db.MesajIslemleri.Where(x => x.ApartmanNo == apartmanBilgi.ApartmanNo && x.DaireNo == apartmanSakiniBilgi.DaireNo && x.MesajTuru == 1).ToList());
            }
            return View();
        }


        [HttpGet]
        public ActionResult YoneticiyeMesaj()
        {

            return View();

        }
        [HttpPost]
        public ActionResult YoneticiyeMesaj(ApartmanModel.MesajIslemleri model)
        {
            var apartmanSakiniBilgi = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == apartmanSakiniBilgi.Apartman);
            var yoneticiBilgi = userManager.Users.FirstOrDefault(x => x.Id == apartmanBilgi.Yonetici_Id);

            ApartmanModel.MesajIslemleri mesaj = new ApartmanModel.MesajIslemleri();
            mesaj.MesajTuru = 5;
            mesaj.Mesaj = model.Mesaj;
            mesaj.ApartmanNo = apartmanBilgi.ApartmanNo;
            mesaj.DaireNo = yoneticiBilgi.DaireNo;
            mesaj.Gonderen_DaireNo = apartmanSakiniBilgi.DaireNo;
            mesaj.Gonderen_Id = User.Identity.GetUserId();
            db.MesajIslemleri.Add(mesaj);
            db.SaveChanges();
            TempData["yoneticiyeMesajBasarili"] = "Yöneticiye mesajınız iletilmiştir.";
            return RedirectToAction("YoneticiyeMesaj");
        }



        [HttpGet]
        public ActionResult TemizlikGorevliListesi()
        {
            return View(userManager.Users.Where(x => x.Gorev == 4).ToList());
        }
        [HttpGet]
        public ActionResult TemizlikRandevusu(string id)
        {
            var tarihtekiSaatler = db.Gorev.Where(x => x.Tarih == DateTime.Today && x.Gorevli_Id == id).ToList();
            if (tarihtekiSaatler.Count != 0)
            {
                ViewBag.GorevliId = id;
                return View(tarihtekiSaatler);
            }
            else
            {
                ViewBag.GorevliId = id;
                ViewBag.TarihKontrol = 0;
                return View();
            }

        }
        [HttpPost]
        public ActionResult TemizlikRandevusu(GorevModel.Gorev model, string id, string saat)
        {
            var tarihtekiSaatler = db.Gorev.Where(x => x.Tarih == DateTime.Today && x.Gorevli_Id == id).ToList();
            if (tarihtekiSaatler.Count != 0)
            {
                foreach (var item in tarihtekiSaatler)
                {
                    if (saat == item.Saat)
                    {
                        TempData["SaatKontrolu"] = "Dolu saatlere dikkat ediniz...";
                        return RedirectToAction("TemizlikRandevusu");
                    }
                }
            }
            var kullanici = userManager.FindByName(User.Identity.Name);
            var kullaniciApartmanBilgi = db.Apartman.Where(x => x.Id == kullanici.Apartman).ToList();
            var randevu = new GorevModel.Gorev();
            randevu.Tarih = DateTime.Today;
            randevu.Saat = saat;
            randevu.Gorevli_Id = id;
            randevu.BakimYeri = 4;
            randevu.DaireNo = kullanici.DaireNo;
            if (kullaniciApartmanBilgi.Count != 0)
            {
                foreach (var item in kullaniciApartmanBilgi)
                {
                    randevu.ApartmanBlok = item.ApartmanBlok;
                    randevu.ApartmanNo = item.ApartmanNo;
                }
            }
            randevu.Aktiflik = 1;
            randevu.Tamamlandi = 0;
            db.Gorev.Add(randevu);
            db.SaveChanges();
            TempData["RandevuBasarili"] = "Randevunuz oluşturulmuştur...";
            return RedirectToAction("TemizlikRandevusu");
        }
        [HttpGet]
        public ActionResult TemizlikRandevularim()
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            var kullaniciApartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == kullanici.Apartman);
            if (kullaniciApartmanBilgi != null)
            {
                var kullaniciTemizlikRandevulari = db.Gorev.Where(x => x.ApartmanBlok == kullaniciApartmanBilgi.ApartmanBlok && x.ApartmanNo == kullaniciApartmanBilgi.ApartmanNo && x.DaireNo == kullanici.DaireNo).ToList();
                return View(kullaniciTemizlikRandevulari);
            }
            return View();
        }

        public ActionResult TemizlikTamamla(int id)
        {
            var temizlikKontrol = db.Gorev.FirstOrDefault(x => x.Id == id);
            if (temizlikKontrol != null)
            {
                temizlikKontrol.Aktiflik = 1;
                temizlikKontrol.Tamamlandi = 1;
                db.SaveChanges();
                TempData["onayIslemiBasarili"] = "Temizlik randevusu tamamlanmıştır.";
                return RedirectToAction("TemizlikRandevularim");
            }
            return View();
        }


        [HttpGet]
        public ActionResult HavuzBakimBilgileri()
        {
            var havuzBakimBilgileri = db.Gorev.Where(x => x.BakimYeri == 5 && x.Tarih == DateTime.Today).ToList();
            return View(havuzBakimBilgileri);
        }

        [HttpGet]
        public ActionResult SporSalonuBakimBilgileri()
        {
            var sporSalonuBakimBilgileri = db.Gorev.Where(x => x.BakimYeri == 6 && x.Tarih == DateTime.Today).ToList();
            return View(sporSalonuBakimBilgileri);
        }

        [HttpGet]
        public ActionResult Sikayetlerim()
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            if(kullanici!=null)
            {
                var sikayetler = db.MesajIslemleri.Where(x => x.Gonderen_Id == kullanici.Id && x.Cevap != null);
                return View(sikayetler);
            }
            return View();
           

            
        }

        [HttpGet]
        public ActionResult TemizlikSikayeti()
        {
            return View(userManager.Users.Where(x => x.Gorev == 4));
        }

        [HttpPost]
        public ActionResult TemizlikSikayeti(string sikayet, string sikayetId)
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == kullanici.Apartman);
            var gorevli = userManager.FindById(sikayetId);
            if (gorevli != null && kullanici != null)
            {
                var sikayetOlustur = new ApartmanModel.MesajIslemleri();
                sikayetOlustur.MesajTuru = 10;
                sikayetOlustur.Mesaj = sikayet;
                sikayetOlustur.Gonderen_Id = kullanici.Id;
                sikayetOlustur.Gonderen_DaireNo = kullanici.DaireNo;
                sikayetOlustur.ApartmanNo = apartmanBilgi.ApartmanNo;
                sikayetOlustur.Cevap = sikayetId;// sikayet edilen görevli
                db.MesajIslemleri.Add(sikayetOlustur);
                db.SaveChanges();
                TempData["SikayetOlusturmaBasarili"] = "Şikayetiniz oluşmuştur. Şikayet durumunu şikayetlerim alanından takip edebilirsiniz";
                return RedirectToAction("Sikayetlerim");
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        public ActionResult HavuzVeyaSporSalonuSikayeti()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HavuzVeyaSporSalonuSikayeti(string sikayet,string sikayetAlani)
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == kullanici.Apartman);
            if(kullanici!=null && apartmanBilgi!=null)
            {
                var sikayetOlustur = new ApartmanModel.MesajIslemleri();
                if(sikayetAlani=="havuz")
                {
                    sikayetOlustur.MesajTuru = 11;
                    sikayetOlustur.Cevap = "havuz";
                }
                    
                if (sikayetAlani == "sporSalonu")
                {
                    sikayetOlustur.Cevap = "sporSalonu";
                    sikayetOlustur.MesajTuru = 12;
                }
                sikayetOlustur.Mesaj = sikayet;
                sikayetOlustur.Gonderen_Id = kullanici.Id;
                sikayetOlustur.Gonderen_DaireNo = kullanici.DaireNo;
                sikayetOlustur.ApartmanNo = apartmanBilgi.ApartmanNo;
                db.MesajIslemleri.Add(sikayetOlustur);
                db.SaveChanges();
                TempData["SikayetOlusturmaBasarili"] = "Şikayetiniz oluşmuştur. Şikayet durumunu şikayetlerim alanından takip edebilirsiniz";
                return RedirectToAction("Sikayetlerim");
            }
            return View();
        }
    }
}