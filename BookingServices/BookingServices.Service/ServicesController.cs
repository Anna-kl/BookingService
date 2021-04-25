using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers.Responce;
using BookingServices.BookingServices.Service.Helpers;
using BookingServices.Helpers.ImageWorker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServicesModel.Context;
using ServicesModel.Models;
using ServicesModel.Models.Images;
using ServicesModel.Models.Services;

namespace BookingServices.BookingServices.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly ServicesContext _context;
        private readonly IResponce _responce;
        private readonly IConfiguration _configuration;
        private readonly IImageHandler _imageHandler;
        public ServicesController(ServicesContext context, IResponce responce,
            IConfiguration configuration, IImageHandler imageHandler)
        {
            _context = context;
            _responce = responce;
            _configuration = configuration;
            _imageHandler = imageHandler;
        }

        //GET: api/Services
       [HttpGet, Authorize]
        public async Task<JsonResult> GetServices([FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        join cc in _context.EmployeeOwners on bb.id equals cc.id_user
                        where aa.access == token
                        select new
                        {
                            id = cc.id,
                            role = bb.role,
                            account_id = cc.id
                        }).FirstOrDefault();
            if (user == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь не найден"));

            }
            var services = (from aa in _context.Services
                            join bb in _context.Categories on aa.category equals bb.id
                            where aa.account_id == user.account_id
                            select new SendServices
                            {
                                descride = aa.descride,
                                subcategory = bb.name,
                                minutes = aa.minutes,
                                name = aa.name,
                                price = aa.price,
                                parent = bb.parent,
                                id = aa.id
                            }).ToList();
            foreach (var a in services)
            {
                a.category = _context.Categories.Find(a.parent).name;
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, services, null));

        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<JsonResult> GetService(int id)
        {
            var service = await _context.Services.Where(x => x.account_id == id).ToListAsync();

            if (service == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, service, null));
            }

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, service, null));
        }

        [HttpGet("services")]
        public async Task<JsonResult> GetService([FromQuery] int account, [FromQuery] int category)
        {
            var service = (from aa in _context.EmployeeOwners
                           join bb in _context.Services on aa.id equals bb.account_id
                           where aa.id == account & bb.category == category
                           select bb).ToList();

            

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, service, null));
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<JsonResult> PutService(int id, [FromBody] SendServices service)
        {
            var ser = await _context.Services.FindAsync(id);

            _context.Entry(ser).State = EntityState.Modified;
            ser.name = service.name;
            ser.minutes = service.minutes;
            ser.price = service.price;
            ser.descride = service.descride;
            try
            {
                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, service, null));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, service, null));
                }
                else
                {
                    throw;
                }
            }

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NoContent, service, null));
        }

        // POST: api/Services
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<JsonResult> PostService([FromBody] SendServices service, [FromHeader] string Authorization)
        {
            try
            {
                string token = Authorization.Split(' ')[1];
                var user = (from bb in _context.Auths
                            join aa in _context.Tokens on bb.id equals aa.user_id
                            join cc in _context.EmployeeOwners on bb.id equals cc.id_user
                            where aa.access == token
                            select cc).FirstOrDefault();
                var categ = await _context.Categories.Where(x => x.name == service.category).FirstOrDefaultAsync();
                ServiceHelpers help = new ServiceHelpers();
                StaffService serv = help.LoadService(service, categ.id);
                
                
                serv.account_id = user.id;
                await _context.Services.AddAsync(serv);
                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, serv, null));
            }
            catch (Exception ex)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null,
                    ex.Message));
            }
        }

        [HttpPost("photo"), Authorize]
        public async Task<JsonResult> UploadPhotoServices(IFormFile file, [FromQuery] int id, [FromHeader] string Authorization)
        {
            
            var responce = await _imageHandler.UploadUserpic(file);
            if (responce[0] == "OK")
            {
                PhotoServices userpic = new PhotoServices
                {
                    path = responce[2],
                    name = responce[1],
                    dttmadd = DateTime.UtcNow,
                    id = id,
                    id_service=id
                };
                await _context.photoServices.AddAsync(userpic);


                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Изображение сохранено"));
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, responce[0]));


        }

        [HttpGet("photo"), Authorize]
        public async Task<ActionResult> GetPhotoServices([FromQuery] int id, [FromHeader] string Authorization)
        {

            var responce = await _context.photoServices.Where(x => x.id_service == id).ToListAsync();
          
            List<FileContentResult> send = new List<FileContentResult>();
            foreach (var temp in responce)
            {
                var image = _imageHandler.DownloadImage(temp.path);
                if (image != null)
                {

                    send.Add(File(image, "application/octet-stream", temp.name));
                    
                }


                //return await _imageHandler.UploadImage(file);
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK,send, null));

        }


        [HttpGet("userpic"), Authorize]
        public async Task<FileStreamResult> GetUserpic2([FromHeader] string Authorization, [FromQuery] long id)
        {
            try
            {
                var image = await _context.fotoServices.Where(x => x.id_services == id).FirstOrDefaultAsync();
                if (image == null)
                {
                    var image2 = System.IO.File.OpenRead(_configuration["unknowservices"]);
                    return File(image2, "image/jpeg");
                }

                var image1 = System.IO.File.OpenRead(image.path);
                return File(image1, "image/jpeg");
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("C:\\inetpub\\log.txt", ex.Message);
                var image2 = System.IO.File.OpenRead(_configuration["unknow"]);
                return File(image2, "image/jpeg");
            }
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<JsonResult> DeleteService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, 
                    "Сервис не найден"));

            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null,
                   "Сервис удален"));
        }

        private bool ServiceExists(int id)
        {
            return _context.Services.Any(e => e.id == id);
        }
    }
}
