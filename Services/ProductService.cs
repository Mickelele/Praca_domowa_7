using WebApplication4.Repositories;

namespace WebApplication4.Services;

public class ProductService
{

    private ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    
    


}