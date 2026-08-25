using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Data;

namespace PizzaShop.Tests.Helpers
{
    public class TestDatabase : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        public TestDatabase(
            SqliteConnection connection,
            PizzaShopDbContext context)
        {
            _connection = connection;
            Context = context;
        }

        public PizzaShopDbContext Context { get; }

        public static async Task<TestDatabase> CreateContextAsync()
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<PizzaShopDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new PizzaShopDbContext(options);

            await context.Database.EnsureCreatedAsync();

            return new TestDatabase(connection, context);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
