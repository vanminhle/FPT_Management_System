namespace FPT_Management_System.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CreateStaffsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Staffs",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    FullName = c.String(nullable: false, maxLength: 30),
                    Age = c.Int(nullable: false),
                    Address = c.String(nullable: false),
                })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Staffs", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Staffs", new[] { "UserId" });
            DropTable("dbo.Staffs");
        }
    }
}