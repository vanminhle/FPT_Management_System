namespace FPT_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTraineesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Trainees",
                c => new
                    {
                        TraineeId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(nullable: false, maxLength: 30),
                        Age = c.Int(nullable: false),
                        DateOfBirth = c.DateTime(nullable: false),
                        Education = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.TraineeId)
                .ForeignKey("dbo.AspNetUsers", t => t.TraineeId)
                .Index(t => t.TraineeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trainees", "TraineeId", "dbo.AspNetUsers");
            DropIndex("dbo.Trainees", new[] { "TraineeId" });
            DropTable("dbo.Trainees");
        }
    }
}
