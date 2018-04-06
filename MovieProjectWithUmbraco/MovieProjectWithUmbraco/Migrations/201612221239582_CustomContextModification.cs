namespace MovieProjectWithUmbraco.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class CustomContextModification : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FilmRating", "UserId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FilmRating", "UserId", c => c.String());
        }
    }
}
