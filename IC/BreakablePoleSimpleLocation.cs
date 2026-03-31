using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    public class BreakablePoleSimpleLocation: AutoLocation {
        private static readonly Dictionary<string, BreakablePoleSimpleLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookBreakablePole();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookBreakablePole();
        }

        private static void HookBreakablePole() {
            IL.BreakablePoleSimple.OnTriggerEnter2D += GrantCheck;
        }

        private static void UnhookBreakablePole() {
            IL.BreakablePoleSimple.OnTriggerEnter2D -= GrantCheck;
        }

        private static void GrantCheck(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdstr("Hero Spell"));
            cursor.GotoNext(i => i.Match(OpCodes.Brfalse));
            cursor.Index++;
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<BreakablePoleSimple>>(pole => {
                string placementName = RandoInterop.GetPlacementName(pole.gameObject);
                if(Ref.Settings.Placements.TryGetValue(placementName, out AbstractPlacement ap)) {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = pole.gameObject.transform
                    };
                    ap.GiveAll(gi);
                }
            });
        }
    }
}
