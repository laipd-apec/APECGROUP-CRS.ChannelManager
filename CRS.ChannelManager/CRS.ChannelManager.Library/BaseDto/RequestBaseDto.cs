using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseDto
{
    public class RequestBaseDto : IValidatableObject
    {
        public long? Id { get; set; }

        public string? CreatedBy { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{yyyy/MM/dd hh:mm:ss}")]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(1, ErrorMessage = "Deleted cannot be longer than 1 characters.")]
        [ListValidation("Y,N")]
        public string? Deleted { get; set; } = "N";

        virtual public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }
    public class UpperCaseData : Attribute
    {
    }
    public class ImportCheckDataViaCodeDto : Attribute
    {
    }
    public class ImportReferedEntity : Attribute
    {
        // Properties or fields to store input values
        public Type Entity { get; private set; }

        // Constructor to initialize the attribute with input values
        public ImportReferedEntity(Type property)
        {
            Entity = property;
        }
    }

    public class ArrayDynamicUpdateSupport : Attribute
    {
        // Properties or fields to store input values
        public string? PropertyName { get; private set; }

        // Constructor to initialize the attribute with input values
        public ArrayDynamicUpdateSupport(string? coModelPropertyName)
        {
            PropertyName = coModelPropertyName;
        }
    }

    public class ListValidationAttribute : ValidationAttribute
    {
        private readonly string _value;
        private readonly List<string> _listValue;

        public ListValidationAttribute(string value)
        {
            _value = value;
            if (value != null && !string.IsNullOrEmpty(value))
            {
                _listValue = value.Split(",").ToList();
            }
        }

        public ListValidationAttribute(List<string> listValue)
        {
            _listValue = listValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Your custom validation logic here
            if (value != null && !String.IsNullOrEmpty(value.ToString()) && _value != null && !string.IsNullOrEmpty(_value))
            {
                if (!_listValue.Any(x => x == value.ToString()))
                {
                    return new ValidationResult($"{validationContext.MemberName} don't exist in list {_value}.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
