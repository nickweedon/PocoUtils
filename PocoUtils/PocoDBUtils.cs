using System;
using System.Collections.Generic;
using System.Reflection;
using PocoUtils.Attributes;

namespace PocoUtils
{
    // ReSharper disable once InconsistentNaming
    public class PocoDBUtils
    {
        /// <summary>
        /// Copy all fields from the DbDataReader to 'destination'.
        /// Each property of the 'destination' POCO is checked for a 'DBField' attribute and
        /// if the property has this attribute then the DbDataReader field of the same name as the
        /// attribute is copied to the property.
        /// 
        /// Note that the DBField attribute also takes an optional argument which allows the field name
        /// in the DbDataReader to be specified such as when the field name is different to the property name.
        /// 
        /// This method will attempt to perform type conversions where possible.
        /// </summary>
        /// <param name="destination">The destination POCO to copy values to</param>
        /// <param name="sourceDataReader">The dbDataReader reader to read values from</param>
        public static void CopyFields(Object destination, System.Data.Common.DbDataReader sourceDataReader)
        {
            // Use reflection to get the type of POCO (e.g. what Class is it instantiated from)
            Type classType = destination.GetType();

            // Use reflection to loop through each of the properties of the POCO
            foreach (PropertyInfo propertyInfo in classType.GetProperties())
            {
                // Get all the Attributes on this property of type 'DBField' (there should be either one or zero).
                DBFieldAttribute[] dbFieldAttributes = (DBFieldAttribute[])propertyInfo.GetCustomAttributes(typeof(DBFieldAttribute), false);

                // No DBField attribute so we ignore this property
                if (dbFieldAttributes.Length == 0)
                {
                    continue;
                }

                // Developer accidentally created more than one attribute of type DBField
                if (dbFieldAttributes.Length > 1)
                {
                    throw new Exception("Field '" + propertyInfo.Name + "' contains more than one DBField attribute.");
                }

                // Retrieve the one and only DBField attribute
                DBFieldAttribute attribute = dbFieldAttributes[0];

                // Use the field name specified in the attribute or otherwise fall back to using the name of the
                // the property as the field name.
                String dbFieldName = attribute.FieldName ?? propertyInfo.Name;

                // Retrieve the actual value from the DbDataReader and attempt to convert to the correct type
                Object dbFieldValue = Convert.ChangeType(sourceDataReader.GetValue(sourceDataReader.GetOrdinal(dbFieldName)), propertyInfo.PropertyType);

                // Invoke the POCO's property setter and set the value
                classType.InvokeMember(propertyInfo.Name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty, Type.DefaultBinder,
                    destination, new Object[] {dbFieldValue});
            }

        }

    }
}
