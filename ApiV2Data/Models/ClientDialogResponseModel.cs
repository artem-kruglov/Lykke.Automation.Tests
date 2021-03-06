// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Client.ApiV2.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ClientDialogResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the ClientDialogResponseModel class.
        /// </summary>
        public ClientDialogResponseModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ClientDialogResponseModel class.
        /// </summary>
        /// <param name="type">Possible values include: 'Info', 'Warning',
        /// 'Question'</param>
        /// <param name="conditionType">Possible values include: 'Pretrade',
        /// 'Predeposit'</param>
        public ClientDialogResponseModel(string id, DialogType type, DialogConditionType conditionType, string header, string text, IList<DialogActionModel> actions)
        {
            Id = id;
            Type = type;
            ConditionType = conditionType;
            Header = header;
            Text = text;
            Actions = actions;
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
        /// Gets or sets possible values include: 'Info', 'Warning', 'Question'
        /// </summary>
        [JsonProperty(PropertyName = "Type")]
        public DialogType Type { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Pretrade', 'Predeposit'
        /// </summary>
        [JsonProperty(PropertyName = "ConditionType")]
        public DialogConditionType ConditionType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Header")]
        public string Header { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Text")]
        public string Text { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Actions")]
        public IList<DialogActionModel> Actions { get; set; }

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
            if (Header == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Header");
            }
            if (Text == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Text");
            }
            if (Actions == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Actions");
            }
            if (Actions != null)
            {
                foreach (var element in Actions)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
