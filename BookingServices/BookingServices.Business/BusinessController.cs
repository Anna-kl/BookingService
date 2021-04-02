using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers.Responce;
using BookingServices.Helpers.ImageWorker;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServicesModel.Context;
using ServicesModel.Models.Business;
using ServicesModel.Models.Categories;
using ServicesModel.Models.Clients;
using ServicesModel.Models.Shedule;

namespace BookingServices.BookingServices.Business
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly ServicesContext _context;
        private readonly IResponce _responce;
        private readonly IImageHandler _imageHandler;
        private readonly IConfiguration _configuration;
        public BusinessController(ServicesContext context, IResponce responce, IImageHandler imageHandler,
             IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _responce = responce;
            _imageHandler = imageHandler;
        }

        // GET: api/Business
        [HttpGet("client")]
        public async Task<JsonResult> GetClients([FromQuery] Category category)
        {

            var r = (from aa in _context.EmployeeOwners
                     join bb in _context.Services on aa.id equals bb.account_id
                     join cc in _context.Accounts on aa.id_owner equals cc.id
                     join ee in _context.categoryAccounts on aa.id_owner equals ee.id_account
                     where bb.category == category.id
                     select new Company
                     {
                         desc = cc.address,
                         category = ee.level1,
                         name = cc.name,

                     }).ToList();
            foreach (var a in r)
            {
                var avatar = await _context.Userpics.Where(x => x.account_id == a.account_id).FirstOrDefaultAsync();
                if (avatar != null)
                {


                    using (Image image = Image.FromFile(avatar.path))
                    {
                        using (MemoryStream m = new MemoryStream())
                        {
                            image.Save(m, image.RawFormat);
                            byte[] imageBytes = m.ToArray();

                            // Convert byte[] to Base64 String
                            a.avatar = Convert.ToBase64String(imageBytes);

                        }
                    }
                }
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, r, null));

        }

        // GET: api/Business/5
        [HttpGet("{id}"), Authorize]
        public async Task<JsonResult> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь не найден"));

            }

            var orders = (from aa in _context.conctereDays
                          join bb in _context.Clients on aa.client_id equals bb.id
                          where aa.dttm_start > DateTime.Now
                          select aa).ToList();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, orders, null));


        }

        [HttpGet("find"), Authorize]
        public async Task<JsonResult> GetClientForPhone([FromQuery] string phone)
        {
            try
            {
                var client = (from aa in _context.Auths
                              join bb in _context.Clients on aa.id equals bb.id_user
                              where (aa.Phone == phone || aa.email == phone) & (aa.role == "client")
                              select bb).FirstOrDefault();

                if (client == null)
                {
                    return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, "Пользователь не найден"));

                }


                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, client, null));
            }
            catch(Exception e)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.BadRequest, null, e.Message));

            }

        }


        [HttpGet("full"), Authorize]
        public async Task<JsonResult> GetClientFull([FromHeader] string Authorization)
        {
            var user = (from aa in _context.Clients
                        join bb in _context.Tokens on aa.id_user equals bb.user_id
                        join cc in _context.conctereDays on aa.id equals cc.client_id
                        select cc).ToList();

            if (user.Count == 0)
            {
                return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.NotFound, null, null));
            }
            var client = await _context.Clients.FindAsync(user[0].client_id);
            List<SendAllInfo> send = new List<SendAllInfo>();
            foreach (var a in user)
            {
                var serv = await _context.Services.FindAsync(a.services_id);
                var temp = new SendAllInfo
                {
                    services_name = serv.name,
                    end_dttm = a.dttm_end,
                    start_dttm = a.dttm_start,
                    comment_service = a.services_comment,
                    iscanceled = a.iscanceled,
                    comment_client = a.comment

                };
                send.Add(temp);
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, send, null));
        }

        [HttpGet("organization"), Authorize]
        public async Task<JsonResult> GetOrganization([FromQuery] int id)
        {
            var client = await _context.Services.Where(x => x.category == id).ToListAsync();

            var res = (from aa in _context.EmployeeOwners
                       join bb in _context.Services on aa.id equals bb.account_id
                       join cc in _context.Accounts on aa.id_owner equals cc.id
                       where bb.category == id
                       select cc).Distinct().ToList();


            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, res, null));


        }


        [HttpGet("services"), Authorize]
        public async Task<JsonResult> GetServices([FromQuery] int id)
        {
            var client = await _context.Services.Where(x => x.category == id).ToListAsync();

            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, client, null));


        }
        [HttpGet("specialist"), Authorize]
        public async Task<JsonResult> GetSpecialist([FromQuery] int category, [FromQuery] int account)
        {
            var res = (from aa in _context.EmployeeOwners
                       join bb in _context.Accounts on aa.id_owner equals bb.id
                       join cc in _context.Services on aa.id equals cc.account_id
                       where bb.id == account && cc.category == category
                       select aa).Distinct().ToList();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, res, null));


        }

        // PUT: api/Business/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Business
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.id }, client);
        }

        // DELETE: api/Business/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.id == id);
        }
    }
}
