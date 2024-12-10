using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NEWPROJECT.Models;
using NEWPROJECT.Services;
using NEWPROJECT.Interfaces;
namespace NEWPROJECT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandBagsController : ControllerBase
{
    private IBrandBagsService BrandBagsService;
    private readonly ILogger<BrandBagsController> _logger;

    public BrandBagsController(IBrandBagsService BrandBagsService)
    {
        this.BrandBagsService = BrandBagsService;
    }



    [HttpGet]
    public ActionResult<List<BrandBags>> Get([FromQuery] int userId)
    {
        try
        {
            Console.WriteLine($"Fetching bags for User ID: {userId}");

            var bags = BrandBagsService.GetAll(userId);

            if (bags == null || bags.Count == 0)
            {
                Console.WriteLine($"No bags found for User ID: {userId}");
                return NotFound("No bags found.");
            }

            Console.WriteLine($"Fetched {bags.Count} bags for User ID: {userId}");
            return bags;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching bags: {ex.Message}");
            return StatusCode(500, "An error occurred.");
        }
    }



    [HttpPost]
    public ActionResult Post(BrandBags newItem)
    {
        BrandBagsService.Add(newItem);
        return NoContent();
    }




    [HttpPut("{id}")]
    public ActionResult Put(int id, BrandBags bags)
    {
        var result = BrandBagsService.Update(id, bags);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }




    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingItem = BrandBagsService.Get(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        BrandBagsService.Delete(id);
        return NoContent();
    }

}
