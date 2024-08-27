namespace CodeHelper.Unity
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using UnityEngine;

    internal static class HelperPrefs
    {

        /// <summary>
        /// Takes needed type of collection
        /// </summary>
        /// <returns>Collection of given type</returns>
        internal static IList GetCollection(string name, SaveTypes type)
        {
            int i = 0;
            switch (type)
            {
                case SaveTypes.String:
                    var strlist = new List<string>();
                    while (PlayerPrefs.HasKey(name + i))
                    {
                        strlist.Add(PlayerPrefs.GetString(name + i));
                        i++;
                    }
                    return strlist;
                case SaveTypes.Int:
                    var lintist = new List<int>();
                    while (PlayerPrefs.HasKey(name + i))
                    {
                        lintist.Add(PlayerPrefs.GetInt(name + i));
                        i++;
                    }
                    return lintist;
                case SaveTypes.Float:
                    var fllist = new List<float>();
                    while (PlayerPrefs.HasKey(name + i))
                    {
                        fllist.Add(PlayerPrefs.GetFloat(name + i));
                        i++;
                    }
                    return fllist;
                case SaveTypes.Vector:
                    var list = new List<Vector3>();
                    while (PlayerPrefs.HasKey(name + i))
                    {
                        list.Add(GetVector(name + i));
                        i++;
                    }
                    return list;
                default:
                    return null;
            }   
        }

        internal static void SetCollection(IEnumerable<string> list, string name)
        {
            var index = 0;
            foreach (var item in list) 
            {
                PlayerPrefs.SetString(name+index, item);
                index++;
            }
        }

        internal static void SetCollection(IEnumerable<int> list, string name)
        {
            var index = 0;
            foreach (var item in list)
            {
                PlayerPrefs.SetInt(name + index, item);
                index++;
            }
        }

        internal static void SetCollection(IEnumerable<float> list, string name)
        {
            var index = 0;
            foreach (var item in list)
            {
                PlayerPrefs.SetFloat(name + index, item);
                index++;
            }
        }

        internal static void SetCollection(IEnumerable<Vector3> list, string name)
        {
            var index = 0;
            foreach (var item in list)
            {
                SetVector(name + index, item);
                index++;
            }
        }

        internal static void SetCollection(IEnumerable<Vector2> list, string name)
        {
            var index = 0;
            foreach (var item in list)
            {
                SetVector(name + index, item);
                index++;
            }
        }

        internal static void SetVector(string name, Vector3 self)
        {
            PlayerPrefs.SetFloat(name + "x", self.x);
            PlayerPrefs.SetFloat(name + "y", self.y);
            PlayerPrefs.SetFloat(name + "z", self.z);
        }

        internal static Vector3 GetVector(string name) =>
            new (PlayerPrefs.GetFloat(name + "x"), PlayerPrefs.GetFloat(name + "y"), PlayerPrefs.HasKey(name + "z") ? PlayerPrefs.GetFloat(name + "z") : 0);
        

        public static void SetImage(Sprite img, string name)
        {
            var mainPath = Path.Combine(Application.streamingAssetsPath, "Code Helper Media");
            if (!Directory.Exists(Application.streamingAssetsPath)) Directory.CreateDirectory(Application.streamingAssetsPath);
            if (!Directory.Exists(mainPath)) Directory.CreateDirectory(mainPath);
            File.WriteAllBytes(Path.Combine(mainPath, name + ".png"), img.texture.DeCompress().EncodeToPNG());
        }

        public static Sprite GetImage(string name)
        {
            var mainPath = Path.Combine(Application.streamingAssetsPath, "Code Helper Media");
            if (!File.Exists(Path.Combine(mainPath, name + ".png")))
            {
                throw new ArgumentException($"Files doesnt contains {name}") ;
            }
            var bytes = File.ReadAllBytes(Path.Combine(mainPath, name + ".png"));
            var texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }


        /// <summary>
        /// <b>[doesnt work with monobehaviour scripts]</b><br/><br></br>
        /// Save your serializeble class to PlayerPrefs
        /// </summary>
        /// <param name="name">The varrible name</param>
        public static async Task SetClass<T>(T self, string name) where T : class
        {
            if (!IsSerializable(typeof(T)))throw new ArgumentException("Type is not serializeble");
            var bytes = SerializeToBytes(self);
            for (int i = 0; i < bytes.Length; i++)
            {
                ((int)bytes[i]).Save(name+i);
                await Task.Yield();
            }
        }

        public static T GetClass<T>(string name)
        {
            var result = new List<int>();
            var i = 0;
            while (PlayerPrefs.HasKey(name + i))
            {
                result.Add(PlayerPrefs.GetInt(name + i));
                i++;
            }

            BinaryFormatter bf = new BinaryFormatter();
            using MemoryStream ms = new (result.ToByteArray());
            return (T)bf.Deserialize(ms);
        }

        private static bool IsSerializable(Type type) =>
            Attribute.IsDefined(type, typeof(SerializableAttribute));
        

        internal static Texture2D DeCompress(this Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        private static byte[] SerializeToBytes<T>(T obj) where T : class
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new ();
            using MemoryStream ms = new();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

    }

    public enum SaveTypes
    {
        Int,
        Float,
        String,
        Vector
    }
}


