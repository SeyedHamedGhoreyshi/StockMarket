using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockMarket.Dtos.Comment;
using StockMarket.Models;

namespace StockMarket.Interfaces
{
    public interface IcommentRepository
    {
        Task<List<Comment>> GetAllAsync() ;
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id , UpdateCommentRequestDto commentModel);
        Task<Comment?> DeleteAsync(int id ) ;

    }
}