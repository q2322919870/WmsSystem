using Wms.Domain.Exceptions;

namespace Wms.Domain.Entities;

public class OutboundOrderDetail
{
    protected OutboundOrderDetail(){}

    private OutboundOrderDetail(int materialId, int requiredQty)
    {
        if (materialId<=0)
            throw new BusinessException("物料ID必须大于0");
        if (requiredQty <= 0)
            throw new BusinessException("需求数量必须大于0");
        MaterialId = materialId;
        RequiredQty = requiredQty;
        AllocatedQty = 0;
        PickedQty = 0;
        Status = "待分配";
    }

    public static OutboundOrderDetail Create(int materialId, int requiredQty)
        => new OutboundOrderDetail(materialId, requiredQty);
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int MaterialId { get; private set; }
    public int RequiredQty { get; private set; }
    public int AllocatedQty { get; private set; }
    public int PickedQty { get; private set; }
    public int? LocationId { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }

    public OutboundOrder Order { get; private set; } = null!;
    public Material Material { get; private set; } = null!;
    public StorageLocation? Location { get; private set; }

    public void Allocate(int locationId, int allocatedQty)
    {
        if(locationId <= 0)
            throw new BusinessException("库位id必须大于0");
        if(allocatedQty <= 0)
            throw new BusinessException("分配数量必须大于0");
        if(allocatedQty>RequiredQty)
            throw new BusinessException($"分配数量{allocatedQty}不能超过需求数量{RequiredQty}");

        AllocatedQty = allocatedQty;
        LocationId = locationId;
        Status = "已分配";
    }

    public void Pick(int pickedQty)
    {
        
        if(pickedQty <= 0)
            throw new BusinessException("拣货数量必须大于0");
        if(pickedQty != AllocatedQty)
            throw new BusinessException($"拣货数量{pickedQty}必须等于分配数量{AllocatedQty}");

        PickedQty = pickedQty;
        Status = "已拣货";
    }

    public void Check()
    {
        if(PickedQty <= 0)
            throw new BusinessException("必须先拣货才能进行复核");
        Status = "已复核";
    }

    public void Ship()
    {
        if(Status is not "已复核")
            throw new BusinessException("必须先复核才能进行发货");
        Status = "已发货";
    }
    public void SoftDelete()=> IsDeleted = true;
}