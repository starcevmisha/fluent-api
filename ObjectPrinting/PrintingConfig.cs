using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace ObjectPrinting
{
    public class PrintingConfig<TOwner>
    {
        private int? maxNestedLevel = null;

        private readonly Dictionary<string, int> TrimStringProperty = 
            new Dictionary<string, int>();
        private readonly HashSet<Type> excludedTypes= new HashSet<Type>();
        private readonly HashSet<string> excludedProperties = new HashSet<string>();
        private readonly Dictionary<Type, CultureInfo> Cultures 
            = new Dictionary<Type, CultureInfo>();
        private readonly Dictionary<Type, Delegate> typeSerializator 
            = new Dictionary<Type, Delegate>();
        private readonly Dictionary<string, Delegate> propSerializator 
            = new Dictionary<string, Delegate>();

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

            if (obj == null)
                return "null" + Environment.NewLine;
            
            var finalTypes = new[]
            {
                typeof(int), typeof(double), typeof(float), typeof(string),
                typeof(DateTime), typeof(TimeSpan)
            };
            if (finalTypes.Contains(obj.GetType()))
            {
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
                if (maxNestedLevel.HasValue && nestingLevel >= maxNestedLevel)
                {
                    sb.Insert(sb.Length - Environment.NewLine.Length, "{}");
                    break;
                }
                sb.Append(identation + propertyInfo.Name + " = " +
                          PrintProperty(obj, nestingLevel, propertyInfo));
            }
            return sb.ToString();
        }

        string PrintProperty(object obj, int nestingLevel, PropertyInfo propertyInfo)
        {
            if (TrimStringProperty.ContainsKey(propertyInfo.Name))
                return propertyInfo.GetValue(obj)
                    .ToString()
                    .Substring(0, TrimStringProperty[propertyInfo.Name]) + Environment.NewLine;
            
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


        public PrintingConfig<TOwner> SetMaxNestedLevel(int max)
        {
            maxNestedLevel = max;
            return this;
        }

        internal static void TrimStringPropertyDictAdd(
            PrintingConfig<TOwner> printingConfig,
            string propName,
            int length)
        {
            printingConfig.TrimStringProperty.Add(propName, length);
        }
        internal static void CulturesDictAdd(
            PrintingConfig<TOwner> printingConfig,
            Type type,
            CultureInfo cultureInfo)
        {
            printingConfig.Cultures.Add(type, cultureInfo);
        }
    }
    
}