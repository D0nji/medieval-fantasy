// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

namespace Content.Server.DeadSpace.Medieval.Skill.Components;

[RegisterComponent]
public sealed partial class MeleeSkillComponent : Component
{
    /// <summary>
    ///     Множитель без какого-либо навыка
    /// </summary>
    [DataField]
    public float DefaultModifier;

    /// <summary>
    ///     Требуемые навыки
    /// </summary>
    [DataField]
    public List<string> Skills;

    /// <summary>
    ///     Множители урона на навык (будет браться максимальный из всех)
    /// </summary>
    [DataField]
    public Dictionary<string, float> DamageModifiers { get; set; } = new Dictionary<string, float>();

}
