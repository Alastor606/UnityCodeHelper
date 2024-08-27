namespace CodeHelper
{
    using System;


    [Serializable]
    public class BoxedObject
    {
        public Type type;
        public object value;

        public T UnBox<T>()
        {
            if (value != null && type == typeof(T)) return (T)value;
            else  return default;
        }

        public BoxedObject(Type type, object value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public static class BoxedExtentions
    {
        public static BoxedObject Box<T>(this T value) =>
            new (value.GetType(), value);
        
    }
}
