using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ServiceLayer
{ 
    public class FileUploadService
    {
        private readonly IHostingEnvironment _hostingEnv;

        public FileUploadService(IHostingEnvironment hostingEnv)
        {
            _hostingEnv = hostingEnv;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string name, string location)
        {
            string root = _hostingEnv.WebRootPath;
            string fileExt = Path.GetExtension(file.FileName);
            string fileName = name;
            string dbName = fileName = fileName + " " + DateTime.Now.ToString("yymmssfff") + fileExt;
            string path = Path.Combine(root + location, dbName);

            using (var filestream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(filestream);
            }

            return dbName;
        }
    }
}
