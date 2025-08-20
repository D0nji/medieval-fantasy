// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Content.Server.DeadSpace.Medieval.ClothingDamage.Components;
using Content.Server.DeadSpace.Medieval.Wear;
using Content.Server.DeadSpace.Medieval.Wear.Components;
using Content.Server.Inventory;
using Content.Shared.Damage;
using Robust.Shared.Random;

namespace Content.Server.DeadSpace.Medieval.ClothingDamage;

public sealed class DamageToClothingSystem : EntitySystem
{
    [Dependency] private readonly ServerInventorySystem _inventory = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly WearSystem _wear = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<DamageToClothingComponent, BeforeDamageChangedEvent>(OnBeforeDamageChanged);
    }

    private void OnBeforeDamageChanged(EntityUid uid, DamageToClothingComponent component, ref BeforeDamageChangedEvent args)
    {
        if (args.Damage == null)
            return;

        var damage = args.Damage.GetTotal().Float();
        int clothCount = (int)(damage / component.DamageThreshold);

        if (clothCount <= 0)
            return;

        // Собираем все доступные вещи с WearComponent
        var wearables = new List<(EntityUid entity, WearComponent wearComp)>();

        foreach (var slot in component.Slots)
        {
            if (_inventory.TryGetSlotContainer(uid, slot, out var containerSlot, out _)
                && containerSlot.ContainedEntity is EntityUid ent
                && TryComp<WearComponent>(ent, out var wearComp))
            {
                wearables.Add((ent, wearComp));
            }
        }

        if (wearables.Count == 0)
            return;

        // Наносим урон случайным вещам clothCount раз
        for (int i = 0; i < clothCount; i++)
        {
            var pick = _random.Next(0, wearables.Count);
            var (ent, wearComp) = wearables[pick];
            _wear.AddWear(ent, -1, wearComp);
        }
    }

}
