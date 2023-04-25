namespace AttentionAxia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("AXIA.DETALLE_SOLICITUD", "ruta_archivo", c => c.String());
            AddColumn("AXIA.DETALLE_SOLICITUD", "nombre_archivo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("AXIA.DETALLE_SOLICITUD", "nombre_archivo");
            DropColumn("AXIA.DETALLE_SOLICITUD", "ruta_archivo");
        }
    }
}
