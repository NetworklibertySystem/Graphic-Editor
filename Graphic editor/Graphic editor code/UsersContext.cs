using System.Data.Entity;
using Graphic_editor;
namespace Users
{
    class UsersContext : MyUsername
    {
        public UsersContext() { }
        public DbSet<Usersname> usersName { get; set; }
    }
}

