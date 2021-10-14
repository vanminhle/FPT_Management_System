namespace FPT_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditStaffTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Staffs", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Staffs", "Email");
        }
    }
}
