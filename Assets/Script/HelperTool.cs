using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
namespace PhongNH.LibTool
{
    public static class HelperTool
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)System.Enum.Parse(typeof(T), value, true);
        }
        public static T JsonToObject<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
        public static string ObjectToJson<T>(T classT)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(classT);
        }

        public static T Spawn<T>(Transform transToSpawn, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            GameObject _obj = ObjectPooler.Instance.Spawn(transToSpawn.gameObject, position, rotation);
            if (parentTransform)
                _obj.transform.SetParent(parentTransform);
            _obj.gameObject.SetActive(true);
            return _obj.GetComponent<T>();
        }

        public static T Spawn<T>(GameObject transToSpawn, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            GameObject _obj = ObjectPooler.Instance.Spawn(transToSpawn, position, rotation);
            _obj.transform.SetParent(parentTransform);
            return _obj.GetComponent<T>();
        }

        public static GameObject Spawn(Transform transToSpawn, Vector3 position, Quaternion rotation, Transform parentTransform)
        {
            GameObject _obj = ObjectPooler.Instance.Spawn(transToSpawn.gameObject, position, rotation);
            _obj.transform.SetParent(parentTransform);
            return _obj;
        }
        public static void Despawn(GameObject _obj)
        {
            if (_obj == null || ObjectPooler.Instance == null)
                return;
            ObjectPooler.Instance.Despawn(_obj);
        }
        public static long ToUnixTimestamp(this DateTime value)
        {
            return (long)(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp</returns>
        public static long UnixTimestamp(this DateTime ignored)
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        /// <summary>
        /// Returns a local DateTime based on provided unix timestamp
        /// </summary>
        /// <param name="timestamp">Unix/posix timestamp</param>
        /// <returns>Local datetime</returns>
        public static DateTime ParseUnixTimestamp(long timestamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(timestamp).ToUniversalTime();
        }
        public static DateTime ParseUnixTimestampUTC(long timestamp, float timezone)
        {
            return ParseUnixTimestampNormal(timestamp).AddHours(-timezone);
        }
        public static DateTime ParseUnixTimestampNormal(long timestamp)
        {
            return (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddSeconds(timestamp);
        }
        public static long ToUnixTimestampNormal(this DateTime value)
        {
            return (long)(value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static string TimeToString(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return ts.ToString(@"hh\:mm\:ss");
            else
                return ts.ToString(@"mm\:ss");
        }
        public static string TimeToShortString(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return ts.ToString(@"hh\:mm");
            else
                return ts.ToString(@"mm\:ss");
        }
        public static string TimeToString_HMS(TimeSpan t)
        {
            if (t.Ticks < 0)
                return string.Format("{0:D2}m:{1:D2}s", 0, 0);
            if (t.TotalDays >= 1)
            {
                return string.Format("{0:D2}d:{1:D2}h:{2:D2}m", t.Days, t.Hours, t.Minutes);
            }
            else if (t.TotalHours >= 1)
            {
                return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
            }
            else
            {
                return string.Format("{0:D2}m:{1:D2}s", t.Minutes, t.Seconds);
            }
        }
        public static string TimeToString_HMS_TWO(TimeSpan t, bool check = true)
        {
            if (!check)
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}:{1}", t.Days, t.Hours);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                else
                {
                    return string.Format("{0}:{1}", t.Minutes, t.Seconds);
                }
            }
            else
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}d:{1}h", t.Days, t.Hours);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}h:{1}m", t.Hours, t.Minutes);
                }
                else
                {
                    return string.Format("{0}m:{1}s", t.Minutes, t.Seconds);
                }
            }
        }
        static string eventStr = "Remaining: ";
        public static string TimeEventToString_HMS_TWO(TimeSpan t, bool check = true)
        {

            if (!check)
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}:{1}", t.Days, t.Hours);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                else
                {
                    return string.Format("{0}:{1}", t.Minutes, t.Seconds);
                }
            }
            else
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{2}{0}d:{1}h", t.Days, t.Hours, eventStr);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{2}{0}h:{1}m", t.Hours, t.Minutes, eventStr);
                }
                else
                {
                    return string.Format("{2}{0}m:{1}s", t.Minutes, t.Seconds, eventStr);
                }
            }
        }
        public static string TimeEventToString_HMS_TWO_NEW(TimeSpan t, bool check = true)
        {

            if (!check)
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}:{1}", t.Days, t.Hours);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}:{1}", t.Hours, t.Minutes);
                }
                else
                {
                    return string.Format("{0}:{1}", t.Minutes, t.Seconds);
                }
            }
            else
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}d:{1}h", t.Days, t.Hours);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}h:{1}m", t.Hours, t.Minutes);
                }
                else
                {
                    return string.Format("{0}m:{1}s", t.Minutes, t.Seconds);
                }
            }
        }
        public static string TimeEventToString_HMS_THREE_NEW(TimeSpan t, bool check = true)
        {

            if (!check)
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}:{1}:{2}", t.Days, t.Hours, t.Minutes);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}:{1}:{2}", t.Hours, t.Minutes, t.Seconds);
                }
                else
                {
                    return string.Format("00:{0}:{1}", t.Minutes, t.Seconds);
                }
            }
            else
            {

                if (t.Ticks < 0)
                    return string.Format("{0:D2}:{1:D2}", 0, 0);
                if (t.TotalDays >= 1)
                {
                    return string.Format("{0}d:{1}h:{2}m", t.Days, t.Hours, t.Minutes);
                }
                else if (t.TotalHours >= 1)
                {
                    return string.Format("{0}h:{1}m:{2}s", t.Hours, t.Minutes, t.Seconds);
                }
                else
                {
                    return string.Format("00h:{0}m:{1}s", t.Minutes, t.Seconds);
                }
            }
        }

        public static Dictionary<string, string> GetSubStats(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                Dictionary<string, string> dictStats = str.Split(new string[] { "}{" }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim('{', '}')).Select(s => s.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries)).ToDictionary(s => s[0], s => s[1]);
                return dictStats;
            }
            else
                return new Dictionary<string, string>();
        }
        public static int GetMaxEnum<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<int>().Max() + 1;
        }
        public static T GetRandomByPercent<T>(Dictionary<T, float> dicPercent)
        {
            T result = From<T>(dicPercent);
            return result;
        }
        private static T From<T>(Dictionary<T, float> spawnRate)
        {
            WeightedRandomBag<T> bag = new WeightedRandomBag<T>();
            foreach (var item in spawnRate)
            {
                bag.AddEntry(item.Key, item.Value);
            }
            return bag.GetRandom();
            //return new WeightedRandomizer<T>(spawnRate);
        }

#if UNITY_EDITOR
        public static IEnumerator IELoadData(string urlData, System.Action<string> actionComplete, bool showAlert = false)
        {
            var www = new WWW(urlData);
            float time = 0;
            //TextAsset fileCsvLevel = null;
            while (!www.isDone)
            {
                time += 0.001f;
                if (time > 10000)
                {
                    yield return null;
                    Debug.Log("Downloading...");
                    time = 0;
                }
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                UnityEditor.EditorUtility.DisplayDialog("Notice", "Load CSV Fail", "OK");
                yield break;
            }
            yield return null;
            actionComplete?.Invoke(www.text);
            yield return null;
            UnityEditor.AssetDatabase.SaveAssets();
            if (showAlert)
                UnityEditor.EditorUtility.DisplayDialog("Notice", "Load Data Success", "OK");
            else
                Debug.Log("<color=yellow>Download Data Complete</color>");
        }
#endif
    }

    public class WeightedRandomBag<T>
    {

        private struct Entry
        {
            public float accumulatedWeight;
            public T item;
        }

        private List<Entry> entries = new List<Entry>();
        private float accumulatedWeight;
        private System.Random rand = new System.Random();




        public void AddEntry(T item, float weight)
        {
            this.accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = weight });
        }

        public T GetRandom()
        {
            //double r = rand.NextDouble() * accumulatedWeight;

            //foreach (Entry entry in entries)
            //{
            //    if (entry.accumulatedWeight >= r)
            //    {
            //        return entry.item;
            //    }
            //}

            var randomPoint = UnityEngine.Random.value * this.accumulatedWeight;

            for (int i = 0; i < entries.Count; i++)
            {
                if (randomPoint < entries[i].accumulatedWeight)
                    return entries[i].item;
                else
                    randomPoint -= entries[i].accumulatedWeight;
            }
            return default(T); //should only happen when there are no entries
        }



    }

}