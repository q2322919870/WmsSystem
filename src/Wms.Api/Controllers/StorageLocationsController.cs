using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class StorageLocationsController : Controller
{
    private readonly StorageLocationService _service;

    public StorageLocationsController(StorageLocationService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var locations = await _service.GetAllAsync();
        return Ok(ApiResponse.Success(locations,"查询成功"));
    }
    [HttpGet("by-area/{storageAreaId}")]
    public async Task<IActionResult> GetByAreaId(int storageAreaId)
    {
        var locations = await _service.GetByAreaIdAsync(storageAreaId);
        return Ok(ApiResponse.Success(locations,"查询成功"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStorageLocationDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse.Success("创建成功"));
    }
}