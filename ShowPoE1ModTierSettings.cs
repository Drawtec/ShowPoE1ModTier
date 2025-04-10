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

        [Menu("Scan Stash and show good Tier icons on items")]
        public ToggleNode InventoryScanSwitch { get; set; } = new ToggleNode(false);
        [Menu("set the Color for T1 Mod")]
        public ColorNode Tier1Mod { get; set; } = new ColorNode(4294902015);
        [Menu("set the Color for T2 Mod")]
        public ColorNode Tier2Mod { get; set; } = new ColorNode(16711935);
    }
}

