using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ZeonTicaret.WebUI.App_Classes
{
    //urun resim boyutlarını webconfigden çekicez.
    public class Settings
    {
        public static Size UrunOrtaBoyut
        {
            get
            {
                Size sz = new Size();
                sz.Width = Convert.ToInt32(ConfigurationManager.AppSettings["UrunOrtaWidth"]);
                sz.Height = Convert.ToInt32(ConfigurationManager.AppSettings["UrunOrtaHeight"]);
                return sz;
            }
        }


        public static Size UrunBuyukBoyut
        {
            get
            {
                Size sz = new Size();
                sz.Width = Convert.ToInt32(ConfigurationManager.AppSettings["UrunBuyukWidth"]);
                sz.Height = Convert.ToInt32(ConfigurationManager.AppSettings["UrunBuyukHeight"]);
                return sz;
            }
        }



        //slider resimlerinin boyutunu belirtiyoruz.webconfigden değerleri okuyoruz

        public static Size SliderResimBoyut
        {
            get
            {
                Size sz = new Size();
                sz.Width = Convert.ToInt32(ConfigurationManager.AppSettings["SliderWidth"]);
                sz.Height = Convert.ToInt32(ConfigurationManager.AppSettings["SliderHeight"]);
                return sz;
            }
        }



    }
}

/*
 webconfige ekledik
 * 
 *  <add key="UrunOrtaWidth" value="300"/>
    <add key="UrunOrtaHeight" value="360"/>
    <add key="UrunBuyukWidth" value="600"/>
    <add key="UrunBuyukHeight" value="700"/>
 * 
 * 
 *   
    <add key="SliderWidth" value="1920"/>
    <add key="SliderHeight" value="700"/>
 */