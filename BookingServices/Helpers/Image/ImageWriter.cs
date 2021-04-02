using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;


namespace BookingServices.Helpers.ImageWorker
{
    public class ImageWriter : IImageWriter
    {
        public async Task<string[]> UploadImage(IFormFile file)
        {
            if (CheckIfImageFile(file))
            {
                return await WriteFile(file);
            }
            string[] responce_error = {"Error", "Invalid image file", null };
            return responce_error;
        }
        public async Task<string[]> UploadUserpic(IFormFile file)
        {
            if (CheckIfImageFile(file))
            {

                return await WriteUserpic(file);
            }
            string[] responce_error = { "Error", "Invalid image file", null };
            return responce_error;
        }

        /// <summary>
        /// Method to check if file is image file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool CheckIfImageFile(IFormFile file)
        {
            byte[] fileBytes;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }

            return WriterHelper.GetImageFormat(fileBytes) != WriterHelper.ImageFormat.unknown;
        }

        /// <summary>
        /// Method to write file onto the disk
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<string[]> WriteFile(IFormFile file)
        {
            string fileName;
            string path;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 
                                                                  //for the file due to security reasons.
                path = Path.Combine("C:\\Images\\", fileName);

                using (var bits = new FileStream(path, FileMode.Create))
                {
                    
                    await file.CopyToAsync(bits);
                }
            }
            catch (Exception e)
            {
                string[] responce_error = {"Error", e.Message, null };
                return responce_error;
            }
            string[] responce = {"OK", fileName, path };
            return responce;
        }
        public async Task<string[]> WriteUserpic(IFormFile file)
        {
            string fileName;
            string path;
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                fileName = Guid.NewGuid().ToString() + extension; //Create a new Name 
                                                                  //for the file due to security reasons.
                path = Path.Combine("C:\\Images\\", fileName);
                using (var bits = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(bits);
                    var memoryStream = new MemoryStream();
                   await file.CopyToAsync(memoryStream);
                    //await bits.CopyToAsync(memoryStream);
                    var bytes = memoryStream.ToArray();
                    string base64 = Convert.ToBase64String(bytes);
                    string[] responce = { "OK", fileName, path, base64 };
                    return responce;
                }

            }
            catch (Exception e)
            {
                string[] responce_error = { "Error", e.Message, null };
                return responce_error;
            }
            
        }
    }
}
