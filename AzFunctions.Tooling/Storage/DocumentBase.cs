using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;

namespace AzFunctions.Tooling.Storage
{
    public class DocumentBase : TableEntity
    {
        public override IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var results = base.WriteEntity(operationContext);
            EntityJsonPropertyConverter.Serialize(this, results);
            return results;
        }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);
            EntityJsonPropertyConverter.Deserialize(this, properties);
        }
    }
}
