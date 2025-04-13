using ExileCore2.Shared.Attributes;
using ExileCore2.Shared.Interfaces;
using ExileCore2.Shared.Nodes;




namespace ShowPoE1ModTier;

public class ShowPoE1ModTierSettings : ISettings
{
    public ShowPoE1ModTierSettings()
    {
        Enable = new ToggleNode(false);

    }


    public ToggleNode Enable { get; set; }
    
    public SMTSettings Features { get; set; } = new SMTSettings();

    [Submenu(CollapsedByDefault = false)]
    public class SMTSettings
    {
        [Menu("Switch between PoE1 and PoE2 ModTier Style")]
        public ToggleNode PoEModSwitch { get; set; } = new ToggleNode(true);

        [Menu("Enable the Inventory/Stash Scanner")]
        public ToggleNode InventoryScanSwitch { get; set; } = new ToggleNode(false);
        [Menu("Enable to ignore QuadStashTabs")]
        public ToggleNode SkipQuadStash { get; set; } = new ToggleNode(false);
        [Menu("Enable to show only T1 Mods")]
        public ToggleNode ShowOnlyT1Mods { get; set; } = new ToggleNode(false);
        [Menu("Scan Stash Items and show good Tier icons on items")]
        public ToggleNode StashScanSwitch { get; set; } = new ToggleNode(false); 
        [Menu("Scan Vendor Items and show good Tier icons on items")]
        public ToggleNode NPCVendorScanSwitch { get; set; } = new ToggleNode(false);
        [Menu("Scan Ritual Items and show good Tier icons on items")]
        public ToggleNode RitualScanSwitch { get; set; } = new ToggleNode(false);
        [Menu("set the Color for T1 Mod")]
        public ColorNode Tier1Mod { get; set; } = new ColorNode(16711935);
        [Menu("set the Color for T2 Mod")]
        public ColorNode Tier2Mod { get; set; } = new ColorNode(4294902015);
        [Menu("set the Color for T3 Mod")]
        public ColorNode Tier3Mod { get; set; } = new ColorNode(4278190335);
        [Menu("set the Background Color")]
        public ColorNode CharBGC { get; set; } = new ColorNode(224);
    }
}

