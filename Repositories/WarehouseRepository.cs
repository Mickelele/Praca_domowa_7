using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Context;
using WebApplication4.Models;
using WebApplication4.Models.DTO_s;

namespace WebApplication4.Repositories;

public class WarehouseRepository
{
    
    
    private readonly IConfiguration _configuration;
    private readonly TestContext _context;

    public WarehouseRepository(IConfiguration configuration, TestContext context)
    {
        _configuration = configuration;
        _context = context;
    }


    public async Task<int> UpdateProductWarehouse(UpdateProductInWarehouse updateProductInWarehouse)
    {
        var cenaProduktu = await _context.Products
            .Where(p => p.IdProduct == updateProductInWarehouse.idProduct)
            .Select(p => p.Price)
            .FirstOrDefaultAsync();

        cenaProduktu *= updateProductInWarehouse.amount;

        var id_order = await _context.Orders
            .Where(o => o.IdProduct == updateProductInWarehouse.idProduct)
            .Select(o => o.IdOrder)
            .FirstOrDefaultAsync();
        
        var productWarehouse = new ProductWarehouse()
        {
            IdWarehouse = updateProductInWarehouse.idWarehouse,
            IdProduct = updateProductInWarehouse.idProduct,
            IdOrder = id_order,
            Amount = updateProductInWarehouse.amount,
            Price = cenaProduktu,
            CreatedAt = DateTime.Now
        };
        
        await _context.ProductWarehouses.AddAsync(productWarehouse);
        await _context.SaveChangesAsync();

        int numer = await _context.ProductWarehouses
            .Where(p => p.IdProduct == productWarehouse.IdProduct && p.IdWarehouse == productWarehouse.IdWarehouse &&
                        p.IdOrder == productWarehouse.IdOrder)
            .Select(p => p.IdProductWarehouse)
            .FirstOrDefaultAsync();
        
        return numer;
    }


    public async Task<bool> czyIstniejeProdukt(UpdateProductInWarehouse updateProductInWarehouse)
    {
        var produkt = await _context.Products
            .Where(p => p.IdProduct == updateProductInWarehouse.idProduct)
            .FirstOrDefaultAsync();
            return produkt != null;
    }
    
    public async Task<bool> czyIstniejeMagazyn(UpdateProductInWarehouse updateProductInWarehouse)
    {
        var magazyn = await _context.Warehouses
            .Where(m => m.IdWarehouse == updateProductInWarehouse.idWarehouse)
            .FirstOrDefaultAsync();
        return magazyn != null;
    }
    
    public async Task<bool> czyIstniejeZamowienie(UpdateProductInWarehouse updateProductInWarehouse)
    {
        var orderExists = await _context.Orders
            .AnyAsync(o => o.IdProduct == updateProductInWarehouse.idProduct
                      && o.Amount == updateProductInWarehouse.amount
                      && o.CreatedAt < updateProductInWarehouse.createdAt);
        return orderExists;
    }
    
    
    public async Task<bool> czyZrealizowane(UpdateProductInWarehouse updateProductInWarehouse)
    {
        var order = await _context.Orders
            .Where(o => o.IdProduct == updateProductInWarehouse.idProduct)
            .FirstOrDefaultAsync();

        var czy_zrealizowane = await _context.ProductWarehouses
            .Where(pw => pw.IdOrder == order.IdOrder)
            .FirstOrDefaultAsync();

        return czy_zrealizowane == null;
    }

    public async Task updateFillfilledAt(UpdateProductInWarehouse updateProductInWarehouse)
    {
        var orderID = await _context.Orders
            .Where(o => o.IdProduct == updateProductInWarehouse.idProduct)
            .Select(o => o.IdOrder)
            .FirstOrDefaultAsync();
        
        var orderToUpdate = await _context.Orders
            .Where(o => o.IdOrder == orderID)
            .FirstOrDefaultAsync();

        if (orderToUpdate != null)
        { 
            orderToUpdate.FulfilledAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }
        
    }


    public async Task<int> wywolanie_procedury(UpdateProductInWarehouse updateProductInWarehouse)
    {
        SqlParameter newIdParameter = new SqlParameter("@NewId", SqlDbType.Int);
        newIdParameter.Direction = ParameterDirection.Output;

        SqlParameter[] parameters = new[]
        {
            new SqlParameter("@IdProduct", SqlDbType.Int) { Value = updateProductInWarehouse.idProduct },
            new SqlParameter("@IdWarehouse", SqlDbType.Int) { Value = updateProductInWarehouse.idWarehouse },
            new SqlParameter("@Amount", SqlDbType.Int) { Value = updateProductInWarehouse.amount },
            new SqlParameter("@CreatedAt", SqlDbType.DateTime) { Value = updateProductInWarehouse.createdAt },
            newIdParameter
        };

        var result = await _context.Database.ExecuteSqlRawAsync("EXEC AddProductToWarehouse @IdProduct, @IdWarehouse, @Amount, @CreatedAt, @NewId OUTPUT", parameters);

        return (int)newIdParameter.Value;
    }
    
    


}