using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Exttensions;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            // Find user by token (body token has claims like (email) )
            var user = await userManager.FindUserByClaimsPrincipal(User);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            };
        }

        #region Email

        [HttpGet]
        [Route("emailexists")]
        public async Task<bool> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        [HttpGet]
        [Authorize]
        [Route("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserByClaimsPrincipalIncludeAddress(User);

            return mapper.Map<Address, AddressDto>(user.Address);
        }

        [HttpPut]
        [Authorize]
        [Route("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await userManager.FindUserByClaimsPrincipalIncludeAddress(User);

            user.Address = mapper.Map<AddressDto, Address>(address);

            await userManager.UpdateAsync(user);

            return mapper.Map<Address, AddressDto>(user.Address);
        }
        #endregion

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDto loginDto)
        {
            var user = await this.userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var reuslt = await this.signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!reuslt.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDTO()
            {
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserDTO>> SignUp(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result)
            {
                return new BadRequestObjectResult(new { Errors = new[] { "Email address is in use" } });
            }

            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await this.userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDTO()
            {
                DisplayName = user.DisplayName,
                Token = tokenService.CreateToken(user),
                Email = user.Email
            };
        }
    }
}