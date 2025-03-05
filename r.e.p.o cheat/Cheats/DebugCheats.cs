using System;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System.Collections.Generic;

namespace r.e.p.o_cheat
{
    static class DebugCheats
    {
        public static bool drawEspBool = false;
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
        private static List<object> enemyList = new List<object>();

        public static void UpdateEnemyList()
        {
            enemyList.Clear();
            Hax2.Log1("Atualizando lista de inimigos");

            var enemyDirectorType = Type.GetType("EnemyDirector, Assembly-CSharp");
            if (enemyDirectorType != null)
            {
                Hax2.Log1("EnemyDirector encontrado");
                var enemyDirectorInstance = enemyDirectorType.GetField("instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);

                if (enemyDirectorInstance != null)
                {
                    Hax2.Log1("Instância de EnemyDirector obtid");
                    var enemiesSpawnedField = enemyDirectorType.GetField("enemiesSpawned", BindingFlags.Public | BindingFlags.Instance);

                    if (enemiesSpawnedField != null)
                    {
                        var enemies = enemiesSpawnedField.GetValue(enemyDirectorInstance) as IEnumerable<object>;
                        if (enemies != null)
                        {
                            foreach (var enemy in enemies)
                            {
                                enemyList.Add(enemy);
                            }
                            Hax2.Log1($"Inimigos encontrados: {enemyList.Count}");
                        }
                        else
                        {
                            Hax2.Log1("Nenhum inimigo encontrado em enemiesSpawned");
                        }
                    }
                    else
                    {
                        Hax2.Log1("Campo 'enemiesSpawned' não encontrado");
                    }
                }
                else
                {
                    Hax2.Log1("Instância de EnemyDirector é nula");
                }
            }
            else
            {
                Hax2.Log1("EnemyDirector não encontrado");
            }
        }
        public static void DrawESP()
        {
            foreach (var enemyParent in enemyList)
            {
                if (enemyParent == null) continue;

                Hax2.Log1("Verificando campos dentro de EnemyParent...");

                var enemyField = enemyParent.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);

                if (enemyField != null)
                {
                    Hax2.Log1($"Campo do inimigo encontrado: {enemyField.Name}");

                    var enemyInstance = enemyField.GetValue(enemyParent);
                    if (enemyInstance != null)
                    {
                        Hax2.Log1("Instância do inimigo obtida.");

                        if (enemyInstance is Enemy enemy)
                        {
                            Transform centerTransform = enemy.CenterTransform;

                            if (centerTransform != null)
                            {
                                Vector3 enemyPosition = centerTransform.position;
                                Hax2.Log1($"Posição do inimigo: {enemyPosition}");

                                Vector3 screenPos = Camera.main.WorldToScreenPoint(enemyPosition);

                                if (screenPos.z > 0) 
                                {
                                    float x = screenPos.x;
                                    float y = Screen.height - screenPos.y;
                                    float width = 50;
                                    float height = 100;

                                    GUI.color = Color.red;
                                    GUI.DrawTexture(new Rect(x - width / 2, y - height / 2, width, height), Texture2D.whiteTexture);
                                    Hax2.Log1($"ESP desenhado na posição ({x}, {y}).");
                                }
                            }
                            else
                            {
                                Hax2.Log1("Propriedade 'CenterTransform' é nula.");
                            }
                        }
                        else
                        {
                            Hax2.Log1("Instância do inimigo não é do tipo Enemy.");
                        }
                    }
                    else
                    {
                        Hax2.Log1("Instância do inimigo é nula.");
                    }
                }
                else
                {
                    Hax2.Log1("Nenhum campo de inimigo encontrado em EnemyParent.");
                }
            }
        }
    }
}
