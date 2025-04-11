using ExileCore2;
using ExileCore2.PoEMemory.Components;
using ExileCore2.PoEMemory.Elements;
using ExileCore2.PoEMemory.MemoryObjects;
using Newtonsoft.Json.Linq;
using ShowPoE1ModTier.Classes;
using System.Collections.Generic;
using System.Drawing;
using Vector2N = System.Numerics.Vector2;





namespace ShowPoE1ModTier;

public class ShowPoE1ModTier : BaseSettingsPlugin<ShowPoE1ModTierSettings>
{

    public override void OnLoad()
    {

    }


    public override void Tick()
    {

    }
    public override void Render()
    {

        if (!Settings.Enable) return;

        var uiHover = GameController.Game.IngameState.UIHover;
        var rPanel = GameController.Game.IngameState.IngameUi.OpenRightPanel;
        var lPanel = GameController.Game.IngameState.IngameUi.OpenLeftPanel;
        var npcPanel = GameController.Game.IngameState.IngameUi.SellWindow;
        var ritualPanel = GameController.Game.IngameState.IngameUi.RitualWindow;

        var inventoryItemIcon = uiHover?.AsObject<HoverItemIcon>();
        if (inventoryItemIcon?.Tooltip != null)
        {
            if (rPanel == null) return;
            if (uiHover?.Address == 0) return;
            var modList = ITier.ModList();
            if (modList == null) return;

            var tooltip = inventoryItemIcon.Tooltip;
            var pEntity = inventoryItemIcon.Item;
            if (tooltip == null || pEntity == null || pEntity.Address == 0) return;

            var iInfoBase = pEntity.GetComponent<Base>();
            if (iInfoBase == null) return;
            var iInfo = iInfoBase.Info;

            var iTags = iInfoBase.Info.BaseItemTypeDat.MoreTagsFromPath[0];
            if (iTags == "flask" || iTags == "currency" || iTags == "map" || iTags == "soul_core" ||
                iTags == "pinnacle" || iTags == "gem" || iTags == "expedition" || iTags == "sanctum" ||
                iTags == "relic" || iTags == "" || iTags == "ultimatum" || iTags == "tower_augment" || iTags == "jewel") return;

            var compMods = pEntity.GetComponent<Mods>();
            if (compMods == null) return;
            var isIdentified = compMods.Identified;
            if (!isIdentified) return;
            var iRarity = compMods.ItemRarity.ToString();
            if (iRarity == "Unique") return;

            var tooltipRect = tooltip.GetClientRect();
            var bottomLefttRect = tooltipRect.BottomLeft;
            var bottomLefttRectSuffix = tooltipRect.BottomLeft + new Vector2N(0.0f, 45.0f);
            var charSize = new Vector2N(0.0f, 15.0f);
            string iBase = iInfo.BaseItemTypeDat.ClassName;
            string iType = iInfo.BaseItemTypeDat.ClassName.ToLower();

            if (iInfo.BaseItemTypeDat.MoreTagsFromPath[0] == "armour")
                iType = iInfo.BaseItemTypeDat.Tags[0];
            var iBasesplit = iBase.Split(" ");
            if (iBasesplit.Length == 3) iType = iBasesplit[2].ToLower();
            var iMods = compMods.ExplicitMods;
            var jBase = modList.GetValue(iBase).ToObject<JObject>();
            var jType = jBase.GetValue(iType).ToObject<JObject>();
            var tierMode = Settings.Features.PoEModSwitch;
            foreach (var iMod in iMods)
            {

                string iTier;
                if (iMod.ModRecord.AffixType.ToString() == "Prefix")
                {
                    var jPrefix = jType.GetValue("Prefix").ToObject<JObject>();
                    iTier = ITier.ItemTier(iMod, jPrefix, tierMode);
                    Graphics.DrawTextWithBackground("P " + iTier, bottomLefttRect, Color.White, Color.Black);
                    bottomLefttRect += charSize;
                }
                else
                {
                    var jSuffix = jType.GetValue("Suffix").ToObject<JObject>();
                    iTier = ITier.ItemTier(iMod, jSuffix, tierMode);
                    Graphics.DrawTextWithBackground("S " + iTier, bottomLefttRectSuffix, Color.White, Color.Black);
                    bottomLefttRectSuffix += charSize;
                }

            }
        }
        if (Settings.Features.InventoryScanSwitch)
        {
            Inventory visiblePanel = new();
            List<Item> iScaner;
            if (lPanel == null && npcPanel == null && ritualPanel == null) return;

            if (lPanel.IsVisible && Settings.Features.StashScanSwitch)
                visiblePanel = GameController.Game.IngameState.IngameUi.StashElement.VisibleStash;
            else if (npcPanel.IsVisible && Settings.Features.NPCVendorScanSwitch)
                visiblePanel = GameController.Game.IngameState.IngameUi.PurchaseWindowHideout.TabContainer.VisibleStash;
            else if (ritualPanel.IsVisible && Settings.Features.RitualScanSwitch)
                visiblePanel = visiblePanel = GameController.Game.IngameState.IngameUi.RitualWindow.InventoryElement;
            else return;

            if (visiblePanel == null) return;
            var visibleStashName = visiblePanel.Address.ToString();

            if (InventoryScaner.StashTabName != visibleStashName)
            {
                iScaner = InventoryScaner.InventoryScan(visiblePanel);
                if (iScaner == null) return;

            }
            else
            {

                iScaner = InventoryScaner.StashItems;
                foreach (var item in iScaner)
                {
                    var charSize = new Vector2N(0.0f, 15.0f);
                    var spos = item.PositionTL + new Vector2N(20.0f, 0.0f);
                    var rpos = item.PositionTR - new Vector2N(5.0f, 0.0f);
                    foreach (var mod in item.IMods)
                    {

                        if (mod.AffixeChar == "P")
                        {
                            if (mod.ModTier == 1)
                            {
                                Graphics.DrawTextWithBackground(mod.AffixeChar + "1", spos, Settings.Features.Tier1Mod, ExileCore2.Shared.Enums.FontAlign.Right, Settings.Features.CharBGC);
                                spos += charSize;
                            }
                            else if (mod.ModTier == 2)
                            {
                                Graphics.DrawTextWithBackground(mod.AffixeChar + "2", spos, Settings.Features.Tier2Mod, ExileCore2.Shared.Enums.FontAlign.Right, Settings.Features.CharBGC);
                                spos += charSize;
                            }
                            else if (mod.ModTier == 3)
                            {
                                Graphics.DrawTextWithBackground(mod.AffixeChar + "3", spos, Settings.Features.Tier3Mod, ExileCore2.Shared.Enums.FontAlign.Right, Settings.Features.CharBGC);
                                spos += charSize;
                            }
                            else continue;
                        }
                        if (mod.AffixeChar == "S")
                        {
                            if (mod.ModTier == 1)
                            {
                                Graphics.DrawTextWithBackground(mod.AffixeChar + "1", rpos, Settings.Features.Tier1Mod, ExileCore2.Shared.Enums.FontAlign.Right, Settings.Features.CharBGC);
                                rpos += charSize;
                            }
                            else if (mod.ModTier == 2)
                            {
                                Graphics.DrawTextWithBackground(mod.AffixeChar + "2", rpos, Settings.Features.Tier2Mod, ExileCore2.Shared.Enums.FontAlign.Right, Settings.Features.CharBGC);
                                rpos += charSize;
                            }
                            else if (mod.ModTier == 3)
                            {
                                Graphics.DrawTextWithBackground(mod.AffixeChar + "3", rpos, Settings.Features.Tier3Mod, ExileCore2.Shared.Enums.FontAlign.Right, Settings.Features.CharBGC);
                                rpos += charSize;
                            }
                            else continue;
                        }
                        continue;
                    }
                }

            }
        }
    }

}



