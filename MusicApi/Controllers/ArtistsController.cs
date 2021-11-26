using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicApi.Database;
using MusicApi.Helper;
using MusicApi.Models;
using MusicApi.Models.Api;

namespace MusicApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public ArtistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ArtistsApiModel apiModel)
        {
           try
            {
                var imageUrl = await FileHelper.UploadImage(apiModel.Image);

                var artist = new Artist();
                artist.Name = apiModel.Name;
                artist.Gender = apiModel.Gender;    
                artist.ImageUrl = imageUrl;
                await _dbContext.Artists.AddAsync(artist);
                await _dbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
      
            }
        }
     }
}
