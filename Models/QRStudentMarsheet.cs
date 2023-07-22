using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeProject.Models
{
    public class QRStudentMarsheet: GenerateQRStudentId
    {
      [Key]
            public int StudentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int MathMarks { get; set; }
            public int ScienceMarks { get; set; }
            public int EnglishMarks { get; set; }
            public int HistoryMarks { get; set; }
            public int TotalMarks { get; set; }
            public double Percentage { get; set; }
        
    }
}
