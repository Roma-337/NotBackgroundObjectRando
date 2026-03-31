using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class JellyEggLocation: AutoLocation {
        private static readonly Dictionary<string, JellyEggLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookJellyEggs();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookJellyEggs();
        }

        private static void HookJellyEggs() {
            On.JellyEgg.Burst += GrantCheck;
        }

        private static void UnhookJellyEggs() {
            On.JellyEgg.Burst -= GrantCheck;
        }

        private static void GrantCheck(On.JellyEgg.orig_Burst orig, JellyEgg self) {
            if(self.bomb) {
                orig(self);
                return;
            }
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
            orig(self);
        }
    }
}
