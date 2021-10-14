namespace FPT_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditStaffsTable : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Staffs", name: "UserId", newName: "StaffId");
            RenameIndex(table: "dbo.Staffs", name: "IX_UserId", newName: "IX_StaffId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Staffs", name: "IX_StaffId", newName: "IX_UserId");
            RenameColumn(table: "dbo.Staffs", name: "StaffId", newName: "UserId");
        }
    }
}
