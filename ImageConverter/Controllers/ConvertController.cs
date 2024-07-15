using Aspose.Imaging;
using Aspose.Imaging.ImageOptions;
using Microsoft.AspNetCore.Mvc;

namespace ImageConverter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConvertController : ControllerBase
    {
        [HttpPost]
        public IActionResult ConvertImage(
            IFormFile file,
            [FromForm] string target = "png")
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            using var image = Image.Load(stream);

            return target.ToLower() switch
            {
                "png" => ConvertToPng(image),
                _ => BadRequest("Target format is not supported.")
            };
        }

        private OkObjectResult ConvertToPng(Image image)
        {
            using var pngStream = new MemoryStream();
            var options = new PngOptions();
            image.Save(pngStream, options);
            pngStream.Position = 0;
            return GenerateResponse(pngStream.ToArray());
        }

        private OkObjectResult GenerateResponse(byte[] bytes)
        {
            var base64String = Convert.ToBase64String(bytes);
            return Ok(base64String);
        }
    }
}
