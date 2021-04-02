using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Helpers.Authentication;
using Application.Helpers.Responce;
using BookingServices.Helpers.EmailServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesModel.Context;
using ServicesModel.Models.Auth;
using ServicesModel.Models.Account;
using BookingServices.BookingServices.Account.AccountServices;
using Microsoft.AspNetCore.Identity;
using ServicesModel.Models.Clients;

namespace BookingServices.BookingServices.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly ServicesContext _context;
        private IAutentication _auth;
        private readonly IResponce _responce;
        private readonly IAccount _account;
        //private UserManager<ServicesModel.Models.Auth.Auth> _manager;
        public AuthsController(ServicesContext context, IAutentication auth, IResponce responce,
            IAccount account)
        {
            _context = context;
            _auth = auth;
            _responce = responce;
            _account = account;
           // _manager = manager;
        }
        public async Task<bool> CheckAccount(ServicesModel.Models.Auth.Auth auth)
        {
            try
            {
                var check = await _context.Auths.Where(x => (x.Phone == auth.Phone && x.email == auth.email&&x.role==auth.role)).FirstOrDefaultAsync();
                if (check == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception  e){
                return false;
            }
        }
       public string CheckEmail (string email)
        {
            string pattern = @"\d{11}|\d{10}";
            var match = Regex.Matches(email, pattern);
            if (match.Count>0)
            {
                return "phone";
            }
            try
            {
                MailAddress mail = new MailAddress(email);
                return "email";

            }
            catch (FormatException)
            {
                return "No";
            }
        }

        [HttpPut("password")]
        public async Task<JsonResult> Change_Password([FromBody] Password email)
        {
            var auth = await _context.Auths.Where(x => x.email == email.email).FirstOrDefaultAsync();
            if (auth == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь с таким email Не найден"));

            }
            bool result = await _auth.ChangePassword(auth);
            if (result)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Пароль сгенерирован"));

            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, "Ошибка"));

        
    

        }
        [HttpGet("activate")]
        public async Task<RedirectResult> Activate([FromQuery] string link)
        {
            var temp = await _context.Confirms.Where(x => x.code == link).FirstOrDefaultAsync();
            if (temp != null)
            {
                var user = await _context.Auths.FindAsync(temp.user_id);
                _context.Entry(user).State = EntityState.Modified;
                user.is_confirm = true;
               await _context.SaveChangesAsync();
               return Redirect("http://ocpio.com/confirm.html");
            }
            return Redirect("http://ocpio.com/");
        }
        // POST: api/Auths
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<JsonResult> PostAuth([FromBody] Register auth)
        {
            if (ModelState.IsValid)
            {
                Token token = null;
                SendAuth send = null;
                try
                {
                    var check = CheckEmail(auth.email);
                   
                    ServicesModel.Models.Auth.Auth authtemp = new ServicesModel.Models.Auth.Auth
                    {
                        data_add = DateTime.Now,
                      last_visit = DateTime.Now,
                        is_active = true,
                        role = auth.role,
                        password = auth.password
                    };
                    ServicesModel.Models.Account.Account account = new ServicesModel.Models.Account.Account
                    {
                       
                        update = DateTime.Now
                    };
                    if (check == "phone")
                    {
                        authtemp.UserName = auth.email;
                        authtemp.Phone = auth.email;
                        account.phone = auth.email;
                    }
                    else if (check == "email")
                    {
                        authtemp.email = auth.email;
                        authtemp.UserName= auth.email;
                        account.email = auth.email;
                    }
                    else
                    {
                        return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, "Укажите номер телефона или почты в правильном формате"));

                    }
                   
                    if (! await CheckAccount(authtemp))
                    {
                        authtemp.is_active = true;
                        await _context.Auths.AddAsync(authtemp);
                     //   var result=await _manager.CreateAsync(authtemp, auth.password);
                        //if (result.Succeeded)
                        //{
                        //    var currentUser = await _manager.FindByNameAsync(auth.email);

                        //    var roleresult = _manager.AddToRoleAsync(authtemp, "owner");
                        //}
                        //else
                        //{
                        //    string error = "";
                        //    foreach (var s in result.Errors.ToList())
                        //    {
                        //        error += s.Description + "\n";
                        //    }
                        //    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Conflict, null,
                        //        error));

                        //}
                        await _context.SaveChangesAsync();
                        var id = await _context.Auths.Where(x => x.email == authtemp.email && x.Phone == authtemp.Phone).Select(x => x.id).FirstOrDefaultAsync();
                        token = _auth.Generate_Tokens(id, auth.role);
                        account.id_user = id;
                        await _context.Accounts.AddAsync(account);
                        await _context.Tokens.AddAsync(token);
                        if (check == "email")
                        {
                           await _auth.Confirm(id, authtemp.email);
                        }
                        await _context.SaveChangesAsync();
                        var accountsend = await _context.Accounts.Where(x => x.id_user == id).FirstOrDefaultAsync();
                         send = new SendAuth
                        {
                            accountid = accountsend.id,
                            token = token.access,
                            role=authtemp.role
                        };
                    }
                    else
                    {
                        return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Forbidden, null, "Такой email уже зарегистрирован"));

                    }
                }
                catch (Exception ex)
                {
                  
                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Conflict, null,"Такой email уже зарегистрирован"));
                }
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, send, null));

            }

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, "Неправильные данные"));

        }

        [HttpPost("client")]
        public async Task<JsonResult> PostAuthClient([FromBody] Register auth)
        {
            if (ModelState.IsValid)
            {
                Token token = null;
                SendAuth send = null;
                try
                {
                    var check = CheckEmail(auth.email);

                    ServicesModel.Models.Auth.Auth authtemp = new ServicesModel.Models.Auth.Auth
                    {
                        data_add = DateTime.Now,
                        last_visit = DateTime.Now,
                        is_active = true,
                        role = auth.role,
                        password = auth.password,
                        email = auth.email,
                        EmailConfirmed = false,
                   UserName=auth.email,

                };

                    Client client;
                   

                    if (!await CheckAccount(authtemp))
                    {
                        authtemp.is_active = true;
                       

                        authtemp.last_visit = DateTime.Now;
                       await _context.Auths.AddAsync(authtemp);
                          await  _context.SaveChangesAsync();
                             client = new Client
                            {
                                update_date = DateTime.Now,
                                id_user=authtemp.id,
                                email=auth.email,
                                status="manual"
                            };
                        
                        //await _context.SaveChangesAsync();
                    //    var id = await _context.Auths.Where(x => x.email == authtemp.email && x.Phone == authtemp.Phone).Select(x => x.id).FirstOrDefaultAsync();
                        token = _auth.Generate_Tokens(authtemp.id, auth.role);
                      
                        await _context.Clients.AddAsync(client);
                        await _context.Tokens.AddAsync(token);
                        if (check == "email")
                        {
                            await _auth.Confirm(authtemp.id, authtemp.email);
                        }
                        await _context.SaveChangesAsync();
                        
                        send = new SendAuth
                        {
                            accountid = client.id,
                            token = token.access
                        };
                    }
                    else
                    {
                        return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Forbidden, null, "Такой email уже зарегистрирован"));

                    }
                }
                catch (Exception ex)
                {
                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Conflict, null, "Такой email уже зарегистрирован"));
                }
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, send, null));

            }

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, "Неправильные данные"));

        }

        [HttpPut("token")]
        public async Task<JsonResult> Refresh_Token([FromBody] Token token)
        {
            var check = await _context.Tokens.Where(x => x.refresh_expire > DateTime.UtcNow && x.access == token.access).FirstOrDefaultAsync();
            if (check == null)
            {

                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Gone, null, "Date expired"));

            }

            var role = (from aa in _context.Tokens
                        join bb in _context.Auths on aa.user_id equals bb.id
                        select new
                        {
                            id = bb.id,
                            role = bb.role
                        }).FirstOrDefault();

            var new_token = _auth.Generate_Tokens(role.id, role.role);
            check.refresh = new_token.refresh; check.access = new_token.access; check.refresh_expire = new_token.refresh_expire;
            check.access_expire = new_token.access_expire; check.access_generate = new_token.access_generate; check.refresh_generate = new_token.refresh_generate;
            _context.Entry(check).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, check, null));

        }

        [HttpPost("token")]
        public async Task<JsonResult> Authorize([FromBody] Register token)
        {
            var user = await _context.Auths.Where(x => (x.email == token.email || x.Phone == token.email) && x.password == token.password).FirstOrDefaultAsync();
            if (user == null)
            {
                bool flag_temp = false;
                if (token.email != null)
                {
                    
                    var check_temp = (from aa in _context.change_Passwords
                                      join bb in _context.Auths on aa.user_id equals bb.id
                                      where bb.email == token.email && aa.password == token.password
                                      select aa).FirstOrDefault();
                    
                    if (check_temp != null)
                    {
                        user = await _context.Auths.FindAsync(check_temp.user_id);
                        _context.Entry(user).State = EntityState.Modified;
                        user.password = check_temp.password;
                        flag_temp = true;
                    }
                }
                if (!flag_temp)
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Нет такого пользователя"));

            }
            var old_token = await _context.Tokens.Where(x => x.user_id == user.id).FirstOrDefaultAsync();
            if (old_token != null)
            {
                _context.Tokens.Remove(old_token);
            }
            var new_token = _auth.Generate_Tokens(user.id, user.role);
            await _context.Tokens.AddAsync(new_token);
            _context.Entry(user).State = EntityState.Modified;
            user.last_visit = DateTime.Now;
            await _context.SaveChangesAsync();
            var send = await _account.ReturnAuth(user.id, user.role);
            send.role = user.role;
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send, null));


        }


        [HttpPost("tokenclient")]
        public async Task<JsonResult> AuthorizeClient([FromBody] Register token)
        {
            var user = await _context.Auths.Where(x => (x.email == token.email || x.Phone == token.email) && x.password == token.password && x.role == token.role).FirstOrDefaultAsync();
            if (user == null)
            {
                bool flag_temp = false;
                if (token.email != null)
                {

                    var check_temp = (from aa in _context.change_Passwords
                                      join bb in _context.Auths on aa.user_id equals bb.id
                                      where bb.email == token.email && aa.password == token.password
                                      select aa).FirstOrDefault();

                    if (check_temp != null)
                    {
                        user = await _context.Auths.FindAsync(check_temp.user_id);
                        _context.Entry(user).State = EntityState.Modified;
                        user.password = check_temp.password;
                        flag_temp = true;
                    }
                }
                if (!flag_temp)
                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Нет такого пользователя"));

            }
            var old_token = await _context.Tokens.Where(x => x.user_id == user.id).FirstOrDefaultAsync();
            if (old_token != null)
            {
                _context.Tokens.Remove(old_token);
            }
            var new_token = _auth.Generate_Tokens(user.id, user.role);
            await _context.Tokens.AddAsync(new_token);
            _context.Entry(user).State = EntityState.Modified;
            user.last_visit = DateTime.Now;
            await _context.SaveChangesAsync();
            var send = await _account.ReturnAuth(user.id, user.role);
            send.role = user.role;
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send, null));


        }




    }
}
