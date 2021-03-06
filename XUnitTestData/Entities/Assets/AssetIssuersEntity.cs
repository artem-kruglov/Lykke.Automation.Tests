﻿using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entities.Assets
{
    public class AssetIssuersEntity : TableEntity, IAssetIssuers
    {
        public static string GeneratePartitionKey()
        {
            return "Issuer";
        }

        public string Id => RowKey;
        public string Name { get; set; }
        public string IconUrl { get; set; }

    }
}
