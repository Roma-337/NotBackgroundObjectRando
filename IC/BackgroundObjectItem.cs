using ItemChanger;
using ItemChanger.Items;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace NotBackgroundObjectRando {
    public class BackgroundObjectItem: VoidItem {
        public BackgroundObjectItem(string name) {
            this.name = name;
            InteropTag tag = RandoInterop.AddTag(this);
            tag.Properties["PinSprite"] = new EmbeddedSprite("pin_barrel");
            UIDef = new MsgUIDef {
                name = new BoxedString(RandoInterop.DisplayName(name)),
                shopDesc = new BoxedString("I thought this was what no Silksong does to a community, but clearly I was mistaken."),
                sprite = new EmbeddedSprite("pin_barrel")
            };
        }
    }
}
