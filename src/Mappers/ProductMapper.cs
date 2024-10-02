using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.models;

namespace api.src.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product productModel)
        {
            return new ProductDto
            {
                Id = productModel.Id,
                Name = productModel.Name,
                Price = productModel.Price
            };
        }
        public static Product ToProductFromCreateDto(this CreateProductRequestDto createProductRequestDto)
        {
            return new Product
            {
                Name = createProductRequestDto.Name,
                Price = createProductRequestDto.Price
            };

        }
    }    
}