using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using solucion.Data;
using solucion.Models;
using solucion.Helpers;
using solucion.Providers;

namespace solucion.Controllers
{
    public class JobsController : Controller
    {
        
        public readonly PruebaContext _context;
        private  readonly HelperUploadFiles helperUpload;

        //LLamamos al contructor que unicializa los modelos
        public JobsController(PruebaContext context, HelperUploadFiles helperUpload ){
            _context = context;
            this.helperUpload = helperUpload;
        }


        //Acción de Listar y buscar
        public IActionResult Index(string search)
        {
            var job = from Jobs in _context.Jobs select Jobs;

            if(!string.IsNullOrEmpty(search))
            {
                
                job = job.Where(
                    result => result.Salary.ToString().Contains(search) || result.NameCompany.Contains(search) || result.OfferTitle.Contains(search) || result.Description.Contains(search)).Take(4);
            }

            return View( job.ToList());
        }

        //Vista Crear
        public IActionResult Create()
        {
            return View();
        }

        //Accion de Crear
        [HttpPost]
        public async Task<IActionResult> Add(Job job, IFormFile imagen, int ubicacion, string option)
        {
            //Capturamos el dato
            string nombreImagen = imagen.FileName;


            string path = "";

            switch (ubicacion)
            {
                case 0:
                    path = await this.helperUpload.UploadFilesAsync(imagen, nombreImagen, Folders.Uploads);
                    break;
                case 1:
                    path = await this.helperUpload.UploadFilesAsync(imagen, nombreImagen, Folders.Images);
                    break;
                case 2:
                    path = await this.helperUpload.UploadFilesAsync(imagen, nombreImagen, Folders.Documents);
                    break;
                case 3:
                    path = await this.helperUpload.UploadFilesAsync(imagen, nombreImagen, Folders.Temp);
                    break;
            }

            job.LogoCompany = path;
             
            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //Vista de Detalles    
        public IActionResult Details(int? id)
        {
            
        {
            return View(_context.Jobs.Find(id));
        }
        }

        
        public IActionResult Delete(int? id)
        {   
            var job = _context.Jobs.Find(id);

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.Jobs.FirstOrDefaultAsync(r => r.Id == id));
        }


        //ACCIÓN DE EDITAR
        [HttpPost]
        public async Task<IActionResult> Update(int id, Job u){
    
            //Actualiza el Usuario en la base de Datos
            _context.Jobs.Update(u);   
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }







    }
}