using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StockMarket.Dtos.Comment;
using StockMarket.Dtos.Stock;
using StockMarket.Models;

namespace StockMarket.Mappers
{
    public static class CommentMappers
    {
        public static CommentDto toCommentDto(this Comment commentModel){
            return new CommentDto{
                Id = commentModel.Id ,
                Title = commentModel.Title ,
                Content = commentModel.Content ,
                CreatedOn = commentModel.CreatedOn ,
                CreatedBy = commentModel.AppUser.UserName,
                StockId = commentModel.StockId 
            } ;


        }
        public static Comment toCommentFromCreate(this CreateCommentRequestDto commentDto , int stockID){
            return new Comment{
                Title = commentDto.Title ,
                Content = commentDto.Content ,
                StockId = stockID

            } ;

        }

    }
}