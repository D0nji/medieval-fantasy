// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Content.Server.DeadSpace.Medieval.Wear.Components;
using Content.Shared.Interaction;
using Content.Shared.Lathe;
using Content.Shared.Weapons.Melee.Events;

namespace Content.Server.DeadSpace.Medieval.Wear;

public sealed class AddWearAfterUseSystem : EntitySystem
{
    [Dependency] private readonly WearSystem _wear = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AddWearAfterUseComponent, MeleeHitEvent>(OnMeleeHit);
        SubscribeLocalEvent<AddWearAfterUseComponent, AfterInteractUsingEvent>(OnAfterInteractUsing);
        SubscribeLocalEvent<AddWearAfterUseComponent, LatheQueueRecipeMessage>(OnLatheQueueRecipeMessage);
    }

    private void OnMeleeHit(EntityUid uid, AddWearAfterUseComponent component, MeleeHitEvent args)
    {
        if (TryComp<WearComponent>(uid, out var wearComp))
            _wear.AddWear(uid, component.AddPoints, wearComp);
    }

    private void OnAfterInteractUsing(EntityUid uid, AddWearAfterUseComponent component, AfterInteractUsingEvent args)
    {
        if (TryComp<WearComponent>(uid, out var wearComp))
            _wear.AddWear(uid, component.AddPoints, wearComp);
    }

    private void OnLatheQueueRecipeMessage(EntityUid uid, AddWearAfterUseComponent component, LatheQueueRecipeMessage args)
    {
        if (TryComp<WearComponent>(uid, out var wearComp))
            _wear.AddWear(uid, component.AddPoints * args.Quantity, wearComp);
    }

}
