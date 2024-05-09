using Microsoft.AspNetCore.Mvc;
using WebApplication4.Services;

namespace WebApplication4.Controllers;


[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    
    private readonly ProductService _productService;
    
    public ProductController(ProductService productService)
    {
        _productService = productService;
    }
    
    
    
    
    
}