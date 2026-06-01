using HotelReservaBack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelesController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelesController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerHoteles()
    {
        var hoteles = await _hotelService.ObtenerHotelesAsync();
        return Ok(hoteles);
    }
}