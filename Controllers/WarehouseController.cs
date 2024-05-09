using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Context;
using WebApplication4.Models.DTO_s;
using WebApplication4.Repositories;
using WebApplication4.Services;

namespace WebApplication4.Controllers;


[ApiController]
[Route("api/warehouse")]
public class WarehouseController : ControllerBase
{

    private readonly WarehouseService _warehouseService;

    public WarehouseController(WarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }



    [HttpPost("AddProductToWarehouse")]
    public async Task<IActionResult> UpdateProductWarehouse([FromBody] UpdateProductInWarehouse updateProductInWarehouse)
    {

        if (await _warehouseService.czyIstniejeProdukt(updateProductInWarehouse) == false)
        {
            return NotFound($"Podany produkt nie istnieje.");
        }

        if (await _warehouseService.czyIstniejeMagazyn(updateProductInWarehouse) == false)
        {
            return NotFound($"Podany magazyn nie istnieje.");
        }
        
        
        if (await _warehouseService.czyIstniejeZamowienie(updateProductInWarehouse) == false)
        {
            return NotFound($"Podane zamowienie nie istnieje lub wprowadzona data jest niepoprawna.");
        }


        if (await _warehouseService.czyZrealizowane(updateProductInWarehouse) == false)
        {
            return NotFound($"Zamowienie zostalo juz zrealizowane.");
        }


        await _warehouseService.updateFillfilledAt(updateProductInWarehouse);

        var numer = await _warehouseService.UpdateProductWarehouse(updateProductInWarehouse);


        return Ok(numer);
    }

    
    [HttpPost("Procedura")]
    public async Task<IActionResult> Prodecura([FromBody] UpdateProductInWarehouse updateProductInWarehouse)
    {
        var newProductWarehouseId = await _warehouseService.wywolanie_procedury(updateProductInWarehouse);
        return Ok(newProductWarehouseId);
    }
    


}