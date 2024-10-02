using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.src.Dtos;
using api.src.models;

namespace api.src.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<Product> Post(Product product);
        Task<Product?> Put(int id, UpdateProductRequestDto productDto);
        Task<Product?> Delete(int id);
    }
}