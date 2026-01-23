using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinguaMeet.Domain.Entities
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(300)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus EventStatus { get; set; }
        public EventType EventType { get; set; }
        public string ?OnlineLink { get; set; }
        public int Capacity { get; set; }
        public string City { get; set; }
        public string Adresse { get; set; }
        public string CoverPhotoPath { get; set; }

        public ICollection<EventRegistration> Registrations { get; set; }

    }
}
