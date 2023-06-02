namespace AttentionAxia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class otra : DbMigration
    {
        public override void Up()
        {
            AlterColumn("AXIA.DETALLE_SOLICITUD", "porcentaje_cumplimiento", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("AXIA.DETALLE_SOLICITUD", "porcentaje_cumplimiento", c => c.Byte(nullable: false));
        }
    }
}
