using System;
using System.Collections;
using System.Runtime.Serialization;

public class ObjectPropertyNullOrEmpty : Exception
{
    public ObjectPropertyNullOrEmpty(string objectName, string propertyName) : base($"{objectName} property {propertyName} cannot be null or empty.")
    {
    }
}