using Application.Helpers.Authentication;
using Application.Helpers.Responce;
using BookingServices.BookingServices.Account.AccountServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesModel.Context;
using ServicesModel.Models.Geo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.BookingServices.Geo
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class GeoController : ControllerBase
    {
        private readonly ServicesContext _context;
        private IAutentication _auth;
        private readonly IResponce _responce;
        private readonly IAccount _account;
        public GeoController(ServicesContext context, IAutentication auth, IResponce responce,
            IAccount account)
        {
            _context = context;
            _auth = auth;
            _responce = responce;
            _account = account;
            // _manager = manager;
        }
        [HttpPost("near")]
        public async Task<JsonResult> GetCoordinateNear([FromBody] AccountGeo coordinate)
        {
            var coor = (from aa in _context.EmployeeOwners
                        join bb in _context.Accounts on aa.id_owner equals bb.id
                        join ee in _context.Coordinates on aa.id_owner equals ee.account_id
                        join cc in _context.Services on aa.id equals cc.account_id
                        where cc.category == coordinate.category_id
                        select new AccountGeoSend
                        {
                            lat=ee.lat,
                            lng=ee.lon,
                            name=bb.name,
                            price=cc.price
                        }).Distinct().ToList();
            foreach (var a in coor)
            {
                double res = Math.Pow((a.lat - coordinate.lat), 2) + Math.Pow((a.lng - coordinate.lng), 2);
                a.distinct = Math.Sqrt(res);
            }
            return new JsonResult(_responce.Return_Responce(System.Net.HttpStatusCode.OK, coor, null));

        }



    }
}
