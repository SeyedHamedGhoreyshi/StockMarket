using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockMarket.Data;
using StockMarket.Dtos.Stock;
using StockMarket.Helpers;
using StockMarket.Interfaces;
using StockMarket.Mappers;

namespace StockMarket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StockController : ControllerBase
    {
        private readonly IstockRepository _stockRepo;
        private readonly ApplicationDbContext _context ;
        public StockController(ApplicationDbContext context , IstockRepository stockRepo)
        {
            _stockRepo = stockRepo;
            _context = context;
        }

        

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto = stocks.Select(s => s.toStockDto()) ;
            return Ok(stockDto) ;
     

        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stock = await _stockRepo.GetByIdAsync(id) ;
            if (stock == null){
                return NotFound();
            }
            return Ok(stock.toStockDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = stockDto.TostockFromCreateDto() ;
            await _stockRepo.CreateAsync(stockModel) ;
            return CreatedAtAction(nameof(GetById) ,  new{ id  = stockModel.ID} ,stockModel.toStockDto()) ;


        }

        [HttpPut]
        [Route("{id:int}")]
        public async  Task<IActionResult> Update([FromRoute] int id , [FromBody] UpdateStockRequestDto updateDto){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel =  await _stockRepo.UpdateAsync(id , updateDto);
            
            if (stockModel == null){
                return NotFound() ;
            }
          
            return Ok(stockModel.toStockDto()) ;
        }
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id){
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var stockModel = await _stockRepo.DeleteAsync(id) ;

            if (stockModel == null){
                return NotFound() ;
            }


            return NoContent() ;

        }



    }
}