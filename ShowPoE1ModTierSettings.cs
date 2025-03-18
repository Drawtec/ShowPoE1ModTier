using ExileCore2.Shared.Interfaces;
using ExileCore2.Shared.Nodes;

namespace ShowPoE1ModTier;

public class ShowPoE1ModTierSettings : ISettings
{
    public ShowPoE1ModTierSettings()
    {
        Enable = new ToggleNode(false);
        Debug = new ToggleNode(true);
    }

    private ToggleNode Debug { get; set; }
    public ToggleNode Enable { get; set; }
}

