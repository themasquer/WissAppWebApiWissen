namespace WissAppEF.Contexts
{
    using System.Data.Entity;
    using WissAppEntities.Entities;

    public partial class WissAppContext : DbContext
    {
        public WissAppContext()
            : base("name=WissAppContext")
        {
        }

        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersMessages> UsersMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Messages>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<Messages>()
                .HasMany(e => e.UsersMessages)
                .WithRequired(e => e.Messages)
                .HasForeignKey(e => e.MessageId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.Roles)
                .HasForeignKey(e => e.RoleId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.School)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.SenderMessages)
                .WithRequired(e => e.Senders)
                .HasForeignKey(e => e.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.ReceiverMessages)
                .WithOptional(e => e.Receivers)
                .HasForeignKey(e => e.ReceiverId);
        }
    }
}
