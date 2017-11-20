using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        public int? MaxLength = null;
        private readonly HashSet<Type> excludedTypes;
        private readonly HashSet<string> excludedProperties;
        internal readonly Dictionary<Type, CultureInfo> Cultures;
        private readonly Dictionary<Type, Delegate> typeSerializator;
        private readonly Dictionary<string, Delegate> propSerializator;

        public PrintingConfig()
        {
            excludedTypes = new HashSet<Type>();
            excludedProperties = new HashSet<string>();
            Cultures = new Dictionary<Type, CultureInfo>();
            typeSerializator = new Dictionary<Type, Delegate>();
            propSerializator = new Dictionary<string, Delegate>();
        }

        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>()
        {
            return new PropertyPrintingConfig<TOwner,TPropType>(this);
        }
        
        public PropertyPrintingConfig<TOwner, TPropType> Printing<TPropType>(Expression<Func<TOwner, TPropType>> selector)
        {
            var propName = GetPropertyName(selector);
            return new PropertyPrintingConfig<TOwner, TPropType>(this, propName);
        }

        public PrintingConfig<TOwner> ExcludeProp<TPropType>(Expression<Func<TOwner, TPropType>> selector)
        {
            excludedProperties.Add(GetPropertyName(selector));
            return this;
        }

        private string GetPropertyName<TPropType>(Expression<Func<TOwner, TPropType>> propertyExtractor)
        {
            var propInfo =
                ((MemberExpression) propertyExtractor.Body).Member as PropertyInfo;
            return propInfo.Name.ToString();
            
        }

        public PrintingConfig<TOwner> ExcludeType<TPropType>()
        {
            excludedTypes.Add(typeof(TPropType));
            return this;
        }

        public void AddCustomTypeSerialize(Type type, Delegate func)
        {
            typeSerializator[type] = func;
        }

        public void AddCustomPropertySerialization(string property, Delegate func)
        {
            propSerializator[property] = func;
        }

        
        public string PrintToString(TOwner obj)
        {
            return PrintToString(obj, 0);
        }

        private string PrintToString(object obj, int nestingLevel)
        {
            //TODO apply configurations
            if (obj == null)
                return "null" + Environment.NewLine;

            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
            {
                if (obj is string str && MaxLength != null)
                    return str.Substring(0, MaxLength.Value) + Environment.NewLine;
                return obj + Environment.NewLine;
            }

            var identation = new string('\t', nestingLevel + 1);
            var sb = new StringBuilder();
            var type = obj.GetType();
            sb.AppendLine(type.Name);
            foreach (var propertyInfo in type.GetProperties())
            {
                if(excludedProperties.Contains(propertyInfo.Name))
                    continue;
                if (excludedTypes.Contains(propertyInfo.PropertyType))
                    continue;    
                sb.Append(identation + propertyInfo.Name + " = " +                          
                          PrintProperty(obj, nestingLevel, propertyInfo));
            }
            return sb.ToString();
        }

        string PrintProperty(object obj, int nestingLevel, PropertyInfo propertyInfo)
        {
            if (propSerializator.ContainsKey(propertyInfo.Name))
                return propSerializator[propertyInfo.Name]
                    .DynamicInvoke(propertyInfo.GetValue(obj)) + Environment.NewLine;
            
            if (typeSerializator.ContainsKey(propertyInfo.PropertyType))
                return typeSerializator[propertyInfo.PropertyType]
                    .DynamicInvoke(propertyInfo.GetValue(obj)) + Environment.NewLine;

            if (Cultures.ContainsKey(propertyInfo.PropertyType))
            {
                return ((IFormattable)propertyInfo.GetValue(obj))
                    .ToString(null, CultureInfo.CurrentCulture) + Environment.NewLine;
            }

            return PrintToString(propertyInfo.GetValue(obj),
                nestingLevel + 1);
        }


    }
}