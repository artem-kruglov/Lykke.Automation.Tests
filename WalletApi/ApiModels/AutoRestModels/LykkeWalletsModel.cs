// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class LykkeWalletsModel
    {
        /// <summary>
        /// Initializes a new instance of the LykkeWalletsModel class.
        /// </summary>
        public LykkeWalletsModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the LykkeWalletsModel class.
        /// </summary>
        public LykkeWalletsModel(IList<ApiWalletAssetModel> assets = default(IList<ApiWalletAssetModel>), double? equity = default(double?))
        {
            Assets = assets;
            Equity = equity;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Assets")]
        public IList<ApiWalletAssetModel> Assets { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Equity")]
        public double? Equity { get; set; }

    }
}
