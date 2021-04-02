using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BookingServices.Helpers.ImageWorker
{
   public interface IImageWriter
    {
        Task<string[]> UploadImage(IFormFile file);
        Task<string[]> UploadUserpic(IFormFile file);
    }
}
