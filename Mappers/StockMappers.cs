using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMarket.Dtos.Stock;
using StockMarket.Models;

namespace StockMarket.Mappers
{
    public static class StockMapper
    {
        public static StockDto toStockDto(this Stock stockModel)
        {
            return new StockDto{
                Id = stockModel.ID ,
                Symbol = stockModel.Symbol ,
                CompanyName = stockModel.CompanyName ,
                Purchase = stockModel.Purchase ,
                LastDiv = stockModel.LastDiv ,
                Industry = stockModel.Industry ,
                MarketCap = stockModel.MArketCap ,
                Comments = stockModel.comments.Select(c => c.toCommentDto()).ToList(),
            } ;

        }
        public static Stock TostockFromCreateDto (this CreateStockRequestDto stockDto){
            return new Stock{
                Symbol = stockDto.Symbol ,
                CompanyName = stockDto.CompanyName ,
                Purchase = stockDto.Purchase ,
                LastDiv = stockDto.LastDiv ,
                Industry = stockDto .Industry ,
                MArketCap = stockDto.MarketCap

            } ;
        }

    }
}