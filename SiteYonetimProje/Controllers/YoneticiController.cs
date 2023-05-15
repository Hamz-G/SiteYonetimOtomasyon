using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SiteYonetimProje.Identity;
using SiteYonetimProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteYonetimProje.Controllers
{
    public class YoneticiController : Controller
    {
        DataContext db = new DataContext();
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public YoneticiController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityContext()));
        }

        public ActionResult Index()
        {
            var yonetici = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.Where(x => x.Yonetici_Id == yonetici.Id).ToList();
            if (apartmanBilgi.Count() != 0)
            {
                foreach (var item in apartmanBilgi)
                {
                    var apartmanSakinleri = userManager.Users.Where(x => x.Apartman == item.Id && x.DaireNo != 99 && x.Id != yonetici.Id).ToList();
                    return View(apartmanSakinleri);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult DaireyeMesaj(string id)
        {
            ViewBag.kullaniciId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DaireyeMesaj(ApartmanModel.MesajIslemleri model, string kullaniciId)
        {
            var yoneticiBilgi = userManager.FindByName(User.Identity.Name);

            var apartmanSakiniBilgi = userManager.Users.FirstOrDefault(x => x.Id == kullaniciId);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == apartmanSakiniBilgi.Apartman);
            ApartmanModel.MesajIslemleri mesaj = new ApartmanModel.MesajIslemleri();
            mesaj.MesajTuru = 1;
            mesaj.Mesaj = model.Mesaj;
            mesaj.ApartmanNo = apartmanBilgi.ApartmanNo;
            mesaj.DaireNo = apartmanSakiniBilgi.DaireNo;
            mesaj.Gonderen_Id = User.Identity.GetUserId();
            mesaj.Gonderen_DaireNo = yoneticiBilgi.DaireNo;
            db.MesajIslemleri.Add(mesaj);
            db.SaveChanges();
            TempData["daireyeMesajBasarili"] = "Daireye mesajınız iletilmiştir.";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult ApartmanaMesaj()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApartmanaMesaj(ApartmanModel.MesajIslemleri model)
        {
            ApartmanModel.MesajIslemleri mesaj = new ApartmanModel.MesajIslemleri();

            var yoneticiBilgi = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.Where(x => x.Yonetici_Id == yoneticiBilgi.Id).ToList();
            if (apartmanBilgi.Count != 0)
            {
                foreach (var item in apartmanBilgi)
                {
                    mesaj.MesajTuru = 2;
                    mesaj.Mesaj = model.Mesaj;
                    mesaj.ApartmanNo = item.ApartmanNo;
                    mesaj.Gonderen_Id = User.Identity.GetUserId();
                    mesaj.Gonderen_DaireNo = yoneticiBilgi.DaireNo;
                    db.MesajIslemleri.Add(mesaj);
                    db.SaveChanges();
                    TempData["apartmanaMesajBasarili"] = "Apartmana mesajınız iletilmiştir.";
                    return RedirectToAction("ApartmanaMesaj");
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult GelenMesajlar()
        {
            var yoneticiId = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == yoneticiId.Apartman);
            var apartmandakiSakinler = userManager.Users.Where(x => x.Apartman == yoneticiId.Apartman).ToList();
            var list = new List<ApartmanModel.MesajIslemleri>();
            foreach (var item in apartmandakiSakinler)
            {
                var mesajApartman = db.MesajIslemleri.Where(x=>x.Gonderen_Id==item.Id).ToList();
                if(mesajApartman!=null)
                {
                    foreach (var item2 in mesajApartman)
                    {
                        if (item2.Gonderen_Id != yoneticiId.Id)
                        {
                            list.Add(item2);

                        }
                        

                    }
                }
            }

            return View(list);
        }


        [HttpGet]
        public ActionResult GenelYoneticiyeMesaj()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenelYoneticiyeMesaj(ApartmanModel.MesajIslemleri model)
        {
            ApartmanModel.MesajIslemleri mesaj = new ApartmanModel.MesajIslemleri();

            var yoneticiBilgi = userManager.FindByName(User.Identity.Name);
            var apartmanBilgi = db.Apartman.Where(x => x.Yonetici_Id == yoneticiBilgi.Id).ToList();
            if (apartmanBilgi.Count != 0)
            {
                foreach (var item in apartmanBilgi)
                {
                    mesaj.MesajTuru = 4;
                    mesaj.Mesaj = model.Mesaj;
                    mesaj.ApartmanNo = item.ApartmanNo;
                    mesaj.Gonderen_Id = User.Identity.GetUserId();
                    mesaj.Gonderen_DaireNo = yoneticiBilgi.DaireNo;
                    db.MesajIslemleri.Add(mesaj);
                    db.SaveChanges();
                    TempData["genelYoneticiyeMesajBasarili"] = "Genel yöneticiye mesajınız iletilmiştir.";
                    return RedirectToAction("GenelYoneticiyeMesaj");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult GorevliListesi()
        {
            return View(userManager.Users.Where(x => x.Gorev > 3).ToList());
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
        public ActionResult TemizlikRandevulari()
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            var kullaniciApartman = db.Apartman.FirstOrDefault(x => x.Id == kullanici.Apartman);
            if (kullaniciApartman != null)
            {
                var temizlikRandevulari = db.Gorev.Where(x => x.BakimYeri == 4 && x.ApartmanBlok == kullaniciApartman.ApartmanBlok && x.ApartmanNo == kullaniciApartman.ApartmanNo && x.DaireNo != null && x.Aktiflik == 1 && x.Tamamlandi == 1).ToList();
                return View(temizlikRandevulari);
            }
            return View();

        }

        [HttpGet]
        public ActionResult GorevliCalismaKontrol(string id)
        {
            var kullanici = userManager.FindByName(User.Identity.Name);
            var kullaniciApartman = db.Apartman.FirstOrDefault(x => x.Id == kullanici.Apartman);
            if (kullanici != null)
            {
                if (kullanici.Gorev == 4)
                {
                    var gorevliCalismaKontrol = db.Gorev.Where(x => x.Gorevli_Id == id && x.Aktiflik == 1 && x.Tamamlandi == 1 && x.ApartmanBlok == kullaniciApartman.ApartmanBlok && x.ApartmanNo == kullaniciApartman.ApartmanNo).ToList();
                    foreach (var item in gorevliCalismaKontrol)
                    {
                        if (item.BakimYeri == 4)
                        {
                            ViewBag.Temizlik = "Temizlik";
                        }
                    }
                    return View(gorevliCalismaKontrol);
                }
                else
                {
                    var gorevliCalismaKontrol = db.Gorev.Where(x => x.Gorevli_Id == id && x.Aktiflik == 1 && x.Tamamlandi == 1).ToList();

                    return View(gorevliCalismaKontrol);
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult GorevliMesaj(string id)
        {
            if (id != null)
                TempData["GorevliMesajId"] = id;
            else
                TempData["GorevliMesajId"] = null;

            return View();
        }

        [HttpPost]
        public ActionResult GorevliMesaj(ApartmanModel.MesajIslemleri model)
        {
            if (TempData["GorevliMesajId"] != null)
            {
                string gorevliId = TempData["GorevliMesajId"].ToString();
                if (model.Mesaj != null && gorevliId != null)
                {
                    ApartmanModel.MesajIslemleri sikayet = new ApartmanModel.MesajIslemleri();
                    var yoneticiBilgi = userManager.FindByName(User.Identity.Name);
                    var apartmanBilgi = db.Apartman.FirstOrDefault(x => x.Yonetici_Id == yoneticiBilgi.Id);
                    var gorevliBilgi = userManager.Users.FirstOrDefault(x => x.Id == gorevliId);
                    var gorevliApartmanBilgi = db.Apartman.FirstOrDefault(x => x.Id == gorevliBilgi.Apartman);
                    if (yoneticiBilgi != null && apartmanBilgi != null && gorevliBilgi != null)
                    {
                        sikayet.Mesaj = model.Mesaj;
                        sikayet.Gonderen_Id = yoneticiBilgi.Id;
                        sikayet.Gonderen_DaireNo = yoneticiBilgi.DaireNo;
                        sikayet.DaireNo = yoneticiBilgi.DaireNo;
                        sikayet.ApartmanNo = apartmanBilgi.ApartmanNo;
                        if (gorevliBilgi.Gorev == 4)
                        {
                            sikayet.Cevap = gorevliBilgi.Id;
                            sikayet.MesajTuru = 20;
                        }
                        if (gorevliBilgi.Gorev == 5)
                        {
                            sikayet.Cevap = "havuz";
                            sikayet.MesajTuru = 21;
                        }
                        if (gorevliBilgi.Gorev == 6)
                        {
                            sikayet.Cevap = "sporSalonu";// YONETİCİDNE GÖREVLİYE
                            sikayet.MesajTuru = 22;
                        }
                        sikayet.OkunduDurum = 1;
                        TempData["gorevliyeMesaj"] = "Görevliye mesajınız iletilmiştir.";
                        db.MesajIslemleri.Add(sikayet);
                        db.SaveChanges();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Mesaj boş bırakılamaz");
                }
            }

            return RedirectToAction("GorevliListesi");
        }
    }
}