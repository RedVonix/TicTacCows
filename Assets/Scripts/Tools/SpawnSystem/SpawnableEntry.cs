using System.Collections.Generic;
using TicTacCows.Logging;
using UnityEngine;

namespace TicTacCows.Tools.SpawnSystem
{
    [System.Serializable]
    public class SpawnableEntry : MonoBehaviour
    {
        [SerializeField] public GameObject prefabObject;
        [SerializeField] public int spawnCount = 1;
        [SerializeField] public List<SpawnableEntryObject> spawnedPool = new List<SpawnableEntryObject>();

        // List of SpawnableEntryObjects that are currently in use, tracked by their GUID, to ensure there is no messy
        // moving of editor-time objects between lists to ensure Unity doesn't have data loss.
        private List<int> RUNTIME_InUseGUIDs = new List<int>();

        private int RUNTIME_ListCount;

        public void RefreshSpawnPool()
        {
            for(int i = spawnedPool.Count - 1; i >= 0; i--)
            {
                spawnedPool[i].DestroyEntry();
                spawnedPool.RemoveAt(i);
            }

            for(int i = 0; i < spawnCount; i++)
            {
                SpawnableEntryObject newEntry = new SpawnableEntryObject(prefabObject, transform);
                spawnedPool.Add(newEntry);
            }
        }

        public void SetupForRuntime()
        {
            // Grabbing the list count frequently can cause a LITTLE overhead, and when done often enough it shows up
            // on garbage collectors. This count doesn't change, so we'll just grab it once and store it.
            RUNTIME_ListCount = spawnedPool.Count;

            spawnedPool.ForEach(s => s.SetupForRuntime());
        }

        /// <summary>
        /// Returns an available object from the spawn pool.
        /// </summary>
        /// <param name="targetObjectTransform"></param>
        /// <returns></returns>
        public GameObject GetObject(Transform targetObjectTransform = null)
        {
            // Find the first available object in the pool
            for(int i = 0; i < RUNTIME_ListCount; i++)
            {
                SpawnableEntryObject entryObject = spawnedPool[i];
                if(!RUNTIME_InUseGUIDs.Contains(entryObject.Runtime_SpawnedObjectID))
                {
                    RUNTIME_InUseGUIDs.Add(entryObject.Runtime_SpawnedObjectID);

                    if (targetObjectTransform != null)
                    {
                        entryObject.spawnedObject.transform.parent = targetObjectTransform;
                        entryObject.spawnedObject.transform.localPosition = Vector3.zero;
                        entryObject.spawnedObject.transform.localEulerAngles = Vector3.zero;
                    }

                    entryObject.spawnedObject.SetActive(true);
                    return entryObject.spawnedObject;
                }
            }

            // Getting here means we couldn't find an object. We could have code here to check if we
            // need to spawn more objects, but this is just a simple example system, so we'll just throw
            // an error and die gracefully.
            LoggingSystem.AddLog(GameValues.LoggingTypes.Error, "--- SpawnableEntry::GetObject - Unable to acquire an available entry for prefab " + prefabObject.name);
            return null;
        }

        public bool ReturnObjectToPoolIfMine(GameObject inObject)
        {
            int objectId = inObject.GetInstanceID();
            for (int i = 0; i < RUNTIME_ListCount; i++)
            {
                SpawnableEntryObject entryObject = spawnedPool[i];
                if (entryObject.Runtime_SpawnedObjectID == objectId)
                {
                    // Object is mine! Claim it and return it.
                    RUNTIME_InUseGUIDs.Remove(entryObject.Runtime_SpawnedObjectID);
                    entryObject.spawnedObject.SetActive(false);
                    return true;
                }
            }

            // Getting here means the object is not mine
            return false;
        }
    }
}