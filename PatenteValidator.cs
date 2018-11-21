using System;
using System.Data;
using System.Collections.Generic;
using  System.Data.Common;
using System.Web;
using System.Globalization;
using System.IO;

namespace csharpMod11CarValidator{

    public class PatenteValidator
    {

      
      public static string filename;

        public class PatenteAntigua
        {
           
           
            public static Dictionary<string, string> ValorLetras
            {
               
                get
                {
                    

                    DataTable dt =
                         //Utils.Common.Files.FileTools.ByteBufferToTable(ValidatorResources.Resources.DiccionarioPatentesAntiguas, true);
                         Utils.ByteBufferToTable(PatenteValidator.FileToByteArray(getAppPathFolder(filename)), true);
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    foreach (DataRow dr in dt.Rows)
                        d[dr[0].ToString()] = dr[1].ToString();
    
                    return d;
                }
            }
        }

      
    
        public class PatenteNueva
        {
            public static Dictionary<string, string> ValorLetras
            {
                get
                {
                    DataTable dt =
                        Utils.ByteBufferToTable(PatenteValidator.FileToByteArray(getAppPathFolder(filename)), true);
                    Dictionary<string, string> d = new Dictionary<string, string>();
                    foreach (DataRow dr in dt.Rows)
                        d[dr[0].ToString()] = dr[1].ToString();
    
                    return d;
                }
            }
        } 
    


        public static byte[] FileToByteArray(string fileName)
            {
                byte[] buff = null;
                FileStream fs = new FileStream(fileName, 
                                            FileMode.Open, 
                                            FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                long numBytes = new FileInfo(fileName).Length;
                buff = br.ReadBytes((int) numBytes);
                return buff;
            }


        public static string getAppPathFolder(string filename){

                string mydocumentsfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                              
                string pathfile =  string.Concat(mydocumentsfolder,"\\",filename);

            return  pathfile;
        }


        public static bool ValidarPatente(string patente)
        {
            if (patente.Trim().Length != 8)
                return false;
            if (!patente.Trim().Contains("-"))
                return false;
    
            int aux;
            bool esPatenteAntigua = int.TryParse(patente.Substring(2,2), out aux);
            int numero;
            string dv;
            if(esPatenteAntigua)
            {
                Dictionary<string, string> valorLetras = PatenteAntigua.ValorLetras;
                string letras = patente.Substring(0, 2);
                if (!valorLetras.ContainsKey(letras.ToUpper()))
                    return false;
                string valor = valorLetras[letras.ToUpper()];
                valor += patente.Substring(2, 4);
                dv = patente.Substring(7, 1);
                numero = int.Parse(valor);
            }
            else
            {
                Dictionary<string, string> valorLetras = PatenteNueva.ValorLetras;
                char[] letras = patente.Substring(0, 4).ToCharArray();
                string valor = string.Empty;
    
                foreach (char c in letras)
                {
                    if (!valorLetras.ContainsKey(c.ToString().ToUpper()))
                        return false;
                    valor += valorLetras[c.ToString().ToUpper()];
                }
    
                valor += patente.Substring(4, 2);
                dv = patente.Substring(7, 1);
                numero = int.Parse(valor);
            }
    
            Mod11Validator mod11Validator = new Mod11Validator(numero, dv);
            return mod11Validator.ObtenerValidezMod11();
        }
    }

}