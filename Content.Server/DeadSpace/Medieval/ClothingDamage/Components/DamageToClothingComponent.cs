// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

namespace Content.Server.DeadSpace.Medieval.ClothingDamage.Components;

[RegisterComponent]
public sealed partial class DamageToClothingComponent : Component
{
    /// <summary>
    ///     Порог урона через который будет наноситься урон по одежде.
    /// </summary>
    [DataField]
    public float DamageThreshold = 10f;

    /// <summary>
    ///     Какие слоты проверять
    /// </summary>
    [DataField]
    public List<string> Slots = new List<string>() { "head", "outerClothing", "neck", "gloves", "shoes", "jumpsuit", "underwearb" };

}
