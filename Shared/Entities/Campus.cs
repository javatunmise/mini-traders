using System.Collections.Generic;

namespace Shared.Entities
{
    public class Campus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Hostel> Hostels { get; set; }
    }
}
