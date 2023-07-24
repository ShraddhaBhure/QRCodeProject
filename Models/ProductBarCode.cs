using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeProject.Models
{
    public class ProductBarCode
    {
        [Key]
        public  int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Weight { get; set; }
        public string Manufacturernname { get; set; }
        public DateTime Manufacturedate { get; set; }
        public DateTime Expiredate { get; set; }
    }
}
