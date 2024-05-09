using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using WebApplication4.Models;

namespace WebApplication4.Repositories;

public class ProductWarehouseRepository
{
    private string _connectionString;

    public ProductWarehouseRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }




    public async Task<int> WstawRekord(ProductWarehouse product)
    {

        string query = "INSERT INTO Product_Warehouse VALUES (@IdWarehouse, @IdProduct, @OrderId, @Amount, @Price, GETDATE());SELECT SCOPE_IDENTITY();";
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdProduct", product.idProduct);
        command.Parameters.AddWithValue("@IdWarehouse", product.idWarehouse);
        command.Parameters.AddWithValue("@Amount", product.amount);
        command.Parameters.AddWithValue("@OrderId", await returnOrderId(product));
        command.Parameters.AddWithValue("@Price", await returnPrice(product));

        await connection.OpenAsync();
        int index = Convert.ToInt32(await command.ExecuteScalarAsync());

        return index;
    }

    public async Task<int> returnOrderId(ProductWarehouse product)
    {
        string query = "SELECT [Order].IdOrder FROM [Order] WHERE [Order].IdProduct = @IdProduct";
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdProduct", product.idProduct);
        
        await connection.OpenAsync();
        
        return (int)await command.ExecuteScalarAsync();
    }
    
    public async Task<decimal> returnPrice(ProductWarehouse product)
    {
        string query = "SELECT Product.Price FROM Product WHERE Product.IdProduct = @IdProduct";
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdProduct", product.idProduct);
        
        await connection.OpenAsync();
        
        return ((decimal)await command.ExecuteScalarAsync() * product.amount);
    }
    
    
    
    
    

    public async Task<bool> czyProduktIstnieje(ProductWarehouse product)
    {
        if (!(product.amount > 0))
        {
            return false;
        }

        string query = "SELECT * FROM Product WHERE Product.IdProduct = @IdProduct;";
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@IdProduct", product.idProduct);
        command.Parameters.AddWithValue("@IdWarehouse", product.idWarehouse);
        
        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            return false;
        }

        return true;
    }


    public async Task<bool> czyIstniejeZamowienie(ProductWarehouse product)
    {

        string query = "SELECT COUNT(*) FROM [Order] WHERE [Order].IdProduct = @IdProduktu and [Order].Amount = @Amount and [Order].CreatedAt < @Data";

        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@IdProduktu", product.idProduct);
            command.Parameters.AddWithValue("@Amount", product.amount);
            command.Parameters.AddWithValue("@Data", product.createdAt);


            await connection.OpenAsync();
            
            var licznik = (int)await command.ExecuteScalarAsync();

            if (licznik == 0)
            {
                return false;
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        
    }



    public async Task<bool> czyZrealizowane(ProductWarehouse product)
    {
        string query = "SELECT COUNT(*) FROM Product_Warehouse WHERE Product_Warehouse.IdOrder = (SELECT [Order].IdOrder FROM [Order] WHERE [Order].IdProduct = @ProductId)";
        
        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@ProductId", product.idProduct);
        command.Parameters.AddWithValue("@Amount", product.idProduct);
        
        
        await connection.OpenAsync();
            
        var licznik = (int)await command.ExecuteScalarAsync();

        if (licznik == 0)
        {
            return false;
        }

        return true;
    }


    public async Task AktualizujFullFilledAt(ProductWarehouse product)
    {

        string query = "UPDATE [Order] SET [Order].FulfilledAt = GETDATE() WHERE [Order].IdProduct = @ProductId";


        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@ProductId", product.idProduct);
        
        await connection.OpenAsync();

        command.ExecuteNonQuery();

    }
    
    
    

    
    
    
}