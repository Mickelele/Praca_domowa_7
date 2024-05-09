using System.Runtime.InteropServices.JavaScript;
using WebApplication4.Services;

namespace WebApplication4.Models.DTO_s;

public class UpdateProductInWarehouse
{
    public int idProduct { get; set; }
    public int idWarehouse { get; set; }
    public int amount { get; set; }
    public DateTime createdAt { get; set; }

}