using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using SiteYonetimProje.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteYonetimProje.Models;
using System.Web.Helpers;

namespace SiteYonetimProje.Controllers
{
    public class GenelYoneticiController : Controller
    {
        DataContext db = new DataContext();
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public GenelYoneticiController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityContext()));
        }
        public ActionResult Index()
        {
            var yoneticiler = db.Apartman.Where(x => x.Yonetici_Id != null).ToList();
            if (yoneticiler.Count() != 0)
            {
                var list = userManager.Users;
                foreach (var item in yoneticiler)
                {

                }
            }
            ModelState.AddModelError("", "Sistemde yönetici bulunmamaktadır.");
            return View();
        }

        [HttpGet]
        public ActionResult ApartmanTanimlama()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApartmanTanimlama(ApartmanModel.ApartmanBilgileri model)
        {
            if (ModelState.IsValid)
            {
                if (model.DaireSayisi > 0 && model.DaireSayisi <= 14)
                {
                    DataContext db = new DataContext();
                    var apartmanBilgileri = new ApartmanModel.ApartmanBilgileri();
                    apartmanBilgileri.ApartmanBlok = model.ApartmanBlok;
                    apartmanBilgileri.ApartmanNo = model.ApartmanNo;
                    apartmanBilgileri.DaireSayisi = model.DaireSayisi;
                    db.Apartman.Add(apartmanBilgileri);
                    db.SaveChanges();
                    TempData["apartmanTanimlamaBasarili"] = "Apartman tanımlama başarıyla tamamlanmıştır.";
                    return View();
                }
                ModelState.AddModelError("", "Daire sayısını doğru giriniz... ");
                return View(model);
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult AtamaIslemleri()
        {

            return View(db.Apartman.Where(x => x.Yonetici_Id == null).ToList());
        }

        [HttpGet]
        public ActionResult AtamaIslemleriYonetici(int id)
        {
            ViewBag.ApartmanId = id;
            return View(userManager.Users.Where(x => x.Gorev == 3 && x.Apartman == id).ToList());
        }


        [HttpPost]
        public ActionResult AtamaIslemleriYonetici(string id, int apartmanId)
        {
            DataContext db = new DataContext();
            var apartmanBilgi = db.Apartman.Where(x => x.Id == apartmanId).ToList();
            var yoneticiBilgi = userManager.FindById(id);
            if (apartmanBilgi != null && yoneticiBilgi != null)
            {
                foreach (var item in apartmanBilgi)
                {
                    yoneticiBilgi.Gorev = 2;
                    userManager.Update(yoneticiBilgi);
                    item.Yonetici_Id = yoneticiBilgi.Id;
                    item.YoneticiAd = yoneticiBilgi.Ad;
                    item.YoneticiSoyad = yoneticiBilgi.Soyad;
                    db.SaveChanges();
                    TempData["yoneticiAtamaBasarili"] = "Yönetici atama işlemi başarıyla tamamlanmıştır";
                    return RedirectToAction("AtamaIslemleri");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult YoneticiListesi()
        {
            var yoneticiler = db.Apartman.Where(x => x.Yonetici_Id != null).ToList();
            if (yoneticiler.Count() != 0)
            {
                return View(yoneticiler);
            }
            return View();
        }


        [HttpGet]
        public ActionResult ApartmanListesi()
        {

            return View(db.Apartman.Where(x => x.Yonetici_Id != null).ToList());
        }

        [HttpGet]
        public ActionResult ApartmanaMesaj(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        public ActionResult ApartmanaMesaj(ApartmanModel.MesajIslemleri model, int apartmanId)
        {
            ApartmanModel.MesajIslemleri mesaj = new ApartmanModel.MesajIslemleri();
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == apartmanId);

            if (apartmanBilgi != null)
            {

                mesaj.MesajTuru = 3;
                mesaj.Mesaj = model.Mesaj;
                mesaj.ApartmanNo = apartmanBilgi.ApartmanNo;
                mesaj.Gonderen_Id = "Genel Yönetici";
                mesaj.Cevap = apartmanBilgi.Yonetici_Id;
                db.MesajIslemleri.Add(mesaj);
                db.SaveChanges();
                TempData["apartmanaMesajBasarili"] = "Apartmana mesajınız iletilmiştir.";
                return RedirectToAction("ApartmanaMesaj");

            }
            return View();

        }

        [HttpGet]
        public ActionResult GelenMesajlar()
        {
            return View(db.MesajIslemleri.Where(x=>x.MesajTuru==4));
        }

        [HttpGet]
        public ActionResult GorevliKayit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GorevliKayit(AccountModel.KayitOlustur model,bool siteSakiniMi)
        {
            ApplicationUser kullanici = new ApplicationUser();
            kullanici.UserName= model.UserName;
            kullanici.Email= model.Email;
            kullanici.OtoSifre = "Kou123";
            if(siteSakiniMi==true)
            {
                kullanici.Apartman = 70;
            }
            else
            {
                kullanici.Apartman = 99;
                kullanici.DaireNo = 99;
            }
            
            
            IdentityResult result = userManager.Create(kullanici,kullanici.OtoSifre);

            //string subject = "Aral konutları sistemi giriş bilgileriniz";
            //string body = "Şifreniz: Kou1234"  +
            //    " /               kullanıcı adınız:" + model.UserName;
            //WebMail.Send(model.Email, subject, body, null, null, null, true, null, null, null, null, null, null);
            if (result.Succeeded)
            {
                TempData["gorevliTanimlamaBasarili"] = "Görevli kayıt işlemi başarıyla tamamlanmıştır. Şifre bilgisi e-posta adresine gönderilmiştir.";
                return RedirectToAction("GorevliKayit");
            }
            else
            {

                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError("", errors);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GorevliListesi()
        {

            return View(userManager.Users.Where(x=>x.Gorev>3).ToList());
        }

        [HttpGet]
        public ActionResult TemizlikRandevulari()
        {
            
            var temizlikRandevulari = db.Gorev.Where(x=>x.BakimYeri==4).ToList();
            return View(temizlikRandevulari);
        }

        [HttpGet]
        public ActionResult HavuzBakimBilgileri()
        {
            var havuzBakimBilgileri = db.Gorev.Where(x => x.BakimYeri == 5).ToList();
            return View(havuzBakimBilgileri);
        }

        [HttpGet]
        public ActionResult SporSalonuBakimBilgileri()
        {
            var sporSalonuBakimBilgileri= db.Gorev.Where(x => x.BakimYeri == 6).ToList();
            return View(sporSalonuBakimBilgileri);
        }

        [HttpGet]
        public ActionResult GorevliCalismaKontrol(string id)
        {
            var gorevliCalismaKontrol = db.Gorev.Where(x => x.Gorevli_Id == id && x.Aktiflik==1 && x.Tamamlandi==1).ToList();
            foreach (var item in gorevliCalismaKontrol)
            {
                if(item.BakimYeri==4)
                {
                    ViewBag.Temizlik = "Temizlik";
                }
            }
            return View(gorevliCalismaKontrol);
        }

    }
}