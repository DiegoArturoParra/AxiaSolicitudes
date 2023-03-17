using AttentionAxia.Core.Data;
using AttentionAxia.DTOs;
using AttentionAxia.Helpers;
using AttentionAxia.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace AttentionAxia.Repositories
{
    public class UserRepository
    {
        public async Task<ResponseDTO> VerifyLogin(LoginDTO login)
        {
            try
            {
                using (var db = new AxiaContext())
                {
                    Usuario user;
                    if (login.EmailOrNickName.Contains("@"))
                    {
                        user = await db.TablaUsuarios.Where(x => x.Email.Equals(login.EmailOrNickName))
                            .Include(y => y.Rol).FirstOrDefaultAsync();
                    }
                    else
                    {
                        user = await db.TablaUsuarios.Where(x => x.NickName.Equals(login.EmailOrNickName))
                         .Include(y => y.Rol).FirstOrDefaultAsync();
                    }
                    if (user == null)
                    {
                        return Responses.SetErrorResponse("El nombre de usuario o la contraseña es incorrecto. El acceso fue denegado.");
                    }

                    bool verifyPass = HashHelper.VerifyPassword(login.Password, user.Clave);
                    if (!verifyPass)
                    {
                        return Responses.SetErrorResponse("El nombre de usuario o la contraseña es incorrecto. El acceso fue denegado.");
                    }
                    return Responses.SetOkResponse("Logeo satisfactorio.", new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Nombres,
                        LastName = user.Apellidos,
                        Rol = user.Rol.Descripcion,
                        RolId = user.Rol.Id,
                    });
                }
            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, "Ha ocurrido un error en el sistema.");
            }

        }

        public async Task<ResponseDTO> ExistUser(string user)
        {
            try
            {
                using (var db = new AxiaContext())
                {
                    bool existe;
                    if (user.Contains("@"))
                    {
                        existe = await db.TablaUsuarios.AnyAsync(x => x.Email.Equals(user));
                    }
                    else
                    {
                        existe = await db.TablaUsuarios.AnyAsync(x => x.NickName.Equals(user));
                    }
                    if (!existe)
                    {
                        return Responses.SetErrorResponse("El nombre de usuario o la contraseña es incorrecto. El acceso fue denegado.");
                    }
                    return Responses.SetOkResponse("existe");
                }
            }
            catch (Exception ex)
            {
                return Responses.SetInternalServerErrorResponse(ex, "Ha ocurrido un error en el sistema.");
            }
        }
    }
}