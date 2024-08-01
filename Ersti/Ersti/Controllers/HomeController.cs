using Ersti.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;
using QRCoder;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;

namespace Ersti.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ErstiContext _context; 

        public HomeController(ILogger<HomeController> logger, ErstiContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HelperDetails()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult GenerateQRCode(string building, int floor)
        {
            string content = $"Gebäude: {building}, Stock: {floor}";
            using (var qrGenerator = new QRCodeGenerator())
            {
                using (var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q))
                {
                    using (var qrCode = new PngByteQRCode(qrCodeData))
                    {
                        var qrCodeImage = qrCode.GetGraphic(20);
                        var base64Data = Convert.ToBase64String(qrCodeImage);
                        return Json(new { ImageBase64 = $"data:image/png;base64,{base64Data}" });
                    }
                }
            }
        }

        [HttpGet("FindHelper")]
        public IActionResult FindHelper(string building, int floor)
        {
            var helpers = _context.Personens
                .Where(p => p.Gebäude == building && p.Stock == floor && p.Aufgabe.Contains("Ersthelfer"))
                .ToList();

            if (!helpers.Any())
            {
                helpers = _context.Personens
                    .Where(p => p.Gebäude == building && (p.Stock == floor + 1 || p.Stock == floor - 1) && p.Aufgabe.Contains("Ersthelfer"))
                    .ToList();
            }

            if (!helpers.Any())
            {
                helpers = _context.Personens
                    .Where(p => p.Gebäude == building && p.Aufgabe.Contains("Ersthelfer"))
                    .ToList();
            }

            if (!helpers.Any())
            {
                ViewData["Message"] = "Derzeit ist kein/e Ersthelfer/in im Haus";
            }

            return View("HelperDetails", helpers);
        }
    }
}