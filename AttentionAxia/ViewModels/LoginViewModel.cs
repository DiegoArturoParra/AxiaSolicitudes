using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AttentionAxia.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string EmailOrNickName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}