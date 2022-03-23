using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolAPI.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<Student> Students { get; set; }
    }
}
