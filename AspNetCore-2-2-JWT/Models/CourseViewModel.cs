using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_2_2_JWT.Models
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description{ get; set; }
        public DateTime Created { get; set; }
        public bool Active { get; set; }
    }
}
