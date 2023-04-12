using log4net;
using System;
using System.Web.Mvc;

namespace AttentionAxia.Core.Middleware
{
    /// <summary>
    /// Excepcion generica filtro que guarda el log (warning y error)
    /// </summary>
    public class CustomExceptionFilter : IExceptionFilter
    {
        private static readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void OnException(ExceptionContext filterContext)
        {
            _logger.Info($"Controlador: {filterContext.Controller}");

            if (filterContext.ExceptionHandled)
            {
                return;
            }

            Exception exception = filterContext.Exception;

            filterContext.Result = new ViewResult
            {

                ViewName = "~/Views/Error/Index.cshtml",
                ViewData = new ViewDataDictionary(new
                {
                    ErrorMessage = "¡Error Page ha ocurrido un error en el sistema!",
                    Exception = exception
                }),              
        };

       
            _logger.Error(exception.Message, exception);
            filterContext.ExceptionHandled = true;
        }
    }

}