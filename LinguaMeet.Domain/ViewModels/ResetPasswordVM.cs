using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinguaMeet.Domain.ViewModels
{
    public  class ResetPasswordVM
    {
        [Required]
        [EmailAddress]
        public string EmailAdresse { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
