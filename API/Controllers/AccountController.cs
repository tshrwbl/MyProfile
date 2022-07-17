using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext dataContext;
        private readonly ITokenService tokenService;

        public AccountController(DataContext dataContext , ITokenService tokenService )
        {
            this.dataContext = dataContext;
            this.tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {
            if (await IsUserExist(registerDTO.UserName))
            {
                return BadRequest("Bad/Existing User Name");
            }
            using var HMAC = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.UserName.ToLower(),
                UserPassword = HMAC.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = HMAC.Key,
                CreatedTimestamp = DateTime.UtcNow

            };

            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();

            return new UserDto() { UserName = user.UserName , Token = tokenService.CreateToken(user)};
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(RegisterDTO LoginDTO)
        {
            var user = await dataContext.Users.SingleOrDefaultAsync(x => x.UserName == LoginDTO.UserName.ToLower());

            if (user == null)
                return Unauthorized("UserName Does not exist");

            using var HMAC = new HMACSHA512(user.PasswordSalt);
            using var PasswordStream = new MemoryStream(Encoding.UTF8.GetBytes(LoginDTO.Password));
            var ComputedHash = await HMAC.ComputeHashAsync(PasswordStream);

            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != user.UserPassword[i]) return Unauthorized("Invalid password");
            }

            return new UserDto() { UserName = user.UserName, Token = tokenService.CreateToken(user) };
        }

        private async Task<bool> IsUserExist(string userName)
        {
            return await dataContext.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}
