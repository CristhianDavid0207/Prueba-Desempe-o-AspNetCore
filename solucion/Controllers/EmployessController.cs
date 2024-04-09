using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using solucion.Data;
using solucion.Models;
using solucion.Helpers;
using solucion.Providers;

namespace solucion.Controllers
{
    public class EmployeesController : Controller
    {
        
        public readonly PruebaContext _context;
        private  readonly HelperUploadFiles helperUpload;

        //LLamamos al contructor que unicializa los modelos
        public EmployeesController(PruebaContext context, HelperUploadFiles helperUpload ){
            _context = context;
            this.helperUpload = helperUpload; 
        }


        //Acción de Listar y buscar
        public IActionResult Index(string search)
        {
            var employee = from employees in _context.Employees select employees;

            if(!string.IsNullOrEmpty(search))
            {
                
                employee = employee.Where(
                    result => result.Id.ToString().Contains(search) || result.Names.Contains(search) || result.LastNames.Contains(search) || result.About.Contains(search)).Take(4);
            }

            return View( employee.ToList());
        }

        //Vista Crear
        public IActionResult Create()
        {
            return View();
        }

        //Accion de Crear
        [HttpPost]
        public async Task<IActionResult> Add(Employee empoyee, IFormFile imagen, int ubicacion, string option)
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

            empoyee.ProfilePicture = nombreImagen;
             
            _context.Employees.Add(empoyee);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        //Vista de Detalles    
        public IActionResult Details(int? id)
        {
            
        {
            return View(_context.Employees.Find(id));
        }
        }

        
        public IActionResult Delete(int? id)
        {   
            var employee = _context.Employees.Find(id);

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _context.Employees.FirstOrDefaultAsync(r => r.Id == id));
        }


        //ACCIÓN DE EDITAR
        [HttpPost]
        public async Task<IActionResult> Update(int id, Employee u){
    
            //Actualiza el Usuario en la base de Datos
            _context.Employees.Update(u);   
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }







    }
}