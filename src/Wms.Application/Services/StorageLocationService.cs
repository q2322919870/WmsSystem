using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class StorageLocationService
{
    private readonly IStorageLocationRepository _repository;
    private readonly IStorageAreaRepository _storageAreaRepository;

    public StorageLocationService(IStorageLocationRepository repository, IStorageAreaRepository storageAreaRepository)
    {
        _repository = repository;
        _storageAreaRepository = storageAreaRepository;
    }

    public async Task<bool> CreateAsync(CreateStorageLocationDto dto)
    {
        var areas = await _storageAreaRepository.GetAllAsync();
        if (areas.All(a=>a.Id != dto.StorageAreaId))
        {
            throw new BusinessException($"库区ID {dto.StorageAreaId} 不存在");
        }

        if (await _repository.ExistsByCodeInAreaAsync(dto.StorageAreaId,dto.Code))
        {
            throw new BusinessException($"库区 {dto.StorageAreaId} 下已存在编码为 {dto.Code} 的库位");
        }
        var location = StorageLocation.Create(dto.StorageAreaId,dto.Code,dto.Name,dto.MaxVolume,dto.MaxWeight);
        await _repository.AddAsync(location);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<StorageLocationDto>> GetAllAsync()
    {
        var locations = await _repository.GetAllAsync();
        var areas = await _storageAreaRepository.GetAllAsync();
        return locations.Select(l =>
        {
            var area = areas.FirstOrDefault(a => a.Id == l.StorageAreaId);
            return new StorageLocationDto()
            {
                Id = l.Id,
                StorageAreaId = l.StorageAreaId,
                StorageName = area?.Name ?? "未知库区",
                Code = l.Code,
                Name = l.Name,
                MaxVolume = l.MaxVolume,
                MaxWeight = l.MaxWeight,
                Status = l.Status.ToString()
            };
        }).ToList();

    }

    public async Task<List<StorageLocationDto>> GetByAreaIdAsync(int storageAreaId)
    {
        var locations = await _repository.GetByAreaIdAsync(storageAreaId);
        var areas = await _storageAreaRepository.GetAllAsync();
        var area = areas.FirstOrDefault(a=>a.Id==storageAreaId);
        return locations.Select(l => new StorageLocationDto()
        {
            Id = l.Id,
            StorageAreaId = l.StorageAreaId,
            StorageName = area?.Name ?? "未知库区",
            Code = l.Code,
            Name = l.Name,
            MaxVolume = l.MaxVolume,
            MaxWeight = l.MaxWeight,
            Status = l.Status.ToString()
        }).ToList();
    }
}