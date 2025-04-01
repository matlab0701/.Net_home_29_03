using Domain.Entites;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IProductService
{
    Task<Response<List<Product>>> GetAllasync();
    Task<Response<string>> CreateProduct(Product product);
    Task<Response<Product>> GetProduct(int id);
    Task<Response<string>> UpdateProduct(Product product);
    Task<Response<string>> DeleteProduct(int id);
    // Task<Response<string>> Import();
    // Task<Response<string>> Export();
}
