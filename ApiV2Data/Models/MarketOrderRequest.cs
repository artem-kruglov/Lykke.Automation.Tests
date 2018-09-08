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

    public partial class MarketOrderRequest
    {
        /// <summary>
        /// Initializes a new instance of the MarketOrderRequest class.
        /// </summary>
        public MarketOrderRequest()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MarketOrderRequest class.
        /// </summary>
        /// <param name="orderAction">Possible values include: 'Buy',
        /// 'Sell'</param>
        public MarketOrderRequest(string assetPairId, string assetId, double volume, OrderAction orderAction)
        {
            AssetPairId = assetPairId;
            AssetId = assetId;
            Volume = volume;
            OrderAction = orderAction;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetPairId")]
        public string AssetPairId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetId")]
        public string AssetId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Volume")]
        public double Volume { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Buy', 'Sell'
        /// </summary>
        [JsonProperty(PropertyName = "OrderAction")]
        public OrderAction OrderAction { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (AssetPairId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AssetPairId");
            }
            if (AssetId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AssetId");
            }
        }
    }
}
