namespace WissAppEntities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Messages
    {
        public Messages()
        {
            UsersMessages = new HashSet<UsersMessages>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Message { get; set; }

        public DateTime Date { get; set; }

        public virtual ICollection<UsersMessages> UsersMessages { get; set; }
    }
}
