// Code from https://blog.bitscry.com/2019/04/12/adding-complex-properties-of-a-tableentity-to-azure-table-storage/

using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[AttributeUsage(AttributeTargets.Property)]
public class EntityJsonPropertyConverterAttribute : Attribute
{
    public EntityJsonPropertyConverterAttribute()
    {
    }
}

public class EntityJsonPropertyConverter
{
    public static void Serialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> results)
    {
        entity.GetType().GetProperties()
            .Where(x => x.GetCustomAttributes(typeof(EntityJsonPropertyConverterAttribute), false).Count() > 0)
            .ToList()
            .ForEach(x => results.Add(x.Name, new EntityProperty(JsonConvert.SerializeObject(x.GetValue(entity)))));
    }

    public static void Deserialize<TEntity>(TEntity entity, IDictionary<string, EntityProperty> properties)
    {
        entity.GetType().GetProperties()
            .Where(x => x.GetCustomAttributes(typeof(EntityJsonPropertyConverterAttribute), false).Count() > 0)
            .ToList()
            .ForEach(x => x.SetValue(entity, JsonConvert.DeserializeObject(properties[x.Name].StringValue, x.PropertyType)));
    }
}