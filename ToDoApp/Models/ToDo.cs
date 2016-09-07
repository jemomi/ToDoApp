namespace ToDoApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToDo")]
    public partial class ToDo
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Created { get; set; }

        public int? CatID { get; set; }

        public bool? Finished { get; set; }

        public virtual Category Category { get; set; }
    }
}
