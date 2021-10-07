using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Exttensions;
using API.Dtos;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Core.Params;

namespace API.Controllers
{
    [ApiController]
    [Route("api/rating")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService ratingService;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public RatingController(
            IRatingService ratingService,
            UserManager<AppUser> userManager,
            IMapper mapper)
        {
            this.ratingService = ratingService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Rating>>> GetRatings()
        {
            var user = await userManager.FindUserByClaimsPrincipal(User);

            var ratings = await ratingService.GetRatings(user.Id);

            return ratings != null ? Ok(ratings) : NotFound();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("products/{id}")]
        public async Task<ActionResult<RatingsReturnDto>> GetRatingsByProduct(string id, [FromQuery] RatingParams param)
        {
            // Alternative query string product and want provide params on link
            param.ProductId = id;

            var count = await ratingService.CountRatingsWithSpec(param);

            var ratings = await ratingService.GetRatingsWithSpec(param);

            var averageRating = ratingService.GetAverageRating(ratings);

            return new RatingsReturnDto { Ratings = ratings, Count = count, AverageRating = averageRating };
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            var user = await userManager.FindUserByClaimsPrincipal(User);

            var rating = await ratingService.GetRating(id);

            return rating ?? (ActionResult<Rating>)NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<RatingReturnDto>> CreateRating(RatingDto ratingDto)
        {
            var user = await userManager.FindUserByClaimsPrincipal(User);

            // 1. Check user has rating yet.
            bool hasRated = await ratingService.HasExistedRating(user.Id, ratingDto.ProductId, null);

            if (hasRated)
            {
                return BadRequest("User has rated yet.");
            }

            // 2. Create Rating from user

            var rating = await ratingService.AddRating(ratingDto.Comment, user.Id, ratingDto.RatingNumber, ratingDto.ProductId);

            var ratingReturnDto = mapper.Map<Rating, RatingReturnDto>(rating);

            return new ObjectResult(ratingReturnDto) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RatingReturnDto>> UpdateRating(int id, RatingDto ratingDto)
        {
            var user = await userManager.FindUserByClaimsPrincipal(User);

            // 1. Check user has rating yet.
            bool hasRated = await ratingService.HasExistedRating(user.Id, ratingDto.ProductId, null);

            if (!hasRated)
            {
                return BadRequest("User has not rated yet.");
            }

            // 2. Create Rating from user

            var rating = await ratingService.UpdateRating(id, ratingDto.Comment, ratingDto.RatingNumber);

            var ratingReturnDto = mapper.Map<Rating, RatingReturnDto>(rating);

            return Ok(ratingReturnDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<RatingReturnDto>> DeleteRating(int id)
        {
            var user = await userManager.FindUserByClaimsPrincipal(User);

            // 1. Check user has rating yet.
            bool hasRated = await ratingService.HasExistedRating(user.Id, null, id);

            if (!hasRated)
            {
                return BadRequest("User has not rated yet.");
            }

            // 2. Remove rating
            var rating = await ratingService.DeleteRating(id);

            var ratingReturnDto = mapper.Map<Rating, RatingReturnDto>(rating);

            return Ok(ratingReturnDto);
        }
    }
}