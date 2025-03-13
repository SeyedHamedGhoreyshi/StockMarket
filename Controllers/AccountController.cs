using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarket.Dtos.Account;
using StockMarket.Interfaces;
using StockMarket.Models;

namespace StockMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ItokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager ;
        public AccountController(UserManager<AppUser> userManager , ItokenService tokenservice , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenservice;
            _signinManager = signInManager;
            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto){
             if(!ModelState.IsValid){
                    return BadRequest() ; 
                }
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower()) ;
                if (user == null) return Unauthorized("Invalid username!");
                var result = await _signinManager.CheckPasswordSignInAsync(user ,loginDto.Password , false) ;
                if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            return Ok(
                new NewUserDto
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.createToken(user)
                }
            );


        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto ){
            try{
                if(!ModelState.IsValid){
                    return BadRequest() ; 
                }
                if (registerDto == null)
                {
                    return BadRequest("Invalid user data.");
                }

                var appUser = new AppUser{
                    UserName = registerDto.UserName,
                    Email = registerDto.Email, 
                } ;
                var createdUser = await _userManager.CreateAsync(appUser , registerDto.Password);
                if(createdUser.Succeeded){
                    var roleResult = await _userManager.AddToRoleAsync(appUser , "User");
                    if (roleResult.Succeeded){
                        return Ok(
                            new NewUserDto{
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = _tokenService.createToken(appUser) 
                            }

                        );
                    }
                    else{
                        return StatusCode(500 , roleResult.Errors) ;
                    }
                }
                else{
                     return StatusCode(500 , createdUser.Errors) ;
                }

            }catch(Exception e){
                return StatusCode(500 ,e ) ;

            }

        }

    }
}