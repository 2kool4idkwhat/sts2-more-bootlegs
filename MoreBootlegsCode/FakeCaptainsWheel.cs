using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoreBootlegs.MoreBootlegsCode;

[Pool(typeof(EventRelicPool))]
public class FakeCaptainsWheel : MoreBootlegsRelic
{
    private const string _customIconPath = "res://MoreBootlegs/images/captains-wheel.png";
    public override string PackedIconPath => _customIconPath;
    protected override string BigIconPath => _customIconPath;
    protected override string PackedIconOutlinePath => _customIconPath;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new BlockVar(6, ValueProp.Unpowered)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.Block)];

    public override async Task AfterBlockCleared(Creature creature)
    {
        ArgumentNullException.ThrowIfNull(Owner.PlayerCombatState);
        if (creature == Owner.Creature && Owner.PlayerCombatState.TurnNumber == 3)
        {
            Flash();
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, null);
        }
    }
}
