using System;
using UnityEngine;
using Photon.Pun;

namespace r.e.p.o_cheat
{
    static class DebugCheats
    {
        public static void SpawnItem()
        {
            var debugAxelType = Type.GetType("DebugAxel, Assembly-CSharp");
            if (debugAxelType != null)
            {
                var debugAxelInstance = GameHelper.FindObjectOfType(debugAxelType);
                if (debugAxelInstance != null)
                {
                    var spawnObjectMethod = debugAxelType.GetMethod("SpawnObject", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                    if (spawnObjectMethod != null)
                    {
                        GameObject itemToSpawn = AssetManager.instance.surplusValuableSmall;
                        Vector3 spawnPosition = new Vector3(0f, 0f, 0f);
                        string path = "Valuables/";

                        spawnObjectMethod.Invoke(debugAxelInstance, new object[] { itemToSpawn, spawnPosition, path });

                        Hax2.Log1("Item spawned successfully.");
                    }
                    else
                    {
                        Hax2.Log1("SpawnObject method not found in DebugAxel.");
                    }
                }
                else
                {
                    Hax2.Log1("DebugAxel instance not found in the scene.");
                }
            }
            else
            {
                Hax2.Log1("DebugAxel type not found.");
            }
        }
    }
}
