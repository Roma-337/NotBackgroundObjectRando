using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;

namespace NotBackgroundObjectRando {
    internal static class RSMInterop {
        public static void Hook() {
            RandoSettingsManagerMod.Instance.RegisterConnection(new NborProxy());
        }
    }

    internal class NborProxy: RandoSettingsProxy<GlobalSettings, string> {
        public override string ModKey => NotBackgroundObjectRando.instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; } = new EqualityVersioningPolicy<string>(NotBackgroundObjectRando.instance.GetVersion());

        public override void ReceiveSettings(GlobalSettings settings) {
            settings ??= new();
            RandoMenuPage.Instance.bgMEF.SetMenuValues(settings);
        }

        public override bool TryProvideSettings(out GlobalSettings settings) {
            settings = NotBackgroundObjectRando.globalSettings;
            return settings.Enabled;
        }
    }
}
