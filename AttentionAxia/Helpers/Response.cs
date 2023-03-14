using System;

namespace AttentionAxia.Helpers
{
    public class ResponseDTO
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Data { get; set; }
    }
    public static class Responses
    {
        public static ResponseDTO SetCreateResponse()
        {
            return new ResponseDTO { Message = "Se ha creado satisfactoriamente.", IsSuccess = true };
        }
        public static ResponseDTO SetOkResponse(string message, object data = null)
        {
            return new ResponseDTO { Message = message, IsSuccess = true, Data = data };
        }
        public static ResponseDTO SetErrorResponse(string message)
        {
            return new ResponseDTO { Message = message, IsSuccess = false };
        }
        public static ResponseDTO SetInternalServerErrorResponse(Exception ex, string message)
        {
            return new ResponseDTO { Message = message, IsSuccess = false };
        }
    }

}