using System.Data.Entity;

namespace Graphic_editor
{
  public  class ApplicationContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public ApplicationContext() : base("Spiel") { }
    }
}
