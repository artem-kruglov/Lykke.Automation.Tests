// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CheckMobilePhoneResultModel
    {
        /// <summary>
        /// Initializes a new instance of the CheckMobilePhoneResultModel
        /// class.
        /// </summary>
        public CheckMobilePhoneResultModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CheckMobilePhoneResultModel
        /// class.
        /// </summary>
        public CheckMobilePhoneResultModel(bool? passed = default(bool?))
        {
            Passed = passed;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Passed")]
        public bool? Passed { get; set; }

    }
}
