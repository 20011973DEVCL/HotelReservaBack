using HotelReservaBack.Application.DTOs;
using HotelReservaBack.Application.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace HotelReservaBack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelesController(IHotelService hotelService) : ControllerBase
{
    private readonly IHotelService _hotelService = hotelService;

    [HttpGet]
    public async Task<IActionResult> ObtenerHoteles()
    {
        List<HotelDto> hoteles = await _hotelService.ObtenerHotelesAsync();
        return Ok(hoteles);
    }
}
