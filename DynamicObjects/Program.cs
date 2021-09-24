using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DynamicObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            TypeObject.GetClasses(File.ReadAllLines(@"../../../Classes.txt"));
            TypeObject.AddProperties(File.ReadAllLines(@"../../../Properties.txt"));
            Object.CreatObjects(File.ReadAllLines(@"../../../Objects.txt"));
        }
    }
    public class TypeObject
    {
        public int ClassId { get; }
        public string ClassName { get; }
        public List<Property> Properties { get; private set; } = new List<Property>();
        public TypeObject(string classId, string className)
        {
            ClassId = int.Parse(classId);
            ClassName = className;
        }
        public static List<TypeObject> TypeObjects { get; private set; } = new List<TypeObject>();
        public static void GetClasses(string[] data)
        {
            for(var i = 1; i < data.Length; i++)
                TypeObjects.Add(GetClass(data[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)));
        }
        private static TypeObject GetClass(string[] oneClass)
            => new TypeObject(oneClass[0], oneClass[1]);
        public static void AddProperties(string[] data)
        {
            for (int i = 1; i < data.Length; i++)
                AddProperty(data[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }
        private static void AddProperty(string[] oneProperty)
            =>TypeObjects.Single(e => e.ClassId == int.Parse(oneProperty[0]))
                .Properties.Add(new Property(oneProperty[1], oneProperty[2], oneProperty[3]));
    }
    public class Property
    {
        public int PropertyId { get; }
        public string PropertyName { get; }
        public string PropertyType { get; }
        public Property(string propertyId, string propertyName, string propertyType)
        {
            PropertyId = int.Parse(propertyId);
            PropertyName = propertyName;
            PropertyType = propertyType;
        }
    }
    public class Object
    {
        public TypeObject Type { get; }
        public string ObjectId { get; }
        public List<ValueProperty> Values { get; private set; } = new List<ValueProperty>();
        public Object(TypeObject type, string objectId)
        {
            Type = type;
            ObjectId = objectId;
        }
        public static List<Object> Objects { get; private set; } = new List<Object>();
        public static void CreatObjects(string[] data)
        {
            for(var i = 1; i < data.Length; i++)
                IsHaveObject(data[i].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        }
        private static void IsHaveObject(string[] oneObject)
        {
            if (Objects.Count == 0 || !Objects.Any(el => el.ObjectId == oneObject[1]))
                AddObject(oneObject);
            else
                AddValue(oneObject, Objects.Single(el => el.ObjectId == oneObject[1]));
        }
        private static void AddObject(string[] oneObject)
        {
            Objects.Add(new Object(TypeObject.TypeObjects.Single(el => el.ClassId == int.Parse(oneObject[0])), oneObject[1]));
            AddValue(oneObject, Objects.Single(el => el.ObjectId == oneObject[1]));
        }
        private static void AddValue(string[] oneObject, Object @object)
            => @object.Values.Add(new ValueProperty(@object.Type.Properties.Single(el => el.PropertyId == int.Parse(oneObject[2])), oneObject[3]));
    }
    public class ValueProperty
    {
        public Property Property { get; }
        public string Value { get; }
        public ValueProperty(Property property, string value)
        {
            Property = property;
            Value = value;
        }
    }
}
