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
        public IActionResult Get()
        {
            var products = _context.Products.ToList()//_context.Users.Include(u => u.Role).Include(u => u.Products)ToList();
            .Select(p => p.ToProductDto());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var product = _context.Products.FirstOrDefault(u => u.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product.ToProductDto());
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreateProductRequestDto productDto)
        {
            var productModel = productDto.ToProductFromCreateDto();
            _context.Products.Add(productModel);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = productModel.Id}, productModel.ToProductDto());
        }

        [HttpPut("{id}")]
        public IActionResult PutId([FromRoute] int id, [FromBody] UpdateProductRequestDto updateDto)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);
            if(existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = updateDto.Name;
            existingProduct.Price = updateDto.Price;

            _context.SaveChanges();
            return Ok(existingProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteId([FromRoute] int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok("Product deleted");
        }

    }
}