using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class BreakableInfectedVineLocation: AutoLocation {
        private static readonly Dictionary<string, BreakableInfectedVineLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookInfectedVine();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookInfectedVine();
        }

        private static void HookInfectedVine() {
            IL.BreakableInfectedVine.OnTriggerEnter2D += GrantCheck;
        }

        private static void UnhookInfectedVine() {
            IL.BreakableInfectedVine.OnTriggerEnter2D -= GrantCheck;
        }

        private static void GrantCheck(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("HeroBox"));
            cursor.GotoNext(i => i.MatchLdarg(0),
                            i => i.MatchLdfld<BreakableInfectedVine>("blobs"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<BreakableInfectedVine>>(vine => {
                string placementName = RandoInterop.GetPlacementName(vine.gameObject);
                if(Ref.Settings.Placements.TryGetValue(placementName, out AbstractPlacement ap)) {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = vine.gameObject.transform
                    };
                    ap.GiveAll(gi);
                }
            });
        }
    }
}
