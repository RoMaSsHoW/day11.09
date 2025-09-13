using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace HomeWork.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrCodeController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetHelloWorldQr(int id)
        {
            string url = $"https://localhost:7258/api/Students/{id}";

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            var pngQrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeBytes = pngQrCode.GetGraphic(10);

            return File(qrCodeBytes, "image/png");
        }
    }
}
