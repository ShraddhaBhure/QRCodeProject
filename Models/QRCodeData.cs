using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeProject.Models
{
    public class QRCodeData: GenerateQRCodeModel
    {
        [Key]
        public int QRid { get; set; }
        public string QRCodeText { get; set; }
        public string QRImagePath { get; set; }
        public DateTime QRGeneratedDate { get; set; }
        public string QRIPMachine { get; set; }
    }
}
