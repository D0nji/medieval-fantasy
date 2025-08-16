// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Robust.Shared.Audio;

namespace Content.Server.DeadSpace.Medieval.Wear.Components;

[RegisterComponent]
public sealed partial class WearComponent : Component
{
    /// <summary>
    ///     Текущие очки износа
    /// </summary>
    [DataField]
    public int CurrentPoints = 0;

    /// <summary>
    ///     Максимум очков износа
    /// </summary>
    [DataField]
    public int MaxPoints = 100;

    [DataField]
    [ViewVariables(VVAccess.ReadOnly)]
    public SoundSpecifier? BreakSound = default!;
}
