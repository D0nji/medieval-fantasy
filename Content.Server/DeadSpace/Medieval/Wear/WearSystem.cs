// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Content.Server.DeadSpace.Medieval.Wear.Components;
using Content.Shared.Examine;
using Robust.Server.Audio;

namespace Content.Server.DeadSpace.Medieval.Wear;

public sealed class WearSystem : EntitySystem
{
    [Dependency] private readonly AudioSystem _audio = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<WearComponent, ExaminedEvent>(OnExamine);
    }

    private void OnExamine(EntityUid uid, WearComponent component, ExaminedEvent args)
    {
        args.PushMarkup(Loc.GetString("wear-exm-info", ("points", component.CurrentPoints.ToString())));
    }

    public void AddWear(EntityUid uid, int ammount, WearComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        component.CurrentPoints += ammount;
        Update(uid, component);
    }

    public void Update(EntityUid uid, WearComponent? component = null)
    {
        if (!Resolve(uid, ref component, false))
            return;

        component.CurrentPoints = Math.Min(component.MaxPoints, component.CurrentPoints);

        if (component.CurrentPoints <= 0)
        {
            if (component.BreakSound != null)
                _audio.PlayPvs(component.BreakSound, Transform(uid).Coordinates);

            QueueDel(uid);
        }
    }


}
