namespace AttentionAxia.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AXIA.CELULA_INICIATIVA",
                c => new
                    {
                        id_celula = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.id_celula);
            
            CreateTable(
                "AXIA.DETALLE_SOLICITUD",
                c => new
                    {
                        id_solicitud = c.Int(nullable: false, identity: true),
                        responsable_id = c.Int(nullable: false),
                        estado_solicitud_id = c.Int(nullable: false),
                        sprint_inicial_id = c.Int(nullable: false),
                        sprint_final_id = c.Int(nullable: false),
                        celula_id = c.Int(nullable: false),
                        iniciativa = c.String(nullable: false, maxLength: 5000, unicode: false),
                        fecha_inicio_sprint = c.DateTime(nullable: false),
                        fecha_fin_sprint = c.DateTime(nullable: false),
                        fecha_creacion_solicitud = c.DateTime(nullable: false),
                        fecha_comienzo_solicitud = c.DateTime(),
                        fecha_finalizacion_solicitud = c.DateTime(),
                        avance_porcentual = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.id_solicitud)
                .ForeignKey("AXIA.ESTADO_SOLICITUD", t => t.estado_solicitud_id)
                .ForeignKey("AXIA.RESPONSABLE", t => t.responsable_id)
                .ForeignKey("AXIA.SPRINT", t => t.sprint_final_id)
                .ForeignKey("AXIA.SPRINT", t => t.sprint_inicial_id)
                .ForeignKey("AXIA.CELULA_INICIATIVA", t => t.celula_id)
                .Index(t => t.responsable_id)
                .Index(t => t.estado_solicitud_id)
                .Index(t => t.sprint_inicial_id)
                .Index(t => t.sprint_final_id)
                .Index(t => t.celula_id);
            
            CreateTable(
                "AXIA.ESTADO_SOLICITUD",
                c => new
                    {
                        id_estado_solicitud = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                        nivel = c.String(nullable: false, maxLength: 7),
                    })
                .PrimaryKey(t => t.id_estado_solicitud);
            
            CreateTable(
                "AXIA.RESPONSABLE",
                c => new
                    {
                        id_responsable = c.Int(nullable: false, identity: true),
                        nombres = c.String(nullable: false, maxLength: 500, unicode: false),
                        linea_trabajo_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_responsable)
                .ForeignKey("AXIA.LINEA_DE_TRABAJO", t => t.linea_trabajo_id)
                .Index(t => t.linea_trabajo_id);
            
            CreateTable(
                "AXIA.LINEA_DE_TRABAJO",
                c => new
                    {
                        id_linea_trabajo = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.id_linea_trabajo);
            
            CreateTable(
                "AXIA.SPRINT",
                c => new
                    {
                        id_sprint = c.Int(nullable: false, identity: true),
                        sigla = c.String(nullable: false, maxLength: 10),
                        periodo = c.String(nullable: false, maxLength: 15),
                        fecha_generacion = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id_sprint);
            
            CreateTable(
                "AXIA.ROL",
                c => new
                    {
                        id_rol = c.Int(nullable: false, identity: true),
                        descripcion = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.id_rol);
            
            CreateTable(
                "AXIA.USUARIO",
                c => new
                    {
                        id_usuario = c.Int(nullable: false, identity: true),
                        nombre = c.String(nullable: false, maxLength: 100, unicode: false),
                        apellido = c.String(nullable: false, maxLength: 100, unicode: false),
                        email = c.String(nullable: false, maxLength: 100, unicode: false),
                        nick_name = c.String(nullable: false, maxLength: 100),
                        clave = c.String(nullable: false, unicode: false, storeType: "text"),
                        rol_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id_usuario)
                .ForeignKey("AXIA.ROL", t => t.rol_id)
                .Index(t => t.rol_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("AXIA.USUARIO", "rol_id", "AXIA.ROL");
            DropForeignKey("AXIA.DETALLE_SOLICITUD", "celula_id", "AXIA.CELULA_INICIATIVA");
            DropForeignKey("AXIA.DETALLE_SOLICITUD", "sprint_inicial_id", "AXIA.SPRINT");
            DropForeignKey("AXIA.DETALLE_SOLICITUD", "sprint_final_id", "AXIA.SPRINT");
            DropForeignKey("AXIA.RESPONSABLE", "linea_trabajo_id", "AXIA.LINEA_DE_TRABAJO");
            DropForeignKey("AXIA.DETALLE_SOLICITUD", "responsable_id", "AXIA.RESPONSABLE");
            DropForeignKey("AXIA.DETALLE_SOLICITUD", "estado_solicitud_id", "AXIA.ESTADO_SOLICITUD");
            DropIndex("AXIA.USUARIO", new[] { "rol_id" });
            DropIndex("AXIA.RESPONSABLE", new[] { "linea_trabajo_id" });
            DropIndex("AXIA.DETALLE_SOLICITUD", new[] { "celula_id" });
            DropIndex("AXIA.DETALLE_SOLICITUD", new[] { "sprint_final_id" });
            DropIndex("AXIA.DETALLE_SOLICITUD", new[] { "sprint_inicial_id" });
            DropIndex("AXIA.DETALLE_SOLICITUD", new[] { "estado_solicitud_id" });
            DropIndex("AXIA.DETALLE_SOLICITUD", new[] { "responsable_id" });
            DropTable("AXIA.USUARIO");
            DropTable("AXIA.ROL");
            DropTable("AXIA.SPRINT");
            DropTable("AXIA.LINEA_DE_TRABAJO");
            DropTable("AXIA.RESPONSABLE");
            DropTable("AXIA.ESTADO_SOLICITUD");
            DropTable("AXIA.DETALLE_SOLICITUD");
            DropTable("AXIA.CELULA_INICIATIVA");
        }
    }
}
