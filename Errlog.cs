using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.IO;

namespace EaseRoom
{
    public static class Errlog
    {
        private static string _ErrorLogSolnDir = MappedApplicationPath.ToString() + "\\EaseRoom" + @"ErrLog\";

        static string _ErrorLogDir = ConfigurationManager.AppSettings["ErrorPath"];

        public static string MappedApplicationPath
        {
            get
            {
                string APP_PATH = System.Web.HttpContext.Current.Request.ApplicationPath.ToLower();
                if (APP_PATH == "/")      //a site
                    APP_PATH = "/";
                else if (!APP_PATH.EndsWith(@"/")) //a virtual
                    APP_PATH += @"/";

                string it = System.Web.HttpContext.Current.Server.MapPath(APP_PATH);
                if (!it.EndsWith(@"\"))
                    it += @"\";
                return it;
            }
        }

        private static string _Heading()
        {
            return string.Format("{0}", DateTime.Now.ToString("[dd/MM/yyyy hh:mm:ss:fff]"));
        }

        public static void ErrorLogFile(string Pgname, string SpName, string ExceptionMessage)
        {
            try
            {
                if (!Directory.Exists(string.Format(@"{0}\", _ErrorLogDir)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\", _ErrorLogDir));
                }

                string strFileName = string.Format(@"{0}\{1}.txt", _ErrorLogDir, DateTime.Now.ToString("ddMMMyyyy"));

                StreamWriter sw = new StreamWriter(strFileName, true);
                sw.WriteLine(_Heading());
                if (Pgname.Length > 0)
                    sw.WriteLine("Page Name: " + Pgname);//Environment.NewLine
                if (SpName.Length > 0)
                    sw.WriteLine("Sp Name: " + SpName);
                sw.WriteLine("Message: " + ExceptionMessage);
                sw.WriteLine("------------------------------");
                sw.Flush();
                sw.Close();

                //LocalSolnErrLog(Pgname, SpName, ExceptionMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}