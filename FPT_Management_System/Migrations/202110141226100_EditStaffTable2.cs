namespace FPT_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EditStaffTable2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Staffs", "Email");
        }

        public override void Down()
        {
            AddColumn("dbo.Staffs", "Email", c => c.String(nullable: false));
        }
    }
}
