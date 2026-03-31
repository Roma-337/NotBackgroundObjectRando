using MenuChanger;
using MenuChanger.Extensions;
using MenuChanger.MenuElements;
using MenuChanger.MenuPanels;
using RandomizerMod.Menu;
using static RandomizerMod.Localization;

namespace NotBackgroundObjectRando {
    public class RandoMenuPage {
        internal MenuPage BackgroundRandoPage;
        internal MenuElementFactory<GlobalSettings> bgMEF;
        internal VerticalItemPanel bgVIP;

        internal SmallButton JumpToBGButton;

        internal static RandoMenuPage Instance { get; private set; }

        public static void OnExitMenu() {
            Instance = null;
        }

        public static void Hook() {
            RandomizerMenuAPI.AddMenuPage(ConstructMenu, HandleButton);
            MenuChangerMod.OnExitMainMenu += OnExitMenu;
        }

        private static bool HandleButton(MenuPage landingPage, out SmallButton button) {
            button = Instance.JumpToBGButton;
            return true;
        }

        private void SetTopLevelButtonColor() {
            if(JumpToBGButton != null) {
                JumpToBGButton.Text.color = NotBackgroundObjectRando.globalSettings.Enabled ? Colors.TRUE_COLOR : Colors.DEFAULT_COLOR;
            }
        }

        private static void ConstructMenu(MenuPage landingPage) => Instance = new(landingPage);

        private RandoMenuPage(MenuPage landingPage) {
            BackgroundRandoPage = new MenuPage(Localize("Background Objects"), landingPage);
            bgMEF = new(BackgroundRandoPage, NotBackgroundObjectRando.globalSettings);
            bgVIP = new(BackgroundRandoPage, new(0, 300), 75f, true, bgMEF.Elements);
            Localize(bgMEF);
            foreach(IValueElement e in bgMEF.Elements) {
                e.SelfChanged += obj => SetTopLevelButtonColor();
            }

            JumpToBGButton = new(landingPage, Localize("Background Objects"));
            JumpToBGButton.AddHideAndShowEvent(landingPage, BackgroundRandoPage);
            SetTopLevelButtonColor();
        }
    }
}
