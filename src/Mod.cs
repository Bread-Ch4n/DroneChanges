using DroneChanges;
using DroneChanges.Preferences;
using HarmonyLib;
using Il2Cpp;
using Il2CppMonomiPark.SlimeRancher.DataModel;
using Il2CppMonomiPark.SlimeRancher.Drone;
using MelonLoader;

[assembly: MelonInfo(
    typeof(Mod),
    "DroneChanges",
    "1.0.1",
    "Bread-Chan",
    "https://www.nexusmods.com/slimerancher2/mods/110"
)]
[assembly: MelonGame("MonomiPark", "SlimeRancher2")]
[assembly: HarmonyDontPatchAll]

namespace DroneChanges;

public class Mod : MelonMod
{
    #region Setup

    public override void OnInitializeMelon()
    {
        PreferenceManager.Init();

        var h = new HarmonyLib.Harmony("com.bread-chan.drone_changes");

        if (PreferenceManager._minimumEnergyLevelToggle!.Value)
            h.PatchAll(typeof(DroneStationGadgetModelGetCurrEnergyPatch));
        if (PreferenceManager._quickDepositGrabToggle!.Value)
            h.PatchAll(typeof(RanchDroneInitPatch));
        MelonLogger.Msg("Initialized.");
    }

    #endregion

    #region Patches

    [HarmonyPatch(typeof(DroneStationGadgetModel), nameof(DroneStationGadgetModel.GetCurrEnergy))]
    public static class DroneStationGadgetModelGetCurrEnergyPatch
    {
        public static void Postfix(
            DroneStationGadgetModel __instance,
            TimeDirector timeDir,
            float energyDepletedPerHour,
            ref float __result
        )
        {
            __result = Math.Clamp(
                PreferenceManager._minimumEnergyLevel != null
                    ? Math.Max(PreferenceManager._minimumEnergyLevel.Value, __result)
                    : __result,
                0,
                1000
            );
            __instance._energyDepleteTime._value = timeDir.HoursFromNow(
                __result / energyDepletedPerHour
            );
        }
    }

    [HarmonyPatch(typeof(RanchDrone), nameof(RanchDrone.Init))]
    public static class RanchDroneInitPatch
    {
        private static void Postfix(RanchDrone __instance)
        {
            var config = __instance.Config;
            config.DepositTimePerItem = 0;
            config.GatherTimePerItem = 0;
        }
    }

    #endregion
}
