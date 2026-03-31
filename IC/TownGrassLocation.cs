using System.Collections.Generic;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class TownGrassLocation: AutoLocation {
        private static readonly Dictionary<string, TownGrassLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookTownGrass();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookTownGrass();
        }

        private static void HookTownGrass() {
            On.TownGrass.OnTriggerEnter2D += GrantCheck;
        }

        private static void UnhookTownGrass() {
            On.TownGrass.OnTriggerEnter2D -= GrantCheck;
        }

        private static void GrantCheck(On.TownGrass.orig_OnTriggerEnter2D orig, TownGrass self, Collider2D collision) {
            if(GrassCut.ShouldCut(collision)) {
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
            }
            orig(self, collision);
        }
    }
}
