using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Interfaces;
using StockMarket.Models;
using StockMarket.Dtos.Comment ;

namespace StockMarket.Repository
{
    public class CommentRepository : IcommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment) ;
            await _context.SaveChangesAsync();
            return comment ;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.Comments.FindAsync(id) ;
            if (commentModel == null){
                return null ;
            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync() ;
        }

        public async  Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

            public  async Task<Comment?> UpdateAsync(int id, UpdateCommentRequestDto commentDto)
        {
            var commentModel =  await _context.Comments.FindAsync(id);
            if(commentModel == null){
                return null ;
            }
            commentModel.Title = commentDto.Title ;
            commentModel.Content = commentDto.Content ;
            await _context.SaveChangesAsync();
            return commentModel ;

        }

       
    }
}