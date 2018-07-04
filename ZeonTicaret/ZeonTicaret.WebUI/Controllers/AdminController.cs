using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZeonTicaret.WebUI.App_Classes;
using ZeonTicaret.WebUI.Models;

namespace ZeonTicaret.WebUI.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Urunler()
        {

            //Singleton pattern  yapısı oluşturduğumuz için  Context.Baglanti diyerek tabloları çekiyoruz
            return View(Context.Baglanti.Uruns.ToList());//ürünleri çekip gönder  
        }



        public ActionResult UrunEkle()
        {
            //select componenti ile seçtirmek için
            ViewBag.Kategoriler = Context.Baglanti.Kategoris.ToList();
            ViewBag.Markalar = Context.Baglanti.Markas.ToList();


            return View();
        }



        [HttpPost]
        public ActionResult UrunEkle(Urun urun)
        {
            try
            {
                urun.EklenmeTarihi = DateTime.Now;


                Context.Baglanti.Uruns.Add(urun);
                Context.Baglanti.SaveChanges();

            }
            catch (DbEntityValidationException ex) //entity framework hatalarını daha iyi görebilmek için .hangi kolonlar doldurulmalı diye falan söylüyor  //http://www.binaryintellect.net/articles/c1bff938-1789-4501-8161-3f38bc465a8b.aspx
            {
                foreach (DbEntityValidationResult item in ex.EntityValidationErrors)
                {
                    // Get entry

                    DbEntityEntry entry = item.Entry;
                    string entityTypeName = entry.Entity.GetType().Name;

                    // Display or log error messages

                    foreach (DbValidationError subItem in item.ValidationErrors)
                    {
                        string message = string.Format("Error '{0}' occurred in {1} at {2}",
                                 subItem.ErrorMessage, entityTypeName, subItem.PropertyName);
                        Console.WriteLine(message);
                    }

                    return null;
                }
            }

            return RedirectToAction("Urunler");//Urunler actionına git.

        }




        public ActionResult Markalar()
        {


            return View(Context.Baglanti.Markas.ToList());
        }



        public ActionResult MarkaEkle()
        {
            return View();
        }


        //Not:     @* resim için  enctype="multipart/form-data" yazmalıyız view'de*@
        [HttpPost]
        public ActionResult MarkaEkle(Marka mrk, HttpPostedFileBase fileUpload) //resmi yakalayabilmek için HttpPostedFile kullanıyoruz // fileupload değişkeni     <input type="file" class="default" name="fileUpload"/> burdaki fileUpload ile aynı olmalı
        {
            int resimId = -1;

            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);

                int width = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaWidth"].ToString());//webconfigdeki eklediğimiz  değeri çektik
                int height = Convert.ToInt32(ConfigurationManager.AppSettings["MarkaHeight"].ToString());//webconfigdeki  eklediğimiz değeri çektik


                string name = "/Content/MarkaResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);//benzersiz resim ismi ürettik.

                //Resimin boyutlarını değiştiricez ve Content içindeki  MarkaResim klasörüne kaydedicez.
                Bitmap bm = new Bitmap(img, width, height);
                bm.Save(Server.MapPath(name)); // 300X160 boyutunda resmi oluşturup kaydedicek klasöre .boyutlarını webconfigde belirtmiştik zaten

                //resmi veritabanına kaydettik.
                Resim rsm = new Resim();
                rsm.OrtaYol = name;
                Context.Baglanti.Resims.Add(rsm);
                Context.Baglanti.SaveChanges();
                if (rsm.Id != null)
                {
                    resimId = rsm.Id;//SaveChanges tan sonra Eklenen Resmin idsi oluşur vede bizde onu değişkene atadık
                }

                if (resimId != -1)
                    mrk.ResimID = resimId;

                //markayı ekledik
                Context.Baglanti.Markas.Add(mrk);
                Context.Baglanti.SaveChanges();



                /*Not:
 WebConfige ekledik
 * 
 *   <add key="MarkaWidth" value="300"/>
    <add key="MarkaHeight" value="160"/>
 */



            }


            return RedirectToAction("Markalar");//markalar actionına git
        }



        public ActionResult Kategoriler()
        {

            return View(Context.Baglanti.Kategoris.ToList());

        }


        public ActionResult KategoriEkle()
        {
            return View();
        }


        [HttpPost]
        public ActionResult KategoriEkle(Kategori ktg)
        {
            Context.Baglanti.Kategoris.Add(ktg);
            Context.Baglanti.SaveChanges();

            return RedirectToAction("Kategoriler");//Kategoriler actionına git.
        }





        public ActionResult OzellikTipleri()
        {


            return View(Context.Baglanti.OzellikTips.ToList());
        }



        public ActionResult OzellikTipEkle()
        {
            return View(Context.Baglanti.Kategoris.ToList());//Kategorileri göndercez select ile seçtiricez
        }


        [HttpPost]
        public ActionResult OzellikTipEkle(OzellikTip ot)
        {

            Context.Baglanti.OzellikTips.Add(ot);
            Context.Baglanti.SaveChanges();
            return RedirectToAction("OzellikTipleri");
        }


        public ActionResult OzellikDegerleri()
        {

            return View(Context.Baglanti.OzellikDegers.ToList());
        }

        public ActionResult OzellikDegerEkle()
        {
            return View(Context.Baglanti.OzellikTips.ToList());//OzellikTipleri  göndercez select ile seçtiricez
        }

        [HttpPost]
        public ActionResult OzellikDegerEkle(OzellikDeger od)
        {
            Context.Baglanti.OzellikDegers.Add(od);
            Context.Baglanti.SaveChanges();

            return RedirectToAction("OzellikDegerleri");//OzellikDegerleri actionına git
        }


        public ActionResult UrunOzellikleri()
        {

            return View(Context.Baglanti.UrunOzelliks.ToList());
        }

        public ActionResult UrunOzellikSil(int urunId, int tipId, int degerId)//view tarafında ekledik (@Url.Action("UrunOzellikSil", "Admin", new {urunId=@ozellik.UrunID ,tipId=@ozellik.OzellikTipID,degerId=@ozellik.OzellikDegerID})) burdaki parametrelere göre metoda parametre verdik
        {
            UrunOzellik uo = Context.Baglanti.UrunOzelliks.FirstOrDefault(x => x.UrunID == urunId && x.OzellikTipID == tipId && x.OzellikDegerID == degerId);
            Context.Baglanti.UrunOzelliks.Remove(uo);
            Context.Baglanti.SaveChanges();

            return View("UrunOzellikleri");
        }


        public ActionResult UrunOzellikEkle()
        {

            return View(Context.Baglanti.Uruns.ToList());
        }


        //partial view dönen metod
        public PartialViewResult UrunOzellikTipWidget(int? katId) //nullable int(null gelirse hata vermesinde verdik.)
        {
            if (katId != null)
            {
                var data = Context.Baglanti.OzellikTips.Where(x => x.KategoriID == katId).ToList();//kategoriId'ye göre ozelliktipleri çektik
                return PartialView(data);//veriyi yolladık partial view'e
            }
            else
            {
                var data = Context.Baglanti.OzellikTips.ToList();//tüm özelliktipleri çektik(İşlemci,Ram,Harddisk gibi)
                return PartialView(data);//veriyi yolladık partial view'e
            }
        }


        //partial view dönen metod
        public PartialViewResult UrunOzellikDegerWidget(int? tipId)//nullable int(null gelirse hata vermesinde verdik.)
        {
            if (tipId != null)
            {
                var data = Context.Baglanti.OzellikDegers.Where(x => x.OzellikTipID == tipId).ToList();//ozelliktip'e göre ozellikdeğerleri getirdik(İşlemcinin özellikleri gibi(i3,i5,i7))

                return PartialView(data);

            }
            else
            {
                var data = Context.Baglanti.OzellikDegers.ToList();//tüm özellikdeğerleri çektik.
                return PartialView(data);
            }

        }


        //ajaxla buraya gelinecek
        public string UrunOzellikTipListele(object list)
        {

            StringBuilder sb = new StringBuilder();//stringleri birleştirir.
            sb.Append("<div class='form-group'><label class='col-lg-2 control-label'>Özellik:</label> <div class='col-lg-10'><select class='form-control m-bot15' name='Id' id='urun'>");

            List<OzellikTip> ot = (List<OzellikTip>)list;

            foreach (OzellikTip tip in ot)
            {
                sb.Append(" <option value=" + tip.Id + ">" + tip.Adi + "</option>");
            }

            sb.Append("  </select> </div> </div>");


            return sb.ToString();
        }


        [HttpPost]
        public ActionResult UrunOzellikEkle(UrunOzellik uo)
        {

            Context.Baglanti.UrunOzelliks.Add(uo);
            Context.Baglanti.SaveChanges();

            return RedirectToAction("UrunOzellikleri");
        }



        public ActionResult UrunResimEkle(int id)
        {
            return View(id);
        }



        [HttpPost]
        public ActionResult UrunResimEkle(int uId, HttpPostedFileBase fileUpload) //    <input type="file" class="default" name="fileUpload" /> fileUpload kısımları aynı olmalı
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);//resmi okuduk

                //resmi istediğimiz şekilde boyutlandırıyoruz
                Bitmap ortaResim = new Bitmap(img, Settings.UrunOrtaBoyut);//oluşturduğumuz Settings classındaki propertiyi çağırdık
                Bitmap buyukResim = new Bitmap(img, Settings.UrunBuyukBoyut);

                //resmin yollarını belirttik
                string ortayol = "/Content/UrunResim/Orta/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                string buyukyol = "/Content/UrunResim/Buyuk/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);

                //resimleri yukarıdaki yazdığımız yollara kaydeder./Content/UrunResim/.. klasörüne 
                ortaResim.Save(Server.MapPath(ortayol));
                buyukResim.Save(Server.MapPath(buyukyol));


                //resmi veritabanına kaydetcez
                Resim rsm = new Resim();
                rsm.BuyukYol = buyukyol;
                rsm.OrtaYol = ortayol;
                rsm.UrunID = uId;


                if (Context.Baglanti.Resims.FirstOrDefault(x => x.UrunID == uId && x.Varsayilan == false) != null)
                {
                    rsm.Varsayilan = true;
                }
                else
                {
                    rsm.Varsayilan = false;
                }

                Context.Baglanti.Resims.Add(rsm);
                Context.Baglanti.SaveChanges();

                return View(uId);//UrunResimEkle viewini aç

            }

            return View(uId);
        }

        public ActionResult SliderResimleri()
        {


            return View();
        }

        [HttpPost]
        public ActionResult SliderResimEkle(HttpPostedFileBase fileUpload)//    <input type="file" class="default" name="fileUpload" /> fileUpload kısımları aynı olmalı
        {
            if (fileUpload != null)
            {
                Image img = Image.FromStream(fileUpload.InputStream);//resmi okuduk

                //slider resmini webconfigde yazdığımız değerlere  göre  boyutlandırıyoruz
                Bitmap bmp = new Bitmap(img, Settings.SliderResimBoyut);//oluşturduğumuz Settings classındaki propertiyi çağırdık
               

                //resmin  yolunu belirttik.verdiğimiz yola eklicez resmi          //uzantı 
                string yol = "/Content/SliderResim/" + Guid.NewGuid() + Path.GetExtension(fileUpload.FileName);
                

                //resimleri yukarıdaki yazdığımız yollara kaydeder./Content/Slider klasörüne 
                bmp.Save(Server.MapPath(yol));
             


                //resmi veritabanına kaydetcez
                Resim rsm = new Resim();
                rsm.BuyukYol = yol;
            
             
                Context.Baglanti.Resims.Add(rsm);
                Context.Baglanti.SaveChanges();

               

            }

            return RedirectToAction("SliderResimleri");



        }
    }
}


