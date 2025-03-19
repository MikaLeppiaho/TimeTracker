using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeTracker.Models
{

    public class Customer
    {
        public int CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        public string ContactInfo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public ICollection<Project> Projects { get; set; }
    }
}
