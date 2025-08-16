// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Content.Server.DeadSpace.Medieval.Wear.Components;
using Content.Shared.Lathe;

namespace Content.Server.DeadSpace.Medieval.Wear;

public sealed class LatheWearSystem : EntitySystem
{
    [Dependency] private readonly WearSystem _wear = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<LatheWearComponent, LatheQueueRecipeMessage>(OnLatheQueueRecipeMessage);
    }

    private void OnLatheQueueRecipeMessage(EntityUid uid, LatheWearComponent component, LatheQueueRecipeMessage args)
    {
        if (!component.AddPoints.TryGetValue(args.ID, out var currentPoints))
            currentPoints = component.DefaultAddPoints;

        if (TryComp<WearComponent>(uid, out var wearComp))
            _wear.AddWear(uid, currentPoints * args.Quantity, wearComp);
    }

}
