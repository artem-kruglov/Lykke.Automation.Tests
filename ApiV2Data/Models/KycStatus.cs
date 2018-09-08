// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for KycStatus.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum KycStatus
    {
        [EnumMember(Value = "NeedToFillData")]
        NeedToFillData,
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "ReviewDone")]
        ReviewDone,
        [EnumMember(Value = "Ok")]
        Ok,
        [EnumMember(Value = "Rejected")]
        Rejected,
        [EnumMember(Value = "RestrictedArea")]
        RestrictedArea,
        [EnumMember(Value = "Complicated")]
        Complicated,
        [EnumMember(Value = "JumioInProgress")]
        JumioInProgress,
        [EnumMember(Value = "JumioOk")]
        JumioOk,
        [EnumMember(Value = "JumioFailed")]
        JumioFailed
    }
    internal static class KycStatusEnumExtension
    {
        internal static string ToSerializedValue(this KycStatus? value)
        {
            return value == null ? null : ((KycStatus)value).ToSerializedValue();
        }

        internal static string ToSerializedValue(this KycStatus value)
        {
            switch( value )
            {
                case KycStatus.NeedToFillData:
                    return "NeedToFillData";
                case KycStatus.Pending:
                    return "Pending";
                case KycStatus.ReviewDone:
                    return "ReviewDone";
                case KycStatus.Ok:
                    return "Ok";
                case KycStatus.Rejected:
                    return "Rejected";
                case KycStatus.RestrictedArea:
                    return "RestrictedArea";
                case KycStatus.Complicated:
                    return "Complicated";
                case KycStatus.JumioInProgress:
                    return "JumioInProgress";
                case KycStatus.JumioOk:
                    return "JumioOk";
                case KycStatus.JumioFailed:
                    return "JumioFailed";
            }
            return null;
        }

        internal static KycStatus? ParseKycStatus(this string value)
        {
            switch( value )
            {
                case "NeedToFillData":
                    return KycStatus.NeedToFillData;
                case "Pending":
                    return KycStatus.Pending;
                case "ReviewDone":
                    return KycStatus.ReviewDone;
                case "Ok":
                    return KycStatus.Ok;
                case "Rejected":
                    return KycStatus.Rejected;
                case "RestrictedArea":
                    return KycStatus.RestrictedArea;
                case "Complicated":
                    return KycStatus.Complicated;
                case "JumioInProgress":
                    return KycStatus.JumioInProgress;
                case "JumioOk":
                    return KycStatus.JumioOk;
                case "JumioFailed":
                    return KycStatus.JumioFailed;
            }
            return null;
        }
    }
}
