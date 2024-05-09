using WebApplication4.Models.DTO_s;
using WebApplication4.Repositories;
namespace WebApplication4.Services;

public class WarehouseService
{
    private WarehouseRepository _warehouseRepository;

    public WarehouseService(WarehouseRepository animalRepository)
    {
        _warehouseRepository = animalRepository;
    }
    
    public async Task<int> UpdateProductWarehouse(UpdateProductInWarehouse updateProductInWarehouse)
    {
        return await _warehouseRepository.UpdateProductWarehouse(updateProductInWarehouse);
    }
    
    
    public async Task<bool> czyIstniejeProdukt(UpdateProductInWarehouse updateProductInWarehouse)
    {
        return await _warehouseRepository.czyIstniejeProdukt(updateProductInWarehouse);
    }
    
    public async Task<bool> czyIstniejeMagazyn(UpdateProductInWarehouse updateProductInWarehouse)
    {
        return await _warehouseRepository.czyIstniejeMagazyn(updateProductInWarehouse);
    }
    
    public async Task<bool> czyIstniejeZamowienie(UpdateProductInWarehouse updateProductInWarehouse)
    {
        return await _warehouseRepository.czyIstniejeZamowienie(updateProductInWarehouse);
    }
    
    
    public async Task<bool> czyZrealizowane(UpdateProductInWarehouse updateProductInWarehouse)
    {
        return await _warehouseRepository.czyZrealizowane(updateProductInWarehouse);
    }

    public async Task updateFillfilledAt(UpdateProductInWarehouse updateProductInWarehouse)
    {
        await _warehouseRepository.updateFillfilledAt(updateProductInWarehouse);
    }
    
    
    public async Task<int> wywolanie_procedury(UpdateProductInWarehouse updateProductInWarehouse)
    {
            return await _warehouseRepository.wywolanie_procedury(updateProductInWarehouse);
            
    }
    

    
    
}