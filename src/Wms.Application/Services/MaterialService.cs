using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class MaterialService
{
    private readonly IMaterialRepository _repository;

    public MaterialService(IMaterialRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateAsync(CreateMaterialDto dto)
    {
        if (await _repository.ExistsByCodeAsync(dto.Code))
        {
            throw new BusinessException($"物料编码 {dto.Code} 已存在");
        }
        var material = Material.Create(dto.Code, dto.Name, dto.Unit, dto.BarCode, dto.Category, dto.ShelfLifeDays);
        await _repository.AddAsync(material);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<MaterialDto>> GetAllAsync()
    {
        var materials = await _repository.GetAllAsync();
        return materials.Select(m => new MaterialDto
        {
            Id = m.Id,
            Code = m.Code,
            Name = m.Name,
            Unit = m.Unit,
            BarCode = m.BarCode,
            Category = m.Category,
            ShelfLifeDays = m.ShelfLifeDays
        }).ToList();
    }

    public async Task<MaterialDto?> GetByIdAsync(int id)
    {
        var material = await _repository.GetByIdAsync(id);
        if (material is null)
            return null;
        return new MaterialDto()
        {
            Id = material.Id,
            Code = material.Code,
            Name = material.Name,
            Unit = material.Unit,
            BarCode = material.BarCode,
            Category = material.Category,
            ShelfLifeDays = material.ShelfLifeDays
        };
    }
}