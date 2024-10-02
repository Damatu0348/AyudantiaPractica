using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos
{
    public class CreateProductRequestDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int Price { get; set; }
    }
}