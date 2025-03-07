using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMarket.Models;

namespace StockMarket.Interfaces
{
    public interface IcommentRepository
    {
        Task<List<Comment>> GetAllAsync() ;
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);

    }
}