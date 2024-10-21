using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.Apis.Errors;
using Store.Apis.Extensions;
using Store.Core.Dtos.Auth;
using Store.Core.Dtos.User;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using Store.Service.Services.Tokens;
using System.Security.Claims;

namespace Store.Apis.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService userService;
        private readonly UserManager<AppUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountController(
            IUserService userService,
            UserManager<AppUser> userManager,
            ITokenService tokenService,
            IMapper mapper
            )
        {
            this.userService = userService;
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }




        [HttpPost("login")]//Post : //API/Account/login
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userService.LoginAsync(loginDto);

            if(user is null) return Unauthorized(new ApiErrorResponse(StatusCodes.Status401Unauthorized,"Invalid Login!"));

            return Ok(user);

        }


        [HttpPost("register")]//Post : //API/Account/register
        public async Task<ActionResult<UserDto>> RegisterDto(RegisterDto registerDto)
        {
            var user = await userService.RegisterAsync(registerDto);

            if(user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest,"Invalid Registration"));

            return Ok(user);

        }



        //I'll give it the BearerToken and return the user with this token
        [HttpGet("getcurrentuser")]
        [Authorize]

        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {

            var useremail=User.FindFirstValue(ClaimTypes.Email);

            if (useremail is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));


            var user = await userManager.FindByEmailAsync(useremail);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });

            
        }


        //I'll give it the BearerToken and return the user with this token
        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<AuthAddressDto>> GetCurrentUserAddress()
        {

            var user = await userManager.FindByEmailWithAddressAsync(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            return Ok(mapper.Map<AuthAddressDto>(user.Address));

        }


        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AuthAddressDto>> UpdateCurrentUserAddress(AuthAddressDto addressDto)
        {

            var user = await userManager.FindByEmailWithAddressAsync(User);

            if (user is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var mappedAddress = mapper.Map<AuthAddressDto, Address>(addressDto);

            //Address returns with 0 Id so it will remove it and create new one
            // So i gave it the id that came from the user before updating
            mappedAddress.Id = user.Address.Id;

            user.Address = mappedAddress;

            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)  return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest)); 


            return Ok(addressDto);

        }



    }
}
