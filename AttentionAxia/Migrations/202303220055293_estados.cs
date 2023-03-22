namespace AttentionAxia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class estados : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.estado",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                        nivel = c.String(nullable: false, maxLength: 7),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.estado");
        }
    }
}
