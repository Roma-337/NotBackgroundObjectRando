using System.Collections.Generic;

namespace NotBackgroundObjectRando {
    public class BackgroundObjectJson {
        public static List<BackgroundObjectJson> allObjects = new();

        public string scene;
        public string type;
        public string name;
        public float x;
        public float y;
        public bool requiresInfection;

        public void store() {
            allObjects.Add(this);
        }
    }
}
