using System;
using System.Collections.Generic;
using UnityEngine;

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
            float controlX = debugX + debugMargin + debugCurrentColumn * (debugWidth / columns);
            float controlY = debugNextControlY + debugCurrentRow * (debugControlHeight + debugControlDist);

            debugCurrentColumn++;

            if (debugCurrentColumn >= columns)
            {
                debugCurrentColumn = 0;
                debugCurrentRow++;
            }

            return new Rect(controlX, controlY, (debugWidth / columns) - debugMargin * 2, debugControlHeight);
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
            GUI.Label(NextDebugControlRect(), text);
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
        }
    }

    public class Hax2 : MonoBehaviour
    {
        private int selectedPlayerIndex = 0;
        private List<string> playerNames = new List<string>(); 
        private List<object> playerList = new List<object>();
        private float sliderValue = 0.5f;
        private bool showMenu = true;
        public static bool godModeActive = false;
        public static List<DebugLogMessage> debugLogMessages = new List<DebugLogMessage>();

        private bool showDebugMenu = false;

        public void Start()
        {
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



        public void OnGUI()
        {
            UIHelper.ResetGrid();

            GUI.Label(new Rect(10, 10, 200, 30), "DARK CHEAT | DEL - MENU");

            if (showMenu)
            {
                UIHelper.ResetGrid();
                UIHelper.Begin("DARK Menu", 50, 50, 500, 800, 30, 30, 10);

                UIHelper.Label("Press F5 to reload!", 70, 80);
                UIHelper.Label("Press DEL to close menu!", 225, 80);
                UIHelper.Label("Press F10 to unload!", 410, 80);

                if (UIHelper.Button("Heal Player", 170, 130))
                {
                    Health_Player.HealPlayer(50);
                }
                if (UIHelper.Button("Damage Player", 170, 180))
                {
                    Health_Player.DamagePlayer(1);
                }
                if (UIHelper.Button("Infinite Health", 170, 230))
                {
                    Health_Player.MaxHealth();
                }
                if (UIHelper.Button("God Mode " + (godModeActive ? "OFF" : "ON"), 170, 280))
                {
                    PlayerController.GodMode();
                }

                // Atualiza a lista de jogadores
                UpdatePlayerList();

                // Exibir dropdown de jogadores
                UIHelper.Label("Select a player to Revive:", 220, 320);
                selectedPlayerIndex = GUI.SelectionGrid(new Rect(170, 350, 272, 100), selectedPlayerIndex, playerNames.ToArray(), 1);

                // Botão para reviver o jogador selecionado
                if (UIHelper.Button("Revive", 170, 480))
                {
                    ReviveSelectedPlayer();
                }

                if (UIHelper.Button("Spawn Money", 170, 530))
                {
                    DebugCheats.SpawnItem();
                }

                UIHelper.Label("Speed Value " + sliderValue, 170, 580);
                sliderValue = UIHelper.Slider(sliderValue, 1f, 30f, 170, 600);

                if (UIHelper.Button("Set Speed", 170, 620))
                {
                    PlayerController.RemoveSpeed(sliderValue);
                }

                if (UIHelper.Button("Enemy ESP", 170, 670))
                {
                    DebugCheats.DrawESP();
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
