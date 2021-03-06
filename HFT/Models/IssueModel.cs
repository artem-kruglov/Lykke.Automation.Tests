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

    /// <summary>
    /// Issue description.
    /// </summary>
    public partial class IssueModel
    {
        /// <summary>
        /// Initializes a new instance of the IssueModel class.
        /// </summary>
        public IssueModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IssueModel class.
        /// </summary>
        /// <param name="type">Gets or sets the issue type.</param>
        /// <param name="value">Gets or sets the issue reason, message or
        /// value.</param>
        public IssueModel(string type, string value)
        {
            Type = type;
            Value = value;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the issue type.
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the issue reason, message or value.
        /// </summary>
        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Type == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Type");
            }
            if (Value == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Value");
            }
        }
    }
}
