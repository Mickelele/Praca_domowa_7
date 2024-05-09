namespace WebApplication4.Models;

public class ProductWarehouse
{
    public int idProduct { get; set; }
    public int idWarehouse { get; set; }
    public int amount { get; set; }
    public DateTime createdAt { get; set; }
}