// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

namespace Content.Server.DeadSpace.Medieval.Wear.Components;

[RegisterComponent]
public sealed partial class AddWearAfterUseComponent : Component
{
    /// <summary>
    ///     Количество добавляемых очков
    /// </summary>
    [DataField]
    public int AddPoints = 0;
}
