using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Extensions;
using StockMarket.Interfaces;
using StockMarket.Models;

namespace StockMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IstockRepository _stockRepo ;
        private readonly IportfolioRepository _portfolioRepo ;
        private readonly UserManager<AppUser> _userManager ;

        public PortfolioController(UserManager<AppUser> userManager , IstockRepository stockRepo , IportfolioRepository portfoliorepo)
        {
            _userManager = userManager;
            _stockRepo = stockRepo;
            _portfolioRepo = portfoliorepo ;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolios(){
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio = await _portfolioRepo.GetUserPortfolios(appUser);
            return Ok(userPortfolio) ;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPortfolio(string symbol){
            var username = User.GetUsername() ;
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if(stock == null){
                return BadRequest("Stock not found");
            }
            var userPortfolio = await _portfolioRepo.GetUserPortfolios(appUser);

            if(userPortfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())){
                return BadRequest("Cannot add same stock to portfolio") ;
             
            }
            var portfolioModel = new Portfolio
            {
                StockId = stock.ID,
                AppUserId = appUser.Id
            };
            await _portfolioRepo.CreateAsync(portfolioModel) ;
            if(portfolioModel == null){
                return StatusCode(500 , "Could not create!") ;
            }
            else{
                return Created() ;
            }


        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string symbol){
            var username = User.GetUsername() ;
            var appUser = await _userManager.FindByNameAsync(username) ;

            var userPortfolio = await _portfolioRepo.GetUserPortfolios(appUser);

            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

              if (filteredStock.Count() == 1)
            {
                await _portfolioRepo.DeletePortfolio(appUser, symbol);
            }
            else
            {
                return BadRequest("Stock not in your portfolio");
            }

            return Ok();

        }

    }
}