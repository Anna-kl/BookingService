using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers.Authentication;
using Application.Helpers.Responce;
using BookingServices.Helpers.ImageWorker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServicesModel.Context;
using ServicesModel.Models.Images;
using ServicesModel.Models.Staff;

namespace BookingServices.BookingServices.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class staffController : ControllerBase
    {
        private readonly ServicesContext _context;
        private readonly IImageHandler _imageHandler;
        private readonly IResponce _responce;
        private readonly IAutentication _auth;
        private readonly IConfiguration _configuration;
        public staffController(ServicesContext context, IResponce responce, IImageHandler imageHandler,
            IAutentication auth, IConfiguration configuration)
        {
            _context = context;
            _responce = responce;
            _imageHandler = imageHandler;
            _auth = auth;
            _configuration = configuration;
        }

        // GET: api/staff
        [HttpGet, Authorize]
        public async Task<JsonResult> GetEmployeeOwners([FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                       join aa in _context.Tokens on bb.id equals aa.user_id
                       join cc in _context.Accounts on bb.id equals cc.id_user
                       where aa.access == token
                       select cc).FirstOrDefault();

            var staffs = await _context.EmployeeOwners.Where(x => x.id_owner == user.id).ToListAsync();
            List<SendEmployee> send = new List<SendEmployee>();
            foreach (var a in staffs)
            {
                var b = new SendEmployee(a);
                var auth = await _context.Auths.FindAsync(a.id_user);
                b.lastvisit = auth.last_visit;
                b.role = auth.role;
                send.Add(b);
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send,
                  null));

        }

        // GET: api/staff
        [HttpGet("{id}"), Authorize]
        public async Task<JsonResult> GetEmployeeOwner(int id, [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        where aa.access == token
                        select bb).FirstOrDefault();
            var staffs = await _context.EmployeeOwners.FindAsync(id);
            var send = new SendEmployeeOwner(staffs);
            var work = await _context.Accounts.Where(x => x.id_user == staffs.id_owner).FirstOrDefaultAsync();
            send.work = work.name;
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send,
                  null));

        }

        // GET: api/staff/5
        [HttpGet("userpic"), Authorize]
        public async Task<FileStreamResult> GetEmployeeOwner([FromQuery] int id)
        {

            try
            {
                var image = await _context.UserpicsStaff.Where(x => x.account_id == id).FirstOrDefaultAsync();
                if (image == null)
                {
                    var image2 = System.IO.File.OpenRead(_configuration["unknowuser"]);
                    return File(image2, "image/jpeg");
                }

                var image1 = System.IO.File.OpenRead(image.path);
                return File(image1, "image/jpeg");
            }
            catch (Exception e)
            {
               
                return null;
            }

        }

        [HttpGet("userpic2"), Authorize]
        public async Task<FileStreamResult> GetEmployeeOwner2([FromQuery] int id)
        {

            try
            {
                var image = await _context.UserpicsStaff.Where(x => x.account_id == id).FirstOrDefaultAsync();
                if (image == null)
                {
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    return File(ms, "image/jpeg");
                }

                var image1 = System.IO.File.OpenRead(image.path);
                return File(image1, "image/jpeg");
            }
            catch (Exception e)
            {

                return null;
            }

        }

        // PUT: api/staff/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut, Authorize]
        public async Task<IActionResult> PutEmployeeOwner([FromBody]  EmployeeOwner employeeOwner, [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        join cc in _context.EmployeeOwners on bb.id equals cc.id_user
                        where aa.access == token
                        select cc).FirstOrDefault();

            _context.Entry(user).State = EntityState.Modified;
            user.phone = employeeOwner.phone;
            user.lastname = employeeOwner.lastname;
            user.firstname = employeeOwner.firstname;
            user.email = user.email;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null,
                       ex.Message));
            }

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, user,
                        null));
        }

        // POST: api/staff
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost, Authorize]
        public async Task<JsonResult> PostEmployeeOwner([FromBody] EmployeeOwner employeeOwner,
            [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = from bb in _context.Auths
                       join aa in _context.Tokens on bb.id equals aa.user_id
                       join cc in _context.Accounts on bb.id equals cc.id_user
                       where aa.access == token
                       select cc;
            var usercheck = user.FirstOrDefault();
            var check = await _context.Auths.Where(x => (x.email == employeeOwner.email && employeeOwner.email!=null)
            || (x.Phone == employeeOwner.phone && employeeOwner.phone!=null)).FirstOrDefaultAsync();
            
            if (check != null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null,
                    "Такой пользователь уже есть"));

            }
            else
            {
                if (employeeOwner.phone.StartsWith('7'))
                {
                    employeeOwner.phone = employeeOwner.phone.Substring(1).Replace("(", "").Replace(")", "");
                }
                ServicesModel.Models.Auth.Auth auth = new ServicesModel.Models.Auth.Auth
                {
                    email = employeeOwner.email,
                    Phone = employeeOwner.phone,
                    data_add = DateTime.Now,
                    password = "1234",
                    role = "staff",
                    UserName = employeeOwner.email,
                    last_visit = DateTime.Now

                };
                await _context.Auths.AddAsync(auth);
                //var result = await _manager.CreateAsync(auth, auth.password);
                //if (result.Succeeded)
                //{
                //    var currentUser = await _manager.FindByNameAsync(auth.email);

                //    var roleresult = _manager.AddToRoleAsync(auth, auth.role);
                //}
                await _context.SaveChangesAsync();
              //  var id = await _context.Auths.Where(x => x.email == auth.email).FirstOrDefaultAsync();
                //await _context.Auths.AddAsync(auth);
                string link = _auth.GenerateRefreshToken().Substring(0,9);
                employeeOwner.link = link;
                employeeOwner.id_user = auth.id;
                employeeOwner.id_owner = usercheck.id;
                employeeOwner.date_add = DateTime.Now;
                await _context.EmployeeOwners.AddAsync(employeeOwner);
                await _context.SaveChangesAsync();
                 
            //    var account = await _context.EmployeeOwners.Where(x => x.id_user == id.id).FirstOrDefaultAsync();
               await _auth.ConfirmStaff(link, auth.email);
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, employeeOwner,
                      null));
            }

        }

        [HttpGet("confirm")]
        public async Task<RedirectResult> ConfirmStaff([FromQuery] string link)
        {
            var check = await _context.EmployeeOwners.Where(x => x.link == link).FirstOrDefaultAsync();
            if (check == null)
            {
                return Redirect("http://ocpio.com/");
            }
            else
            {
                _context.Entry(check).State = EntityState.Modified;
                check.accepted = true;
                var auth = await _context.Auths.FindAsync(check.id_user);
                _context.Entry(auth).State = EntityState.Modified;
                auth.is_confirm = true;
               await _auth.SendLogin(auth.email, auth.password);
                return Redirect("http://ocpio.com/confirm.html");
            }
        }

        [HttpPost("userpic"), Authorize]
        public async Task<JsonResult> UploadUserpic(IFormFile file, [FromQuery] int id)
        {
          
            var responce = await _imageHandler.UploadUserpic(file);

            if (responce[0] == "OK")
            {

                UserpicStaff userpic = new UserpicStaff
                    {
                        path = responce[2],
                        name = responce[1],
                        dttmadd = DateTime.UtcNow,
                        account_id = id
                    };

                await _context.UserpicsStaff.AddAsync(userpic);

                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Изображение сохранено"));
            }
            else
            {

                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, responce[1], "Изображение не сохранено"));
            }

        }
        // DELETE: api/staff/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeOwner>> DeleteEmployeeOwner(int id)
        {
            var employeeOwner = await _context.EmployeeOwners.FindAsync(id);
            if (employeeOwner == null)
            {
                return NotFound();
            }

            _context.EmployeeOwners.Remove(employeeOwner);
            await _context.SaveChangesAsync();

            return employeeOwner;
        }

        private bool EmployeeOwnerExists(int id)
        {
            return _context.EmployeeOwners.Any(e => e.id == id);
        }
    }
}
