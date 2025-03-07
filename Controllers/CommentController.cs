using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Dtos.Comment;
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
        private readonly IstockRepository _stockRepo;

        public CommentController(IcommentRepository commentRepo , IstockRepository stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.toCommentDto()) ;
            return Ok(commentDto) ;
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id ){
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null){
                return NotFound() ;

            }
            return Ok(comment.toCommentDto());

        }
 

        [HttpPost("{stockId}")]

        public async Task<IActionResult> Create([FromRoute] int  stockId , CreateCommentRequestDto commentDto ){
            if( ! await _stockRepo.StockExists(stockId)){
                return BadRequest("Stock does not exist");
            }

            var commentModel  = commentDto.toCommentFromCreate(stockId) ;
            await _commentRepo.CreateAsync(commentModel) ;
            return CreatedAtAction(nameof(GetById) ,  new{ id  = commentModel} ,commentModel.toCommentDto()) ;


        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id , UpdateCommentRequestDto updateModel){
            var comment = await _commentRepo.UpdateAsync(id , updateModel ) ;
            if( comment == null){
                return NotFound("Comment not found") ;
            }
            return Ok(comment.toCommentDto()) ;

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            var commentModel = await _commentRepo.DeleteAsync(id) ;

            if( commentModel == null){
                return NotFound("Comment not found") ;
            }
            return NoContent();

        }

    }
}