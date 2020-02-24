namespace WissAppEntities.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Users
    {
        public Users()
        {
            SenderMessages = new HashSet<UsersMessages>();
            ReceiverMessages = new HashSet<UsersMessages>();
        }

        public int Id { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(300)]
        public string School { get; set; }

        [StringLength(150)]
        public string Location { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        public bool IsActive { get; set; }

        public virtual Roles Roles { get; set; }

        public virtual ICollection<UsersMessages> SenderMessages { get; set; }

        public virtual ICollection<UsersMessages> ReceiverMessages { get; set; }
    }
}
