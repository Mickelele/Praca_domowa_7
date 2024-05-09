using WebApplication4.Models;

namespace WebApplication4.Repositories;

public class OrderRepository
{
    
    private readonly IConfiguration _configuration;

    public OrderRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
    
}