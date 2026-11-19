namespace Wms.Application.DTOs;

public class CreateStocktakeOrderDto
{
    public string OrderNo { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string OperatorName { get; set; } = string.Empty;
    public List<StocktakeItemDto> Items { get; set; } = new();
}

public class StocktakeItemDto
{
    public int LocationId { get; set; }
    public int MaterialId { get; set; }
    public int ActualQty { get; set; } 
}

public class StocktakeOrderDto
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<StocktakeOrderDetailDto> Details { get; set; } = new();
}

public class StocktakeOrderDetailDto
{
    public string LocationCode { get; set; } = string.Empty;
    public string MaterialName { get; set; } = string.Empty;
    public int SystemQty { get; set; }
    public int ActualQty { get; set; }
    public int DifferenceQty { get; set; }
}
public class SubmitStocktakeOrderDto
{
    public string ReviewerName { get; set; } = string.Empty;
}