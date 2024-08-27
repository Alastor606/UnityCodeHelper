namespace CodeHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    [Serializable]
    public class MTPArray : IEnumerable
    {
        private List<BoxedObject> _mainArr;

        public void Add(Type type, object value) =>
           _mainArr.Add(new (type, value));
        public void Add<T>(T item) =>
            _mainArr.Add(new (typeof(T), item));
        public void Add(BoxedObject value) =>
            _mainArr.Add(value);


        /// <summary>
        /// Create the instance of multi-type array
        /// </summary>
        public MTPArray() =>
            _mainArr = new();

        /// <summary>
        /// Create the instance of multi-type array
        /// </summary>
        public MTPArray(IEnumerable collection)
        {
            _mainArr = new();
            foreach(var item in collection)
            {
                this.Add(item);
            }
        }
        
        public object this[int index] 
        { get => _mainArr[index].value; }
        public object[] this[Type type]
        { 
            get
            {
                var result = new List<object>();
                foreach (var item in _mainArr) if (item.type == type) result.Add(item.value);
                return result.ToArray();
            }
        }

        /// <returns>All objects of given type</returns>
        public T[] GetAll<T>()
        {
            var result = new List<T>();
            foreach(var item in _mainArr)
            {
                if(item.type == typeof(T))
                {
                    result.Add((T)item.value);
                }
            }
            if (result.Count < 1) throw new ArgumentException("MTP doesnt contains this type");
            
            return result.ToArray();
        }

        /// <returns>All objects of given type</returns>
        public T[] TryGetAll<T>()
        {
            var result = new List<T>();
            foreach (var item in _mainArr)
            {
                if (item.type == typeof(T))
                {
                    result.Add((T)item.value);
                }
            }
            if(result.Count < 1)return null;

            return result.ToArray();
        }

        public T GetFirstOrDefault<T>()
        {
            foreach(var item in _mainArr)
            {
                if (item.type == typeof(T)) return (T)item.value;
            }
            return default;
        }

       
        public bool ContainsType<T>()
        {
            foreach (var item in _mainArr) if (item.type == typeof(T)) return true;
            return false;
        }

        public bool ContainsType(Type t)
        {
            foreach (var item in _mainArr) if (item.type == t) return true;
            return false;
        }


        public IEnumerator GetEnumerator() =>
            _mainArr.GetEnumerator();

        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("COLLECTION\n");
            var index = 0;
            foreach(var item in _mainArr)
            {
                index++;
                result.Append(index+". " + item.value + $" ({item.type})\n");
            }
            return result.ToString();
        }

        public bool Contains(object obj)
        {
            for (int i = 0; i < _mainArr.Count; i++)
            {
                if (obj.Equals(_mainArr[i])) return true;
            }
            return false;
        }

        public void Remove(object obj)
        {
            foreach (var item in _mainArr)
            {
                if (item.value.Equals(obj))
                {
                    _mainArr.Remove(item);
                    return;
                }
            }
        }

        public int IndexOf(object obj)
        {
            foreach(var item in _mainArr)
            {
                if (item.value.Equals(obj))
                {
                    return _mainArr.IndexOf(item);
                }
            }
            return -1;
        }

        public void RemoveType<T>() =>
            _mainArr.RemoveAll(x => x.type == typeof(T));
        
    }
}

