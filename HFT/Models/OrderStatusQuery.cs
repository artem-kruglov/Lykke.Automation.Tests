// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.AutorestClient.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for OrderStatusQuery.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatusQuery
    {
        [EnumMember(Value = "All")]
        All,
        [EnumMember(Value = "Open")]
        Open,
        [EnumMember(Value = "InOrderBook")]
        InOrderBook,
        [EnumMember(Value = "Processing")]
        Processing,
        [EnumMember(Value = "Matched")]
        Matched,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Rejected")]
        Rejected
    }
    internal static class OrderStatusQueryEnumExtension
    {
        internal static string ToSerializedValue(this OrderStatusQuery? value)
        {
            return value == null ? null : ((OrderStatusQuery)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this OrderStatusQuery value)
        {
            switch( value )
            {
                case OrderStatusQuery.All:
                    return "All";
                case OrderStatusQuery.Open:
                    return "Open";
                case OrderStatusQuery.InOrderBook:
                    return "InOrderBook";
                case OrderStatusQuery.Processing:
                    return "Processing";
                case OrderStatusQuery.Matched:
                    return "Matched";
                case OrderStatusQuery.Cancelled:
                    return "Cancelled";
                case OrderStatusQuery.Rejected:
                    return "Rejected";
            }
            return null;
        }

        internal static OrderStatusQuery? ParseOrderStatusQuery(this string value)
        {
            switch( value )
            {
                case "All":
                    return OrderStatusQuery.All;
                case "Open":
                    return OrderStatusQuery.Open;
                case "InOrderBook":
                    return OrderStatusQuery.InOrderBook;
                case "Processing":
                    return OrderStatusQuery.Processing;
                case "Matched":
                    return OrderStatusQuery.Matched;
                case "Cancelled":
                    return OrderStatusQuery.Cancelled;
                case "Rejected":
                    return OrderStatusQuery.Rejected;
            }
            return null;
        }
    }
}
