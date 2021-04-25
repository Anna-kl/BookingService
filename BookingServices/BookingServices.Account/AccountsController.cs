using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Helpers.Responce;
using BookingServices.BookingServices.Account.AccountServices;
using BookingServices.Helpers.ImageWorker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using ServicesModel.Context;
using ServicesModel.Models.Account;
using ServicesModel.Models.Categories;
using ServicesModel.Models.Images;

namespace BookingServices.BookingServices.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IResponce _responce;
        private readonly ServicesContext _context;
        private readonly IAccount _account;
        private readonly IImageHandler _imageHandler;
        private readonly IConfiguration _configuration;

        public AccountsController(ServicesContext context, 
            IAccount account, IResponce responce, IImageHandler imageHandler, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
             _account = account;
            _responce = responce;
            _imageHandler = imageHandler;
        }

        // GET: api/Accounts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        //{
        //    return await _context.Accounts.ToListAsync();
        //}

     //   GET: api/Accounts/5
        [HttpGet("{id}"), Authorize]
        public async Task<JsonResult> GetAccount(int id, string returnUrl = null)
        {
       
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь не найден"));

            }
            var send =await _account.GetData(account);

           return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send, null));

        }
        [HttpGet("userpic2"), Authorize]
        public async Task<FileStreamResult> GetUserpic2([FromHeader] string Authorization, [FromQuery] long id)
        {
            try
            {
                var image = await _context.Userpics.Where(x => x.account_id == id).FirstOrDefaultAsync();
                if (image == null)
                {
                    var image2 = System.IO.File.OpenRead(_configuration["unknow"]);
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



        [HttpGet("userpic"), Authorize]
        public async Task<FileStreamResult> GetUserpic([FromHeader] string Authorization, [FromQuery] long id)
        {
           
            try
            {
                string token = Authorization.Split(' ')[1];
                var user = from bb in _context.Auths
                           join aa in _context.Tokens on bb.id equals aa.user_id
                           join cc in _context.Accounts on bb.id equals cc.id_user
                           where aa.access == token
                           select new
                           {
                               id = cc.id,
                               role = bb.role,
                               account_id = cc.id
                           };

                var usercheck = user.FirstOrDefault();
                if (usercheck != null)
               
                {
                   
                    try
                    {
                        var image = await _context.Userpics.Where(x => x.account_id == usercheck.account_id).FirstOrDefaultAsync();
                        if (image == null)
                        {
                            var image2 = System.IO.File.OpenRead(_configuration["unknow"]);
                            return File(image2, "image/jpeg");
                        }

                        var image1 = System.IO.File.OpenRead(image.path);
                        return File(image1, "image/jpeg");
                    }
                    catch (Exception e)
                    {
                        var image1 = System.IO.File.OpenRead(_configuration["unknow"]);
                        return File(image1, "image/jpeg");
                       
                    }
                }
                else
                {
                    try
                    {
                        var image = await _context.Userpics.Where(x => x.account_id == id).FirstOrDefaultAsync();
                        if (image == null)
                        {
                            var image2 = System.IO.File.OpenRead(_configuration["unknow"]);
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

                //return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, image, "Пользователь не найден"));
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText("C:\\inetpub\\log.txt", ex.Message);
                var image2 = System.IO.File.OpenRead(_configuration["unknow"]);
                return File(image2, "image/jpeg");
            }
        }


        [HttpPost("userpic"), Authorize]
        public async Task<JsonResult> UploadUserpic(IFormFile file, [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = from bb in _context.Auths
                       join aa in _context.Tokens on bb.id equals aa.user_id
                       join cc in _context.Accounts on bb.id equals cc.id_user
                       where aa.access == token
                       select cc;
            var usercheck = user.FirstOrDefault();
            if (usercheck == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь не найден"));

            }
            var responce = await _imageHandler.UploadUserpic(file);
          

            if (responce[0] == "OK")
            {
                var checkuser = await _context.Userpics.Where(x => x.account_id == usercheck.id).FirstOrDefaultAsync();
                if (checkuser == null)
                {
                    Userpic userpic = new Userpic
                    {
                        path = responce[2],
                        name = responce[1],
                        dttmadd = DateTime.UtcNow,
                        account_id = usercheck.id
                    };
                    await _context.Userpics.AddAsync(userpic);
                }
                else
                {
                    System.IO.File.Delete(checkuser.path);
                    _context.Entry(checkuser).State = EntityState.Modified;
                    checkuser.dttmadd = DateTime.UtcNow;
                    checkuser.name = responce[1];
                    checkuser.path = responce[2];

                }
               
                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Изображение сохранено"));
            }
            else
            {
                //var er = _localizer["error_image"];

                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, responce[1], "Изображение не сохранено"));
            }

        }
        [HttpPut("category"), Authorize]
        public async Task<JsonResult> PutAccount([FromBody] List<Category> category, [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = from bb in _context.Auths
                       join aa in _context.Tokens on bb.id equals aa.user_id
                       join cc in _context.Accounts on bb.id equals cc.id_user
                       where aa.access == token
                       select cc;
            var usercheck = user.FirstOrDefault();
            var temp = await _context.categoryAccounts.Where(x => x.id_account == usercheck.id).ToListAsync();
            if (temp.Count == 0)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Аккаунт не найден"));

            }
            foreach (var cat in category)
            {
                var tt = temp.Find(x => x.id == cat.id);
                if (tt!=null)
                {
                    temp.Remove(tt);
                }
                else
                {
                    CategoryAccount categ = new CategoryAccount
                    {

                        id_account = usercheck.id,
                        level0 = cat.parent,
                        level1 = cat.id
                    };
                    await _context.categoryAccounts.AddAsync(categ);
                }
            }
             foreach (var t in temp)
            {
                _context.categoryAccounts.Remove(t);
            }
           await _context.SaveChangesAsync();

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Категории обновлены"));

        }

        // PUT: api/Accounts/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]


        // POST: api/Accounts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost, Authorize]
        public async Task<JsonResult> PostAccount([FromBody] SendAccount account, [FromHeader] string Authorization)
        {
            string jwt = Authorization.Split(' ')[1];
            var user = HttpContext.User.Identity.Name;
            var account_main = await _context.Accounts.Where(x => x.id_user == Convert.ToInt32(user)).FirstOrDefaultAsync();
         
            _context.Entry(account_main).State = EntityState.Modified;
            account_main = _account.Changeacount(account, account_main);
            var category = await _context.categoryAccounts.Where(x => x.id_account == account_main.id).FirstOrDefaultAsync();
            if (category == null)
            {
                 category = new CategoryAccount
                {
                    id_account = account_main.id,
                    level0 = account.level0,
                    level1 = account.level1
                };
                await _context.categoryAccounts.AddAsync(category);
            }
            else
            {
                _context.Entry(category).State = EntityState.Modified;
                category.level0 = account.level0;
                category.level1 = account.level1;
            }
            var http = new HttpClient();
            string url = String.Format
                ("https://geocode-maps.yandex.ru/1.x/?apikey=a2c8035f-05f9-4489-aea1-ad9b2a841572&geocode={0}&format=json", account.address);
            try
            {
                var result = await http.GetStringAsync(url);
                JObject o = JObject.Parse(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
                await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Данные внесены"));

        }

        // DELETE: api/Accounts/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Account>> DeleteAccount(int id)
        //{
        //    var account = await _context.Accounts.FindAsync(id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Accounts.Remove(account);
        //    await _context.SaveChangesAsync();

        //    return account;
        //}

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.id == id);
        }
    }
}
