using System.Net;
using Dapper;
using Domain.Entites;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces;

namespace Infrastructure.Servers;

public class ProductService : IProductService
{
    private readonly DataContext _context = new DataContext();

    public async Task<Response<List<Product>>> GetAllasync()
    {
        using var connection = await _context.GetConnection();
        var sql = @"SELECT * FROM Products";
        var products = await connection.QueryAsync<Product>(sql);
        return new Response<List<Product>>(products.ToList());

    }
    public async Task<Response<Product>> GetProduct(int id)
    {
        using var connection = await _context.GetConnection();
        var sql = @"SELECT * FROM Products where Id=@Id";
        var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
        return product == null
       ? new Response<Product>(HttpStatusCode.BadRequest, "Something went wrong")
       : new Response<Product>(product);

    }

    public async Task<Response<string>> UpdateProduct(Product product)
    {
        using var connection = await _context.GetConnection();
        var sql = @"update from Products set Name=@,Description=@Description,Price=@Price,StockQuantity=@StockQuantity,CategoryName=@CategoryName,CreatedDate=@CreatedDate where id=@id";
        var res = await connection.ExecuteAsync(sql, product);
        return res == 0
       ? new Response<string>(HttpStatusCode.BadRequest, "Id not found to Update")
       : new Response<string>("Succesfuly to Update");

    }
    public async Task<Response<string>> CreateProduct(Product product)
    {
        using var connection = await _context.GetConnection();
        var sql = @"insert into Products (Name,Description,Price,StockQuantity,CategoryName,CreatedDate)
        values (@Name,@Description,@Price,@StockQuantity,@CategoryName,@CreatedDate) ";
        var res = await connection.ExecuteAsync(sql, product);
        return res == 0
       ? new Response<string>(HttpStatusCode.BadRequest, "Id not found to Create")
       : new Response<string>("Succesfuly to Create");
    }
    public async Task<Response<string>> DeleteProduct(int id)
    {
        using var connection = await _context.GetConnection();
        var sql = @"delete from Products where id =@id ";
        var res = await connection.ExecuteAsync(sql, new { Id = id });
        return res == 0
       ? new Response<string>(HttpStatusCode.BadRequest, "Id not found to Delete")
       : new Response<string>("Succesfuly to Delete");
    }
    public async Task<Response<string>> Idit()
    {
        const string path = "C:\\Users\\HP\\Desktop\\C# Course\\add.txt";
        const string cmd = @"update Products set Name=@Name,Description=@Description,Price=@Price,StockQuantity=@StockQuantity,CategoryName=@CategoryName,CreatedDate=@CreatedDate";

        if (File.Exists(path) == false)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "File does not exist");
        }

        var lines = await File.ReadAllLinesAsync(path);

        using var connection = await _context.GetConnection();
        var counter = 0;
        foreach (var line in lines)
        {
            var values = line.Split(',');
            var product = new Product
            {
                Id = Convert.ToInt32(values[0]),
                Name = values[1],
                Description = values[2],
                Price = Convert.ToDecimal(values[3]),
                StockQuantity = Convert.ToInt32(values[4]),
                CategoryName = values[5],
                CreatedDate = Convert.ToDateTime(values[6])
            };

            var result = await connection.ExecuteAsync(cmd, product);

            if (result == 1)
            {
                counter++;
            }
        }

        return new Response<string>($"Products imported successfully {counter} records");
    }

    public async Task<Response<string>> Export()
    {
        const string path = "C:\\Users\\HP\\Desktop\\C# Course\\export.txt";
        const string cmd = @"select * from Products";

        var list = new List<string>();

        if (File.Exists(path) == false)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "File does not exist");
        }

        var connection = await _context.GetConnection();
        var products = await connection.QueryAsync<Product>(cmd);

        foreach (var product in products.ToList())
        {
            var text = $"{product.Id}.{product.Name} - {product.Price} - {product.StockQuantity} - {product.CategoryName} - {product.Description} - {product.CreatedDate}";
            list.Add(text);
        }

        await File.WriteAllLinesAsync(path, list);

        return new Response<string>("product exported successfully");
    }

    public async Task<Response<string>> Import()
    {
        const string path = "C:\\Users\\HP\\Desktop\\C# Course\\add.txt";
        const string cmd = @"insert into Products (Name,Description,Price,StockQuantity,CategoryName,CreatedDate)
        values (@Name,@Description,@Price,@StockQuantity,@CategoryName,@CreatedDate)";

        if (File.Exists(path) == false)
        {
            return new Response<string>(HttpStatusCode.BadRequest, "File does not exist");
        }

        var lines = await File.ReadAllLinesAsync(path);

        using var connection = await _context.GetConnection();
        var counter = 0;
        foreach (var line in lines)
        {
            var values = line.Split(',');
            var product = new Product
            {
                Id = Convert.ToInt32(values[0]),
                Name = values[1],
                Description = values[2],
                Price = Convert.ToDecimal(values[3]),
                StockQuantity = Convert.ToInt32(values[4]),
                CategoryName = values[5],
                CreatedDate = Convert.ToDateTime(values[6])
            };

            var result = await connection.ExecuteAsync(cmd, product);

            if (result == 1)
            {
                counter++;
            }
        }

        return new Response<string>($"Product imported successfully {counter} records");
    }



}
