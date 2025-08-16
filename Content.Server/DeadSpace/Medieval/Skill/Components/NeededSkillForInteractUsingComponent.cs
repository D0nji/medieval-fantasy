// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

namespace Content.Server.DeadSpace.Medieval.Skill.Components;

[RegisterComponent]
public sealed partial class NeededSkillForInteractUsingComponent : Component
{
    /// <summary>
    ///     Требуемые навыки
    /// </summary>
    [DataField]
    public List<string> NeededSkills;
}
