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

    public partial class WatchList
    {
        /// <summary>
        /// Initializes a new instance of the WatchList class.
        /// </summary>
        public WatchList()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the WatchList class.
        /// </summary>
        public WatchList(int order, bool readOnlyProperty, IList<string> assetIds = default(IList<string>), string id = default(string), string name = default(string))
        {
            AssetIds = assetIds;
            Id = id;
            Name = name;
            Order = order;
            ReadOnlyProperty = readOnlyProperty;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetIds")]
        public IList<string> AssetIds { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Order")]
        public int Order { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ReadOnly")]
        public bool ReadOnlyProperty { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}
