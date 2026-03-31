using System;
using UnityEngine;
using ItemChanger.Internal;
using ItemChanger.Modules;

namespace NotBackgroundObjectRando {
    public class NborModule: Module {
        public override void Initialize() {
            On.GameManager.OnNextLevelReady += editScene;
        }

        public override void Unload() {
            On.GameManager.OnNextLevelReady -= editScene;
        }

        private void editScene(On.GameManager.orig_OnNextLevelReady orig, GameManager self) {
            orig(self);
            //this check is not needed as the Module is only conditionally added to itemchanger
            //if(NotBackgroundObjectRando.globalSettings.Enabled && NotBackgroundObjectRando.globalSettings.LockBehindItems) {
                foreach(Type t in Consts.types) {
                    foreach(Component c in GameObject.FindObjectsOfType(t)) {
                        string placementName = RandoInterop.GetPlacementName(c.gameObject);
                        if(Ref.Settings.Placements.ContainsKey(placementName) && RandomizerMod.RandomizerMod.RS.TrackerData.pm.Get(placementName) == 0) {
                            c.gameObject.SetActive(false);
                        }
                    }
                }
            //}
        }
    }
}
