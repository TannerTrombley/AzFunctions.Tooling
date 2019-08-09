using AzFunctions.Tooling.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzFnAuthTest
{
    public class User : DocumentBase
    {
        public User(string id, string partitionKey)
        {
            RowKey = id;
            PartitionKey = partitionKey;
        }

        public string Name { get; set; }

        [EntityJsonPropertyConverter]
        public Complex C { get; set; }
    }
}
