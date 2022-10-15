using HarmonyLib;
using UnityModManagerNet;
using PavonisInteractive.TerraInvicta;

namespace PopBasedMiltechCalculation
{
    // Based on https://github.com/TROYTRON/ti-mods/blob/main/tutorials/code-mods-with-umm.md, thanks Amineri!
    public class PopBasedMiltechCalculation
    {
        public static bool enabled;
        public static UnityModManager.ModEntry mod;
        public static Settings settings;

        //This is standard code, you can just copy it directly into your mod
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll();

            settings = Settings.Load<Settings>(modEntry);
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;

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

        //Boilerplate code, draws the configurable settings in the UMM
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        //Boilerplate code, saves settings changes to the xml file
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }
    }

    //Settings class to interface with Unity Mod Manager
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        [Draw("Use flat values for Armies & Navies?", Collapsible = true)] public bool useFlatValues = false;
        [Draw("[non-flat] Percentage of population an Army is equal to", Collapsible = true)] public float armyMultiplier = 0.5f;
        [Draw("[non-flat] Percentage of population a Navy is equal to", Collapsible = true)] public float navyMultiplier = 0.5f;
        [Draw("[flat] Millions of population an Army is equal to", Collapsible = true)] public float armyFlatValue = 5f;
        [Draw("[flat] Millions of population a Navy is equal to", Collapsible = true)] public float navyFlatValue = 5f;

        //Boilerplate code to save your settings to a Settings.xml file when changed
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        //Hook to allow to do things when a value is changed, if you want
        public void OnChange()
        {
        }
    }

    [HarmonyPatch(typeof(TINationState), nameof(TINationState.AbsorbNation))]
    public class AbsorbNationMiltechCalculationPatch
    {
        static void Prefix(out float __state, TINationState __instance, TIFactionState actingFaction, TINationState joiningNationState)
        {
            __state = 0f;
            if (PopBasedMiltechCalculation.enabled)
            {
                float joiningNationStatePopulationAndArmiesMultiplier;
                float thisNationStatePopulationAndArmiesMultiplier;
                if (PopBasedMiltechCalculation.settings.useFlatValues)
                {
                    joiningNationStatePopulationAndArmiesMultiplier = joiningNationState.population_Millions + (joiningNationState.numArmies * PopBasedMiltechCalculation.settings.armyFlatValue) + (joiningNationState.numNavies * PopBasedMiltechCalculation.settings.navyFlatValue);
                    thisNationStatePopulationAndArmiesMultiplier = __instance.population_Millions + (__instance.numArmies * PopBasedMiltechCalculation.settings.armyFlatValue) + (__instance.numNavies * PopBasedMiltechCalculation.settings.navyFlatValue);
                }
                else
                {
                    joiningNationStatePopulationAndArmiesMultiplier = joiningNationState.population_Millions + (joiningNationState.numArmies * PopBasedMiltechCalculation.settings.armyMultiplier * joiningNationState.population_Millions) + (joiningNationState.numNavies * PopBasedMiltechCalculation.settings.navyMultiplier * joiningNationState.population_Millions);
                    thisNationStatePopulationAndArmiesMultiplier = __instance.population_Millions + (__instance.numArmies * PopBasedMiltechCalculation.settings.armyMultiplier * __instance.population_Millions) + (__instance.numNavies * PopBasedMiltechCalculation.settings.navyMultiplier * __instance.population_Millions);

                }
                __state = (float)((joiningNationState.militaryTechLevel * joiningNationStatePopulationAndArmiesMultiplier + __instance.militaryTechLevel * thisNationStatePopulationAndArmiesMultiplier) / (thisNationStatePopulationAndArmiesMultiplier + joiningNationStatePopulationAndArmiesMultiplier));
            }
        }

        static void Postfix(float __state, TINationState __instance)
        {
            if (PopBasedMiltechCalculation.enabled)
            {
                var nationState = Traverse.Create(__instance);
                FileLog.Log($"[PopBasedMiltechCalculation] Game set {__instance.militaryTechLevel}, replaced with {__state}");
                nationState.Property("militaryTechLevel").SetValue(__state);
                __instance.SetDataDirty();
            }
        }
    }
}
