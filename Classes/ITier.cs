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

        public static string ItemTier(Entity pEntity,ItemMod iMod, JObject jAffixeType)
        {

            var modRecord = iMod.ModRecord;
            var modName = iMod.Name;
            var modGroup = modRecord.TypeName;
            var rawModName = iMod.RawName;
            int rawModTier = Int32.Parse(Regex.Match(rawModName, @"\d+").Value);      
            
            var jModGroup = jAffixeType.GetValue(modGroup);
            var imodGroup = jModGroup.ToList<JToken>();
            var poeModTier = (imodGroup.Count + 1) - rawModTier;
            var result = modGroup + "  T" + poeModTier.ToString();
            return result;
        }
        public static JObject ModList()
        {


            string json = @"Plugins/Temp/ShowPoE1ModTier/json/mods.json";
            var jsonFile = File.ReadAllText(json);
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
