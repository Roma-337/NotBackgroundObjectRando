using System.Collections.Generic;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class GrassBehaviourLocation: AutoLocation {
        private static readonly Dictionary<string, GrassBehaviourLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookGrassBehaviour();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookGrassBehaviour();
        }

        private static void HookGrassBehaviour() {
            On.GrassBehaviour.CutReact += GrantCheck;
        }

        private static void UnhookGrassBehaviour() {
            On.GrassBehaviour.CutReact -= GrantCheck;
        }

        private static void GrantCheck(On.GrassBehaviour.orig_CutReact orig, GrassBehaviour self, Collider2D collision) {
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
            orig(self, collision);
        }
    }
}
