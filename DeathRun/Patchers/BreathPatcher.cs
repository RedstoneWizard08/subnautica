﻿/**
 * DeathRun mod - Cattlesquat "but standing on the shoulders of giants"
 * 
 * Adapted from Seraphim Risen's NitrogenMod
 * * I have moved all of the "nitrogen/bends" code into the single NitroLevPatcher, so I could tune that stuff all in the same place
 * * I have revised and rebalanced the "crush depth" code, mostly to make it "less forgiving", but also to vary the effect more geometrically by the amount the crush depth is exceeded.
 */
namespace DeathRun.Patchers
{
    using HarmonyLib;
    using Items;
    using Common;
    using UnityEngine;

    [HarmonyPatch(typeof(NitrogenLevel))]
    [HarmonyPatch("OnTookBreath")]
    internal class BreathPatcher
    {
        private static bool crushEnabled = false;
        private static bool crushed = false;

        [HarmonyPrefix]
        public static bool Prefix(ref NitrogenLevel __instance, Player player)
        {
            if (GameModeUtils.RequiresOxygen())
            {
                float depthOf = Ocean.main.GetDepthOf(player.gameObject);

                // Player's personal crush depth
                if (crushEnabled) {
                    if (Player.main.GetDepthClass() == Ocean.DepthClass.Crush)
                    {
                        if (!crushed)
                        {
                            ErrorMessage.AddMessage("Personal crush depth exceeded. Return to safe depth!");
                            crushed = true;
                        }                        
                        if (UnityEngine.Random.value < 0.5f)
                        {
                            float crushDepth = PlayerGetDepthClassPatcher.divingCrushDepth;
                            if (depthOf > crushDepth)
                            {
                                float crush = depthOf - crushDepth;
                                if (crush < 50)
                                {
                                    DamagePlayer(4);
                                }
                                else if (crush < 100)
                                {
                                    DamagePlayer(8);
                                }
                                else if (crush < 200)
                                {
                                    DamagePlayer(16);
                                } 
                                else
                                {
                                    DamagePlayer(32); // "Okay, Sparky..."
                                }
                            }
                        }
                    } else
                    {
                        crushed = false;
                    }
                }
            }
            return false;
        }

        private static void DamagePlayer(float ouch)
        {
            LiveMixin component = Player.main.gameObject.GetComponent<LiveMixin>();
            component.TakeDamage(UnityEngine.Random.value * ouch/2 + ouch/2, default, DamageType.Normal, null);
        }

        public static void EnableCrush(bool isEnabled)
        {
            crushEnabled = isEnabled;
        }
    }
}