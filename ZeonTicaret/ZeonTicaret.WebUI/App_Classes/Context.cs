using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using ZeonTicaret.WebUI.Models;//ekledik
namespace ZeonTicaret.WebUI.App_Classes
{
    public class Context
    {

        //SINGLETON PATTERN (TEk context üzerinden  işlem yapmak için )
        private static B403_eTicaretContext baglanti;

        public static B403_eTicaretContext Baglanti
        {
            get
            {
                if (baglanti == null)
                    baglanti = new B403_eTicaretContext();

                return baglanti;
            }
            set { baglanti = value; }
        }

    }
}