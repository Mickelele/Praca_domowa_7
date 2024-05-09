using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Repositories;

namespace WebApplication4.Controllers;

[ApiController]
[Route("api/warehouses")]
public class ProductWarehouseController : ControllerBase
{

    private ProductWarehouseRepository _productWarehouseRepository;

    public ProductWarehouseController(ProductWarehouseRepository productWarehouseRepository)
    {
        _productWarehouseRepository = productWarehouseRepository;
    }
    

    [HttpPost]
    public async Task<IActionResult> AddProductToProduct_Warehouse([FromBody]ProductWarehouse product)
    {
        if (!await _productWarehouseRepository.czyProduktIstnieje(product)) return NotFound("Produkt nieistnieje.");
        if (!await _productWarehouseRepository.czyIstniejeZamowienie(product)) return NotFound("Zamowienie nieistnieje.");
        if (await _productWarehouseRepository.czyZrealizowane(product)) return NotFound("Zamowienie zostalo juz zrealizowane");
        await _productWarehouseRepository.AktualizujFullFilledAt(product);

        var index = await _productWarehouseRepository.WstawRekord(product);
        
        return Ok(index);
    }

    
    
    
}