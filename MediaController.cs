using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

//[Authorize]
[Route("api/[controller]")]
[ApiController]
public class MediaController : ControllerBase
{
	private readonly string _storagePath = "D:/BabyPhotoApp/BabyMediaUploads";

	[HttpPost("upload")]
	public async Task<IActionResult> UploadMedia([FromForm] IFormFile file)
	{
		if (file == null || file.Length == 0)
			return BadRequest("No file uploaded.");

		string filePath = Path.Combine(_storagePath, file.FileName);
		using (var stream = new FileStream(filePath, FileMode.Create))
		{
			await file.CopyToAsync(stream);
		}

		return Ok(new { Message = "File uploaded successfully" });
	}
}
