using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

using System.IO;
using System.Drawing.Imaging;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Drawing;
using ImageMagick;

namespace BookingServices.Helpers.ImageWorker
{
    public interface IImageHandler
    {
         Task<string> Resize(string path);
        Task<string[]> UploadImage(IFormFile file);
        Task<string[]> UploadUserpic(IFormFile file);
        byte[] DownloadImage(string file);
    }
   
    public class ImageHandler : IImageHandler
    {
        private readonly IImageWriter _imageWriter;
        private readonly IImageReader _imageReader;
        const int size = 70;
        const int quality = 15;
        public ImageHandler(IImageWriter imageWriter, IImageReader imageReader)
        {
            _imageWriter = imageWriter;
            _imageReader = imageReader;
        }
        
        public async Task<string[]> UploadImage(IFormFile file)
        {
            string[] result = await _imageWriter.UploadImage(file);
            return result;
        }
        public async Task<string[]> UploadUserpic(IFormFile file)
        {
            string[] result = await _imageWriter.UploadUserpic(file);
            return result;
        }
        public async Task<string> Resize(string fileName)
        {
            
            MagickImage objMagick = new MagickImage();
            objMagick.Read(fileName);
            
            objMagick.Quality = 100;
            objMagick.Resize(new ImageMagick.MagickGeometry("50x50"));
            string path = fileName.Split(".jpg")[0] + "userpic.jpg";
            objMagick.Write(path);
            string data = null;
            MemoryStream stream = new MemoryStream();
            var file =await File.ReadAllBytesAsync(path);
            //using (var  = new FileStream(path, FileMode.Open))
            //{
                //await file.CopyToAsync(bits);
                //var memoryStream = new MemoryStream();
                //file.CopyToAsync(memoryStream);
                ////await bits.CopyToAsync(memoryStream);
                //var bytes = memoryStream.ToArray();
                 data = Convert.ToBase64String(file);

            //}
            File.Delete(path);
            return data;
        }
        public byte[] DownloadImage(string file)
        {
            try
            {
                byte[] result = _imageReader.DownloadImage(file);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
