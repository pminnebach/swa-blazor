using System;

namespace BlazorApp.Shared
{
    public class TimeRegistration
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        public string RegistrationType { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
    }
} 