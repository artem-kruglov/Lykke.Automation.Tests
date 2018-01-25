// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ApiBankCardModel
    {
        /// <summary>
        /// Initializes a new instance of the ApiBankCardModel class.
        /// </summary>
        public ApiBankCardModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ApiBankCardModel class.
        /// </summary>
        public ApiBankCardModel(string id = default(string), string type = default(string), string lastDigits = default(string), string name = default(string), int? monthTo = default(int?), int? yearTo = default(int?))
        {
            Id = id;
            Type = type;
            LastDigits = lastDigits;
            Name = name;
            MonthTo = monthTo;
            YearTo = yearTo;
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
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LastDigits")]
        public string LastDigits { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MonthTo")]
        public int? MonthTo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "YearTo")]
        public int? YearTo { get; set; }

    }
}