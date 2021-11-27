using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NickelProject.Logic.Entity
{
    public class AuthorEntity
    {
        [Key]//主键 
       
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
