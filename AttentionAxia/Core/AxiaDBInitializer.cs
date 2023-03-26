using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using System.Collections.Generic;

namespace AttentionAxia.Core
{
    public class AxiaDBInitializer
    {
        public static AxiaContext Seed(AxiaContext context)
        {
            IList<Rol> defaultRoles = new List<Rol>
            {
              new Rol
              {
                  Id = 1,
                  Descripcion = "Administrador-Axia"
              },
               new Rol
              {
                  Id = 2,
                  Descripcion = "Genérico-Axia"
              }
            };
            context.TablaRoles.AddRange(defaultRoles);

            IList<Usuario> defaultUsers = new List<Usuario>
            {
              new Usuario
              {
                  Email= "axia-admin@gmail.com",
                  Apellidos = "axia",
                  Nombres = "administrador",
                  Clave = HashHelper.GenerateHashWithPassword("admin12345"),
                  NickName = "admin-axia",
                  RolId = 1
              },
                new Usuario
              {
                  Email= "axia-generico@gmail.com",
                  Apellidos = "lectura",
                  Nombres = "Axia",
                  Clave = HashHelper.GenerateHashWithPassword("axia12345"),
                  NickName = "axia",
                  RolId = 2
              }
            };

            context.TablaUsuarios.AddRange(defaultUsers);

            IList<Celula> defaultCelulas = new List<Celula>
            {
              new Celula
              {
                Descripcion = "Móvil"
              },
              new Celula
              {
                 Descripcion = "Axia"
              }
            };

            context.TablaCelulas.AddRange(defaultCelulas);

            IList<Estado> defaultEstados = new List<Estado>
            {
              new Estado
              {
                Descripcion = "Por Hacer",
                 Nivel = "#A28F8F",
              },
              new Estado
              {
                 Descripcion = "En Progreso",
                 Nivel = "#74DEE9",

              },
                new Estado
              {
                Descripcion = "Finalizado",
                 Nivel = "#8AF5A9",
              }
            };

            context.TablaEstados.AddRange(defaultEstados);

            IList<Linea> defaultLineas = new List<Linea>
            {
              new Linea
              {
                Descripcion = "QA"
              },
              new Linea
              {
                 Descripcion = "Soporte"
              },
                new Linea
              {
                Descripcion = "Desarrollo"
              }
            };

            context.TablaLineas.AddRange(defaultLineas);

            return context;
        }
    }
}