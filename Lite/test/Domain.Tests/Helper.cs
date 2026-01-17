using System.Reflection;
using Domain.Entity;
using Domain.Tests.AlbumEntity;

namespace Domain.Tests;

internal static class RandomHelper
{
    extension(Random random)
    {
        public string Chars(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            Span<char> span = stackalloc char[length];
            for (int i = 0; i < length; i++)
            {
                span[i] = chars[random.Next(chars.Length)];
            }
            return new string(span);
        }

        public string Chars(int minLength, int maxLength)
        {
            int length = random.Next(minLength, maxLength + 1);
            return random.Chars(length);
        }
    }

    extension<T>(IEnumerable<T> values)
    {
        public T Random => values.ElementAt(new Random().Next(0, values.Count()));
    }
}

public static class AccessHelper
{
    extension<TObject>(TObject obj)
    {
        public TValue GetValue<TValue>()
        {
            var field = typeof(TObject)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(f => f.FieldType == typeof(TValue));

            Assert.IsNotNull(field);

            object? value = field.GetValue(obj);

            Assert.IsNotNull(value);

            return (TValue)value;
        }

        public void SetValue<TValue>(TValue value)
        {
            var field = typeof(TObject)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(f => f.FieldType == typeof(TValue));

            Assert.IsNotNull(field);

            field.SetValue(obj, value);
        }

        public void SetValue<TValue>(string fieldName, TValue value)
        {
            var field = typeof(TObject)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .First(f =>
                    f.FieldType == typeof(TValue)
                    && f.Name.Contains(fieldName, StringComparison.OrdinalIgnoreCase)
                );

            Assert.IsNotNull(field);

            field.SetValue(obj, value);
        }
    }
}

public static class EntityHelper
{
    extension<TEntity>(TEntity entity)
        where TEntity : IBaseEntity
    {
        public void SetId<TId>(TId id)
            where TId : IBaseTypedId, IEquatable<TId>
        {
            typeof(EntityBase<TId>)
                .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(f => f.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
                .SetValue(entity, id);
        }
    }
}
