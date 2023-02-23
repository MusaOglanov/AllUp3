using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AllUp3.Helpers
{
    public static class Extensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        } 
        public static bool IsOlder2MB(this IFormFile file)
        {
            return file.Length / 1024>2048;
        }
        public static async Task<string> SaveImageAsync(this IFormFile file,string folder) 
        {
            string filename = Guid.NewGuid().ToString()+file.FileName;
            string path=Path.Combine(folder,filename);
            using (FileStream fileStream= new FileStream(path,FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return filename;
        }
    }
}
