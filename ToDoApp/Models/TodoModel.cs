namespace ToDoApp.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TodoModel : DbContext
    {
        public TodoModel()
            : base("name=TodoModel")
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<ToDo> ToDo { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.ToDo)
                .WithOptional(e => e.Category)
                .HasForeignKey(e => e.CatID);
        }
    }
}
