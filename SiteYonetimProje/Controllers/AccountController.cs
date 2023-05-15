using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SiteYonetimProje.Identity;
using SiteYonetimProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiteYonetimProje.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public AccountController()
        {
            userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityContext()));


            //şifreleme için kullanılabilecek özellikler
            userManager.PasswordValidator = new CustomPasswordValidator()//custompasswordvalidator sınıfını oluşturduk ve içerisinde özellik tanımladık
            {
                RequiredLength = 7,//minimum 7 karakter
                RequireDigit = true, //sayısal ifade olsun mu
                RequireLowercase = true, // en az 1 tane küçük harf
                RequireUppercase = true, // en az 1 tane büyük harf
                RequireNonLetterOrDigit = true // @#$ gibi işaretlerden en az 1 tane
            };

            userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
            {
                RequireUniqueEmail = true, //aynı isimli email olmaması
                AllowOnlyAlphanumericUserNames = false
            };


        }

        public string seflink(string x)
        {
            x = x.Trim();
            x = x.Replace("ã¢", "a");
            x = x.Replace("ã‚", "a");
            x = x.Replace("ãª", "e");
            x = x.Replace("ãš", "e");
            x = x.Replace("ã§", "c");
            x = x.Replace("ã‡", "c");
            x = x.Replace("äÿ", "g");
            x = x.Replace("ä", "g");
            x = x.Replace("ä°", "i");
            x = x.Replace("ä±", "i");
            x = x.Replace("ã¶", "o");
            x = x.Replace("ã–", "o");
            x = x.Replace("åÿ", "s");
            x = x.Replace("å", "s");
            x = x.Replace("ã¼", "u");
            x = x.Replace("ãœ", "u");
            x = x.Replace("â", "a");
            x = x.Replace("Â", "a");
            x = x.Replace("ê", "e");
            x = x.Replace("Ê", "e");
            x = x.Replace("ç", "c");
            x = x.Replace("Ç", "c");
            x = x.Replace("ğ", "g");
            x = x.Replace("Ğ", "g");
            x = x.Replace("İ", "i");
            x = x.Replace("I", "i");
            x = x.Replace("ı", "i");
            x = x.Replace("î", "i");
            x = x.Replace("Î", "i");
            x = x.Replace("î", "i");
            x = x.Replace("ö", "o");
            x = x.Replace("Ö", "o");
            x = x.Replace("ş", "s");
            x = x.Replace("Ş", "s");
            x = x.Replace("ü", "u");
            x = x.Replace("Ü", "u");
            x = x.Replace(" ", "");
            x = x.ToLower();
            return x;
        }




        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(Models.AccountModel.Giris model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = userManager.Find(model.UserName, model.Sifre);
                if (kullanici != null)
                {
                    if(kullanici.OtoSifre!="Kou123")
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;// kullanıcı girdi çıktılarını yönetmek için
                        var identityclaims = userManager.CreateIdentity(kullanici, "ApplicationCookie"); // kullanıcı için cookie oluşturmak için
                        var authProperties = new AuthenticationProperties();
                        authProperties.IsPersistent = true;//hatırlamak için
                        authManager.SignOut();
                        authManager.SignIn(authProperties, identityclaims);

                        if (kullanici.Gorev == 0 && kullanici.Apartman != 99 && kullanici.Apartman != 70)
                        {
                            return RedirectToAction("KisiselBilgiler", "Account");
                        }
                        else if (kullanici.Gorev == 1)
                        {
                            return RedirectToAction("Index", "GenelYonetici");
                        }
                        else if (kullanici.Gorev == 2)
                        {
                            return RedirectToAction("Index", "Yonetici");
                        }
                        else if (kullanici.Gorev == 3)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if (kullanici.Gorev > 3)
                        {
                            return RedirectToAction("Index", "Gorevli");
                        }
                        else
                        {
                            if (kullanici.Apartman == 99 && kullanici.DaireNo == 99)
                            {
                                return RedirectToAction("GorevliBilgiler", "Account");

                            }
                            else
                            {
                                return RedirectToAction("GorevliBilgileri", "Account");
                            }
                        }
                    }
                    else
                    {
                        var authManager = HttpContext.GetOwinContext().Authentication;
                        authManager.SignOut();
                        TempData["SifreniziYenileyiniz"] = "Önce kendinize özel şifrenizi tanımlayınız";
                        return RedirectToAction("SifreYenileme");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Bilgilerinizi doğru giriniz");
                    return View(model);
                }


            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult KayitOl()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KayitOl(Models.AccountModel.KayitOlustur model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser kullanici = new ApplicationUser();
                kullanici.UserName = model.UserName;
                kullanici.Email = model.Email;
                kullanici.OtoSifre = model.Sifre;


                IdentityResult result = userManager.Create(kullanici, model.Sifre);
                if (result.Succeeded)
                {
                    TempData["kayitBasarili"] = "Kayıt işlemi başarıyla tamamlanmıştır giriş yapabilirsiniz";
                    return RedirectToAction("GirisYap");
                }
                else
                {

                    foreach (var errors in result.Errors)
                    {
                        ModelState.AddModelError("", errors);
                    }
                }
            }

            return View();

        }

        public ActionResult Cikis()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Account");
        }


        [HttpGet]
        public ActionResult KisiselBilgiler()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KisiselBilgiler(Models.AccountModel.KisiselBilgiler model, DateTime dg)
        {
            if (ModelState.IsValid)
            {
                DataContext db = new DataContext();
                string kullaniciIsim = model.Ad + " " + model.Soyad;
                string seflinkKullanici = seflink(kullaniciIsim);
                var kullanici = userManager.FindByName(User.Identity.Name);
                kullanici.Ad = model.Ad;
                kullanici.Soyad = model.Soyad;
                kullanici.DogumTarihi = dg;
                kullanici.PhoneNumber = model.Telefon;
                kullanici.Gorev = 3;
                kullanici.Seflink = seflinkKullanici;
                kullanici.Cinsiyet = model.Cinsiyet;
                var apartmanBilgi = db.Apartman.Where(x => x.ApartmanNo == model.ApartmanNo && x.ApartmanBlok == model.ApartmanBlok).ToList();
                if (apartmanBilgi.Count() != 0)
                {
                    foreach (var item in apartmanBilgi)
                    {
                        kullanici.Apartman = item.Id;
                        var daireBilgi = userManager.Users.Where(x => x.Apartman == item.Id && x.DaireNo == model.DaireNo).ToList();
                        if (daireBilgi.Count() != 0)
                        {
                            ModelState.AddModelError("", "Girdiğiniz daire doludur");
                            return View(model);
                        }
                        else
                        {
                            kullanici.DaireNo = model.DaireNo;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Apartman bilgileri yanlış...");
                    return View(model);
                }
                userManager.Update(kullanici);

                var authManager = HttpContext.GetOwinContext().Authentication;
                authManager.SignOut();
                TempData["guncellemeBasarili"] = "Güncelleme işleminiz başarıyla tamamlanmıştır";

                var apartmanaAitDoluDaireSayisi = userManager.Users.Where(x => x.Apartman == kullanici.Apartman).ToList();
                var doluDaireGuncelleme = db.Apartman.Where(x => x.Id == kullanici.Apartman).ToList();
                if (doluDaireGuncelleme != null)
                {
                    foreach (var item in doluDaireGuncelleme)
                    {
                        item.DoluDaireSayisi = apartmanaAitDoluDaireSayisi.Count();
                    }
                    db.SaveChanges();
                }

                if (kullanici.Gorev == 3)
                {
                    return RedirectToAction("Index", "Home");
                }




            }
            return View();



        }

        [HttpGet]
        public ActionResult GorevliBilgiler()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GorevliBilgiler(Models.AccountModel.KisiselBilgiler model, DateTime dg)
        {
            // SİTEDE YAŞAMAYAN ÇALIŞAN
            DataContext db = new DataContext();
            string kullaniciIsim = model.Ad + " " + model.Soyad;
            string seflinkKullanici = seflink(kullaniciIsim);
            var kullanici = userManager.FindByName(User.Identity.Name);
            kullanici.Ad = model.Ad;
            kullanici.Soyad = model.Soyad;
            kullanici.DogumTarihi = dg;
            kullanici.PhoneNumber = model.Telefon;
            kullanici.Gorev = model.Gorev;
            kullanici.Seflink = seflinkKullanici;
            kullanici.Cinsiyet = model.Cinsiyet;
            userManager.Update(kullanici);

            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            TempData["guncellemeBasarili"] = "Güncelleme işleminiz başarıyla tamamlanmıştır";
            return RedirectToAction("Index", "Account");
        }

        [HttpGet]

        public ActionResult GorevliBilgileri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GorevliBilgileri(Models.AccountModel.KisiselBilgiler model, DateTime dg)
        {

            //SİTEDE YAŞIYAN VE ÇALIŞAN
            if(ModelState.IsValid)
            {
                DataContext db = new DataContext();
                string kullaniciIsim = model.Ad + " " + model.Soyad;
                string seflinkKullanici = seflink(kullaniciIsim);
                var kullanici = userManager.FindByName(User.Identity.Name);
                kullanici.Ad = model.Ad;
                kullanici.Soyad = model.Soyad;
                kullanici.DogumTarihi = dg;
                kullanici.PhoneNumber = model.Telefon;
                kullanici.Gorev = model.Gorev;
                kullanici.Seflink = seflinkKullanici;
                kullanici.Cinsiyet = model.Cinsiyet;
                var apartmanBilgi = db.Apartman.Where(x => x.ApartmanNo == model.ApartmanNo && x.ApartmanBlok == model.ApartmanBlok).ToList();
                if (apartmanBilgi.Count() != 0)
                {
                    foreach (var item in apartmanBilgi)
                    {
                        kullanici.Apartman = item.Id;
                        var daireBilgi = userManager.Users.Where(x => x.Apartman == item.Id && x.DaireNo == model.DaireNo).ToList();
                        if (daireBilgi.Count() != 0)
                        {
                            ModelState.AddModelError("", "Girdiğiniz daire doludur");
                            return View(model);
                        }
                        else
                        {
                            kullanici.DaireNo = model.DaireNo;
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Apartman bilgileri yanlış...");
                    return View(model);
                }
                userManager.Update(kullanici);

                var authManager = HttpContext.GetOwinContext().Authentication;
                authManager.SignOut();
                TempData["guncellemeBasarili"] = "Güncelleme işleminiz başarıyla tamamlanmıştır";

                var apartmanaAitDoluDaireSayisi = userManager.Users.Where(x => x.Apartman == kullanici.Apartman).ToList();
                var doluDaireGuncelleme = db.Apartman.Where(x => x.Id == kullanici.Apartman).ToList();
                if (doluDaireGuncelleme != null)
                {
                    foreach (var item in doluDaireGuncelleme)
                    {
                        item.DoluDaireSayisi = apartmanaAitDoluDaireSayisi.Count();
                    }
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Gorevli");
        }

        [HttpGet]
        public ActionResult SifreYenileme()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return View();
        }

        [HttpPost]
        public ActionResult SifreYenileme(AccountModel.KayitOlustur model)
        {
            var kullanici = userManager.FindByName(model.UserName);
            if (kullanici != null)
            {
                if(model.Sifre!="Kou12345")
                {
                    kullanici.OtoSifre = model.Sifre;
                    kullanici.PasswordHash = userManager.PasswordHasher.HashPassword(model.Sifre);
                    userManager.Update(kullanici);
                    TempData["SifreYenileme"] = "Şifre yenileme işlemi başarılı";
                    return RedirectToAction("GirisYap");
                }
                else
                {
                    ModelState.AddModelError("", "Başlangıçta atanan şifreyle aynı olamaz. Farklı şifre deneyiniz");
                    return View(model);
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return View(model);
            }
        }
    }
}