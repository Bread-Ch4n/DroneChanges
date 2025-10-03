using MelonLoader;
using MelonLoader.Utils;

namespace DroneChanges.Preferences;

public class PreferenceManager
{
    private static readonly string PreferencePath = Path.Combine(
        MelonEnvironment.UserDataDirectory
    );

    public static MelonPreferences_Category? _droneChangesCategory;
    public static MelonPreferences_Entry<bool>? _minimumEnergyLevelToggle;
    public static MelonPreferences_Entry<float>? _minimumEnergyLevel;

    public static MelonPreferences_Entry<bool>? _quickDepositGrabToggle;

    public static void Init()
    {
        Directory.CreateDirectory(PreferencePath);

        var configFile = Path.Combine(PreferencePath, "DroneChanges.cfg");

        _droneChangesCategory = MelonPreferences.CreateCategory("Drone_Changes", "Drone Changes");
        _droneChangesCategory.SetFilePath(configFile, true, false);

        _minimumEnergyLevelToggle = _droneChangesCategory.CreateEntry(
            "minimum_energy_level_toggle",
            false,
            "Minimum energy level toggle"
        );
        _minimumEnergyLevel = _droneChangesCategory.CreateEntry<float>(
            "minimum_energy_level",
            100,
            "Minimum energy level"
        );

        _quickDepositGrabToggle = _droneChangesCategory.CreateEntry(
            "quick_deposit_grab_toggle",
            false,
            "Quick Deposit & Grab toggle"
        );
    }
}
