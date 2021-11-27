﻿using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> GetArtists(int? pageNumber, int? pageSize)
        {
            int currentPageNumber = pageNumber ?? 1;
            int currentPageSize = pageSize ?? 5;

            var artists = await (from artist in _dbContext.Artists
            select new
            {
                Id = artist.Id,
                Name = artist.Name,
                ImageUrl = artist.ImageUrl,
            }).ToListAsync();

            return Ok(artists.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Details(int artistId)
        {
            var artistDetails = await _dbContext.Artists.Where(artist => artist.Id == artistId).Include(a => a.Songs).ToListAsync();
            return Ok(artistDetails);
        }

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArtistsApiModel value)
        {
            var album = await _dbContext.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound("No record found against this id");
            }
            else
            {
                album.Name = value.Name;
                await _dbContext.SaveChangesAsync();
                return Ok("Record Updated Successfully");
            }
        }
    }
}
