using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.BaseEnum
{
    public enum FilterType
    {
        [Description("None")]
        [EnumMember(Value = "None")]
        None,

        [Description("Less")]
        [EnumMember(Value = "Less")]
        Less,

        [Description("LessOrEquals")]
        [EnumMember(Value = "LessOrEquals")]
        LessOrEquals,

        [Description("Equals")]
        [EnumMember(Value = "Equals")]
        Equals,

        [Description("Greater")]
        [EnumMember(Value = "Greater")]
        Greater,

        [Description("GreaterOrEquals")]
        [EnumMember(Value = "GreaterOrEquals")]
        GreaterOrEquals,

        [Description("Contains")]
        [EnumMember(Value = "Contains")]
        Contains,

        [Description("DateTime")]
        [EnumMember(Value = "DateTime.Compare")]
        DateTime,

        [Description("Any")]
        [EnumMember(Value = "Any")]
        Any,

        [Description("Where")]
        [EnumMember(Value = "Where")]
        Where,
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class FieldFilterAttribute : Attribute
    {
        public string PropName { get; }
        public FilterType FilterType { get; }

        public FieldFilterAttribute() : this(null, FilterType.Equals)
        {
        }

        public FieldFilterAttribute(FilterType filterType) : this(null, filterType)
        {
        }

        public FieldFilterAttribute(string propName, FilterType filterType)
        {
            PropName = propName;
            FilterType = filterType;
        }
    }
}
