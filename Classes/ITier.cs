using ExileCore2.PoEMemory.MemoryObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace ShowPoE1ModTier.Classes
{
    class ITier
    {

        public static string ItemTier(ItemMod iMod, JObject jAffixeType, bool tierMode)
        {

            var modRecord = iMod.ModRecord;
            var modGroup = modRecord.TypeName;
            var rawModName = iMod.RawName;

            int rawModTier = Int32.Parse(Regex.Match(rawModName, @"\d+").Value);      
            
            var jModGroup = jAffixeType.GetValue(modGroup);
            var imodGroup = jModGroup.ToList<JToken>();
            var poeModTier = (imodGroup.Count + 1) - rawModTier;
            var maxTiers = imodGroup.Count;
            string result;
            if (tierMode)
            {
                result = modGroup + "  T" + poeModTier.ToString();
            }
            else 
            {
                result = modGroup + "  T" + rawModTier.ToString() + " / " + maxTiers.ToString();
            }

                return result;
        }
        public static JObject ModList()
        {


            string json = @"Plugins/Temp/ShowPoE1ModTier/json/mods.json";
            if(!File.Exists(json)) json = @"Plugins/Compiled/ShowPoE1ModTier/json/mods.json";
            if (File.Exists(json))
            {
                var mods = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(json));
                return mods;
            }
            
                return null;
        }
    }
}
