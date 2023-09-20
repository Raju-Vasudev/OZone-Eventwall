using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using OZone.Api.Constants;
using OZone.Api.Domain;
using OZone.Api.Domain.Models;
using OZone.Api.Integrations;
using OZone.Api.Models;
using OZone.Api.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

namespace OZone.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ArtifactsController : ControllerBase
{
    private readonly ILogger<ArtifactsController> _logger;
    public ArtifactsController(ILogger<ArtifactsController> logger)
    {
        _logger = logger;        
    }
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    { try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file selected");
            }
            string folderName = "NewFolder";
            string currentDirectory = Directory.GetCurrentDirectory();
            string newPath = Path.Combine(currentDirectory, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            string fileName = file.FileName;
            string fullPath = Path.Combine(newPath, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }    
            return Ok("File uploaded successfully");  
        }
        catch (System.Exception)
        {            
            _logger.LogError("Error uploading file");
            return BadRequest("Error uploading file");
        }
             
    } 

    [HttpGet("download/{id}")]
    public IActionResult DownloadFiles(string id)
    {        
        try
        {
            string currentDirectory = Directory.GetCurrentDirectory();        
            string folderPath = Path.Combine(currentDirectory, "NewFolder");        
            string[] fileNames = Directory.GetFiles(folderPath);
            string matchingFileName = "";
            foreach (string fileName in fileNames)
            {  
                if(fileName.Contains(id))
                {
                    Console.WriteLine(fileName);
                    matchingFileName =fileName;
                    break;
                }
            }
            if (matchingFileName == null)
            {
                return NotFound();
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(matchingFileName);
            return File(fileBytes, "application/pdf", Path.GetFileName(matchingFileName));
        }
        catch (System.Exception)
        {
            _logger.LogError("Error downloading file"); 
            return BadRequest("Error downloading file");           
        }
    }  
}