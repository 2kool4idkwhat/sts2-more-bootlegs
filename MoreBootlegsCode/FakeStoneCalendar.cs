using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Rooms;

namespace MoreBootlegs.MoreBootlegsCode;

[Pool(typeof(EventRelicPool))]
public class FakeStoneCalendar : MoreBootlegsRelic
{
    private const string _customIconPath = "res://MoreBootlegs/images/stone-calendar.png";
    public override string PackedIconPath => _customIconPath;
    protected override string BigIconPath => _customIconPath;
    protected override string PackedIconOutlinePath => _customIconPath;

    private const string _damageTurnKey = "DamageTurn";

    private bool _isActivating;

    public override bool ShowCounter => DisplayAmount > -1;

    public override int DisplayAmount
    {
        get
        {
            if (!CombatManager.Instance.IsInProgress)
            {
                return -1;
            }
            if (IsCanonical)
            {
                return -1;
            }
            int intValue = DynamicVars["DamageTurn"].IntValue;
            if (IsActivating)
            {
                return intValue;
            }
            ArgumentNullException.ThrowIfNull(Owner.Creature.CombatState);
            int roundNumber = Owner.Creature.CombatState.RoundNumber;
            if (roundNumber >= intValue)
            {
                return -1;
            }
            return roundNumber;
        }
    }

    private bool IsActivating
    {
        get
        {
            return _isActivating;
        }
        set
        {
            AssertMutable();
            _isActivating = value;
            InvokeDisplayAmountChanged();
        }
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(20, ValueProp.Unpowered),
        new DynamicVar("DamageTurn", 7)
    ];

    public override Task AfterSideTurnStart(CombatSide side, CombatState combatState)
    {
        if (side != Owner.Creature.Side)
        {
            return Task.CompletedTask;
        }
        if (combatState.RoundNumber == DynamicVars["DamageTurn"].IntValue)
        {
            Status = RelicStatus.Active;
        }
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override async Task BeforeTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == Owner.Creature.Side)
        {
            ArgumentNullException.ThrowIfNull(Owner.Creature.CombatState);

            int intValue = DynamicVars["DamageTurn"].IntValue;
            int roundNumber = Owner.Creature.CombatState.RoundNumber;
            Status = RelicStatus.Normal;
            if (roundNumber == intValue)
            {
                await TaskHelper.RunSafely(DoActivateVisuals());
                await CreatureCmd.Damage(choiceContext, Owner.Creature.CombatState.HittableEnemies, DynamicVars.Damage, Owner.Creature);
                InvokeDisplayAmountChanged();
            }
        }
    }

    public override Task AfterCombatEnd(CombatRoom _)
    {
        Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    public override Task AfterRoomEntered(AbstractRoom room)
    {
        if (!(room is CombatRoom))
        {
            return Task.CompletedTask;
        }
        Status = RelicStatus.Normal;
        InvokeDisplayAmountChanged();
        return Task.CompletedTask;
    }

    private async Task DoActivateVisuals()
    {
        IsActivating = true;
        Flash();
        await Cmd.Wait(1);
        IsActivating = false;
    }

}
