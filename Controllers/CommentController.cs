using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StockMarket.Dtos.Comment;
using StockMarket.Extensions;
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

        private readonly UserManager<AppUser> _usermanager ;

        public CommentController(IcommentRepository commentRepo , IstockRepository stockRepo , UserManager<AppUser> userManager)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _usermanager = userManager;
        
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comments = await _commentRepo.GetAllAsync();
            var commentDto = comments.Select(s => s.toCommentDto()) ;
            return Ok(commentDto) ;
        }

        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id ){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var comment = await _commentRepo.GetByIdAsync(id);

            if(comment == null){
                return NotFound() ;

            }
            return Ok(comment.toCommentDto());

        }
 

        [HttpPost("{stockId:int}")]

        public async Task<IActionResult> Create([FromRoute] int  stockId , CreateCommentRequestDto commentDto ){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if( ! await _stockRepo.StockExists(stockId)){
                return BadRequest("Stock does not exist");
            }
            var username = User.GetUsername() ;
            var appUser = await _usermanager.FindByNameAsync(username) ;

            var commentModel  = commentDto.toCommentFromCreate(stockId) ;
            commentModel.AppUserId =appUser.Id ;
            await _commentRepo.CreateAsync(commentModel) ;
            return CreatedAtAction(nameof(GetById) ,  new{ id  = commentModel} ,commentModel.toCommentDto()) ;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute]int id , UpdateCommentRequestDto updateModel){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepo.UpdateAsync(id , updateModel ) ;
            if( comment == null){
                return NotFound("Comment not found") ;
            }
            return Ok(comment.toCommentDto()) ;
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
                
            var commentModel = await _commentRepo.DeleteAsync(id) ;

            if( commentModel == null){
                return NotFound("Comment not found") ;
            }
            return NoContent();

        }

    }
}