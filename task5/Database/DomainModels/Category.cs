using System.ComponentModel.DataAnnotations.Schema;
using Task.Database.Abstracts;

namespace Task.Database.DomainModels
{
    [Table("categories")]
    public class Category : IEntity
    {
        public Category(int ıd, string name)
        {
            Id = ıd;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

    }
}
