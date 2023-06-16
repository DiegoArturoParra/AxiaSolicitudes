using AttentionAxia.DTOs;
using log4net;
using System;
using System.IO;
using System.Web;

namespace AttentionAxia.Helpers
{
    public static class FileHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void FolderIsExist(string rutaInicial, string rutaArchivo)
        {
            var rutaCompleta = $@"{rutaInicial}\{rutaArchivo}";
            bool folderExists = Directory.Exists(rutaCompleta);
            if (!folderExists)
            {
                Directory.CreateDirectory(rutaCompleta);
            }
        }

        /// <summary>
        /// guardar un archivo en el servidor
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rutaInicial"></param>
        /// <param name="rutaArchivo"></param>
        /// <param name="nombreArchivo"></param>
        /// <returns></returns>
        public static ResponseDTO SaveFile(HttpPostedFileBase file, string rutaInicial, string rutaArchivo, string nombreArchivo)
        {
            ResponseDTO respuesta = null;
            try
            {
                rutaInicial = !string.IsNullOrWhiteSpace(rutaInicial) ? rutaInicial.Trim() : rutaInicial;
                nombreArchivo = !string.IsNullOrWhiteSpace(rutaArchivo) ? nombreArchivo.Trim() : nombreArchivo;

                if (string.IsNullOrWhiteSpace(rutaInicial))
                {
                    string mensaje = "Error, la ruta no es valida. Por favor contacte al administrador del sistema.";
                    _logger.Error(mensaje);
                    respuesta = Responses.SetErrorResponse(mensaje);
                }

                // valida existencia ruta inicial
                bool rutaInicialExiste = Directory.Exists(rutaInicial);
                if (!rutaInicialExiste)
                {
                    string mensaje = "Error. No se encuentra la ruta principal. Por favor contacte al administrador del sistema.";
                    _logger.Error(mensaje);
                    respuesta = Responses.SetErrorResponse(mensaje);
                }
                // arma la ruta completa
                var rutaCompleta = $@"{rutaInicial}\{rutaArchivo}";
                nombreArchivo = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                // arma la ruta del archivo
                var rutaNombreArchivo = $@"{rutaCompleta}\{nombreArchivo}";

                file.SaveAs(rutaNombreArchivo);

                respuesta = Responses.SetOkResponse($"Se crea el archivo {nombreArchivo}", new FileDTO
                {
                    NombreArchivo = Path.GetFileNameWithoutExtension(file.FileName),
                    PathArchivo = $"{GetConstants.CARPETA_ARCHIVOS_SOLICITUDES}/{nombreArchivo}"
                });
                _logger.Info($"Se crea el archivo {nombreArchivo}");

            }
            catch (Exception ex)
            {
                _logger.Error("Error al guardar el archivo.", ex);
                respuesta = Responses.SetInternalServerErrorResponse(ex, "Error al guardar el archivo.");
            }
            return respuesta;
        }

        /// <summary>
        /// Eliminar un archivo del directorio de archivos
        /// </summary>
        /// <param name="rutaInicial">Ruta inicial directorio de archivos</param>
        /// <param name="rutaArchivo">Ruta interna (Nombre modulo/Nombre Archivo)</param>
        /// <returns></returns>
        public static ResponseDTO DeleteFile(string rutaInicial, string rutaArchivo)
        {
            ResponseDTO respuesta = null;
            try
            {
                rutaInicial = !string.IsNullOrWhiteSpace(rutaInicial) ? rutaInicial.Trim() : rutaInicial;
                rutaArchivo = !string.IsNullOrWhiteSpace(rutaArchivo) ? rutaArchivo.Trim() : rutaArchivo;

                if (string.IsNullOrWhiteSpace(rutaInicial) && string.IsNullOrWhiteSpace(rutaArchivo))
                {
                    respuesta = Responses.SetErrorResponse("Error, la ruta no es valida. Por favor contacte al administrador del sistema.");
                }

                // valida existencia ruta inicial
                bool rutaInicialExiste = Directory.Exists(rutaInicial);
                if (!rutaInicialExiste)
                {
                    respuesta = Responses.SetErrorResponse($"Error. No se encuentra la ruta principal. Por favor contacte al administrador del sistema.");
                    _logger.Error($"Error al eliminar el archivo. No se encuentra la ruta {rutaInicial}");
                }

                // arma la ruta completa
                var rutaCompletaArchivo = $@"{rutaInicial}\{rutaArchivo}";
                bool rutaCompletaArchivoExiste = File.Exists(rutaCompletaArchivo);
                if (!rutaCompletaArchivoExiste)
                {
                    _logger.Error($"Error al eliminar el archivo. No se pudo eliminar el archivo de la ruta {rutaCompletaArchivo}. Ruta o archivo no encontrado.");
                    respuesta = Responses.SetErrorResponse($"Error al eliminar el archivo. No se pudo eliminar el archivo de la ruta {rutaCompletaArchivo}. Ruta o archivo no encontrado.");
                }

                // se elimina
                File.Delete(rutaCompletaArchivo);
                _logger.Info($"Se elimna el archivo {rutaCompletaArchivo}");
                respuesta = Responses.SetOkResponse("Se ha eliminado correctamente el archivo.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error al eliminar el archivo. Por favor contacte al administrador del sistema.", ex);
                respuesta = Responses.SetInternalServerErrorResponse(ex, "Error al eliminar el archivo. Por favor contacte al administrador del sistema.");
            }
            return respuesta;
        }
    }
}