using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class WarehouseService
{
    private readonly IWarehouseRepository _repository;

    public WarehouseService(IWarehouseRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<bool> CreateWarehouse(CreateWarehouseDto dto)
    {
        if (await _repository.ExistsAsync(dto.Code))
        {
            throw new BusinessException($"仓库编码{dto.Code}已存在");
        }
        var warehouse = Warehouse.Create(dto.Code,dto.Name,dto.Address);
        await _repository.AddAsync(warehouse);
        await _repository.SaveChangesAsync();
        return true;
    }
    
    public async Task<List<WarehouseDto>> GetAllWarehouses()
    {
        var warehouses = await _repository.GetAllAsync();
        return warehouses.Select(w => new WarehouseDto()
        {
            Id = w.Id,
            Code = w.Code,
            Name = w.Name,
            Address = w.Address
        }).ToList();
    }
}