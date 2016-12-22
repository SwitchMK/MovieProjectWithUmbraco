namespace MovieProjectWithUmbraco.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomContextInitialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilmRating",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Rating = c.Double(nullable: false),
                        UserId = c.String(),
                        FilmId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FilmRating");
        }
    }
}
