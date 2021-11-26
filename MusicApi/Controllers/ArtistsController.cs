using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await (from artist in _dbContext.Artists
            select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl,
            }).ToListAsync();

            return Ok(artists);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Details(int artistId)
        {
            var artistDetails = await _dbContext.Artists.Where(artist => artist.Id == artistId).Include(a => a.Songs).ToListAsync();
            return Ok(artistDetails);
        }
     }
}
