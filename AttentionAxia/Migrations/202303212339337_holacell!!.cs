namespace AttentionAxia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class holacell : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.celula",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.celula");
        }
    }
}
