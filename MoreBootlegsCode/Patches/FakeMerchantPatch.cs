// fake merchant relic pool is hardcoded, so we need to patch it with harmony

using HarmonyLib;
using System.Reflection;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace MoreBootlegs.MoreBootlegsCode;

[HarmonyPatch(typeof(FakeMerchant))]
public static class FakeMerchantPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(MethodType.StaticConstructor)]
    public static void PatchStaticCtor()
    {
        var field = typeof(FakeMerchant).GetField(
            "_inventoryRelics",
            BindingFlags.Static | BindingFlags.NonPublic
        );
        ArgumentNullException.ThrowIfNull(field);

        var existing = (RelicModel[]?)field.GetValue(null);
        ArgumentNullException.ThrowIfNull(existing);

        var additions = new RelicModel[]
        {
            ModelDb.Relic<FakeBronzeScales>(),
            ModelDb.Relic<FakeAkabeko>(),

            ModelDb.Relic<FakeHornCleat>(),
            ModelDb.Relic<FakeCaptainsWheel>(),

            ModelDb.Relic<FakePotionBelt>(),
        };

        var combined = existing
            .Concat(additions)
            .ToArray();

        field.SetValue(null, combined);
    }
}
