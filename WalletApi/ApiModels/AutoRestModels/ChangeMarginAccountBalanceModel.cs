// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ChangeMarginAccountBalanceModel
    {
        /// <summary>
        /// Initializes a new instance of the ChangeMarginAccountBalanceModel
        /// class.
        /// </summary>
        public ChangeMarginAccountBalanceModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ChangeMarginAccountBalanceModel
        /// class.
        /// </summary>
        public ChangeMarginAccountBalanceModel(string accountId = default(string), double? amount = default(double?))
        {
            AccountId = accountId;
            Amount = amount;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AccountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Amount")]
        public double? Amount { get; set; }

    }
}
