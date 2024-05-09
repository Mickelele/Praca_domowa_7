using Azure.Core;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Context;
using WebApplication4.Models;

namespace WebApplication4.Repositories
{
    public class ProductRepository
    {
        private readonly IConfiguration _configuration;

        public ProductRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        


    }
}