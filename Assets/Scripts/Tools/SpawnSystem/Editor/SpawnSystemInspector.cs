#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace TicTacCows.Tools.SpawnSystem
{
    [CustomEditor(typeof(SpawnSystem))]
    public class SpawnSystemInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            SpawnSystem spawnSys = target as SpawnSystem;

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginVertical("box");
                int deleteIndex = -1;
                for(int i = 0; i < spawnSys.entries.Count; i++)
                {
                    SpawnableEntry entry = spawnSys.entries[i];

                    GUILayout.BeginHorizontal("box");
                        GUI.color = Color.red;
                            if(GUILayout.Button("X", GUILayout.Width(20f)))
                            {
                                deleteIndex = i;
                            }
                        GUI.color = Color.white;

                        if(entry == null)
                        {
                            GUILayout.Label("Null Entry Found!");
                            GUILayout.EndHorizontal();
                            continue;
                        }

                        entry.spawnCount = EditorGUILayout.IntField(entry.spawnCount, GUILayout.Width(30f));

                        GameObject oldPrefab = entry.prefabObject;
                        entry.prefabObject = (GameObject)EditorGUILayout.ObjectField(entry.prefabObject, typeof(GameObject), false);     
                        if(oldPrefab != entry.prefabObject)
                        {
                            entry.gameObject.name = entry.prefabObject == null ? "Spawn System Entry" : entry.prefabObject.name;
                        }

                    GUILayout.EndHorizontal();
                }
            GUILayout.EndVertical();

            if(deleteIndex != -1)
            {
                SpawnableEntry deleteEntry = spawnSys.entries[deleteIndex];
                if (deleteEntry != null)
                {
                    DestroyImmediate(deleteEntry.gameObject);
                }

                spawnSys.entries.RemoveAt(deleteIndex);
            }

            GUI.color = Color.yellow;
                if(GUILayout.Button("Add Entry"))
                {
                    GameObject entryObj = new GameObject("Spawn System Entry");
                    entryObj.transform.parent = spawnSys.transform;
                    SpawnableEntry newEntry = entryObj.AddComponent<SpawnableEntry>();

                    spawnSys.entries.Add(newEntry);
                }
            GUI.color = Color.white;

            GUI.color = Color.cyan;
                if(GUILayout.Button("Rebuild Pools"))
                {
                    spawnSys.entries.ForEach(s => s.RefreshSpawnPool());
                }
            GUI.color = Color.white;

            if(EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(spawnSys.gameObject);
            }
        }
    }
}
#endif