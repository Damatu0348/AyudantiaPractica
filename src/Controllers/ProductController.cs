using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Data;
using api.src.Dtos;
using api.src.Mappers;
using api.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ProductController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _context.Products.ToListAsync();
            var productDto = products.Select(p => p.ToProductDto());
            return Ok(productDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product.ToProductDto());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductRequestDto productDto)
        {
            var productModel = productDto.ToProductFromCreateDto();
            await _context.Products.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id = productModel.Id}, productModel.ToProductDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutId([FromRoute] int id, [FromBody] UpdateProductRequestDto updateDto)
        {
            var ProductModel = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(ProductModel == null)
            {
                return NotFound();
            }
            ProductModel.Name = updateDto.Name;
            ProductModel.Price = updateDto.Price;

            await _context.SaveChangesAsync();
            return Ok(ProductModel.ToProductDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}