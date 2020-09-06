namespace Shared.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int TempId { get; set; }
        public int? TempParentId { get; set; }
    }
}
