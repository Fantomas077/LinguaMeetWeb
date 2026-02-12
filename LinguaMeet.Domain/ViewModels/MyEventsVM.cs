using LinguaMeet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinguaMeet.Domain.ViewModels
{
    public  class MyEventsVM
    {
        public List<Event> Created { get; set; } = new();
        public List<Event> Joined { get; set; } = new();
    }
}
