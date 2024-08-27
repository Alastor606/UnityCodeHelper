namespace CodeHelper.Unity
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.UI;

    public static class PrefsExtentions
    {
        internal static void Save(this int self, string name) => PlayerPrefs.SetInt(name, self);
        internal static void Save(this float self, string name) => PlayerPrefs.SetFloat(name, self);
        internal static void Save(this string self, string name) => PlayerPrefs.SetString(name, self);
        internal static void Save(this Vector2 self, string name) => HelperPrefs.SetVector(name, self);
        internal static void Save(this Vector3 self, string name) => HelperPrefs.SetVector(name, self);
        internal static void Save(this IEnumerable<string> self, string name) => HelperPrefs.SetCollection(self, name);
        internal static void Save(this IEnumerable<int> self, string name) => HelperPrefs.SetCollection(self, name);
        internal static void Save(this IEnumerable<float> self, string name) => HelperPrefs.SetCollection(self, name);
        internal static void Save(this IEnumerable<Vector3> self, string name) => HelperPrefs.SetCollection(self, name);
        internal static void Save(this IEnumerable<Vector2> self, string name) => HelperPrefs.SetCollection(self, name);
        internal static void Save(this Sprite self, string name) => HelperPrefs.SetImage(self, name);
        internal static void Save(this Image self, string name) => HelperPrefs.SetImage(self.sprite, name);
        internal static void SavePosition<T>(this T self, string name) where T : MonoBehaviour => HelperPrefs.SetVector(name, self.transform.position);
        internal static async Task SaveClass<T>(this T self, string name) where T : class => await HelperPrefs.SetClass(self, name);
        internal static int Load(this ref int self, string name) => self = PlayerPrefs.GetInt(name);
        internal static float Load(this ref float self, string name) => self = PlayerPrefs.GetFloat(name);
        internal static string Load(this string _, string name) => PlayerPrefs.GetString(name);
        internal static Vector3 Load(this ref Vector2 self, string name) => self = HelperPrefs.GetVector(name);
        internal static List<string> Load(this IEnumerable<string> self, string name) => HelperPrefs.GetCollection(name, SaveTypes.String) as List<string>;
        internal static List<int> Load(this IEnumerable<int> self, string name) => HelperPrefs.GetCollection(name, SaveTypes.Int) as List<int>;
        internal static List<float> Load(this IEnumerable<float> self, string name) => HelperPrefs.GetCollection(name, SaveTypes.Float) as List<float>;
        internal static List<Vector3> Load(this IEnumerable<Vector3> self, string name) => HelperPrefs.GetCollection( name, SaveTypes.Vector) as List<Vector3>;
    }

}
