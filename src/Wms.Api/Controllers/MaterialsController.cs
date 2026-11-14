using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.DTOs;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class MaterialsController : Controller
{
    private readonly MaterialService _service;

    public MaterialsController(MaterialService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(ApiResponse.Success(result,"查询成功"));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null)
            return NotFound(ApiResponse.Fail("物料不存在",404));
        return Ok(ApiResponse.Success(result,"查询成功"));
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateMaterialDto dto)
    {
        await _service.CreateAsync(dto);
        return Ok(ApiResponse.Success("创建成功"));
    }
}