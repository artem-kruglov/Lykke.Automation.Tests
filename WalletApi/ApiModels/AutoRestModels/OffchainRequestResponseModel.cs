// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class OffchainRequestResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the OffchainRequestResponseModel
        /// class.
        /// </summary>
        public OffchainRequestResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the OffchainRequestResponseModel
        /// class.
        /// </summary>
        /// <param name="type">Possible values include: 'None',
        /// 'RequestTransfer'</param>
        public OffchainRequestResponseModel(string asset = default(string), string requestId = default(string), string type = default(string))
        {
            Asset = asset;
            RequestId = requestId;
            Type = type;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Asset")]
        public string Asset { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RequestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'None', 'RequestTransfer'
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

    }
}