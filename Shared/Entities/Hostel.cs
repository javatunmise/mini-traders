namespace Shared.Entities
{
    public class Hostel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CampusId { get; set; }
        public Campus Campus { get; set; }
    }
}
