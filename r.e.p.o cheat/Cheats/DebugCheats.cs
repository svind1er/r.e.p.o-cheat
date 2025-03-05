using System;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System.Collections.Generic;

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
                        Vector3 spawnPosition = new Vector3(0f, 1f, 0f);
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
        public static void DrawESP()
        {
            var debugAxelType = Type.GetType("EnemyDirector, Assembly-CSharp");
            if (debugAxelType != null)
            {
                var debugAxelInstance = GameHelper.FindObjectOfType(debugAxelType);
                if (debugAxelInstance != null)
                {
                    GameObject debugEnemyInstance = UnityEngine.Object.Instantiate<GameObject>(
                       AssetManager.instance.debugEnemyInvestigate, new Vector3(0f, 1f, 0f), Quaternion.identity);

                    if (debugEnemyInstance != null)
                    {
                        var enemyDirectoyType = Type.GetType("DebugEnemyInvestigate, Assembly-CSharp");
                        if (enemyDirectoyType != null)
                        {
                            var enemyDirectoyInstance = GameHelper.FindObjectOfType(enemyDirectoyType);
                            var radiusMethod = enemyDirectoyInstance.GetType().GetMethod("radius", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            radiusMethod.Invoke(enemyDirectoyInstance, new object[] { 100f });
                        }
                        else
                        {
                            Hax2.Log1("DebugAxel type not found.");
                        }
                    }
                    else
                    {
                        Hax2.Log1("Falha ao instanciar o DebugEnemyInvestigate.");
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
