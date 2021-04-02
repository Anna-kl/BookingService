using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Helpers.Responce;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using ServicesModel.Context;
using ServicesModel.Models.Categories;

namespace BookingServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ServicesContext _context;
        private readonly IResponce _responce;
        public CategoriesController(ServicesContext context, IResponce responce)
        {
            _context = context;
            _responce = responce;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<JsonResult> GetCategories()
        {
            var categories =  await _context.Categories.Where(x=>x.level==0).ToListAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, categories,
                null));

        }
        [HttpGet("services")]
        public async Task<JsonResult> CategoryServices([FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = from bb in _context.Auths
                       join aa in _context.Tokens on bb.id equals aa.user_id
                       join cc in _context.EmployeeOwners on bb.id equals cc.id_user
                       where aa.access == token
                       select new
                       {
                           id = bb.id,
                           account_id = cc.id,
                           owner_id = cc.id_owner
                       };
            var category = (from aa in _context.Accounts
                            join bb in user on aa.id_user equals bb.owner_id
                            join cc in _context.categoryAccounts on aa.id equals cc.id_account
                            select new
                            {
                                level0 = cc.level0,
                                level1 = cc.level1,

                            }).ToList();
            var cat = category.Select(x => x.level1).ToList();
            var categorysend = await _context.Categories.Where(x => x.level == 1 && cat.Contains(x.id)).ToListAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, categorysend,
               null));
        }
        [HttpGet("Subcategory")]
        public async Task<JsonResult> GetSubCategories([FromQuery] int level, [FromQuery] int parent)
        {
            var categories = await _context.Categories.Where(x => x.level == level && x.parent ==parent).ToListAsync();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, categories,
                null));

        }
        [HttpGet("bissness"), Authorize]
        public async Task<JsonResult> GetSubCategoriesBissness([FromHeader] string Authorization)
        {
            string token = Authorization.Split(' ')[1];
            var user = (from bb in _context.Auths
                        join aa in _context.Tokens on bb.id equals aa.user_id
                        join cc in _context.Accounts on bb.id equals cc.id_user
                        where aa.access == token
                        select new
                        {
                            id = bb.id,
                            account_id = cc.id,
                            owner_id = cc.id
                        }).FirstOrDefault();
            var categories = (from aa in _context.Categories
                              join bb in _context.categoryAccounts on aa.id equals bb.level1
                              where bb.id_account == user.account_id
                              select aa).ToList();
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, categories,
                null));

        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return category;
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.id == id);
        }
    }
}
