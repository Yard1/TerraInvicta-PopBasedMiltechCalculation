using HarmonyLib;
using UnityModManagerNet;
using PavonisInteractive.TerraInvicta;

namespace PopBasedMiltechCalculation
{
    public class PopBasedMiltechCalculation
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;

        //This is standard code, you can just copy it directly into your mod
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();
            mod = modEntry;
            modEntry.OnToggle = OnToggle;
            FileLog.Log("[PopBasedMiltechCalculation] Loaded");
            return true;
        }

        //This is also standard code, you can just copy it
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            enabled = value;
            return true;
        }
    }

    [HarmonyPatch(typeof(TINationState), nameof(TINationState.AbsorbNation))]
    public class AbsorbNationMiltechCalculationPatch
    {
        static void Prefix(out float __state, TINationState __instance, TIFactionState actingFaction, TINationState joiningNationState)
        {
            var joiningNationStatePopulationAndArmiesMultiplier = joiningNationState.population + ((joiningNationState.numArmies + joiningNationState.numNavies) * 0.5 * joiningNationState.population);
            var thisNationStatePopulationAndArmiesMultiplier = __instance.population + ((__instance.numArmies + __instance.numNavies) * 0.5 * __instance.population);
            __state = (float)((joiningNationState.militaryTechLevel * joiningNationStatePopulationAndArmiesMultiplier + __instance.militaryTechLevel * thisNationStatePopulationAndArmiesMultiplier) / (thisNationStatePopulationAndArmiesMultiplier + joiningNationStatePopulationAndArmiesMultiplier));
        }

        static void Postfix(float __state, TINationState __instance)
        {
            var nationState = Traverse.Create(__instance);
            FileLog.Log($"[PopBasedMiltechCalculation] Game set {__instance.militaryTechLevel}, replaced with {__state}");
            nationState.Property("militaryTechLevel").SetValue(__state);
            __instance.SetDataDirty();
        }
    }
}
