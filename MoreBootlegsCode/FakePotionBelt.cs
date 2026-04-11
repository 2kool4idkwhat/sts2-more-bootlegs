using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;

namespace MoreBootlegs.MoreBootlegsCode;

[Pool(typeof(EventRelicPool))]
public class FakePotionBelt : MoreBootlegsRelic
{
    private const string _customIconPath = "res://MoreBootlegs/images/potion-belt.png";
    public override string PackedIconPath => _customIconPath;
    protected override string BigIconPath => _customIconPath;
    protected override string PackedIconOutlinePath => _customIconPath;

    public override bool HasUponPickupEffect => true;

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("PotionSlots", 1),
    ];

    public override async Task AfterObtained()
    {
        await PlayerCmd.GainMaxPotionCount(DynamicVars["PotionSlots"].IntValue, Owner);
    }
}
