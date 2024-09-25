using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.src.models
{
    public class User
    {
        public int Id { get; set; }
        public string Rut { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        //Entityframework relationships
        public List<Product> Products {get; set;} = [];

        public int RoleId {get; set;}

        public Role Role {get; set;} = null!;

    }
}