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
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

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
        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<bool> CheckAccount(ServicesModel.Models.Auth.Auth auth)
        {
            try
            {
                var check = await _context.Auths.Where(x => (x.Phone == auth.Phone && x.email == auth.email && x.role == auth.role)).FirstOrDefaultAsync();
                if (check == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e) {
                return false;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string CheckEmail(string email)
        {
            string pattern = @"\d{11}|\d{10}";
            var match = Regex.Matches(email, pattern);
            if (match.Count > 0)
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


        /// <summary>
        /// Проверка номер устройства 
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>Результат проверки</returns>
        /// <response code="200">Устройство уже зарегистрировано</response>
        /// <response code="400">Устройство не найдено</response>
        [Produces("application/json")]
        [HttpGet("{uid}")]
        public async Task<ActionResult> CheckUid(string uid)
        {
            var temp = await _context.Uids.Where(x => x.uid == uid).FirstOrDefaultAsync();
            if (temp != null)
            {
                var old_token = await _context.Tokens.Where(x => x.user_id == temp.id_user).FirstOrDefaultAsync();
                if (old_token != null)
                {
                    _context.Tokens.Remove(old_token);
                }
                var user = await _context.Auths.FindAsync(temp.id_user);
                var token = _auth.Generate_Tokens(old_token.user_id, user.role);
                await _context.Tokens.AddAsync(token);
                await _context.SaveChangesAsync();
                if (user.role == "owner") {
                    var name = await _context.Accounts.Where(x => x.id_user == user.id).FirstOrDefaultAsync();
                    var send = new SendAccountPhone
                    {
                        name = name.name,
                        role = user.role,
                        token = token.access

                    };
                

                return Ok(send);
            } else
                {
                    return null;
                }
            }
            return NotFound();
        }

        /// <summary>
        /// Стереть номер устройства, выйти из аккаунта
        /// </summary>
        /// <param name="Authorization"></param>
        /// <returns>Результат удаления номера</returns>
        /// <response code="200">Устройство уже зарегистрировано</response>
        /// <response code="400">Устройство не найдено</response>
        [Produces("application/json")]
        [HttpGet("out"), Authorize]
        public async Task<ActionResult> OutUser([FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        join cc in _context.Uids on bb.id equals cc.id_user
                        where aa.access == token
                        select cc).FirstOrDefault();
            if (user != null)
            {
                _context.Uids.Remove(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return NotFound();
           
        }


        [HttpGet("code")]
        public async Task<ActionResult> SendCode([FromQuery] string phone)
        {
            var random = new Random();
            string code = random.Next(1001, 9999).ToString();


            string request = String.Format("https://api.ucaller.ru/v1.0/initCall?service_id=424899&key=87P72MxNzTKK2IaHC4J2dtCvckI8dr3C&phone={0}&code={1}", phone, code);
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            HttpClient client = new HttpClient(clientHandler);
            var result =await client.GetAsync(request);

            if (result.StatusCode == System.Net.HttpStatusCode.OK) {

                var codeSend = new SMSCode
                {
                    dttm_add = DateTime.Now,
                    code = code,
                    status = false,
                    phone = phone
                };

                await _context.SMSCodes.AddAsync(codeSend);
                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null, "Код отправлен"));

            }
            else
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null,
                    "Невозможно совершить звонок"));
            }

        }

        [HttpPost("code")]
        public async Task<ActionResult> CheckCode([FromBody] CheckCode phone)
        {
            var code = (from aa in _context.SMSCodes
                        join bb in _context.Auths on aa.phone equals bb.Phone
                        where aa.phone == phone.phone && aa.code == phone.code && aa.dttm_add.AddMinutes(5) > DateTime.Now
                        select aa).FirstOrDefault();
            if (code != null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, null,
                    "Клиент подтвержден"));
            } else
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null,
                    "Такой код не найден"));
            }

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
        
        /// <summary>
        /// Создание пользователя (собственник или сотрудник)
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>Json Object</returns>
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

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="auth"></param>
        /// <returns>JSON Object</returns>
        [Produces("application/json")]
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

        /// <summary>
        /// Авторизация зарегистрированного пользователя
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("token")]
        public async Task<JsonResult> Authorize([FromBody] Register token)
        {
            if (token.email.StartsWith("+7") || token.email.StartsWith("+7"))
            {
                if (token.email.StartsWith("+7") && !token.email.Contains("@"))
                {
                    token.email = token.email.Substring(2).Replace("(","").Replace(")","");
                } else if (token.email.StartsWith("8") || token.email.StartsWith("+7"))
                {
                    token.email = token.email.Substring(1).Replace("(", "").Replace(")", "");
                }
            }
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
           
              var  uid = new UID
                {
                    updateDttm = DateTime.Now,
                    id_user = user.id,
                    uid = token.uid,

                };
                await _context.Uids.AddAsync(uid);
            await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send, null));


        }

        [HttpPost("phone")]
        public async Task<ActionResult> AuthorizeFromPhone([FromBody] SendRegister token)
        {
            var user = await _context.Auths.Where(x => x.role == token.role && x.Phone == token.phone ).FirstOrDefaultAsync();
            if (user != null)
            {
                return BadRequest();
            }
            user = new ServicesModel.Models.Auth.Auth
            {
                data_add = DateTime.Now,
                last_visit = DateTime.Now,
                is_active = true,
                role = token.role,
                password = "1234",
                email = token.email,
                EmailConfirmed = false,
                UserName = token.email,
                Phone = token.phone
            };
            await _context.Auths.AddAsync(user);
            await _context.SaveChangesAsync();
            UID uid = new UID
            {
                uid = token.id,
                id_user = user.id,
                updateDttm = DateTime.Now

            };
            await _context.Uids.AddAsync(uid);
            var new_token = _auth.Generate_Tokens(user.id, user.role);
            await _context.Tokens.AddAsync(new_token);
            user.last_visit = DateTime.Now;
            ServicesModel.Models.Account.Account account = new ServicesModel.Models.Account.Account
            {
                address = token.address,
                name = token.name,
                email = token.email,
                id_user = user.id,
                phone = token.phone,
                site = token.site,
                update = DateTime.Now
            };
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
            foreach(var acc in token.level1)
            {
                var category = new CategoryAccount
                {
                    id_account = account.id,
                    level0 = token.level0,
                    level1 = acc
                };
                await _context.categoryAccounts.AddAsync(category);
            }
            await _context.SaveChangesAsync();
            return Ok(new_token.access);
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
