using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ItemChanger;
using ItemChanger.Internal;
using ItemChanger.Locations;

namespace NotBackgroundObjectRando {
    internal class GrassSpriteBehaviourLocation: AutoLocation {
        private static readonly Dictionary<string, GrassSpriteBehaviourLocation> SubscribedLocations = new();

        protected override void OnLoad() {
            if(SubscribedLocations.Count == 0)
                HookGrassSprite();
            SubscribedLocations[UnsafeSceneName] = this;
        }

        protected override void OnUnload() {
            SubscribedLocations.Remove(UnsafeSceneName);
            if(SubscribedLocations.Count == 0)
                UnhookGrassSprite();
        }

        private static void HookGrassSprite() {
            IL.GrassSpriteBehaviour.OnTriggerEnter2D += GrantCheck;
        }

        private static void UnhookGrassSprite() {
            IL.GrassSpriteBehaviour.OnTriggerEnter2D -= GrantCheck;
        }

        private static void GrantCheck(ILContext il) {
            ILCursor cursor = new ILCursor(il).Goto(0);
            cursor.GotoNext(i => i.MatchLdarg(0),
                            i => i.MatchLdfld<GrassSpriteBehaviour>("animator"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.EmitDelegate<Action<GrassSpriteBehaviour>>(gsb => {
                string placementName = RandoInterop.GetPlacementName(gsb.gameObject);
                if(Ref.Settings.Placements.TryGetValue(placementName, out AbstractPlacement ap)) {
                    GiveInfo gi = new() {
                        FlingType = FlingType.Everywhere,
                        Container = Container.Unknown,
                        MessageType = MessageType.Corner,
                        Transform = gsb.gameObject.transform
                    };
                    ap.GiveAll(gi);
                }
            });
        }
    }
}
