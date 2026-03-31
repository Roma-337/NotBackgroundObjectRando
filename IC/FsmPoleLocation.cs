using System.Collections.Generic;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.FsmStateActions;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class FsmPoleLocation: AutoLocation {
        private static readonly Dictionary<string, FsmPoleLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookFsmPoles();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookFsmPoles();
        }

        private static void HookFsmPoles() {
            On.PlayMakerFSM.OnEnable += GrantCheck;
        }

        private static void UnhookFsmPoles() {
            On.PlayMakerFSM.OnEnable -= GrantCheck;
        }

        private static void GrantCheck(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self) {
            orig(self);
            string placementName = RandoInterop.GetPlacementName(self.gameObject);
            if(self.FsmName == "Break Pole" && Ref.Settings.Placements.ContainsKey(placementName)) {
                self.GetState("Check Direction").AddFirstAction(new Lambda(() => {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = self.transform
                    };
                    Ref.Settings.Placements[placementName].GiveAll(gi);
                }));
            }
        }
    }
}
