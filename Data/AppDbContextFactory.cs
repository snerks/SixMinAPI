using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SixMinAPI.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        // public AppDbContextFactory(IConfiguration configuration)
        // {
        //     Configuration = configuration;
        // }

        // public IConfiguration Configuration { get; }

        public AppDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<AppDbContext>? optionsBuilder = new();

            // SqlConnectionStringBuilder? sqlConBuilder = new SqlConnectionStringBuilder
            // {
            //     ConnectionString = Configuration.GetConnectionString("SQLDbConnection"),
            //     UserID = Configuration["UserId"],
            //     Password = Configuration["Password"]
            // };

            // // builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(sqlConBuilder.ConnectionString));

            // // optionsBuilder.UseSqlite("Data Source=blog.db");
            _ = optionsBuilder.UseSqlServer("Server=localhost,1433;Initial Catalog=CommandDb;User Id=sa;Password=pa55w0rd!");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}