namespace IPerformersEval.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial3 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.FIN_PE_UserProfile");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FIN_PE_UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
    }
}
