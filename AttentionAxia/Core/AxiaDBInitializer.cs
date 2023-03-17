using AttentionAxia.Core.Data;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using System.Collections.Generic;
using System.Data.Entity;

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

            return context;
        }
    }
}