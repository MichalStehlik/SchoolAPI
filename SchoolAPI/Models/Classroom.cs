using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolAPI.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string Name { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
