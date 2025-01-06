﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace basic_fra_hw_02.Models
{
    public class Cinema
    {
        [Key]  // Data annotation specifying that Cinema_id is the primary key
        [JsonIgnore] // Hides this field from Swagger and JSON serialization
        public Guid CinemaId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
