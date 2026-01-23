using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LinguaMeet.Domain.Entities
{
    public  class EventRegistration
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public Event Event { get; set; }

        public DateTime RegisteredAt { get; set; }

        public RegistrationStatus Status { get; set; }
    }
}
