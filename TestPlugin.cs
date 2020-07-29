using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Harmony;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using ShinyShoe;
using System.IO;

namespace MutatorProgressMod
{
    [BepInPlugin("MutatorProgressMod", "Mutator Progress Mod", "0.0.1")]
    [BepInProcess("MonsterTrain.exe")]
    [BepInProcess("MtLinkHandler.exe")]
    public class MutatorProgress : BaseUnityPlugin
    {
        void Awake()
        {
            var harmony = new Harmony("MutatorProgressMod");
            harmony.PatchAll();
            Console.WriteLine("MutatorProgressMod 0.0.1");
        }

    }

    [HarmonyPatch(typeof(SaveManager), "FinishRun")]
    public class XPForCustoms
    {
        public static void Prefix(SaveManager __instance)
        {
            if (__instance.HasMainClass() && __instance.GetRunType() == RunType.Custom)
            {
                SaveData sData = ((SaveData)(AccessTools.Property(typeof(SaveManager), "ActiveSaveData")).GetValue(__instance));
                __instance.AddXPToClass(sData.GetMainClassID(), __instance.GetScore(), true);
                __instance.AddXPToClass(sData.GetSubClassID(), __instance.GetScore(), false);
                __instance.UpdateLifetimeRunStats();
                __instance.SaveMetagame("metagameSave", "metagameSaveBackup");
            }
        }
    }

    [HarmonyPatch(typeof(ClassProgressInfoUI), "ToggleProgressionLocked")]
    public class RemoveCustomWarningText
    {
        [HarmonyPrefix]
        public static void patch(ClassProgressInfoUI __instance, ref bool progressionLocked)
        {
            progressionLocked = false;
        }
    }
}
