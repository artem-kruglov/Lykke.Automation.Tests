// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class StartCashOutRequestModel
    {
        /// <summary>
        /// Initializes a new instance of the StartCashOutRequestModel class.
        /// </summary>
        public StartCashOutRequestModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the StartCashOutRequestModel class.
        /// </summary>
        public StartCashOutRequestModel(string offchainTransferId, string clientId, string address, double amount, string assetId, string countryId = default(string), string additionalData = default(string), string blockchainHash = default(string))
        {
            OffchainTransferId = offchainTransferId;
            ClientId = clientId;
            Address = address;
            Amount = amount;
            AssetId = assetId;
            CountryId = countryId;
            AdditionalData = additionalData;
            BlockchainHash = blockchainHash;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "offchainTransferId")]
        public string OffchainTransferId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "clientId")]
        public string ClientId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "assetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "countryId")]
        public string CountryId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "additionalData")]
        public string AdditionalData { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "blockchainHash")]
        public string BlockchainHash { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (OffchainTransferId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "OffchainTransferId");
            }
            if (ClientId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ClientId");
            }
            if (Address == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Address");
            }
            if (AssetId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AssetId");
            }
        }
    }
}
