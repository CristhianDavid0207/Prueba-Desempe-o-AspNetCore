using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace solucion.Providers
{
    public enum Folders
    {
        Documents = 0, Images = 1, Uploads =2, Temp = 3
    }

    public class PathProvider
    {
        private IWebHostEnvironment hostEnvironment;

        //Declaración en el contructor
        public PathProvider(IWebHostEnvironment hostEnvironment){
            this.hostEnvironment = hostEnvironment;
        }

        //Optencion de ruta Absoluta
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Uploads){
                carpeta = "uploads";
            }
            else if (folder == Folders.Images){
                carpeta = "images";
            }
            else if (folder == Folders.Documents){
                carpeta = "documents";
            }

            string path = Path.Combine(this.hostEnvironment.WebRootPath, carpeta, fileName);

            if (folder == Folders.Temp){
                path = Path.Combine(Path.GetTempPath(), fileName);
            }

            return path;


        }

    }

}