using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Interfaces;
using StockMarket.Models;

namespace StockMarket.Repository
{
    public class PortfolioRepository : IportfolioRepository
    {
        private readonly ApplicationDbContext _context ;
        public PortfolioRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<Portfolio> CreateAsync(Portfolio portfolio)
        {
            await _context.Portfolios.AddAsync(portfolio);
            await _context.SaveChangesAsync();
            return portfolio ;
            
        }

        public async Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(p => p.AppUserId == appUser.Id && p.Stock.Symbol.ToLower() == symbol.ToLower());  
            if (portfolioModel == null){
                return null ;
            }
            _context.Portfolios.Remove(portfolioModel) ;
            await _context.SaveChangesAsync();
            return portfolioModel ;
        }

        public async Task<List<Stock>> GetUserPortfolios(AppUser user)
        {
              return await _context.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                ID = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                LastDiv = stock.Stock.LastDiv,
                Industry = stock.Stock.Industry,
                MArketCap = stock.Stock.MArketCap
            }).ToListAsync();
        }
    }
}