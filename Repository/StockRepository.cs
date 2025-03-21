using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Dtos.Stock;
using StockMarket.Helpers;
using StockMarket.Interfaces;
using StockMarket.Models;

namespace StockMarket.Repository
{
    public class StockRepository : IstockRepository
    {
        private readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel) ;
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.ID == id) ;
            if (stockModel == null){
                return null ;
            }
            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
            


        }

        public async  Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks =   _context.Stocks.Include(c => c.comments).ThenInclude(a => a.AppUser).AsQueryable();

            if(!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));  
            }

            if(!string.IsNullOrWhiteSpace(query.Symbol)){
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));    
                
            }
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;


            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
            
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return  await _context.Stocks.Include(c => c.comments).FirstOrDefaultAsync(x => x.ID == id);
        }

        public async Task<Stock?> GetBySymbolAsync(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol == symbol) ;
        }

        public async Task<bool> StockExists(int id)
        {
            return await _context.Stocks.AnyAsync(x => x.ID == id) ;
        }

        public  async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockModel =  await _context.Stocks.FirstOrDefaultAsync(x => x.ID == id) ;
            if(stockModel == null){
                return null ;
            }
            stockModel.Symbol = stockDto.Symbol ;
            stockModel.CompanyName = stockDto.CompanyName ;
            stockModel.Purchase = stockDto.Purchase ;
            stockModel.LastDiv = stockDto.LastDiv ;
            stockModel.Industry = stockDto.Industry ;
            stockModel.MArketCap = stockDto.MarketCap ;
            await _context.SaveChangesAsync();
            return stockModel ;

        }
    }
}