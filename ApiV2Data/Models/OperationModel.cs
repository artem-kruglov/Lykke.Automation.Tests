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

    public partial class OperationModel
    {
        /// <summary>
        /// Initializes a new instance of the HistoryResponseModel class.
        /// </summary>
        public OperationModel()
        {
            CustomInit();
        }

       
        public OperationModel(string id, System.DateTime dateTime, OperationType type, OperationStatus status)
        {
            Id = id;
            DateTime = dateTime;
            Type = type;
            Status = status;
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
        [JsonProperty(PropertyName = "DateTime")]
        public System.DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'CashIn', 'CashOut', 'Trade',
        /// 'LimitTrade', 'LimitOrderEvent'
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public OperationType Type { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'InProgress', 'Finished',
        /// 'Canceled', 'Failed'
        /// </summary>
        [JsonProperty(PropertyName = "Status")]
        public OperationStatus Status { get; set; }

    }
}