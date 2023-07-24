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
    public class QRCodedbController : Controller
    {
        private readonly myDbContext _dbContext; // Inject the DbContext
        private readonly IWebHostEnvironment _environment;
        public QRCodedbController(myDbContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;
            _environment = environment;
        }
        public IActionResult CreateQRCode()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CreateQRCode(GenerateQRCodeModel generateQRCode)
        {
            try
            {
                GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(generateQRCode.QRCodeText, 200);
              //  barcode.AddBarcodeValueTextBelowBarcode();
                // Styling a QR code and adding annotation text
                barcode.SetMargins(10);
                barcode.ChangeBarCodeColor(Color.BlueViolet);
                string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = $"{Guid.NewGuid()}.png"; // Use a unique file name for the generated QR code
                string filePath = Path.Combine(path, fileName);
                barcode.SaveAsPng(filePath);
                string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
                ViewBag.QrCodeUri = imageUrl;

                // Save QR code data to the database
                QRCodeData qrCodeData = new QRCodeData
                {
                    QRCodeText = generateQRCode.QRCodeText,
                    QRImagePath = fileName,
                    QRGeneratedDate = DateTime.Now,
                    QRIPMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                };

                _dbContext.QRCodeData.Add(qrCodeData);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            return View(generateQRCode);
        }


        //[HttpPost]
        //public IActionResult CreateQRCode(GenerateQRCodeModel generateQRCode)
        //{
        //    try
        //    {
        //        GeneratedBarcode barcode = QRCodeWriter.CreateQrCode(generateQRCode.QRCodeText, 200);
        //        barcode.AddBarcodeValueTextBelowBarcode();
        //        // Styling a QR code and adding annotation text
        //        barcode.SetMargins(10);
        //        barcode.ChangeBarCodeColor(Color.BlueViolet);
        //        string path = Path.Combine(_environment.WebRootPath, "GeneratedQRCode");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        string fileName = $"{Guid.NewGuid()}.png"; // Use a unique file name for the generated QR code
        //        string filePath = Path.Combine(path, fileName);
        //        barcode.SaveAsPng(filePath);
        //        string imageUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}" + "/GeneratedQRCode/" + fileName;
        //        ViewBag.QrCodeUri = imageUrl;

        //        // Save QR code data to the database
        //        QRCodeData qrCodeData = new QRCodeData
        //        {
        //            QRLinkName = generateQRCode.QRCodeText,
        //            QRImagePath = fileName,
        //            QRGeneratedDate = DateTime.Now,
        //            QRIPMachine = Request.HttpContext.Connection.RemoteIpAddress.ToString()
        //        };

        //        _dbContext.QRCodeData.Add(qrCodeData);
        //        _dbContext.SaveChanges();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return View();
        //}

        public IActionResult Index()
        {
            return View();
        }
    }
}
