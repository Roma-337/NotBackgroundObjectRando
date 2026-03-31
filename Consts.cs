using System;
using System.Collections.Generic;

namespace NotBackgroundObjectRando {
    public class Consts {
        public const string prefix = "BgObj-";

        public static readonly List<Type> types = [
            typeof(Breakable),
            typeof(BreakablePoleSimple),
            typeof(GrassSpriteBehaviour),
            typeof(GrassBehaviour),
            typeof(InfectedBurstLarge),
            typeof(BreakableInfectedVine),
            typeof(JellyEgg),
            typeof(PlayMakerFSM),
            typeof(TownGrass),
            typeof(HealthManager)
        ];
    }
}
