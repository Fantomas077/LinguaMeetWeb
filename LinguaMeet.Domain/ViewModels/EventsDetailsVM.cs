using LinguaMeet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Domain.ViewModels
{
    public class EventDetailsVM
    {
        public Event Event { get; set; } = null!;

        public int RegisteredCount { get; set; }

        public bool IsRegistered { get; set; }

        public bool IsFull =>
            Event.Capacity > 0 && RegisteredCount >= Event.Capacity;
    }

}
