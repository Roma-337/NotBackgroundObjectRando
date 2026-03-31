using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class FlukeBabyLocation: AutoLocation {
        private static readonly Dictionary<string, FlukeBabyLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookFlukeBabies();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookFlukeBabies();
        }

        private static void HookFlukeBabies() {
            On.HealthManager.OnEnable += GrantCheck;
        }

        private static void UnhookFlukeBabies() {
            On.HealthManager.OnEnable -= GrantCheck;
        }

        private static void GrantCheck(On.HealthManager.orig_OnEnable orig, HealthManager self) {
            orig(self);
            string placementName = RandoInterop.GetPlacementName(self.gameObject);
            if(self.gameObject.name.StartsWith("fluke_baby") && Ref.Settings.Placements.TryGetValue(placementName, out AbstractPlacement ap)) {
                self.OnDeath += () => {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = self.gameObject.transform
                    };
                    ap.GiveAll(gi);
                };
            }
        }
    }
}
