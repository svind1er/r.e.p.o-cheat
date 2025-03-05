using System;
using UnityEngine;
using Photon.Pun;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
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
                    Hax2.Log1("Instância de EnemyDirector obtida");
                    var enemiesSpawnedField = enemyDirectorType.GetField("enemiesSpawned", BindingFlags.Public | BindingFlags.Instance);

                    if (enemiesSpawnedField != null)
                    {
                        var enemies = enemiesSpawnedField.GetValue(enemyDirectorInstance) as IEnumerable<object>;
                        if (enemies != null)
                        {
                            foreach (var enemy in enemies)
                            {
                                // Verificar se o inimigo é válido e está ativo
                                if (enemy != null)
                                {
                                    var enemyInstanceField = enemy.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                                          ?? enemy.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                                          ?? enemy.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (enemyInstanceField != null)
                                    {
                                        var enemyInstance = enemyInstanceField.GetValue(enemy) as Enemy;
                                        if (enemyInstance != null && enemyInstance.gameObject != null && enemyInstance.gameObject.activeInHierarchy)
                                        {
                                            enemyList.Add(enemy);
                                        }
                                    }
                                }
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
            Thread.Sleep(1);
        }
        public static void RectFilled(float x, float y, float width, float height, Texture2D text)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), text);
        }
        public static void RectOutlined(float x, float y, float width, float height, Texture2D text, float thickness = 1f)
        {
            RectFilled(x, y, thickness, height, text);
            RectFilled(x + width - thickness, y, thickness, height, text);
            RectFilled(x + thickness, y, width - thickness * 2f, thickness, text);
            RectFilled(x + thickness, y + height - thickness, width - thickness * 2f, thickness, text);
        }
        public static void Box(float x, float y, float width, float height, Texture2D text, float thickness = 2f)
        {
            RectOutlined(x - width / 2f, y - height, width, height, text, thickness);
        }
        public static Texture2D texture2;
        public static void DrawESP()
        {
            if (!drawEspBool) return;

            Camera playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Hax2.Log1("Camera.main não encontrada!");
                return;
            }

            float scaleX = (float)Screen.width / playerCamera.pixelWidth;
            float scaleY = (float)Screen.height / playerCamera.pixelHeight;

            int enemyIndex = 0;

            foreach (var enemyParent in enemyList)
            {
                if (enemyParent == null) continue;

                var enemyField = enemyParent.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);

                if (enemyField != null)
                {
                    var enemyInstance = enemyField.GetValue(enemyParent) as Enemy;
                    if (enemyInstance != null && enemyInstance.gameObject != null && enemyInstance.gameObject.activeInHierarchy && enemyInstance.CenterTransform != null)
                    {
                        Vector3 footPosition = enemyInstance.transform.position;
                        float enemyHeightEstimate = 2f;
                        Vector3 headPosition = enemyInstance.transform.position + Vector3.up * enemyHeightEstimate;

                        Vector3 screenFootPos = playerCamera.WorldToScreenPoint(footPosition);
                        Vector3 screenHeadPos = playerCamera.WorldToScreenPoint(headPosition);

                        if (screenFootPos.z > 0 && screenHeadPos.z > 0)
                        {
                            float footX = screenFootPos.x * scaleX;
                            float footY = Screen.height - (screenFootPos.y * scaleY);
                            float headY = Screen.height - (screenHeadPos.y * scaleY);

                            float height = Mathf.Abs(footY - headY);
                            float enemyScale = enemyInstance.transform.localScale.y;
                            float baseWidth = enemyScale * 200f;
                            float distance = screenFootPos.z;
                            float width = (baseWidth / distance) * scaleX;

                            width = Mathf.Clamp(width, 30f, height * 1.2f);
                            height = Mathf.Clamp(height, 40f, 400f);

                            float x = footX;
                            float y = footY;

                            Box(x, y, width, height, texture2, 1f);

                            float labelWidth = 100f;
                            float labelHeight = 20f;
                            float labelX = x - labelWidth / 2f;
                            float labelY = y - height - labelHeight - 5f;
                            GUI.Label(new Rect(labelX + 28f, labelY, labelWidth, labelHeight), "Enemie");


                            enemyIndex++;
                        }
                        else
                        {
                            Hax2.Log1("Inimigo ou parte dele atrás da câmera");
                        }
                    }
                }
            }
            Thread.Sleep(1);
        }
        public static void KillAllEnemies()
        {
            Hax2.Log1("Tentando matar todos os inimigos");

            foreach (var enemyParent in enemyList)
            {
                if (enemyParent == null) continue;

                var enemyField = enemyParent.GetType().GetField("enemyInstance", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("Enemy", BindingFlags.NonPublic | BindingFlags.Instance)
                                 ?? enemyParent.GetType().GetField("childEnemy", BindingFlags.NonPublic | BindingFlags.Instance);

                if (enemyField != null)
                {
                    var enemyInstance = enemyField.GetValue(enemyParent);
                    if (enemyInstance != null)
                    {
                        try
                        {
                            var healthField = enemyInstance.GetType().GetField("Health", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            if (healthField != null)
                            {
                                var healthComponent = healthField.GetValue(enemyInstance);
                                if (healthComponent != null)
                                {
                                    var healthType = healthComponent.GetType();

                                    var hurtMethod = healthType.GetMethod("Hurt", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    if (hurtMethod != null)
                                    {
                                        hurtMethod.Invoke(healthComponent, new object[] { 9999, Vector3.zero });
                                        Hax2.Log1($"Inimigo ferido com 9999 de dano via Hurt");
                                    }
                                    else
                                    {
                                        Hax2.Log1("Método 'Hurt' não encontrado em EnemyHealth");
                                    }
                                }
                                else
                                {
                                    Hax2.Log1("Componente EnemyHealth é nulo");
                                }
                            }
                            else
                            {
                                Hax2.Log1("Campo 'Health' não encontrado em Enemy");
                            }
                        }
                        catch (Exception e)
                        {
                            Hax2.Log1($"Erro ao matar inimigo: {e.Message}");
                        }
                    }
                    else
                    {
                        Hax2.Log1("Inimigo inválido ou inativo");
                    }
                }
            }

            UpdateEnemyList();
        }
    }
}
