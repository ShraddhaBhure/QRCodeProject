using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeProject.Models
{
    public class GenerateQRStudentId
    {
        [Display(Name = "Enter Student Id")]
        public int StudentId
        {
            get;
            set;
        }
    }
}
