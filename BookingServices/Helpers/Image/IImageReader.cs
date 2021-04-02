using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingServices.Helpers.ImageWorker
{
   public interface IImageReader
    {
     
        byte[] DownloadImage(string path);
    }
}
