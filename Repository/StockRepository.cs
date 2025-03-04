using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Dtos.Stock;
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

        public async  Task<List<Stock>> GetAllAsync()
        {
            return  await _context.Stocks.ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return  await _context.Stocks.FindAsync(id);
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