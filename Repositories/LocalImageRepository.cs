using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Diagnostics;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NZWalksDbContext dbContext;    

        public LocalImageRepository(IWebHostEnvironment webHostEnvironment,
           IHttpContextAccessor httpContextAccessor, NZWalksDbContext dbContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.dbContext = dbContext;
        }
        public async Task<Image> Upload(Image image)
        {
           var localFilePath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", 
               $"{image.FileName}{image.FileExtension}");
            // Upload the image to local Path
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            // Http : Local Path mapping
            var urlFilePath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.FilePath = urlFilePath;

            // Add image to the Images table
            await dbContext.Images.AddAsync(image); 
            await dbContext.SaveChangesAsync();

            return image;

        }
    }
}
