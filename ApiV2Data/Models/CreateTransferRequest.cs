// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CreateTransferRequest
    {
        /// <summary>
        /// Initializes a new instance of the CreateTransferRequest class.
        /// </summary>
        public CreateTransferRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CreateTransferRequest class.
        /// </summary>
        public CreateTransferRequest(string assetId, double amount, System.Guid sourceWalletId, System.Guid walletId)
        {
            AssetId = assetId;
            Amount = amount;
            SourceWalletId = sourceWalletId;
            WalletId = walletId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SourceWalletId")]
        public System.Guid SourceWalletId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "WalletId")]
        public System.Guid WalletId { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (AssetId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AssetId");
            }
        }
    }
}
