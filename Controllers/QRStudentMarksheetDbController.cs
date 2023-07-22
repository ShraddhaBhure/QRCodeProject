using IronBarCode;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCodeProject.Data;
using QRCodeProject.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QRCodeProject.Controllers
{
    public class QRStudentMarksheetDbController : Controller
    {


        private readonly myDbContext _dbContext; // Inject the DbContext
        private readonly IWebHostEnvironment _environment;
        public QRStudentMarksheetDbController(myDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }


        [HttpPost]
        public IActionResult CreateQRCode(GenerateQRCodeModel generateQRCode)
        {
        
            GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(generateQRCode.QRCodeText, 200);
            barcode.AddBarcodeValueTextBelowBarcode();
            // Styling a QR code and adding annotation text
            barcode.SetMargins(10);
            barcode.ChangeBarCodeColor(Color.BlueViolet);
            string path = Path.Combine(_environment.WebRootPath, "GeneratedStudentQRCode");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(_environment.WebRootPath, "GeneratedStudentQRCode");
            barcode.SaveAsPng(filePath);
            string fileName = Path.GetFileName(filePath);
            string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
            ViewBag.QrCodeUri = imageUrl;
            return View();
        }
        public IActionResult GenerateStudentQRCode()
        {
            return View();
        }
        // New action to generate QR code for a specific student based on StudentId
        [HttpPost]
        public IActionResult GenerateStudentQRCode(GenerateQRStudentId generateQRStudentId)
        {
            //try
            //{
                // Retrieve the student's data from the database based on the provided StudentId
                int studentId;
                if (int.TryParse(generateQRStudentId.StudentId.ToString(), out studentId))
                {
                    QRStudentMarsheet student = _dbContext.QRStudentMarsheet.FirstOrDefault(s => s.StudentId == studentId);
                    if (student != null)
                    {
                        // Generate QR code for the student's data
                        string qrData = $"StudentId: {student.StudentId}\nName: {student.FirstName} {student.LastName}\nMath Marks: {student.MathMarks}\nScience Marks: {student.ScienceMarks}\nEnglish Marks: {student.EnglishMarks}\nHistory Marks: {student.HistoryMarks}\nTotal Marks: {student.TotalMarks}\nPercentage: {student.Percentage}";
                        GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(qrData, 200);
                       // barcode.AddBarcodeValueTextBelowBarcode();
                        barcode.SetMargins(10);
                        barcode.ChangeBarCodeColor(Color.BlueViolet);
                        string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string fileName = $"{Guid.NewGuid()}.png";
                        string filePath = Path.Combine(path, fileName);
                        barcode.SaveAsPng(filePath);
                        string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                        ViewBag.QrCodeUri = imageUrl;
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Student not found!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid StudentId format!";
                }
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.ErrorMessage = "Error generating QR code: " + ex.Message;
            //}

            return View(); // Return the same view as the original CreateQRCode action
        }

        //[HttpPost]
        //public IActionResult GenerateStudentQRCode(GenerateQRStudentId generateQRStudentId)
        //{
        //    try
        //    {
        //        int studentId;
        //        if (int.TryParse(generateQRStudentId.StudentId, out studentId))
        //        {
        //            QRStudentMarsheet student = _dbContext.QRStudentMarsheet.FirstOrDefault(s => s.StudentId == studentId);
        //            if (student != null)
        //            {
        //                string qrData = $"StudentId: {student.StudentId}\nName: {student.FirstName} {student.LastName}\nMath Marks: {student.MathMarks}\nScience Marks: {student.ScienceMarks}\nEnglish Marks: {student.EnglishMarks}\nHistory Marks: {student.HistoryMarks}\nTotal Marks: {student.TotalMarks}\nPercentage: {student.Percentage}";
        //                GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(qrData, 200);
        //                barcode.AddBarcodeValueTextBelowBarcode();
        //                barcode.SetMargins(10);
        //                barcode.ChangeBarCodeColor(Color.BlueViolet);
        //                string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
        //                if (!Directory.Exists(path))
        //                {
        //                    Directory.CreateDirectory(path);
        //                }
        //                string fileName = $"{Guid.NewGuid()}.png";
        //                string filePath = Path.Combine(path, fileName);
        //                barcode.SaveAsPng(filePath);
        //                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
        //                ViewBag.QrCodeUri = imageUrl;
        //            }
        //            else
        //            {
        //                ViewBag.ErrorMessage = "Student not found!";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage = "Invalid StudentId format!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = "Error generating QR code: " + ex.Message;
        //    }

        //    return View("CreateQRCode");
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
