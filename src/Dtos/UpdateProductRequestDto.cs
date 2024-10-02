using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.Dtos
{
    public class UpdateProductRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; }
    }
}