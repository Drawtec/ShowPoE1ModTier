using ExileCore2.PoEMemory.Components;
using ExileCore2.PoEMemory.Elements;
using ExileCore2.PoEMemory.MemoryObjects;
using ExileCore2.Shared;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ShowPoE1ModTier.Classes
{
    public class InventoryScaner
    {
        public static string StashTabName { get; set; } = "";
        public static List<Item> StashItems { get; set; }
        public static List<Item> InventoryScan(Inventory inventory)
        {
            StashTabName = inventory.Address.ToString();
            List<Item> allitems = [];
            var uiItems = inventory.VisibleInventoryItems;
            if (uiItems == null) return null;
            foreach (var uiItem in uiItems)
            {
                if (uiItem.Item.Type.ToString() != "Item") continue;
                var item = uiItem.Entity;
                var modList = ITier.ModList();
                if (modList == null) continue;

                var modsComp = item.GetComponent<Mods>();
                if (modsComp == null) continue;
                var iMods = modsComp.ExplicitMods;
                var baseComp = item.GetComponent<Base>();
                var iInfo = baseComp.Info;
                var iTags = iInfo.BaseItemTypeDat.MoreTagsFromPath[0];

                if (iTags == "flask" || iTags == "currency" || iTags == "map" || iTags == "soul_core" ||
                    iTags == "pinnacle" || iTags == "gem" || iTags == "expedition" || iTags == "sanctum" ||
                    iTags == "relic" || iTags == "" || iTags == "ultimatum" || iTags == "tower_augment" || iTags == "jewel") continue;

                if (modsComp == null) continue;
                var isIdentified = modsComp.Identified;
                if (!isIdentified) continue;
                var iRarity = modsComp.ItemRarity.ToString();
                if (iRarity == "Unique") continue;

                string iBase = iInfo.BaseItemTypeDat.ClassName;
                string iType = iInfo.BaseItemTypeDat.ClassName.ToLower();

                if (iInfo.BaseItemTypeDat.MoreTagsFromPath[0] == "armour")
                    iType = iInfo.BaseItemTypeDat.Tags[0];
                var iBasesplit = iBase.Split(" ");
                if (iBasesplit.Length == 3) iType = iBasesplit[2].ToLower();


                var jBase = modList.GetValue(iBase).ToObject<JObject>();
                var jType = jBase.GetValue(iType).ToObject<JObject>();
                var iPosition = uiItem.GetClientRect();
                var topLeft = iPosition.TopLeft;
                var topRight = iPosition.TopRight;

                List<IMod> sMod = [];
                foreach (var iMod in iMods)
                {

                    var modRecord = iMod.ModRecord;
                    var affixeType = modRecord.AffixType.ToString();
                    string affixeChar;
                    if (affixeType == "Prefix") affixeChar = "P";
                    else if (affixeType == "Suffix") affixeChar = "S";
                    else continue;
                        var modGroup = modRecord.TypeName;
                    var rawModName = iMod.RawName;
                    int rawModTier = int.Parse(Regex.Match(rawModName, @"\d+").Value);
                    var jAffixeType = jType.GetValue(affixeType).ToObject<JObject>();
                    var jModGroup = jAffixeType.GetValue(modGroup);

                    var imodGroup = jModGroup.ToList<JToken>();
                    var poeModTier = (imodGroup.Count + 1) - rawModTier;
                    var maxTiers = imodGroup.Count;
                    sMod.Add(new IMod()
                    {
                        MinMaxStat = iMod.ValuesMinMax,
                        Stat = iMod.Values,
                        MaxTier = maxTiers,
                        AffixeChar = affixeChar,
                        AffixeType = affixeType,
                        ModName = iMod.Name,
                        ModGroup = modGroup,
                        ModTier = poeModTier
                    });
                }
                Item item1 = new()
                {
                    PositionTR = topRight,
                    PositionTL = topLeft,
                    Iid = item.Id,
                    IName = modsComp.UniqueName,
                    IMods = sMod
                };
                allitems.Add(item1);

            }



            StashItems = allitems;
            return allitems;
        }
    }
    public class Item
    {

        public string IName { get; set; }
        public uint Iid { get; set; }

        public List<IMod> IMods { get; set; }
        public Vector2 PositionTL { get; set; }
        public Vector2 PositionTR { get; set; }
    }
    public class IMod
    {
        public string AffixeChar { get; set; }
        public string AffixeType { get; set; }
        public string ModName { get; set; }
        public string ModGroup { get; set; }
        public int ModTier { get; set; }
        public int MaxTier { get; set; }
        public List<int> Stat { get; set; }
        public IntRange[] MinMaxStat { get; set; }

    }
}
