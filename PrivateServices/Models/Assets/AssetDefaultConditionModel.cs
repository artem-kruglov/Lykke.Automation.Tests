// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Represents a default (wildcard) asset condition.
    /// </summary>
    public partial class AssetDefaultConditionModel
    {
        /// <summary>
        /// Initializes a new instance of the AssetDefaultConditionModel class.
        /// </summary>
        public AssetDefaultConditionModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the AssetDefaultConditionModel class.
        /// </summary>
        /// <param name="availableToClient">Indicated that specified asset
        /// available to client.</param>
        /// <param name="isTradable">Indicates that assets is tradable.</param>
        /// <param name="bankCardsDepositEnabled">Indicates that bank cards
        /// deposit enabled for asset.</param>
        /// <param name="swiftDepositEnabled">Indicates that swift deposit
        /// enabled for asset.</param>
        /// <param name="regulation">The regulation for specified
        /// asset.</param>
        public AssetDefaultConditionModel(bool? availableToClient = default(bool?), bool? isTradable = default(bool?), bool? bankCardsDepositEnabled = default(bool?), bool? swiftDepositEnabled = default(bool?), string regulation = default(string))
        {
            AvailableToClient = availableToClient;
            IsTradable = isTradable;
            BankCardsDepositEnabled = bankCardsDepositEnabled;
            SwiftDepositEnabled = swiftDepositEnabled;
            Regulation = regulation;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets indicated that specified asset available to client.
        /// </summary>
        [JsonProperty(PropertyName = "AvailableToClient")]
        public bool? AvailableToClient { get; set; }

        /// <summary>
        /// Gets or sets indicates that assets is tradable.
        /// </summary>
        [JsonProperty(PropertyName = "IsTradable")]
        public bool? IsTradable { get; set; }

        /// <summary>
        /// Gets or sets indicates that bank cards deposit enabled for asset.
        /// </summary>
        [JsonProperty(PropertyName = "BankCardsDepositEnabled")]
        public bool? BankCardsDepositEnabled { get; set; }

        /// <summary>
        /// Gets or sets indicates that swift deposit enabled for asset.
        /// </summary>
        [JsonProperty(PropertyName = "SwiftDepositEnabled")]
        public bool? SwiftDepositEnabled { get; set; }

        /// <summary>
        /// Gets or sets the regulation for specified asset.
        /// </summary>
        [JsonProperty(PropertyName = "Regulation")]
        public string Regulation { get; set; }

    }
}
