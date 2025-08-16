// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

namespace Content.Server.DeadSpace.Medieval.Wear.Components;

[RegisterComponent]
public sealed partial class LatheWearComponent : Component
{
    /// <summary>
    ///     Стандартное количество добавляемых очков за один произведённый продукт
    /// </summary>
    [DataField]
    public int DefaultAddPoints = 0;

    /// <summary>
    ///     Количество добавляемых очков за еденицу заданного произведённого продукта
    /// </summary>
    [DataField]
    public Dictionary<string, int> AddPoints { get; set; } = new Dictionary<string, int>();
}
