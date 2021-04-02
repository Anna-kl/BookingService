
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using ServicesModel.Context;
using ServicesModel.Models.Auth;
using BookingServices.Helpers.EmailServices;

namespace Application.Helpers.Authentication
{
    public class AuthenticationRepository : IAutentication
    {
        private ServicesContext db_base;
        public AuthenticationRepository(ServicesContext db)
        {
            db_base = db;
        }
        //public bool Check_Registration(string user)
        //{

        //    var found = db_base.Users.Where(p => p.email == user).FirstOrDefault();
        //    if (found != null)
        //        return true;
        //    // Insert some data



        //    return false;
        //}
        public async Task Insert_Tokens(Token token)
        {

            await db_base.Tokens.AddAsync(token);
            //await db_base.SaveChangesAsync();

        }
        public Token Return_tokens(string access, string refresh)
        {
            Token tokens = db_base.Tokens.Where(r => r.access == access && r.refresh == refresh).FirstOrDefault();

            return tokens;
        }
        //public Token Find_tokens(string user, string password)
        //{

        //    var users = db_base.Users.Where(r => r.email == user && r.psw == password).FirstOrDefault();
        //    if (users == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        Token tokens = db_base.Tokens.Where(r => r.id_user == users.Id).FirstOrDefault();

        //        return tokens;
        //    }

        //}
        //public async Task Write_Users(string user, string password)
        //{
        //    Auth req = new Auth
        //    {
        //        email = user,
        //        psw = password,
        //        registrations = DateTime.UtcNow

        //    };


        //    await db_base.Users.AddAsync(req);
        //    await db_base.SaveChangesAsync();
        //    //await db_base.SaveChangesAsync();
        //    var _User = db_base.Users.Where(r => r.email == user).FirstOrDefault();
        //    Account account = new Account
        //    {
        //        Iduser = _User.Id,
        //        Email = _User.email,
        //        dttmChange = DateTime.UtcNow,
        //        userpic = "iVBORw0KGgoAAAANSUhEUgAAAG4AAABuCAYAAADGWyb7AAAACXBIWXMAAAsTAAALEwEAmpwYAAAuE2lUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDUgNzkuMTYzNDk5LCAyMDE4LzA4LzEzLTE2OjQwOjIyICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczpkYz0iaHR0cDovL3B1cmwub3JnL2RjL2VsZW1lbnRzLzEuMS8iIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bWxuczpzdEV2dD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL3NUeXBlL1Jlc291cmNlRXZlbnQjIiB4bWxuczppbGx1c3RyYXRvcj0iaHR0cDovL25zLmFkb2JlLmNvbS9pbGx1c3RyYXRvci8xLjAvIiB4bWxuczp4bXBUUGc9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC90L3BnLyIgeG1sbnM6c3REaW09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9EaW1lbnNpb25zIyIgeG1sbnM6c3RGbnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9Gb250IyIgeG1sbnM6eG1wRz0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL2cvIiB4bWxuczpwZGY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGRmLzEuMy8iIHhtbG5zOnBkZng9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGRmeC8xLjMvIiB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iIGRjOmZvcm1hdD0iaW1hZ2UvcG5nIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIElsbHVzdHJhdG9yIENDIDIyLjEgKE1hY2ludG9zaCkiIHhtcDpDcmVhdGVEYXRlPSIyMDIwLTAxLTE4VDE0OjE3OjAzKzAzOjAwIiB4bXA6TW9kaWZ5RGF0ZT0iMjAyMC0wMS0xOFQxNDoyNDowOCswMzowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAyMC0wMS0xOFQxNDoyNDowOCswMzowMCIgeG1wTU06UmVuZGl0aW9uQ2xhc3M9InByb29mOnBkZiIgeG1wTU06T3JpZ2luYWxEb2N1bWVudElEPSJ1dWlkOjY1RTYzOTA2ODZDRjExREJBNkUyRDg4N0NFQUNCNDA3IiB4bXBNTTpEb2N1bWVudElEPSJhZG9iZTpkb2NpZDpwaG90b3Nob3A6ZDc2Mjc2ZDgtZTdmNi02MTQxLTgxNGMtMmRjZmExNTFlZGM5IiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOmU2NjA0MWI1LTk3MzctNGVjNC05NDIwLTY2YzY3NzQ4ZDFjNiIgaWxsdXN0cmF0b3I6U3RhcnR1cFByb2ZpbGU9IldlYiIgeG1wVFBnOk5QYWdlcz0iMSIgeG1wVFBnOkhhc1Zpc2libGVUcmFuc3BhcmVuY3k9IkZhbHNlIiB4bXBUUGc6SGFzVmlzaWJsZU92ZXJwcmludD0iRmFsc2UiIHBkZjpQcm9kdWNlcj0iQWRvYmUgUERGIGxpYnJhcnkgMTUuMDAiIHBkZng6Q3JlYXRvclZlcnNpb249IjIxLjAuMCIgcGhvdG9zaG9wOkNvbG9yTW9kZT0iMyIgcGhvdG9zaG9wOklDQ1Byb2ZpbGU9InNSR0IgSUVDNjE5NjYtMi4xIj4gPGRjOnRpdGxlPiA8cmRmOkFsdD4gPHJkZjpsaSB4bWw6bGFuZz0ieC1kZWZhdWx0Ij5XZWI8L3JkZjpsaT4gPC9yZGY6QWx0PiA8L2RjOnRpdGxlPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDoxMDMzYTllYS05ZTM1LTQ4OTgtYjRkOS1lZDcyZmQxMWM5NjQiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6MTAzM2E5ZWEtOWUzNS00ODk4LWI0ZDktZWQ3MmZkMTFjOTY0IiBzdFJlZjpvcmlnaW5hbERvY3VtZW50SUQ9InV1aWQ6NjVFNjM5MDY4NkNGMTFEQkE2RTJEODg3Q0VBQ0I0MDciIHN0UmVmOnJlbmRpdGlvbkNsYXNzPSJwcm9vZjpwZGYiLz4gPHhtcE1NOkhpc3Rvcnk+IDxyZGY6U2VxPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0ic2F2ZWQiIHN0RXZ0Omluc3RhbmNlSUQ9InhtcC5paWQ6MGM3ZTFjZjAtMmY5MS00YmNiLThmZTUtODdjZWFhMWJjNTRlIiBzdEV2dDp3aGVuPSIyMDIwLTAxLTE4VDE0OjAyOjExKzAzOjAwIiBzdEV2dDpzb2Z0d2FyZUFnZW50PSJBZG9iZSBJbGx1c3RyYXRvciBDQyAyMi4xIChNYWNpbnRvc2gpIiBzdEV2dDpjaGFuZ2VkPSIvIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJzYXZlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDpjZjIxZjdmNS1kZWJmLTQ5YWUtYTc0YS00Y2YyNTFhNmIxYTkiIHN0RXZ0OndoZW49IjIwMjAtMDEtMThUMTQ6MTc6MDMrMDM6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIElsbHVzdHJhdG9yIENDIDIyLjEgKE1hY2ludG9zaCkiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBhcHBsaWNhdGlvbi9wb3N0c2NyaXB0IHRvIGltYWdlL2Vwc2YiLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjEwMzNhOWVhLTllMzUtNDg5OC1iNGQ5LWVkNzJmZDExYzk2NCIgc3RFdnQ6d2hlbj0iMjAyMC0wMS0xOFQxNDoxODoxMiswMzowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTkgKE1hY2ludG9zaCkiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249ImNvbnZlcnRlZCIgc3RFdnQ6cGFyYW1ldGVycz0iZnJvbSBhcHBsaWNhdGlvbi9wb3N0c2NyaXB0IHRvIGltYWdlL3BuZyIvPiA8cmRmOmxpIHN0RXZ0OmFjdGlvbj0iZGVyaXZlZCIgc3RFdnQ6cGFyYW1ldGVycz0iY29udmVydGVkIGZyb20gaW1hZ2UvZXBzZiB0byBpbWFnZS9wbmciLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOjNlMGFlYTM3LTRkMjYtNGE0Zi1hOTFiLTUzOTBlYjgxNDAwZiIgc3RFdnQ6d2hlbj0iMjAyMC0wMS0xOFQxNDoxODoxMiswMzowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTkgKE1hY2ludG9zaCkiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPHJkZjpsaSBzdEV2dDphY3Rpb249InNhdmVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmU2NjA0MWI1LTk3MzctNGVjNC05NDIwLTY2YzY3NzQ4ZDFjNiIgc3RFdnQ6d2hlbj0iMjAyMC0wMS0xOFQxNDoyNDowOCswMzowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTkgKE1hY2ludG9zaCkiIHN0RXZ0OmNoYW5nZWQ9Ii8iLz4gPC9yZGY6U2VxPiA8L3htcE1NOkhpc3Rvcnk+IDx4bXBUUGc6TWF4UGFnZVNpemUgc3REaW06dz0iMTM2Ni4wMDAwMDAiIHN0RGltOmg9Ijc2OC4wMDAwMDAiIHN0RGltOnVuaXQ9IlBpeGVscyIvPiA8eG1wVFBnOkZvbnRzPiA8cmRmOkJhZz4gPHJkZjpsaSBzdEZudDpmb250TmFtZT0iTGFuZ2RvbiIgc3RGbnQ6Zm9udEZhbWlseT0iTGFuZ2RvbiIgc3RGbnQ6Zm9udEZhY2U9IlJlZ3VsYXIiIHN0Rm50OmZvbnRUeXBlPSJPcGVuIFR5cGUiIHN0Rm50OnZlcnNpb25TdHJpbmc9IlZlcnNpb24gMS4wMDAiIHN0Rm50OmNvbXBvc2l0ZT0iRmFsc2UiIHN0Rm50OmZvbnRGaWxlTmFtZT0iTGFuZ2Rvbi5vdGYiLz4gPC9yZGY6QmFnPiA8L3htcFRQZzpGb250cz4gPHhtcFRQZzpQbGF0ZU5hbWVzPiA8cmRmOlNlcT4gPHJkZjpsaT5DeWFuPC9yZGY6bGk+IDxyZGY6bGk+TWFnZW50YTwvcmRmOmxpPiA8cmRmOmxpPlllbGxvdzwvcmRmOmxpPiA8cmRmOmxpPkJsYWNrPC9yZGY6bGk+IDwvcmRmOlNlcT4gPC94bXBUUGc6UGxhdGVOYW1lcz4gPHhtcFRQZzpTd2F0Y2hHcm91cHM+IDxyZGY6U2VxPiA8cmRmOmxpPiA8cmRmOkRlc2NyaXB0aW9uIHhtcEc6Z3JvdXBOYW1lPSLQk9GA0YPQv9C/0LAg0L7QsdGA0LDQt9GG0L7QsiDQv9C+INGD0LzQvtC70YfQsNC90LjRjiIgeG1wRzpncm91cFR5cGU9IjAiPiA8eG1wRzpDb2xvcmFudHM+IDxyZGY6U2VxPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0i0JHQtdC70YvQuSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjI1NSIgeG1wRzpncmVlbj0iMjU1IiB4bXBHOmJsdWU9IjI1NSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0i0KfQtdGA0L3Ri9C5IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMCIgeG1wRzpncmVlbj0iMCIgeG1wRzpibHVlPSIwIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSR0Ig0LrRgNCw0YHQvdGL0LkiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIyNTUiIHhtcEc6Z3JlZW49IjAiIHhtcEc6Ymx1ZT0iMCIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUkdCINC20LXQu9GC0YvQuSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjI1NSIgeG1wRzpncmVlbj0iMjU1IiB4bXBHOmJsdWU9IjAiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlJHQiDQt9C10LvQtdC90YvQuSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjAiIHhtcEc6Z3JlZW49IjI1NSIgeG1wRzpibHVlPSIwIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSR0Ig0LPQvtC70YPQsdC+0LkiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIwIiB4bXBHOmdyZWVuPSIyNTUiIHhtcEc6Ymx1ZT0iMjU1Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSR0Ig0YHQuNC90LjQuSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjAiIHhtcEc6Z3JlZW49IjAiIHhtcEc6Ymx1ZT0iMjU1Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSR0Ig0L/Rg9GA0L/Rg9GA0L3Ri9C5IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjU1IiB4bXBHOmdyZWVuPSIwIiB4bXBHOmJsdWU9IjI1NSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0xOTMgRz0zOSBCPTQ1IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMTkzIiB4bXBHOmdyZWVuPSIzOSIgeG1wRzpibHVlPSI0NSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yMzcgRz0yOCBCPTM2IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjM3IiB4bXBHOmdyZWVuPSIyOCIgeG1wRzpibHVlPSIzNiIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yNDEgRz05MCBCPTM2IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjQxIiB4bXBHOmdyZWVuPSI5MCIgeG1wRzpibHVlPSIzNiIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yNDcgRz0xNDcgQj0zMCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjI0NyIgeG1wRzpncmVlbj0iMTQ3IiB4bXBHOmJsdWU9IjMwIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTI1MSBHPTE3NiBCPTU5IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjUxIiB4bXBHOmdyZWVuPSIxNzYiIHhtcEc6Ymx1ZT0iNTkiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MjUyIEc9MjM4IEI9MzMiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIyNTIiIHhtcEc6Z3JlZW49IjIzOCIgeG1wRzpibHVlPSIzMyIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yMTcgRz0yMjQgQj0zMyIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjIxNyIgeG1wRzpncmVlbj0iMjI0IiB4bXBHOmJsdWU9IjMzIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTE0MCBHPTE5OCBCPTYzIiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMTQwIiB4bXBHOmdyZWVuPSIxOTgiIHhtcEc6Ymx1ZT0iNjMiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9NTcgRz0xODEgQj03NCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjU3IiB4bXBHOmdyZWVuPSIxODEiIHhtcEc6Ymx1ZT0iNzQiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MCBHPTE0NiBCPTY5IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMCIgeG1wRzpncmVlbj0iMTQ2IiB4bXBHOmJsdWU9IjY5Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTAgRz0xMDQgQj01NSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjAiIHhtcEc6Z3JlZW49IjEwNCIgeG1wRzpibHVlPSI1NSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0zNCBHPTE4MSBCPTExNSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjM0IiB4bXBHOmdyZWVuPSIxODEiIHhtcEc6Ymx1ZT0iMTE1Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTAgRz0xNjkgQj0xNTciIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIwIiB4bXBHOmdyZWVuPSIxNjkiIHhtcEc6Ymx1ZT0iMTU3Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTQxIEc9MTcxIEI9MjI2IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iNDEiIHhtcEc6Z3JlZW49IjE3MSIgeG1wRzpibHVlPSIyMjYiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MCBHPTExMyBCPTE4OCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjAiIHhtcEc6Z3JlZW49IjExMyIgeG1wRzpibHVlPSIxODgiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9NDYgRz00OSBCPTE0NiIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjQ2IiB4bXBHOmdyZWVuPSI0OSIgeG1wRzpibHVlPSIxNDYiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MjcgRz0yMCBCPTEwMCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjI3IiB4bXBHOmdyZWVuPSIyMCIgeG1wRzpibHVlPSIxMDAiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MTAyIEc9NDUgQj0xNDUiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxMDIiIHhtcEc6Z3JlZW49IjQ1IiB4bXBHOmJsdWU9IjE0NSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0xNDcgRz0zOSBCPTE0MyIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjE0NyIgeG1wRzpncmVlbj0iMzkiIHhtcEc6Ymx1ZT0iMTQzIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTE1OCBHPTAgQj05MyIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjE1OCIgeG1wRzpncmVlbj0iMCIgeG1wRzpibHVlPSI5MyIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yMTIgRz0yMCBCPTkwIiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjEyIiB4bXBHOmdyZWVuPSIyMCIgeG1wRzpibHVlPSI5MCIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yMzcgRz0zMCBCPTEyMSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjIzNyIgeG1wRzpncmVlbj0iMzAiIHhtcEc6Ymx1ZT0iMTIxIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTE5OSBHPTE3OCBCPTE1MyIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjE5OSIgeG1wRzpncmVlbj0iMTc4IiB4bXBHOmJsdWU9IjE1MyIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0xNTMgRz0xMzQgQj0xMTciIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxNTMiIHhtcEc6Z3JlZW49IjEzNCIgeG1wRzpibHVlPSIxMTciLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MTE1IEc9OTkgQj04NyIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjExNSIgeG1wRzpncmVlbj0iOTkiIHhtcEc6Ymx1ZT0iODciLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9ODMgRz03MSBCPTY1IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iODMiIHhtcEc6Z3JlZW49IjcxIiB4bXBHOmJsdWU9IjY1Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTE5OCBHPTE1NiBCPTEwOSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjE5OCIgeG1wRzpncmVlbj0iMTU2IiB4bXBHOmJsdWU9IjEwOSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0xNjYgRz0xMjQgQj04MiIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjE2NiIgeG1wRzpncmVlbj0iMTI0IiB4bXBHOmJsdWU9IjgyIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTE0MCBHPTk4IEI9NTciIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxNDAiIHhtcEc6Z3JlZW49Ijk4IiB4bXBHOmJsdWU9IjU3Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTExNyBHPTc2IEI9MzYiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxMTciIHhtcEc6Z3JlZW49Ijc2IiB4bXBHOmJsdWU9IjM2Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTk2IEc9NTYgQj0xOSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9Ijk2IiB4bXBHOmdyZWVuPSI1NiIgeG1wRzpibHVlPSIxOSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj02NiBHPTMzIEI9MTEiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSI2NiIgeG1wRzpncmVlbj0iMzMiIHhtcEc6Ymx1ZT0iMTEiLz4gPC9yZGY6U2VxPiA8L3htcEc6Q29sb3JhbnRzPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6bGk+IDxyZGY6bGk+IDxyZGY6RGVzY3JpcHRpb24geG1wRzpncm91cE5hbWU9ItCe0YLRgtC10L3QutC4INGB0LXRgNC+0LPQviIgeG1wRzpncm91cFR5cGU9IjEiPiA8eG1wRzpDb2xvcmFudHM+IDxyZGY6U2VxPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0wIEc9MCBCPTAiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIwIiB4bXBHOmdyZWVuPSIwIiB4bXBHOmJsdWU9IjAiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MjYgRz0yNiBCPTI2IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjYiIHhtcEc6Z3JlZW49IjI2IiB4bXBHOmJsdWU9IjI2Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTUxIEc9NTEgQj01MSIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjUxIiB4bXBHOmdyZWVuPSI1MSIgeG1wRzpibHVlPSI1MSIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj03NyBHPTc3IEI9NzciIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSI3NyIgeG1wRzpncmVlbj0iNzciIHhtcEc6Ymx1ZT0iNzciLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MTAyIEc9MTAyIEI9MTAyIiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMTAyIiB4bXBHOmdyZWVuPSIxMDIiIHhtcEc6Ymx1ZT0iMTAyIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTEyOCBHPTEyOCBCPTEyOCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjEyOCIgeG1wRzpncmVlbj0iMTI4IiB4bXBHOmJsdWU9IjEyOCIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0xNTMgRz0xNTMgQj0xNTMiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxNTMiIHhtcEc6Z3JlZW49IjE1MyIgeG1wRzpibHVlPSIxNTMiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MTc5IEc9MTc5IEI9MTc5IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMTc5IiB4bXBHOmdyZWVuPSIxNzkiIHhtcEc6Ymx1ZT0iMTc5Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTIwNCBHPTIwNCBCPTIwNCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjIwNCIgeG1wRzpncmVlbj0iMjA0IiB4bXBHOmJsdWU9IjIwNCIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yMzAgRz0yMzAgQj0yMzAiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIyMzAiIHhtcEc6Z3JlZW49IjIzMCIgeG1wRzpibHVlPSIyMzAiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MjQyIEc9MjQyIEI9MjQyIiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iMjQyIiB4bXBHOmdyZWVuPSIyNDIiIHhtcEc6Ymx1ZT0iMjQyIi8+IDwvcmRmOlNlcT4gPC94bXBHOkNvbG9yYW50cz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOmxpPiA8cmRmOmxpPiA8cmRmOkRlc2NyaXB0aW9uIHhtcEc6Z3JvdXBOYW1lPSLQptCy0LXRgtC+0LLQsNGPINCz0YDRg9C/0L/QsCDQtNC70Y8gV2ViIiB4bXBHOmdyb3VwVHlwZT0iMSI+IDx4bXBHOkNvbG9yYW50cz4gPHJkZjpTZXE+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTYzIEc9MTY5IEI9MjQ1IiB4bXBHOm1vZGU9IlJHQiIgeG1wRzp0eXBlPSJQUk9DRVNTIiB4bXBHOnJlZD0iNjMiIHhtcEc6Z3JlZW49IjE2OSIgeG1wRzpibHVlPSIyNDUiLz4gPHJkZjpsaSB4bXBHOnN3YXRjaE5hbWU9IlI9MTIyIEc9MjAxIEI9NjciIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxMjIiIHhtcEc6Z3JlZW49IjIwMSIgeG1wRzpibHVlPSI2NyIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0yNTUgRz0xNDcgQj0zMCIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjI1NSIgeG1wRzpncmVlbj0iMTQ3IiB4bXBHOmJsdWU9IjMwIi8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTI1NSBHPTI5IEI9MzciIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIyNTUiIHhtcEc6Z3JlZW49IjI5IiB4bXBHOmJsdWU9IjM3Ii8+IDxyZGY6bGkgeG1wRzpzd2F0Y2hOYW1lPSJSPTI1NSBHPTEyMyBCPTE3MiIgeG1wRzptb2RlPSJSR0IiIHhtcEc6dHlwZT0iUFJPQ0VTUyIgeG1wRzpyZWQ9IjI1NSIgeG1wRzpncmVlbj0iMTIzIiB4bXBHOmJsdWU9IjE3MiIvPiA8cmRmOmxpIHhtcEc6c3dhdGNoTmFtZT0iUj0xODkgRz0yMDQgQj0yMTIiIHhtcEc6bW9kZT0iUkdCIiB4bXBHOnR5cGU9IlBST0NFU1MiIHhtcEc6cmVkPSIxODkiIHhtcEc6Z3JlZW49IjIwNCIgeG1wRzpibHVlPSIyMTIi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"
        //    };

        //    await db_base.Accounts.AddAsync(account);


        //}
        //private ClaimsIdentity GetIdentity(string username, string password)
        //{
        //    Auth person = db_base.Users.FirstOrDefault(x => x.email == username && x.psw == password);
        //    if (person != null)
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimsIdentity.DefaultNameClaimType, person.email),
        //            new Claim(ClaimsIdentity.DefaultRoleClaimType, person.psw)
        //        };
        //        ClaimsIdentity claimsIdentity =
        //        new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
        //            ClaimsIdentity.DefaultRoleClaimType);
        //        return claimsIdentity;
        //    }

        //    // если пользователя не найдено
        //    return null;
        //}

        public string BuildToken(long id, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("key to some_big_key_value_here_secret"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),


                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //var token = new JwtSecurityToken("Data",
            //  "Application",
            //  expires: DateTime.UtcNow.AddDays(7),
            //  signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        private string Generate_link()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber).Substring(0, 5);
            }
        }

        public Token Generate_Tokens(int user_id, string roles)
        {
           
            var str_token = BuildToken(user_id, roles);
            Token token = new Token
            {
                access = str_token,
                refresh = GenerateRefreshToken(),
                refresh_generate = DateTime.UtcNow,
                access_generate = DateTime.UtcNow,
                access_expire = DateTime.UtcNow.AddDays(7),
                refresh_expire = DateTime.UtcNow.AddDays(30),
                user_id = user_id
            };


            return token;


        }

        public string GetPrincipalFromExpiredToken(string access)
        {
            var token = db_base.Tokens.Where(x => x.access == access).FirstOrDefault();
            if (token == null)
                return null;
            if (token.access_expire < DateTime.UtcNow)
                return "expired";
            else
                return "valid";


        }
        public async Task<string> Confirm(int user, string email)
        {

            
                Confirm pas = new Confirm
                {
                    user_id = user,
                  
                    code = Get_Hash(user.ToString()+"secret")
                };
                db_base.Confirms.Add(pas);
                await db_base.SaveChangesAsync();
                string message = "Для подтверждения  перейдите по ссылке -" +
                    " http://185.220.35.179/api/auths/activate?link=" + pas.code;
                EmailServices emailsend = new EmailServices();
                var result = emailsend.SendEmailAsync(email, "Подтверждение аккаунта", message);
                return pas.code;

            
        }
        public async Task<bool> ConfirmStaff(string link, string email)
        {

            string message = "Вас пригласили на проект ocpio.com. Для подтверждения  перейдите по ссылке -" +
                " http://185.220.35.179/api/staff/confirm?link=" + link;
            EmailServices emailsend = new EmailServices();
            var result = emailsend.SendEmailAsync(email, "Подтверждение приглашения", message);
            return true;

        }
        public async Task<bool> SendLogin(string email, string password)
        {

            string message =String.Format("Вы подтвердили приглашение, на сайте http://ocpio.com/ используйте логин и пароль для входа " +
                " логин: {0}, пароль: {1} ", email, password);
            EmailServices emailsend = new EmailServices();
            var result = emailsend.SendEmailAsync(email, "Данные для входа", message);
            return true;

        }
        public async Task<bool> ChangePassword (Auth auth)
        {
            try
            {
                string new_password = Get_Hash(auth.password).Substring(0, 5);
                Change_Password change = new Change_Password
                {
                    dttm = DateTime.Now,
                    password = new_password,
                    user_id = auth.id
                };
                await db_base.change_Passwords.AddAsync(change);
                await db_base.SaveChangesAsync();
                string message = "Вы сменили пароль на  - " + new_password;
                EmailServices emailsend = new EmailServices();
                var result = emailsend.SendEmailAsync(auth.email, "Подтверждение аккаунта", message);
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public string Check_Tokens(string access, string refresh)
        {
            var tokens = db_base.Tokens.Where(r => r.access == access && r.refresh == refresh.Replace(' ', '+')).FirstOrDefault();
            if (tokens == null)
            {
                return null;
            }
            else
            {
                if (tokens.access_expire < DateTime.UtcNow)
                    return "expired";
                else
                    return "valid";


            }
        }
        //public async Task<string> CheckEmail(string link)
        //{
        //    var data = db_base.Cheking.Where(x => x.hash == link).FirstOrDefault();
        //    if (data == null)
        //    {
        //        return null;
        //    }
        //    else
        //    {
        //        var user = db_base.Users.Where(x => x.Id == data.id_user).FirstOrDefault();
        //        user.state = "AA";
        //        data.dttm_confirm = DateTime.UtcNow;
        //        data.state = true;
        //        await db_base.SaveChangesAsync();
        //        return "Ok";
        //    }

        //}

        //public async Task<Token> Refresh_Tokens(string refresh)
        //{
        //    try
        //    {
        //        var user = db_base.Tokens.Where(r => r.refresh == refresh.Replace(' ', '+')).FirstOrDefault();
        //        var temp_id = user.Id;
        //        var account = db_base.Users.Where(r => r.Id == user.id_user).FirstOrDefault();
        //        var tokens = Generate_Tokens(account.email, account.psw);
        //        //user = tokens;
        //        user.access = tokens.access;
        //        user.refresh = tokens.refresh;
        //        user.refresh_expire = tokens.refresh_expire;
        //        user.refresh_generate = tokens.refresh_generate;
        //        user.access_expire = tokens.access_expire;
        //        user.access_generate = tokens.access_generate;
        //        db_base.Update(user);
        //        //await db_base.Token.AddAsync(user);
        //        await db_base.SaveChangesAsync();
        //        return tokens;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }

        //}
        //public bool Refresh_access(Token token)
        //{

        //    var tokens = db_base.Tokens.Where(r => r.refresh == token.refresh).FirstOrDefault();
        //    var user = db_base.Users.Where(r => r.Id == token.id_user).FirstOrDefault();
        //    var sha = SHA512.Create();
        //    var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(user.email + DateTime.UtcNow.ToString()));
        //    var access = Convert.ToBase64String(hash).Substring(0, 20);
        //    tokens.access = access;
        //    tokens.access_expire = DateTime.UtcNow;

        //    return true;
        //}


        private string Get_Hash(string password)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {

                string input = password + '&' + DateTime.UtcNow.Millisecond.ToString();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes).Substring(0, 15).Replace('+', ' ');
            }
        }


        //public async Task<string> Order_Change_Password(string user)
        //{

        //    var users = db_base.Users.Where(r => r.email == user).FirstOrDefault();
        //    if (users == null)
        //        return null;
        //    else
        //    {

        //        Password pas = new Password
        //        {
        //            Id_user = users.Id,
        //            dttm_change = DateTime.UtcNow.AddDays(3),
        //            state = 0,
        //            link = Get_Hash(users.psw)
        //        };
        //        db_base.Pass.Add(pas);
        //        await db_base.SaveChangesAsync();
        //        string message = "Для восстановления доступа перейдите по ссылке - http://195.133.146.22/api/auths/change_password/user?link=" + pas.link;
        //        EmailServices email = new EmailServices();
        //        var result = email.SendEmailAsync(users.email, "Восстановление доступа", message);
        //        return pas.link;

        //    }
        //}
        //public async Task<Password> Change_Password(string link)
        //{

        //    var pass = db_base.Pass.Where(r => r.link == link).FirstOrDefault();
        //    if (pass == null)
        //    {
        //        Password reg = new Password
        //        {
        //            Id = -2
        //        };
        //        return reg;
        //    }

        //    else
        //    {
        //        if ((DateTime.UtcNow - pass.dttm_change).TotalDays > 1)
        //        {
        //            Password reg = new Password
        //            {
        //                Id = -1
        //            };
        //            return reg;
        //        }
        //        if (pass.state == 2)
        //        {
        //            Password reg = new Password
        //            {
        //                Id = -3
        //            };
        //            return reg;
        //        }
        //        else
        //        {

        //            pass.state = 1;


        //            await db_base.SaveChangesAsync();
        //            return pass;
        //        }



        //    }
        //}

        //public async Task<Auth> Change_PasswordEnd(string link, string password)
        //{

        //    var pass = db_base.Pass.Where(r => r.link == link && r.state == 1 && r.dttm_change > DateTime.UtcNow).FirstOrDefault();
        //    if (pass == null)
        //    {
        //        Auth reg = new Auth
        //        {
        //            Id = -2
        //        };
        //        return reg;
        //    }

        //    else
        //    {
        //        pass.state = 2;

        //        pass.password = password;

        //        pass.dttm_change = DateTime.UtcNow;
        //        var users = db_base.Users.Where(r => r.Id == pass.Id_user).FirstOrDefault();
        //        users.psw = password;
        //        await db_base.SaveChangesAsync();
        //        return users;




        //    }
        //}
        //public async Task GenerateCheck(string user)
        //{
        //    var id_user = db_base.Users.Where(x => x.email == user).FirstOrDefault();
        //    Check ch = new Check
        //    {
        //        id_user = id_user.Id,
        //        dttm_create = DateTime.UtcNow,
        //        state = false,
        //        hash = Get_Hash(user)
        //    };
        //    await db_base.Cheking.AddAsync(ch);
        //    await db_base.SaveChangesAsync();
        //    string message = "Для подтверждения регистрации перейдите по ссылке - http://195.133.146.22/api/auths/checkemail?link=" + ch.hash;
        //    EmailServices email = new EmailServices();
        //    var result = email.SendEmailAsync(user, "Подтверждение адреса", message);

        //}
    }
}
