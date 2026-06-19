using BaseLib.Utils;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;

namespace MoreBootlegs.MoreBootlegsCode;

[Pool(typeof(EventRelicPool))]
public class FakeBronzeScales : MoreBootlegsRelic
{
    private const string _customIconPath = "res://MoreBootlegs/images/bronze-scales.png";
    public override string PackedIconPath => _customIconPath;
    protected override string BigIconPath => _customIconPath;
    protected override string PackedIconOutlinePath => _customIconPath;

    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ThornsPower>(1)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ThornsPower>()];

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            Flash();
            await CommonActions.Apply<ThornsPower>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                this
            );
        }
    }
}
