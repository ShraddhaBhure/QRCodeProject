using Microsoft.EntityFrameworkCore;
using QRCodeProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeProject.Data
{
    public class myDbContext : DbContext
    {

        public myDbContext(DbContextOptions<myDbContext> options)
           : base(options)
        {

        }


        public DbSet<QRCodeData> QRCodeData { get; set; }
        public DbSet<QRStudentMarsheet> QRStudentMarsheet { get; set; }


    }
}
