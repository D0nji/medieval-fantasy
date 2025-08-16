// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Robust.Shared.Audio;

namespace Content.Server.DeadSpace.Medieval.Skill.Components;

[RegisterComponent]
public sealed partial class LearnSkillWhenUsingComponent : Component
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
    ///     Длительность изучения (секунд)
    /// </summary>
    [DataField]
    public float Duration;

    [DataField]
    [ViewVariables(VVAccess.ReadOnly)]
    public SoundSpecifier? Sound = default!;
}
