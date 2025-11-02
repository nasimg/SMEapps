// after var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7206/"); // or read from config
});