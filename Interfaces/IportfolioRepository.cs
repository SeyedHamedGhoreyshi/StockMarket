using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMarket.Models;

namespace StockMarket.Interfaces
{
    public interface IportfolioRepository
    {
        Task<List<Stock>> GetUserPortfolios(AppUser user) ;
        Task<Portfolio> CreateAsync(Portfolio portfolio) ;

        Task<Portfolio> DeletePortfolio(AppUser appUser , string symbol) ;



    }
}