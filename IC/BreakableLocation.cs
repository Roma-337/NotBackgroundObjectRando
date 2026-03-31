using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class BreakableLocation: AutoLocation {
        private static readonly Dictionary<string, BreakableLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookBreakable();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookBreakable();
        }

        private static void HookBreakable() {
            On.Breakable.Break += GrantCheck;
        }

        private static void UnhookBreakable() {
            On.Breakable.Break -= GrantCheck;
        }

        private static void GrantCheck(On.Breakable.orig_Break orig, Breakable self, float flingAngleMin, float flingAngleMax, float impactMultiplier) {
            string placementName = RandoInterop.GetPlacementName(self.gameObject);
            if(Ref.Settings.Placements.TryGetValue(placementName, out AbstractPlacement ap)) {
                GiveInfo gi = new() {
                    FlingType = FlingType.Everywhere,
                    Container = Container.Unknown,
                    MessageType = MessageType.Corner,
                    Transform = self.transform
                };
                ap.GiveAll(gi);
            }
            orig(self, flingAngleMin, flingAngleMax, impactMultiplier);
        }
    }
}
