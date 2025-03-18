using ExileCore2;
using ExileCore2.PoEMemory.Components;
using ExileCore2.PoEMemory.Elements;
using Newtonsoft.Json.Linq;
using ShowPoE1ModTier.Classes;
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
        if (rPanel == null) return;
        if (uiHover?.Address == 0) return;

        var inventoryItemIcon = uiHover?.AsObject<HoverItemIcon>();
        if (inventoryItemIcon?.Tooltip == null)
            return;

        var modList = ITier.ModList();
        if (modList == null) return;

        var tooltip = inventoryItemIcon.Tooltip;
        var pEntity = inventoryItemIcon.Item;
        if (tooltip == null || pEntity == null || pEntity.Address == 0) return;

        var iInfoBase = pEntity.GetComponent<Base>();
        var iInfo = iInfoBase.Info;
        
        var iTags = iInfoBase.Info.BaseItemTypeDat.MoreTagsFromPath[0];
        if (iTags == "flask" || iTags == "currency" || iTags == "map" || iTags == "soul_core" ||
            iTags == "pinnacle" || iTags == "gem" || iTags == "expedition" || iTags == "sanctum" ||
            iTags == "relic" || iTags == "" || iTags == "ultimatum" || iTags == "tower_augment" || iTags == "jewel") return;

        var compMods = pEntity.GetComponent<Mods>();
        var isIdentified = compMods.Identified;
        if (!isIdentified) return;
        var iRarity = compMods.ItemRarity.ToString();
        if (iRarity == "Unique") return;

        var tooltipRect = tooltip.GetClientRect();
        var bottomLefttRect = tooltipRect.BottomLeft;
        var bottomRightRect = tooltipRect.BottomRight;
        Vector2N charSize = new Vector2N(0.0f, 15.0f);

        string iBase = iInfo.BaseItemTypeDat.ClassName;
        string iType = iInfo.BaseItemTypeDat.ClassName.ToLower();

        if (iInfo.BaseItemTypeDat.MoreTagsFromPath[0] == "armour")
            iType = iInfo.BaseItemTypeDat.Tags[0];
        var iBasesplit = iBase.Split(" ");
        if (iBasesplit.Length == 3) iType = iBasesplit[2].ToLower();
        var iMods = compMods.ExplicitMods;
        var jBase = modList.GetValue(iBase).ToObject<JObject>();
        var jType = jBase.GetValue(iType).ToObject<JObject>();

        
   
    

        foreach (var iMod in iMods)
        {

            string iTier;
            if (iMod.ModRecord.AffixType.ToString() == "Prefix")
            {
                var jPrefix = jType.GetValue("Prefix").ToObject<JObject>();
                iTier = ITier.ItemTier(pEntity, iMod, jPrefix);
            }
            else
            {
                var jSuffix = jType.GetValue("Suffix").ToObject<JObject>();
                iTier = ITier.ItemTier(pEntity, iMod, jSuffix);
            }

            Graphics.DrawTextWithBackground(iTier, bottomLefttRect, Color.White, Color.Black);
            bottomLefttRect += charSize;

        }

    }

}



