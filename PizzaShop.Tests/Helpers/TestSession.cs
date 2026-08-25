using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace PizzaShop.Tests.Helpers
{
    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _data = new();

        public bool IsAvailable => true;

        public string Id => "test session";

        public IEnumerable<string> Keys => _data.Keys;

        public void Clear()
        {
            _data.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _data.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _data[key] = value;
        }

        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
        {
            return _data.TryGetValue(key, out value);
        }
    }
}