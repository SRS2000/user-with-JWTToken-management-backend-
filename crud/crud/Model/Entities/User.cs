using System.ComponentModel.DataAnnotations;

namespace crud.Model.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }    
        public string Email { get; set; }   
        public int Age { get; set; }       
        public DateTime CreatedDate { get; set; }    
        public DateTime UpdatedDate { get; set; }
    }
}
