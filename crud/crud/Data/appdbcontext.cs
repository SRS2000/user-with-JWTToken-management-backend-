using crud.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace crud.Data
{
    public class appdbcontext : DbContext
    {
        public appdbcontext(DbContextOptions<appdbcontext> options) : base(options){}
        public DbSet<User> Users { get; set; }
    }
}
