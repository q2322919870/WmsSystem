using Wms.Application.DTOs;
using Wms.Domain.Entities;
using Wms.Domain.Exceptions;
using Wms.Domain.Interfaces;

namespace Wms.Application.Services;

public class InboundOrderService
{
    private readonly IInboundOrderRepository _repository;
    private readonly IMaterialRepository _materialRepository;

    public InboundOrderService(IInboundOrderRepository repository, IMaterialRepository materialRepository)
    {
        _repository = repository;
        _materialRepository = materialRepository;
    }

    public async Task<bool> CreateAsync(CreateInboundOrderDto dto)
    {
        if (await _repository.ExistsByCodeAsync(dto.OrderNo))
            throw new BusinessException($"入库单号{dto.OrderNo}已存在");
        foreach (var detail in dto.Details)
        {
            var material = await _materialRepository.GetByIdAsync(detail.MaterialId);
            if(material is null)
                throw new BusinessException($"物料ID{detail.MaterialId}不存在");
        }
        var order = InboundOrder.Create(dto.OrderNo,dto.SupplierName,dto.ExpectedTime);
        foreach (var detailDto in dto.Details)
        {
            var detail = InboundOrderDetail.Ceeate(detailDto.MaterialId, detailDto.ExpectedQty);
            order.AddDetail(detail);
        }
        await _repository.AddAsync(order);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<List<InboundOrderDto>> GetAllAsync()
    {
        var order = await _repository.GetAllWithDetailsAsync();
        return order.Select(o=>new InboundOrderDto()
        {
            Id = o.Id,
            OrderNo = o.OrderNo,
            SupplierName = o.SupplierName,
            ExpectedTime = o.ExpectedTime,
            Status = o.Status,
            Details = o.Details.Select(d=>new InboundOrderDetailDto()
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name?? "未知物料",
                ExpectedQty = d.ExpectedQty,
                ReceivedQty = d.ReceivedQty,
                LocationId = d.LocationId,
                LocationName = d.Location?.Name?? "未上架"
            }).ToList()
        }).ToList();
    }   
    public async Task<InboundOrderDto?> GetByIdForReceivingAsync(int id)
    {
        var order = await _repository.GetByIdForReceivingAsync(id);
        if (order is null)
            return null;
        return new InboundOrderDto()
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            SupplierName = order.SupplierName,
            ExpectedTime = order.ExpectedTime,
            Status = order.Status,
            Details = order.Details.Select(d=>new InboundOrderDetailDto()
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name?? "未知物料",
                ExpectedQty = d.ExpectedQty,
                ReceivedQty = d.ReceivedQty,
                LocationId = d.LocationId,
                LocationName = d.Location?.Name?? "未上架"
            }).ToList()
        };
    }
    public async Task<InboundOrderDto?> GetBasicByIdAsync(int id)
    {
        var order = await _repository.GetBasicByIdAsync(id);
        if (order is null)
            return null;
        return new InboundOrderDto()
        {
            Id = order.Id,
            OrderNo = order.OrderNo,
            SupplierName = order.SupplierName,
            ExpectedTime = order.ExpectedTime,
            Status = order.Status,
            Details = order.Details.Select(d=>new InboundOrderDetailDto()
            {
                Id = d.Id,
                MaterialId = d.MaterialId,
                MaterialName = d.Material?.Name?? "未知物料",
                ExpectedQty = d.ExpectedQty,
                ReceivedQty = d.ReceivedQty,
                LocationId = d.LocationId,
                LocationName = d.Location?.Name?? "未上架"
            }).ToList()
        };
    }
}