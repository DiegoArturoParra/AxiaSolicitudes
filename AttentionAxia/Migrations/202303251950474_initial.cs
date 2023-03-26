namespace AttentionAxia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
            
            CreateTable(
                "dbo.responsable",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombres = c.String(nullable: false, maxLength: 500, unicode: false),
                        celula_id = c.Int(nullable: false),
                        linea_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.linea", t => t.linea_id)
                .ForeignKey("dbo.celula", t => t.celula_id)
                .Index(t => t.celula_id)
                .Index(t => t.linea_id);
            
            CreateTable(
                "dbo.detalle_solicitud",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        responsable_id = c.Int(nullable: false),
                        estado_id = c.Int(nullable: false),
                        sprint_id = c.Int(nullable: false),
                        iniciativa = c.String(nullable: false, maxLength: 5000, unicode: false),
                        fecha_inicio_sprint = c.DateTime(nullable: false),
                        fecha_fin_sprint = c.DateTime(nullable: false),
                        avance = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.estado", t => t.estado_id)
                .ForeignKey("dbo.sprint", t => t.sprint_id)
                .ForeignKey("dbo.responsable", t => t.responsable_id)
                .Index(t => t.responsable_id)
                .Index(t => t.estado_id)
                .Index(t => t.sprint_id);
            
            CreateTable(
                "dbo.estado",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                        nivel = c.String(nullable: false, maxLength: 7),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.sprint",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 10),
                        periodo = c.String(nullable: false, maxLength: 15),
                        fecha_generacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.linea",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.rol",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.usuario",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        apellido = c.String(nullable: false, maxLength: 100, unicode: false),
                        email = c.String(nullable: false, maxLength: 100, unicode: false),
                        nick_name = c.String(nullable: false, maxLength: 100),
                        clave = c.String(nullable: false, unicode: false, storeType: "text"),
                        rol_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.rol", t => t.rol_id)
                .Index(t => t.rol_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.usuario", "rol_id", "dbo.rol");
            DropForeignKey("dbo.responsable", "celula_id", "dbo.celula");
            DropForeignKey("dbo.responsable", "linea_id", "dbo.linea");
            DropForeignKey("dbo.detalle_solicitud", "responsable_id", "dbo.responsable");
            DropForeignKey("dbo.detalle_solicitud", "sprint_id", "dbo.sprint");
            DropForeignKey("dbo.detalle_solicitud", "estado_id", "dbo.estado");
            DropIndex("dbo.usuario", new[] { "rol_id" });
            DropIndex("dbo.detalle_solicitud", new[] { "sprint_id" });
            DropIndex("dbo.detalle_solicitud", new[] { "estado_id" });
            DropIndex("dbo.detalle_solicitud", new[] { "responsable_id" });
            DropIndex("dbo.responsable", new[] { "linea_id" });
            DropIndex("dbo.responsable", new[] { "celula_id" });
            DropTable("dbo.usuario");
            DropTable("dbo.rol");
            DropTable("dbo.linea");
            DropTable("dbo.sprint");
            DropTable("dbo.estado");
            DropTable("dbo.detalle_solicitud");
            DropTable("dbo.responsable");
            DropTable("dbo.celula");
        }
    }
}
