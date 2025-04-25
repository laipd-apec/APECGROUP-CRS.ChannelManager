using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Base
{
    public class ExtendedAttribute
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class SearchableAttribute : Attribute
        {

        }

        public class CheckListValueAttribute : ValidationAttribute
        {
            private readonly List<string> _values;

            public CheckListValueAttribute()
            {
                _values = new List<string>();
            }

            public CheckListValueAttribute(string value)
            {
                if (value != null && !string.IsNullOrEmpty(value))
                {
                    _values = value.Split(",").ToList();
                }
            }

            public CheckListValueAttribute(List<string> values)
            {
                _values = values;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value != null && !string.IsNullOrEmpty(value.ToString()))
                {
                    if (!_values.Any(x => x == value.ToString()))
                    {
                        return new ValidationResult($"value does not exist in list {string.Join(",", _values)}");
                    }
                }
                return ValidationResult.Success;
            }
        }
    }
}
