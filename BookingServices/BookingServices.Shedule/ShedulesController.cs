using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Application.Helpers.Responce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServicesModel.Context;
using ServicesModel.Models.Clients;
using ServicesModel.Models.Shedule;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookingServices.BookingServices.Shedule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShedulesController : ControllerBase
    {
        private readonly ServicesContext _context;
        private readonly IResponce _responce;



        public ShedulesController(ServicesContext context, IResponce responce)
        {
            _context = context;
            _responce = responce;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<JsonResult> Get([FromQuery] string month, [FromQuery] string year, 
            [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from aa in _context.EmployeeOwners
                        join bb in _context.Auths on aa.id_user equals bb.id
                        join dd in _context.Tokens on aa.id_user equals dd.user_id
                        where dd.access == token
                        select aa).FirstOrDefault();
            if (user == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь не найден"));

            }
            else
            {
                try
                {
                    string s = String.Format("01/{0}/{1}", month, year);
                    var start = DateTime.Parse(s);
                    var end = start.AddMonths(1);
                    var days = await _context.dayOfWorks.Where(x => x.dttmStart > start && x.dttmEnd < end && x.accountId == user.id).ToListAsync();

                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, days, null));
                }
                catch (Exception ex)
                {
                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, ex.Message));

                }
            }
           

        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<JsonResult> Get(int id, [FromHeader] string Authorization)
        {
            var data = await _context.conctereDays.Where(x => x.daysof == id).ToListAsync();
            var day = await _context.dayOfWorks.FindAsync(id);
            var send =await (from aa in _context.conctereDays
                             join ee in _context.dayOfWorks on aa.daysof equals ee.id
                       join bb in _context.Services on aa.services_id equals bb.id
                       join cc in _context.Clients on aa.client_id equals cc.id
                       where ee.id==id && aa.iscanceled==false
                       orderby aa.dttm_start
                       select new SendAllInfo
                       {
                           id=aa.id,
                           start_dttm = aa.dttm_start,
                           end_dttm = aa.dttm_end,
                           name_client = cc.name,
                           services_name = bb.name,
                           price = bb.price,
                           comment_client=aa.comment
                       }). ToListAsync();
            var sendm = new SendDaysWork
            {
                start = day.dttmStart,
                end=day.dttmEnd,
                id_day=day.id,
                send = send
            };
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK,
                sendm, null));

        }


        [HttpGet("shedule")]
        public async Task<JsonResult> GetShedule([FromHeader] string Authorization, [FromQuery] int id)
        {
            var data = (from aa in _context.Services
                        join bb in _context.dayOfWorks on aa.account_id equals bb.accountId
                        where aa.id == id & bb.dttmStart < DateTime.Now.AddDays(30) & bb.dttmEnd>DateTime.Now
                        select bb).ToList();
            var services = await _context.Services.Where(x => x.id == id).FirstOrDefaultAsync();
            List<SheduleSend> send = new List<SheduleSend>();
            foreach (var d in data)
            {
                List<DateTime[]> temp = new List<DateTime[]>();
                var start = d.dttmStart;
                while (d.dttmEnd > start)
                {
                    DateTime[] t = new DateTime[2];
                    t[0] = start; t[1] = start.AddMinutes(services.minutes);
                    temp.Add(t);
                    start = start.AddMinutes(15);
                }
                var concrete = await _context.conctereDays.Where(x => x.daysof == d.id).ToListAsync();
                foreach (var d1 in concrete)
                {
                    temp = temp.Where(x => (x[0] <= d1.dttm_start && x[0] <= d1.dttm_end) &
                      (x[1] <= d1.dttm_start && x[1] <= d1.dttm_end) ||(
                      (x[0] >= d1.dttm_start && x[0] >= d1.dttm_end) &
                      (x[1] >= d1.dttm_start && x[1] >= d1.dttm_end))).ToList();
                }
                var r = temp.Select(x => x[0]).ToList();
                send.Add(new SheduleSend { dttm=r, dayof=d.id});
               
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send,
               null));
        }

        [HttpGet("all"), Authorize]
        public async Task<JsonResult> GetAllInfo([FromHeader] string Authorization, [FromQuery] int id)
        {
            var data = await (from aa in _context.conctereDays
                              join bb in _context.Services on aa.services_id equals bb.id
                              join cc in _context.Clients on aa.client_id equals cc.id
                              where aa.id == id && aa.iscanceled==false
                              select new SendAllInfo
                              {
                                  start_dttm = aa.dttm_start,
                                  end_dttm = aa.dttm_end,
                                  comment_client = aa.comment,
                                  iscanceled = aa.iscanceled,
                                  name_client = cc.name,
                                  client_id = cc.id,
                                  services_name = bb.name,
                                  price = bb.price,
                                  id=aa.id,
                                  comment_service = aa.services_comment
                              }).FirstOrDefaultAsync();
          
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, data,
               null));
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<JsonResult> Post([FromBody] DayOfWork days, [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        join cc in _context.EmployeeOwners on bb.id equals cc.id_user
                        where aa.access == token
                        select cc).FirstOrDefault();
            days.accountId = user.id;
            try
            {
                days.dttmStart = days.dttmStart.AddHours(3);
                days.dttmEnd = days.dttmEnd.AddHours(3);
                await _context.dayOfWorks.AddAsync(days);
                await _context.SaveChangesAsync();
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, days,
                  null));
            }
            catch (Exception ex)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null,
                  ex.Message));
            }

        }

        [HttpPost("add"),Authorize]
        public async Task<JsonResult> AddRecord([FromBody] SendTime days, 
            [FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        join cc in _context.Clients on bb.id equals cc.id_user
                        where aa.access == token
                        select cc).FirstOrDefault();
            var services = await _context.Services.Where(x => x.id == days.services_id).FirstOrDefaultAsync();
            ConctereDay day = new ConctereDay
            {
                daysof = days.dayof,
                dttm_start = days.start,
                dttm_end = days.start.AddMinutes(services.minutes),
                client_id = user.id,
                services_id = days.services_id,
                is_complete = false,
                price = 0,

            };
            await _context.conctereDays.AddAsync(day);
            await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, day,
               null));
        }
        public string CheckEmail(string email)
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

        [HttpGet("cancelled"), Authorize]
        public async Task<JsonResult> GetCancelled([FromQuery] int id,
            [FromHeader] string Authorization)
        {
            var data = await (from aa in _context.conctereDays
                              join bb in _context.Services on aa.services_id equals bb.id
                              join cc in _context.Clients on aa.client_id equals cc.id
                              where aa.daysof == id && aa.iscanceled == true
                              select new SendAllInfo
                              {
                                  start_dttm = aa.dttm_start,
                                  end_dttm = aa.dttm_end,
                                  comment_client = aa.comment,
                                  iscanceled = aa.iscanceled,
                                  name_client = cc.name,
                                  client_id = cc.id,
                                  services_name = bb.name,
                                  price = bb.price,
                                  id = aa.id,
                                  comment_service = aa.services_comment
                              }).ToListAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, data,
              null));
        }

        [HttpPost("addServices"), Authorize]
        public async Task<JsonResult> AddRecordServices([FromBody] SendTime days,
            [FromHeader] string Authorization)
        {
            Client client;
            if (days.client_id == 0)
            {
                ServicesModel.Models.Auth.Auth authtemp = new ServicesModel.Models.Auth.Auth
                {
                    data_add = DateTime.Now,
                    last_visit = DateTime.Now,
                    is_active = true,
                    role = "client",
                    password = "1234"
                };
               client = new Client
                {
                    name = days.client_name,
                    update_date = DateTime.Now,
                    status = "service"
                };
                if (CheckEmail(days.phone) == "email") {
                    authtemp.email = days.phone;
                    client.email = days.phone;
                }
                else
                {
                    authtemp.Phone = days.phone;
                    client.phone = days.phone;
                }
                await _context.Auths.AddAsync(authtemp);
                await _context.SaveChangesAsync();
                client.id_user = authtemp.id;
                await _context.Clients.AddAsync(client);
                await _context.SaveChangesAsync();
            }
            else
            {
                client = await _context.Clients.FindAsync(days.client_id);
            }
            var services = await _context.Services.Where(x => x.id == days.services_id).FirstOrDefaultAsync();
            ConctereDay day = new ConctereDay
            {
                daysof = days.dayof,
                dttm_start = days.start.AddHours(3),
                dttm_end = days.start.AddHours(3).AddMinutes(services.minutes),
                client_id = client.id,
                services_id = days.services_id,
                is_complete = false,
                price = 0,

            };
            await _context.conctereDays.AddAsync(day);
            await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.Created, day,
               null));
        }
        // PUT api/<ValuesController>/5
        [HttpPut("{id}"), Authorize]
        public async Task<JsonResult> AddComment(int id, [FromBody] AComment value)
        {
            var day = await _context.conctereDays.FindAsync(id);
            _context.Entry(day).State = EntityState.Modified;
            day.services_comment = value.comment;
            await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, day,
            null));
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}"), Authorize]
        public async Task<JsonResult> Delete(int id)
        {
            var day = await _context.conctereDays.FindAsync(id);
           
            _context.Entry(day).State = EntityState.Modified;
            day.iscanceled = true;
            await _context.SaveChangesAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, day,
             null));

        }
    }
}
