using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace r.e.p.o_cheat
{
    internal class playerColor
    {
        public static void colorRandomizer()
        {
            var colorControllerType = Type.GetType("PlayerAvatar, Assembly-CSharp");
            if (colorControllerType != null)
            {
                cheats.Log("colorControllerType found.");

                var colorControllerInstance = Helper.FindObjectOfType(colorControllerType);
                if (colorControllerInstance != null)
                {
                    cheats.Log("colorControllerInstance found. ");
                    var playerSetColorMethod = colorControllerType.GetMethod("PlayerAvatarSetColor");
                    if (playerSetColorMethod != null)
                    {
                        var colorIndex = new Random().Next(0, 30);
                        playerSetColorMethod.Invoke(colorControllerInstance, new object[] { colorIndex });
                    }
                    else
                    {
                        cheats.Log("PlayerAvatarSetColor method not found in PlayerAvatar.");
                    }
                }
                else
                {
                    cheats.Log("colorControllerInstance not found.");
                }
            }
            else
            {
                cheats.Log("colorControllerType not found.");
            }
        }
    }
}