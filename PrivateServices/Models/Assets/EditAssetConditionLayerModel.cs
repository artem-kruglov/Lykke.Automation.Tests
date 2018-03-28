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
    /// Represents an edited properties of asset default condition layer.
    /// </summary>
    public partial class EditAssetConditionLayerModel
    {
        /// <summary>
        /// Initializes a new instance of the EditAssetConditionLayerModel
        /// class.
        /// </summary>
        public EditAssetConditionLayerModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the EditAssetConditionLayerModel
        /// class.
        /// </summary>
        /// <param name="priority">The asset condition layer priority.</param>
        /// <param name="id">The layer id.</param>
        /// <param name="description">The asset condition layer
        /// description.</param>
        /// <param name="clientsCanCashInViaBankCards">The client ability to
        /// cash in via bank cards.</param>
        /// <param name="swiftDepositEnabled">The client ability to swift
        /// deposit.</param>
        public EditAssetConditionLayerModel(double priority, string id = default(string), string description = default(string), bool? clientsCanCashInViaBankCards = default(bool?), bool? swiftDepositEnabled = default(bool?))
        {
            Id = id;
            Priority = priority;
            Description = description;
            ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards;
            SwiftDepositEnabled = swiftDepositEnabled;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the layer id.
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the asset condition layer priority.
        /// </summary>
        [JsonProperty(PropertyName = "Priority")]
        public double Priority { get; set; }

        /// <summary>
        /// Gets or sets the asset condition layer description.
        /// </summary>
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the client ability to cash in via bank cards.
        /// </summary>
        [JsonProperty(PropertyName = "ClientsCanCashInViaBankCards")]
        public bool? ClientsCanCashInViaBankCards { get; set; }

        /// <summary>
        /// Gets or sets the client ability to swift deposit.
        /// </summary>
        [JsonProperty(PropertyName = "SwiftDepositEnabled")]
        public bool? SwiftDepositEnabled { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
