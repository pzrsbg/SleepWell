namespace SleepWell.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reclamation_DateTime_Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Reclamations", "DateCreated", c => c.DateTime());
            AlterColumn("dbo.Reclamations", "DateFinished", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Reclamations", "DateFinished", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Reclamations", "DateCreated", c => c.DateTime(nullable: false));
        }
    }
}
