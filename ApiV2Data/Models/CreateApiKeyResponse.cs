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

    public partial class CreateApiKeyResponse
    {
        /// <summary>
        /// Initializes a new instance of the CreateApiKeyResponse class.
        /// </summary>
        public CreateApiKeyResponse()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CreateApiKeyResponse class.
        /// </summary>
        public CreateApiKeyResponse(string apiKey, string walletId)
        {
            ApiKey = apiKey;
            WalletId = walletId;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ApiKey")]
        public string ApiKey { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "WalletId")]
        public string WalletId { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ApiKey == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ApiKey");
            }
            if (WalletId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "WalletId");
            }
        }
    }
}
