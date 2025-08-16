// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Content.Shared.Whitelist;
using Robust.Shared.Audio;

namespace Content.Server.DeadSpace.Medieval.Skill.Components;

[RegisterComponent]
public sealed partial class LearnSkillWhenMeleeAttackComponent : Component
{
    /// <summary>
    ///     Изучаемые навыки навыки
    /// </summary>
    [DataField]
    public List<string> Skills;

    /// <summary>
    ///     Количество даваемых очков при изучении
    /// </summary>
    [DataField]
    public Dictionary<string, float> Points { get; set; } = new Dictionary<string, float>();

    /// <summary>
    ///     Кого нужно бить чтобы навык прокачивался
    /// </summary>
    [DataField]
    public EntityWhitelist? Whitelist = null;
}
