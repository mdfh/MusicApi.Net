using System.Linq;
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
    public class SongsController : ControllerBase
    {
        private ApiDbContext _dbContext;

        public SongsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] SongsApiModel apiModel)
        {
            var imageUrl = await FileHelper.UploadImage(apiModel.Image);
            var audioUrl = await FileHelper.UploadImage(apiModel.AudioFile);

            var song = new Song();
            song.Title = apiModel.Title;
            song.Duration = apiModel.Duration;
            song.IsFeatured = apiModel.IsFeatured;
            song.UploadedDate = DateTime.Now;
            song.ArtistId = apiModel.ArtistId;
            song.AlbumId = apiModel.AlbumId;  
            song.ImageUrl = imageUrl;
            song.AudioUrl = audioUrl;

            await _dbContext.Songs.AddAsync(song);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSongs(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;
            var allSongs = await (from songs in _dbContext.Songs
                                select new
                                {
                                    Id = songs.Id,
                                    Title = songs.Title,
                                    Duration = songs.Duration,
                                    ImageUrl = songs.ImageUrl,
                                    AudioUrl = songs.AudioUrl
                                }).ToListAsync();

                return Ok(allSongs.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> FeaturedSongs()
        {
            var allSongs = await (from songs in _dbContext.Songs
                                  where songs.IsFeatured
                                  select new
                                  {
                                      Id = songs.Id,
                                      Title = songs.Title,
                                      Duration = songs.Duration,
                                      ImageUrl = songs.ImageUrl,
                                      AudioUrl = songs.AudioUrl
                                  }).ToListAsync();

            return Ok(allSongs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> NewSongs()
        {
            var allSongs = await (from songs in _dbContext.Songs
                                  orderby songs.UploadedDate descending
                                  select new
                                  {
                                      Id = songs.Id,
                                      Title = songs.Title,
                                      Duration = songs.Duration,
                                      ImageUrl = songs.ImageUrl,
                                      AudioUrl = songs.AudioUrl
                                  }).Take(15).ToListAsync();

            return Ok(allSongs);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Search(String query)
        {
            var allSongs = await (from songs in _dbContext.Songs
                                  where songs.Title.StartsWith(query)
                                  select new
                                  {
                                      Id = songs.Id,
                                      Title = songs.Title,
                                      Duration = songs.Duration,
                                      ImageUrl = songs.ImageUrl,
                                      AudioUrl = songs.AudioUrl
                                  }).Take(15).ToListAsync();

            return Ok(allSongs);
        }
    }
}
