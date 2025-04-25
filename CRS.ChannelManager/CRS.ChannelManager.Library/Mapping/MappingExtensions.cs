using AutoMapper;
using CRS.ChannelManager.Library.Base;
using CRS.ChannelManager.Library.BaseDto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRS.ChannelManager.Library.Mapper
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination, object p)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        public static void SimpleMap<TSource, TDestination>(this TSource source, TDestination destination) => AutoMapperConfiguration.Mapper.Map(source, destination);

        public static IMappingExpression<TSource, TDestination> IgnoreNoMap<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> expression)
        {
            //Fetching Type of the TSource
            var sourceType = typeof(TSource);

            //Fetching All Properties of the Source Type using GetProperties() method
            foreach (var property in sourceType.GetProperties())
            {
                //Get the Property Name
                PropertyDescriptor descriptor = TypeDescriptor.GetProperties(sourceType)[property.Name];

                //Check if Property is Decorated with the NoMapAttribute
                NotMappedAttribute attribute = (NotMappedAttribute)descriptor.Attributes[typeof(NotMappedAttribute)];
                if (attribute != null)
                {
                    //If Property is Decorated with NoMap Attribute, call the Ignore Method
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }
            return expression;
        }

        public static IMappingExpression<TSource, TDestination> IgnoreNoChangeValue<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {

            //Fetching Type of the TDestination
            var destinationType = typeof(TDestination);

            //Fetching Type of the TSource
            var sourceType = typeof(TSource);

            //Fetching All Properties of the Source Type using GetProperties() method
            foreach (var property in destinationType.GetProperties())
            {
                //Get the Property Name
                var checkProperty = sourceType.GetProperty(property.Name);

                //Check if Property not found then ignore
                if (checkProperty == null)
                {
                    var attr = property.Attributes;

                    //If Property is Decorated with NoMap Attribute, call the Ignore Method
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }

            return expression;
        }

        internal static EntityBase MapTo<T>(RequestBaseDto request)
        {
            throw new NotImplementedException();
        }
    }
}
