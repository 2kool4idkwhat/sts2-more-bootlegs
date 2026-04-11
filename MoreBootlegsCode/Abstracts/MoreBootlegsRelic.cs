using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Relics;

namespace MoreBootlegs.MoreBootlegsCode;

public abstract class MoreBootlegsRelic : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Event;

    // to avoid https://discord.com/channels/309399445785673728/1478917653551906947/1492547444846755911
    // (all basegame fake merchant relics do this too)
    public override int MerchantCost => 50;
}
