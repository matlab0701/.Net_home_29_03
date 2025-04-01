using Domain.Entites;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Servers;
using Microsoft.AspNetCore.Mvc;
using Npgsql.Replication;

namespace WepApi.Controllers;

public class ProductController(IProductService _productService)
{
   
    [HttpGet]
    public async Task<Response<List<Product>>> GetAllasync()
    {
        var products = _productService.GetAllasync();
        return await products;
    }
    [HttpGet("{id:int}")]
    public async Task<Response<Product>> GetProduct(int id)
    {
        var product = _productService.GetProduct(id);
        return await product;
    }
    [HttpPut]
    public async Task<Response<string>> UpdateProduct(Product product)
    {
        var res = _productService.UpdateProduct(product);
        return await res;
    }
    [HttpPost]
    public async Task<Response<string>> CreateProduct(Product product)
    {
        var res = _productService.CreateProduct(product);
        return await res;
    }
    [HttpDelete]
    public async Task<Response<string>> DeleteProduct(int id)
    {
        var res = _productService.DeleteProduct(id);
        return await res;
    }
    



}

