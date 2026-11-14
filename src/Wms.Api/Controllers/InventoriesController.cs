using Microsoft.AspNetCore.Mvc;
using Wms.Api.Common;
using Wms.Application.Services;

namespace Wms.Api.Controllers;
[ApiController]
[Route("api/[controller]/[action]")]
public class InventoriesController : Controller
{
    private readonly InventoryService _service;

    public InventoriesController(InventoryService service)=> _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()=>Ok(ApiResponse.Success(await _service.GetAllAsync(),"查询成功"));
    
    [HttpGet("Transactions/{referenceNo}")]
    public async Task<IActionResult> GetTransactions(string referenceNo)
        =>Ok(ApiResponse.Success(await _service.GetTransactionsAsync(referenceNo),"查询成功"));
}