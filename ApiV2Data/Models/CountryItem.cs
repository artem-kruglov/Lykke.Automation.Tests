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

    public partial class CountryItem
    {
        /// <summary>
        /// Initializes a new instance of the CountryItem class.
        /// </summary>
        public CountryItem()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CountryItem class.
        /// </summary>
        public CountryItem(string id, string iso2, string name, string prefix)
        {
            Id = id;
            Iso2 = iso2;
            Name = name;
            Prefix = prefix;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Iso2")]
        public string Iso2 { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Prefix")]
        public string Prefix { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Id == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Id");
            }
            if (Iso2 == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Iso2");
            }
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (Prefix == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Prefix");
            }
        }
    }
}
