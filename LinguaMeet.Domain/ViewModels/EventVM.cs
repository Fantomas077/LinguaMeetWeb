using LinguaMeet.Domain.Entities;
using Microsoft.AspNetCore.Http; 
using System;
using System.ComponentModel.DataAnnotations;

namespace LinguaMeet.Domain.ViewModels
{
    public class EventVM
    {
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus EventStatus { get; set; }
        public EventType EventType { get; set; }
        public string? OnlineLink { get; set; }
        public int Capacity { get; set; }
        public string City { get; set; }
        public string Adresse { get; set; }
        public string CoverPhotoPath { get; set; }

        [Required]
        public IFormFile CoverPhoto { get; set; }  
    }
}
