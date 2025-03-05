using System;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon;
namespace r.e.p.o_cheat
{
    public static class UIHelper
    {
        private static float x, y, width, height, margin, controlHeight, controlDist, nextControlY;
        private static int columns = 3;
        private static int currentColumn = 0;
        private static int currentRow = 0;

        private static float debugX, debugY, debugWidth, debugHeight, debugMargin, debugControlHeight, debugControlDist, debugNextControlY;
        private static int debugCurrentColumn = 0;
        private static int debugCurrentRow = 0;
        private static int debugColumns = 1;

        private static GUIStyle debugLabelStyle = null;

        public static void Begin(string text, float _x, float _y, float _width, float _height, float _margin, float _controlHeight, float _controlDist)
        {
            x = _x;
            y = _y;
            width = _width;
            height = _height;
            margin = _margin;
            controlHeight = _controlHeight;
            controlDist = _controlDist;
            nextControlY = y + margin;

            GUI.Box(new Rect(x, y, width, height), text);
        }

        public static void BeginDebugMenu(string text, float _x, float _y, float _width, float _height, float _margin, float _controlHeight, float _controlDist)
        {
            debugX = _x;
            debugY = _y;
            debugWidth = _width;
            debugHeight = _height;
            debugMargin = _margin;
            debugControlHeight = _controlHeight;
            debugControlDist = _controlDist;
            debugNextControlY = debugY + debugMargin;

            GUI.Box(new Rect(debugX, debugY, debugWidth, debugHeight), text);

            if (debugLabelStyle == null)
            {
                debugLabelStyle = new GUIStyle(GUI.skin.label)
                {
                    wordWrap = true,
                    clipping = TextClipping.Clip
                };
            }
        }

        private static Rect NextControlRect(float? customX = null, float? customY = null)
        {
            float controlX = customX ?? (x + margin + currentColumn * ((width - (columns + 1) * margin) / columns));
            float controlY = customY ?? nextControlY;

            Rect rect = new Rect(controlX, controlY, ((width + 500) / columns) - 2 * margin, controlHeight);

            currentColumn++;
            if (currentColumn >= columns)
            {
                currentColumn = 0;
                currentRow++;
                nextControlY += controlHeight + controlDist;
            }

            return rect;
        }

        private static Rect NextDebugControlRect()
        {
            float controlX = debugX + debugMargin + debugCurrentColumn * (debugWidth / debugColumns);
            float controlY = debugNextControlY + debugCurrentRow * (debugControlHeight + debugControlDist);

            Rect rect = new Rect(controlX, controlY, debugWidth - debugMargin * 2, debugControlHeight);

            debugCurrentColumn++;
            if (debugCurrentColumn >= debugColumns)
            {
                debugCurrentColumn = 0;
                debugCurrentRow++;
                debugNextControlY += debugControlHeight + debugControlDist;
            }

            return rect;
        }

        public static string MakeEnable(string text, bool state)
        {
            return $"{text}{(state ? "ON" : "OFF")}";
        }

        public static void Label(string text, float? customX = null, float? customY = null)
        {
            GUI.Label(NextControlRect(customX, customY), text);
        }

        public static bool Button(string text, float? customX = null, float? customY = null)
        {
            return GUI.Button(NextControlRect(customX, customY), text);
        }

        public static float Slider(float val, float min, float max, float? customX = null, float? customY = null)
        {
            float newValue = GUI.HorizontalSlider(NextControlRect(customX, customY), val, min, max);
            return Mathf.Round(newValue);
        }

        public static void DebugLabel(string text)
        {
            Rect rect = NextDebugControlRect();
            float textHeight = debugLabelStyle.CalcHeight(new GUIContent(text), rect.width);
            if (textHeight > debugControlHeight)
            {
                rect.height = textHeight; 
                debugNextControlY += (textHeight - debugControlHeight); 
            }
            GUI.Label(rect, text, debugLabelStyle);
        }

        public static void ResetGrid()
        {
            currentColumn = 0;
            currentRow = 0;
        }

        public static void ResetDebugGrid()
        {
            debugCurrentColumn = 0;
            debugCurrentRow = 0;
            debugNextControlY = debugY + debugMargin;
        }
    }

    public class Hax2 : MonoBehaviour
    {
        private float nextUpdateTime = 0f;
        private const float updateInterval = 5f;

        private int selectedPlayerIndex = 0;
        private List<string> playerNames = new List<string>(); 
        private List<object> playerList = new List<object>();
        private float oldSliderValue = 0.5f;
        private float sliderValue = 0.5f;
        public static float offsetESp = 0.5f;
        private bool showMenu = true;
        public static bool godModeActive = false;
        public static List<DebugLogMessage> debugLogMessages = new List<DebugLogMessage>();

        private bool showDebugMenu = false;

        public void Start()
        {
            Cursor.visible = showMenu;
            DebugCheats.texture2 = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            DebugCheats.texture2.SetPixel(0, 0, Color.red);
            DebugCheats.texture2.SetPixel(1, 0, Color.red);
            DebugCheats.texture2.SetPixel(0, 1, Color.red);
            DebugCheats.texture2.SetPixel(1, 1, Color.red);
            DebugCheats.texture2.Apply();
            var playerHealthType = Type.GetType("PlayerHealth, Assembly-CSharp");
            if (playerHealthType != null)
            {
                Log1("playerHealthType n é null");
                Health_Player.playerHealthInstance = FindObjectOfType(playerHealthType);
                if (Health_Player.playerHealthInstance != null)
                {
                    Log1("playerHealthInstance n é null");
                }
                else
                {
                    Log1("playerHealthInstance null");
                }
            }
            else
            {
                Log1("playerHealthType null");
            }

            var playerMaxHealth = Type.GetType("ItemUpgradePlayerHealth, Assembly-CSharp");
            if (playerMaxHealth != null)
            {
                Health_Player.playerMaxHealthInstance = FindObjectOfType(playerMaxHealth);
                Log1("playerMaxHealth n é null");
            }
            else
            {
                Log1("playerMaxHealth null");
            }
        }

        public void UnloadCheat()
        {
            Destroy(this.gameObject);
            Health_Player.playerHealthInstance = null;
            PlayerController.playerSpeedInstance = null;
            Health_Player.playerMaxHealthInstance = null;
            System.GC.Collect();
        }

        public void Update()
        {
            if (Time.time >= nextUpdateTime)
            {
                DebugCheats.UpdateEnemyList();
                Hax2.Log1("Lista de inimigos atualizada!");
                nextUpdateTime = Time.time + updateInterval; 
            }

            if(oldSliderValue != sliderValue)
            {
                PlayerController.RemoveSpeed(sliderValue);
                oldSliderValue = sliderValue;
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                showMenu = !showMenu;

                Cursor.visible = showMenu;
                Cursor.lockState = showMenu ? CursorLockMode.None : CursorLockMode.Locked;
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                Start();
            }
            if (Input.GetKeyDown(KeyCode.F10))
            {
                UnloadCheat();
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                showDebugMenu = !showDebugMenu;
            }

            for (int i = debugLogMessages.Count - 1; i >= 0; i--)
            {
                if (Time.time - debugLogMessages[i].timestamp > 3f)
                {
                    debugLogMessages.RemoveAt(i);
                }
            }
        }
        private void UpdatePlayerList()
        {
            playerNames.Clear();
            playerList.Clear();

            var players = SemiFunc.PlayerGetList(); 

            foreach (var player in players)
            {
                playerList.Add(player);
                string playerName = SemiFunc.PlayerGetName(player) ?? "Unknown Player";
                playerNames.Add(playerName);
            }

            if (playerNames.Count == 0)
            {
                playerNames.Add("No player Found");
            }
        }
        private void ReviveSelectedPlayer()
        {
            if (selectedPlayerIndex < 0 || selectedPlayerIndex >= playerList.Count)
            {
                Log1("Índice de jogador inválido!");
                return;
            }

            var selectedPlayer = playerList[selectedPlayerIndex];

            var playerDeathHeadField = selectedPlayer.GetType().GetField("playerDeathHead", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (playerDeathHeadField != null)
            {
                var playerDeathHeadInstance = playerDeathHeadField.GetValue(selectedPlayer);
                if (playerDeathHeadInstance != null)
                {
                    var inExtractionPointField = playerDeathHeadInstance.GetType().GetField("inExtractionPoint", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    var reviveMethod = playerDeathHeadInstance.GetType().GetMethod("Revive");

                    if (inExtractionPointField != null)
                    {
                        inExtractionPointField.SetValue(playerDeathHeadInstance, true);
                    }

                    reviveMethod?.Invoke(playerDeathHeadInstance, null);
                    Log1("Jogador revivido: " + playerNames[selectedPlayerIndex]);
                }
                else
                {
                    Log1("Instância de playerDeathHead não encontrada.");
                }
            }
            else
            {
                Log1("Campo playerDeathHead não encontrado.");
            }
        }

        private void KillSelectedPlayer()
        {
            if (selectedPlayerIndex < 0 || selectedPlayerIndex >= playerList.Count)
            {
                Log1("Índice de jogador inválido!");
                return;
            }

            var selectedPlayer = playerList[selectedPlayerIndex];
            if (selectedPlayer == null)
            {
                Log1("Jogador selecionado é nulo!");
                return;
            }

            try
            {
                Log1($"Tipo do jogador selecionado: {selectedPlayer.GetType().FullName}");

                var playerHealthField = selectedPlayer.GetType().GetField("playerHealth", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (playerHealthField != null)
                {
                    var playerHealthInstance = playerHealthField.GetValue(selectedPlayer);
                    if (playerHealthInstance != null)
                    {
                        var healthType = playerHealthInstance.GetType();

                        var hurtMethod = healthType.GetMethod("Hurt", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        if (hurtMethod != null)
                        {
                            if (PhotonNetwork.IsMasterClient)
                            {
                                var photonViewField = selectedPlayer.GetType().GetField("photonView", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                if (photonViewField != null && photonViewField.GetValue(selectedPlayer) is PhotonView photonView)
                                {
                                    hurtMethod.Invoke(playerHealthInstance, new object[] { 9999, true, -1 });
                                    photonView.RPC("Hurt", RpcTarget.All, 9999, true, -1);
                                    Log1($"Jogador {playerNames[selectedPlayerIndex]} morto com 9999 de dano via RPC");
                                }
                                else
                                {
                                    hurtMethod.Invoke(playerHealthInstance, new object[] { 9999, true, -1 });
                                    Log1($"Jogador {playerNames[selectedPlayerIndex]} morto localmente com 9999 de dano (sem PhotonView)");
                                }
                            }
                            else
                            {
                                Log1("Somente o Master Client pode matar outros jogadores!");
                            }
                        }
                        else
                        {
                            Log1("Método 'Hurt' não encontrado em playerHealth");
                        }
                    }
                    else
                    {
                        Log1("Instância de playerHealth é nula");
                    }
                }
                else
                {
                    Log1("Campo 'playerHealth' não encontrado no jogador selecionado");
                }
            }
            catch (Exception e)
            {
                Log1($"Erro ao matar jogador {playerNames[selectedPlayerIndex]}: {e.Message}");
            }
        }

        public void OnGUI()
        {
            if (DebugCheats.drawEspBool)
            {
                DebugCheats.DrawESP();
                Thread.Sleep(1);
            }
            UIHelper.ResetGrid();

            // Título do menu
            GUI.Label(new Rect(10, 10, 200, 30), "DARK CHEAT | DEL - MENU");

            if (showMenu)
            {
                UIHelper.ResetGrid();
                UIHelper.Begin("DARK Menu", 50, 50, 500, 900, 30, 30, 10);

                UIHelper.Label("Press F5 to reload!", 70, 80);
                UIHelper.Label("Press DEL to close menu!", 225, 80);
                UIHelper.Label("Press F10 to unload!", 410, 80);

                if (UIHelper.Button("Heal Player", 170, 130))
                {
                    Health_Player.HealPlayer(50);
                    Debug.Log("Player healed.");
                }
                if (UIHelper.Button("Damage Player", 170, 180))
                {
                    Health_Player.DamagePlayer(1);
                    Debug.Log("Player damaged.");
                }
                if (UIHelper.Button("Infinite Health", 170, 230))
                {
                    Health_Player.MaxHealth();
                    Debug.Log("Infinite health activated.");
                }
                if (UIHelper.Button("God Mode " + (godModeActive ? "ON" : "OFF"), 170, 280))
                {
                    PlayerController.GodMode();
                    godModeActive = !godModeActive;
                    Debug.Log("God mode toggled: " + godModeActive);
                }

                UpdatePlayerList();

                UIHelper.Label("Select a player to Revive:", 220, 320);
                selectedPlayerIndex = GUI.SelectionGrid(new Rect(170, 350, 272, 100), selectedPlayerIndex, playerNames.ToArray(), 1);

                if (UIHelper.Button("Revive", 170, 480))
                {
                    ReviveSelectedPlayer();
                    Debug.Log("Player revived: " + playerNames[selectedPlayerIndex]);
                }

                if (UIHelper.Button("Spawn Money", 170, 530))
                {
                    DebugCheats.SpawnItem();
                    Debug.Log("Money spawned.");
                }

                UIHelper.Label("Speed Value " + sliderValue, 170, 580);
                oldSliderValue = sliderValue;
                sliderValue = UIHelper.Slider(sliderValue, 1f, 30f, 170, 600);
                
                if (UIHelper.Button("Enemy ESP", 170, 650))
                {
                    DebugCheats.drawEspBool = !DebugCheats.drawEspBool;
                    Debug.Log("ESP toggled: " + DebugCheats.drawEspBool);
                }
                if (UIHelper.Button("Infinity Stamina", 170, 700))
                {
                    PlayerController.MaxStamina();
                }
                if (UIHelper.Button("Send Player To Void", 170, 750))
                {
                    PlayerController.SendFirstPlayerToVoid();
                }
                if (UIHelper.Button("Kill All Enemies", 170, 800))
                {
                    DebugCheats.KillAllEnemies();
                }
                if (UIHelper.Button("Kill Selected Player", 170, 850))
                {
                    KillSelectedPlayer();
                    Debug.Log("Tentativa de matar o jogador selecionado realizada.");
                }
            }

            if (showDebugMenu)
            {
                UIHelper.ResetDebugGrid();
                UIHelper.BeginDebugMenu("Debug Log", 600, 50, 500, 500, 30, 30, 10);

                UIHelper.Label("Press F12 to close debug log", 630, 70);

                foreach (var logMessage in debugLogMessages)
                {
                    if (!string.IsNullOrEmpty(logMessage.message))
                    {
                        UIHelper.DebugLabel(logMessage.message);
                    }
                }
            }
        }
        public static void Log1(string message)
        {
            debugLogMessages.Add(new DebugLogMessage(message, Time.time));
        }

        public class DebugLogMessage
        {
            public string message;
            public float timestamp;

            public DebugLogMessage(string msg, float time)
            {
                message = msg;
                timestamp = time;
            }
        }
    }
}
