using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Interfaces;
using Wms.Domain.Exceptions;

namespace Wms.Application.Services;

public class StorageAreaService
{
    private readonly IStorageAreaRepository _repository;
    private readonly IWarehouseRepository _warehouseRepository;

    public StorageAreaService(IStorageAreaRepository repository, IWarehouseRepository warehouseRepository)
    {
        _repository = repository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<bool> CreateAsync(CreateStorageAreaDto dto)
    {
        var warehouse = await _warehouseRepository.GetAllAsync();
        if (warehouse.All(w => w.Id != dto.WarehouseId))
        {
            throw new BusinessException($"仓库Id{dto.WarehouseId}不存在");
        }

        if (await _repository.ExistsByCodeInWarehouseAsync(dto.WarehouseId, dto.Code))
        {
            throw new BusinessException($"仓库Id{dto.WarehouseId}下，存储区域编码{dto.Code}已存在");
        }

        var storageArea = StorageArea.Create(dto.WarehouseId,dto.Code,dto.Name,dto.Type);
        await _repository.AddAsync(storageArea);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<StorageAreaDto>> GetAllAsync()
    {
        var areas = await _repository.GetAllAsync();
        var warehouses = await _warehouseRepository.GetAllAsync();
        return areas.Select(a =>
        {
            var warehouse = warehouses.FirstOrDefault(w => w.Id == a.WarehouseId);
            return new StorageAreaDto
            {
                Id = a.Id,
                WarehouseId = a.WarehouseId,
                WarehouseName = warehouse?.Name ?? "未知仓库",
                Code = a.Code,
                Name = a.Name,
                Type = a.Type
            };
        }).ToList();
    }

    public async Task<List<StorageAreaDto>> GetByWarehouseIdAsync(int warehouseId)
    {
        var area = await _repository.GetByWarehouseIdAsync(warehouseId);
        var warehouses = await _warehouseRepository.GetAllAsync();
        var warehouse = warehouses.FirstOrDefault(w=>w.Id==warehouseId);
        return area.Select(a => new StorageAreaDto()
        {
            Id = a.Id,
            WarehouseId = a.WarehouseId,
            WarehouseName = warehouse?.Name ?? "未知仓库",
            Code = a.Code,
            Name = a.Name,
            Type = a.Type
        }).ToList();
    }
}