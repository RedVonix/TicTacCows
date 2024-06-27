using UnityEditor;
using UnityEngine;

namespace TicTacCows.Tools.SpawnSystem
{
    [System.Serializable]
    public class SpawnableEntryObject
    {
        [SerializeField] public GameObject spawnedObject;

        // How we know what exact object this is at runtime. This ID changes between each run of the application, therefore we cannot store it
        // and have to grab it on each start.
        public int Runtime_SpawnedObjectID;

#if UNITY_EDITOR
        public SpawnableEntryObject(GameObject inSourceObject, Transform poolTransform)
        {
            spawnedObject = PrefabUtility.InstantiatePrefab(inSourceObject) as GameObject;
            spawnedObject.transform.parent = poolTransform;
            spawnedObject.SetActive(false);
        }

        public void DestroyEntry()
        {
            GameObject.DestroyImmediate(spawnedObject);
        }
#endif

        public void SetupForRuntime()
        {
            // InstanceID is assigned at runtime, so grab it now.
            Runtime_SpawnedObjectID = spawnedObject.GetInstanceID();
        }
    }
}