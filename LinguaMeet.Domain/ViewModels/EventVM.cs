using LinguaMeet.Domain.Entities;
using Microsoft.AspNetCore.Http;

using System;
using System.ComponentModel.DataAnnotations;

namespace LinguaMeet.Domain.ViewModels
{
    public class EventVM
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public EventStatus EventStatus { get; set; }

        [Required]
        public EventType EventType { get; set; }

        public string? OnlineLink { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string City { get; set; }

        public string Adresse { get; set; }

        // Ne PAS valider - rempli par le controller
        
        public string? CoverPhotoPath { get; set; }

        // Le fichier uploadé
        public IFormFile? CoverPhoto { get; set; }
    }
}