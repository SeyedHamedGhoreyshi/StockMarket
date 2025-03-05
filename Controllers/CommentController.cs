using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Interfaces;
using StockMarket.Mappers;
using StockMarket.Models;

namespace StockMarket.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly IcommentRepository _commentRepo;

        public CommentController(IcommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.toCommentDto()) ;
            return Ok(commentDto) ;
        }

    }
}