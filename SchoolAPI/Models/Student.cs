using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolAPI.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int? ClassroomId { get; set; }
        [JsonIgnore]
        public Classroom Classroom { get; set; }
    }
}
