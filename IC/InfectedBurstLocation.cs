using System.Collections.Generic;
using UnityEngine;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class InfectedBurstLocation: AutoLocation {
        private static readonly Dictionary<string, InfectedBurstLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookInfectedBurst();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookInfectedBurst();
        }

        private static void HookInfectedBurst() {
            On.InfectedBurstLarge.OnTriggerEnter2D += GrantCheck;
        }

        private static void UnhookInfectedBurst() {
            On.InfectedBurstLarge.OnTriggerEnter2D -= GrantCheck;
        }

        private static void GrantCheck(On.InfectedBurstLarge.orig_OnTriggerEnter2D orig, InfectedBurstLarge self, Collider2D otherCollider) {
            if(otherCollider.gameObject.tag == "Nail Attack" || otherCollider.gameObject.tag == "Hero Spell" || (otherCollider.tag == "HeroBox" && HeroController.instance.cState.superDashing)) {
                string placementName = RandoInterop.GetPlacementName(self.gameObject);
                if(Ref.Settings.Placements.TryGetValue(placementName, out AbstractPlacement ap)) {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = self.gameObject.transform
                    };
                    ap.GiveAll(gi);
                }
            }
            orig(self, otherCollider);
        }
    }
}
