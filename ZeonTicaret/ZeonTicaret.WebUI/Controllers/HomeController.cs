using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeonTicaret.WebUI.App_Classes;
using ZeonTicaret.WebUI.Models;

namespace ZeonTicaret.WebUI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }



        /*
         Sitelayout ta bu şekilde çağırdık
         *   @{
                        
                        Html.RenderAction("Sepet","Home");   //partial view'i çağırıyoruz.Home controllerdaki Sepet actionına git.
                        
                    }
         */
        public PartialViewResult Sepet()
        {

            return PartialView();
        }



        /*  Sitelayout ta bu şekilde çağırdık
           @{
            //partial view 
            Html.RenderAction("Slider","Home");
        }
         
         */
        public PartialViewResult Slider()
        {

            var data = Context.Baglanti.Resims.Where(x => x.BuyukYol.Contains("Slider")).ToList();///Content/SliderResim/5c2f1a50-137e-462c-9136-861e87f46fd2.jpg   //içinde Slider geçen kayıtları çektik


            return PartialView(data);
        }




        /* Sitelayout ta bu şekilde çağırdık
           @{
            
            //partial view
            Html.RenderAction("YeniUrunler", "Home");
        }*/
        public PartialViewResult YeniUrunler()
        {
            var data = Context.Baglanti.Uruns.ToList();
            return PartialView(data);
        }


        //Servisler.cshtml partial view'ini döndürür
        public PartialViewResult Servisler()
        {

            return PartialView();
        }


        //Markalar.cshtml partial view'ini döndürür
        public PartialViewResult Markalar()
        {
            var data = Context.Baglanti.Markas.ToList();//markaları çektik

            return PartialView(data);
        }

        //ajaxla gelinecek
        public void SepeteEkle(int id)
        {
            SepetItem si = new SepetItem();

            Urun u = Context.Baglanti.Uruns.FirstOrDefault(x => x.Id == id);
            si.Urun = u;
            si.Adet = 1;
            si.Indirim = 0;

            Sepet s = new Sepet();
            s.SepeteEkle(si);







        }



        //partial view döndürür
        public PartialViewResult MiniSepetWidget()
        {

            if (HttpContext.Session["AktifSepet"] != null)
            {
                return PartialView((Sepet)HttpContext.Session["AktifSepet"]);//partial view'e modeli yolladık
            }
            else
            {
                return PartialView();
            }

        }


        //ajaxla gelinecek
        public void SepetUrunAdetDusur(int id)
        {
            if (HttpContext.Session["AktifSepet"] != null)
            {
                Sepet s = (Sepet)HttpContext.Session["AktifSepet"];

                if (s.Urunler.FirstOrDefault(x => x.Urun.Id == id).Adet > 1)
                {
                    s.Urunler.FirstOrDefault(x => x.Urun.Id == id).Adet--;  //adeti 1 azalt
                }
                else
                {
                    SepetItem si = s.Urunler.FirstOrDefault(x => x.Urun.Id == id);

                    s.Urunler.Remove(si);  //sepet itemi kaldırdık
                }

            }
        }



        public ActionResult UrunDetay(string id)
        {
        
            Urun u = Context.Baglanti.Uruns.FirstOrDefault(x => x.Adi == id);


            List<UrunOzellik> uos = Context.Baglanti.UrunOzelliks.Where(x=>x.UrunID==u.Id).ToList();

            //İşlemci -->i3,i5 gibi tutabilmek için
            Dictionary<string, List<OzellikDeger>> ozellik = new Dictionary<string, List<OzellikDeger>>();


         
            List<OzellikDeger> degers = new List<OzellikDeger>();

            foreach (UrunOzellik uo in uos)
            {
                OzellikTip ot = Context.Baglanti.OzellikTips.FirstOrDefault(x => x.Id == uo.OzellikTipID);

                bool feriha = false;
                foreach (var item in ozellik)
                {
                    if (item.Key!=ot.Adi)
                    {
                        feriha = true;
                    }
                    else
                    {
                        feriha = false;
                    }
                }

                if (feriha==true)
                {
                    degers = new List<OzellikDeger>();
                }

                foreach (OzellikDeger deger in Context.Baglanti.OzellikDegers.Where(x=>x.OzellikTipID==ot.Id).ToList())
                {
                    OzellikDeger od = Context.Baglanti.OzellikDegers.FirstOrDefault(x=>x.OzellikTipID==ot.Id && x.Id==uo.OzellikDegerID);


                    if (!degers.Any(x=>x.Id==od.Id)) //yoksa ekle
                    {
                        degers.Add(od);
                    }
                }



                if (ozellik.Any(x=>x.Key==ot.Adi))
                {
                    ozellik[ot.Adi] = degers;
                }
                else
                {
                    ozellik.Add(ot.Adi, degers);//Kamera ve değerleri gibi,İşlemci ve değerleri gibi
                }

             
              


            }


         
            ViewBag.Ozellikler = ozellik;

            return View(u);


        }



    }
}