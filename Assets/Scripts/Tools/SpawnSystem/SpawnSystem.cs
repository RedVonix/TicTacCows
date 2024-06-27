using System.Collections.Generic;
using TicTacCows.Logging;
using UnityEngine;

namespace TicTacCows.Tools.SpawnSystem
{
    [System.Serializable]
    public class SpawnSystem : MonoBehaviour
    {
        [SerializeField] public List<SpawnableEntry> entries = new List<SpawnableEntry>();

        public static SpawnSystem singleton;

        private int RUNTIME_EntryCount;

        public void SetupForRuntime()
        {
            singleton = this;

            // Grabbing the list count frequently can cause a LITTLE overhead, and when done often enough it shows up
            // on garbage collectors. This count doesn't change, so we'll just grab it once and store it.
            RUNTIME_EntryCount = entries.Count;

            entries.ForEach(s => s.SetupForRuntime());
        }

        public GameObject SpawnFromPrefab(GameObject inPrefabObject, Transform spawnTransform = null)
        {
            if(inPrefabObject == null)
            {
                // We can't spawn from nothing!
                LoggingSystem.AddLog(GameValues.LoggingTypes.Error, "--- SpawnSystem:SpawnFromPrefab - Entered inPrefabObject was null!");
                return null;
            }

            for (int i = 0; i < RUNTIME_EntryCount; i++)
            {
                SpawnableEntry entry = entries[i];
                if(entry.prefabObject == inPrefabObject)
                {
                    return entry.GetObject(spawnTransform);
                }
            }

            // Ruh-roh... no object found. That's bad. Fail gracefully!
            LoggingSystem.AddLog(GameValues.LoggingTypes.Error, "--- SpawnSystem:SpawnFromPrefab - Unable to spawn an object for the prefabe " + inPrefabObject.name);
            return null;
        }

        public void ReturnObjectToPool(GameObject inObject)
        {
            for (int i = 0; i < RUNTIME_EntryCount; i++)
            {
                SpawnableEntry entry = entries[i];
                if(entry.ReturnObjectToPoolIfMine(inObject))
                {
                    return;
                }
            }

            LoggingSystem.AddLog(GameValues.LoggingTypes.Error, "--- SpawnSystem:ReturnObjectToPool - No SpawnableEntry pool could be found that can return object " + inObject.name);
        }
    }
}