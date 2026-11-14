using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class StorageAreasController : Controller
{
    private readonly StorageAreaService _service;
    public StorageAreasController(StorageAreaService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse.Success(result,"查询成功"));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStorageAreaDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return Ok(ApiResponse.Success(result,"创建成功"));
        
    }
}