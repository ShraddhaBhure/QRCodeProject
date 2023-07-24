using Barcode.Generator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCodeProject.Data;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ZXing;
//using BarcodeGenerator;

namespace QRCodeProject.Controllers
{
    public class BarCodeController : Controller
    {
        private readonly myDbContext _context;

        public BarCodeController(myDbContext context)
        {
            _context = context;
        }

        public IActionResult GenerateBarcode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateBarcode(int productId)
        {
            
            var product = await _context.ProductBarCode.FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
            {
                ModelState.AddModelError(string.Empty, "Product not found with the given ProductId.");
                return View();
            }

            string barcodeData = $"{product.Name} - Price: {product.Price} - Weight: {product.Weight}";

            
            byte[] barcodeBytes = GenerateBarcodeImage(barcodeData);

       
            string base64Image = Convert.ToBase64String(barcodeBytes);

       
            return View("GenerateBarcodeResult", base64Image);
        }
        private byte[] GenerateBarcodeImage(string data)
        {

            BarcodeDraw barcodeDraw = BarcodeDrawFactory.GetSymbology(BarcodeSymbology.Code128);

        
            const int width = 300;
            const int height = 100;

       
           Bitmap barcodeImage = barcodeDraw.Draw(data, width, height);

         
            using (MemoryStream stream = new MemoryStream())
            {
            //    barcodeImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }


     
        public IActionResult Index()
        {
            return View();
        }
    }
}
