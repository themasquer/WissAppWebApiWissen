namespace WissAppEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBv1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false, maxLength: 500, unicode: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsersMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(),
                        MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ReceiverId)
                .ForeignKey("dbo.Users", t => t.SenderId)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.MessageId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 10, unicode: false),
                        Email = c.String(nullable: false, maxLength: 200, unicode: false),
                        School = c.String(maxLength: 300, unicode: false),
                        Location = c.String(maxLength: 150, unicode: false),
                        BirthDate = c.DateTime(storeType: "date"),
                        Gender = c.String(maxLength: 1, unicode: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersMessages", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.UsersMessages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UsersMessages", "ReceiverId", "dbo.Users");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.UsersMessages", new[] { "MessageId" });
            DropIndex("dbo.UsersMessages", new[] { "ReceiverId" });
            DropIndex("dbo.UsersMessages", new[] { "SenderId" });
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.UsersMessages");
            DropTable("dbo.Messages");
        }
    }
}
