using System;
using System.Collections.Generic;
using UnityEngine;

namespace r.e.p.o_cheat
{
    static class PlayerController
    {
        public static object playerSpeedInstance;
        static private object reviveInstance;
        static private object enemyDirectorInstance;

        public static void GodMode()
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp"); 
            if (playerControllerType != null)
            {
                Hax2.Log1("PlayerController found.");

                var playerControllerInstance = GameHelper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    var playerAvatarScriptField = playerControllerInstance.GetType().GetField("playerAvatarScript", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    if (playerAvatarScriptField != null)
                    {
                        var playerAvatarScriptInstance = playerAvatarScriptField.GetValue(playerControllerInstance);

                        var playerHealthField = playerAvatarScriptInstance.GetType().GetField("playerHealth", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                        if (playerHealthField != null)
                        {
                            var playerHealthInstance = playerHealthField.GetValue(playerAvatarScriptInstance);

                            var godModeField = playerHealthInstance.GetType().GetField("godMode", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                            if (godModeField != null)
                            {
                                bool currentGodMode = (bool)godModeField.GetValue(playerHealthInstance);

                                bool newGodModeState = !currentGodMode;
                                godModeField.SetValue(playerHealthInstance, newGodModeState);

                                Hax2.godModeActive = !newGodModeState;

                                Hax2.Log1("God Mode " + (newGodModeState ? "enabled" : "disabled"));
                            }
                            else
                            {
                                Hax2.Log1("godMode field not found in playerHealth.");
                            }
                        }
                        else
                        {
                            Hax2.Log1("playerHealth field not found in playerAvatarScript.");
                        }
                    }
                    else
                    {
                        Hax2.Log1("playerAvatarScript field not found in PlayerController.");
                    }
                }
                else
                {
                    Hax2.Log1("playerControllerInstance not found.");
                }
            }
            else
            {
                Hax2.Log1("PlayerController type not found.");
            }
        }


        public static void RemoveSpeed(float sliderValue)
        {
            var playerInSpeedType = Type.GetType("PlayerController, Assembly-CSharp"); 
            if (playerInSpeedType != null)
            {
                Hax2.Log1("playerInSpeedType n é null");
                playerSpeedInstance = GameHelper.FindObjectOfType(playerInSpeedType);
                if (playerSpeedInstance != null)
                {
                    Hax2.Log1("playerSpeedInstance n é null");
                }
                else
                {
                    Hax2.Log1("playerSpeedInstance null");
                }
            }
            else
            {
                Hax2.Log1("playerInSpeedType null");
            }
            if (playerSpeedInstance != null)
            {
                Hax2.Log1("playerSpeedInstance n é null");

                var playerControllerType = playerSpeedInstance.GetType();

                var moveSpeedField1 = playerControllerType.GetField("MoveSpeed", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                if (moveSpeedField1 != null)
                {
                    moveSpeedField1.SetValue(playerSpeedInstance, sliderValue);
                    Hax2.Log1("MoveSpeed value set to " + sliderValue);
                }
                else
                {
                    Hax2.Log1("MoveSpeed field not found in PlayerController.");
                }
            }
        }
        public static void Revive()
        {
            var playerDeathHeadType = Type.GetType("PlayerDeathHead, Assembly-CSharp");
            if (playerDeathHeadType != null)
            {
                reviveInstance = GameHelper.FindObjectOfType(playerDeathHeadType);
                if (reviveInstance != null)
                {
                    Hax2.Log1("reviveInstance encontrado.");
                }
                else
                {
                    Hax2.Log1("reviveInstance é null.");
                    return;
                }
            }
            else
            {
                Hax2.Log1("PlayerDeathHead não encontrado.");
                return;
            }

            var enemyDirectorType = Type.GetType("EnemyDirector, Assembly-CSharp");
            if (enemyDirectorType != null)
            {
                enemyDirectorInstance = GameHelper.FindObjectOfType(enemyDirectorType);
                if (enemyDirectorInstance != null)
                {
                    Hax2.Log1("enemyDirectorInstance encontrado.");
                }
                else
                {
                    Hax2.Log1("enemyDirectorInstance é null.");
                    return;
                }
            }
            else
            {
                Hax2.Log1("EnemyDirector não encontrado.");
                return;
            }

            var semiFuncType = Type.GetType("SemiFunc, Assembly-CSharp");
            if (semiFuncType != null)
            {
                enemyDirectorInstance = GameHelper.FindObjectOfType(semiFuncType);
                if (enemyDirectorInstance != null)
                {
                    Hax2.Log1("enemyDirectorInstance encontrado.");
                }
                else
                {
                    Hax2.Log1("enemyDirectorInstance é null.");
                    return;
                }
            }
            else
            {
                Hax2.Log1("EnemyDirector não encontrado.");
                return;
            }

            var reviveMethod = reviveInstance.GetType().GetMethod("Revive");
            var inExtractionPointMethod = reviveInstance.GetType().GetMethod("inExtractionPoint");
            var setInvestigateMethod = enemyDirectorInstance.GetType().GetMethod("SetInvestigate");

            if (setInvestigateMethod != null)
            {
                Hax2.Log1("SetInvestigate chamado.");
            }
            else
            {
                Hax2.Log1("Método SetInvestigate não encontrado.");
            }

            if (inExtractionPointMethod != null)
            {
                inExtractionPointMethod.Invoke(reviveInstance, new object[] { true });
                Hax2.Log1("inExtractionPoint definido para true.");
            }
            else
            {
                Hax2.Log1("Método inExtractionPoint não encontrado.");
            }

            if (reviveMethod != null)
            {
                reviveMethod.Invoke(reviveInstance, null);
                Hax2.Log1("Player revivido com sucesso.");
            }
            else
            {
                Hax2.Log1("Método Revive não encontrado.");
            }
        }

    }
}
