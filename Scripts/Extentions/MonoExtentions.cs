namespace CodeHelper.Unity
{
    using System;
    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine.UI;
    using System.Text;
    using UnityEngine.XR;

    internal static class MonoExtentions
    {

        /// <summary>Starts local coroutine, wait given time and invoke action </summary>
        /// <param name="delay">Delay</param>
        internal static void WaitAndDo(this MonoBehaviour self, float delay, Action<MonoBehaviour> toDo)
        {
            self.StartCoroutine(Waiter());
            IEnumerator Waiter()
            {
                yield return new WaitForSeconds(delay);
                toDo.Invoke(self);
            }
        }

        /// <summary>Starts local coroutine, wait given time and invoke action </summary>
        /// <param name="delay">Delay</param>
        internal static void WaitAndDo<T>(this T self, float delay, Action<T> toDo) where T : UnityEngine.Object
        {
            var go = Camera.main.gameObject.GetComponent<MonoBehaviour>();
            go.StartCoroutine(Waiter());
            IEnumerator Waiter()
            {
                yield return new WaitForSeconds(delay);
                toDo.Invoke(self);
            }
        }

        internal static T WaitAndDoWithReturn<T>(this T self, float delay, Action<T> toDo) where T : UnityEngine.Object
        {
            var go = Camera.main.gameObject.GetComponent<MonoBehaviour>();
            
            go.StartCoroutine(Waiter());
            IEnumerator Waiter()
            {
                yield return new WaitForSeconds(delay);
                toDo.Invoke(self);
            }
            return self;
        }


        /// <summary>Instanse object with given params</summary>
        internal static T Instance<T>(this MonoBehaviour self, T prefab, Vector3 position, Transform parent = null, Quaternion rotation = default) where T : UnityEngine.Object
        {
            var rot = rotation == default ? Quaternion.identity : rotation;
            var obj = parent == null ? UnityEngine.Object.Instantiate(prefab, position, rot) : UnityEngine.Object.Instantiate(prefab, parent);
            return obj;
        }

        /// <summary> Log the object value to unity console</summary>
        internal static void Print<T>(this T self) => Debug.Log(self);

        /// <summary> Log error the object value to unity console</summary>
        internal static void PrintError<T>(this T self) => Debug.LogError(self);

        /// <summary> Log the all objects to unity console</summary>
        /// <param name="withIndex">If true log the index of every objet</param>
        internal static void PrintAll<T>(this T[] self, bool withIndex = false)
        {
            for (int i = 0; i < self.Length; i++)
                Debug.Log(withIndex == true ? $"Index - {i} \n Value - " + self[i] : self[i]) ;
            
        }

        internal static void PrintAll(this IEnumerable self)
        {
            foreach(var item in self)
            {
                item.Print();
            }
        }


        /// <summary> Log error the all objects to unity console</summary>
        internal static void PrintErrorAll<T>(this T[] self) => self.AllDo((x) => Debug.LogError(x));

        /// <summary> Log object from collection by index to unity console</summary>
        internal static void Print<T>(this T[] self, int index) => Debug.Log(self[index]);

        internal static void Destroy<T>(this T self) where T : UnityEngine.Object => UnityEngine.Object.Destroy(self);

        internal static void Destroy<T>(this T self, float time) where T : UnityEngine.Object => UnityEngine.Object.Destroy(self, time);


        /// <summary> Log`s the object name, message prints after the name</summary>
        internal static void PrintName<T>(this T self, string message = null) where T : UnityEngine.Object => Debug.Log(self.name + message);

        internal static T[] ToArray<T>(this T self, params T[] additional) where T : UnityEngine.Object
        {
            T[] array = new T[additional.Length + 1];
            array[0] = self;
            for(int i = 0; i < additional.Length; i++) array[i + 1] = additional[i];
            return array;
        }

        internal static T CheckSphere<T>(this MonoBehaviour self, float radius, Transform startPoint = null) where T : UnityEngine.Object
        {
            var results = Physics.OverlapSphere(startPoint == null ? self.transform.position : startPoint.position , radius);
            foreach (var item in results) if (item.TryGetComponent(out T res)) return res;
            throw new Exception($"No `{typeof(T)}` objects in given area");
        }

        internal static bool TryCheckSphere<T>(this MonoBehaviour self, float radius, out T result, Transform startPoint = null)
        {
            var results = Physics.OverlapSphere(startPoint == null ? self.transform.position : startPoint.position, radius);
            foreach (var item in results)
            {
                if (item.TryGetComponent(out T res))
                {
                    result = res;
                    return true;
                }
            }
            result = default;
            return false;
        }

        internal static T[] CheckSphereAll<T>(this MonoBehaviour self, float radius, Transform startPoint = null) where T : UnityEngine.Object
        {
            List<T> res = new();
            var results = Physics.OverlapSphere(startPoint == null ? self.transform.position : startPoint.position, radius);
            foreach (var item in results)
            {
                if (item.TryGetComponent(out T r)) res.Add(r);
            }
            return res.ToArray();
        }

        /// <summary>
        /// Call in only in OnGizmos methods!
        /// </summary>
        internal static void DrawSphere(float radius, Transform startPoint, Color color = default)
        {
            if(color != default)Gizmos.color = color;
            Gizmos.DrawWireSphere(startPoint.position, radius);
        }

        internal static IList GetPrefsCollection(this MonoBehaviour _, string name, SaveTypes type) => HelperPrefs.GetCollection(name,type);
        internal static List<int> GetPrefsListInt(this MonoBehaviour _, string name) => HelperPrefs.GetCollection(name, SaveTypes.Int) as List<int>;
        internal static List<string> GetPrefsListStr(this MonoBehaviour _, string name) => HelperPrefs.GetCollection(name, SaveTypes.String) as List<string>;
        internal static List<float> GetPrefsListFloat(this MonoBehaviour _, string name) => HelperPrefs.GetCollection(name, SaveTypes.Float) as List<float>;
        internal static List<Vector3> GetPrefsListVector(this MonoBehaviour _, string name) => HelperPrefs.GetCollection(name, SaveTypes.Vector) as List<Vector3>;
        internal static Sprite GetPrefsImage(this MonoBehaviour _, string name) => HelperPrefs.GetImage(name);
        internal static Sprite LoadFromPrefs(this Image self, string name) => self.sprite = HelperPrefs.GetImage(name);
        internal static byte[] GetBytes(this Sprite self) => self.texture.DeCompress().EncodeToPNG();
        internal static Sprite ToSprite(this byte[] self)
        {
            var texture = new Texture2D(1, 1);
            texture.LoadImage(self);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        internal static T GetPrefsClass<T>(this MonoBehaviour _, string name) => HelperPrefs.GetClass<T>(name);
        internal static void PrintAny<T>(this T self) => Debug.Log("its work");
    }
}