using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using UserBookToken.Context;
using UserBookToken.DTO;
using UserBookToken.Entities;
using UserBookToken.Token;

namespace UserBookToken.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public UserController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext appDbContext, IMapper mapper)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        
        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserDto dto)
        {
            var user = _mapper.Map<AppUser>(dto);
            
            var result = await _userManager.CreateAsync(user,dto.Password);

            if (!result.Succeeded) 
            {
                return BadRequest("bir hata oluştu");
            
            }



            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            TokenGenerator token = new TokenGenerator();
            var result2 = "";
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null) 
            {
              return BadRequest("Kullanıcı adı veya şifre yanlış.");
            
            }

            var result = await _signInManager.PasswordSignInAsync(user, dto.Password,false,false);

            if (result.Succeeded)
            {
               var roleList = await _userManager.GetRolesAsync(user);
                var roleClaims = roleList.Select(x=>new Claim(ClaimTypes.Role,x)).ToList();

                var claimList = await _userManager.GetClaimsAsync(user);
                var listUserİnfo = new List<Claim>() 
                {
                 new Claim(ClaimTypes.Name,user.Name),
                 new Claim(ClaimTypes.Surname,user.SurName),
                 new Claim(ClaimTypes.DateOfBirth,user.BirthDate.ToString())
  
                };

                roleClaims.AddRange(claimList);
                roleClaims.AddRange(listUserİnfo);
                result2 = token.GenerateToken(roleClaims);
 
            }
            else return BadRequest("Kullanıcı adı veya şifre yanlış");

            return Ok(result2);
            
        }
    }
}
