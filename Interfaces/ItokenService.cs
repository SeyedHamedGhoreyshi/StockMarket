using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMarket.Models;

namespace StockMarket.Interfaces
{
    public interface ItokenService
    {

        string createToken(AppUser user) ;

    }
}