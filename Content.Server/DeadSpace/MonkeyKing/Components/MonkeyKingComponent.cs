// Мёртвый Космос, Licensed under custom terms with restrictions on public hosting and commercial use, full text: https://raw.githubusercontent.com/dead-space-server/space-station-14-fobos/master/LICENSE.TXT

using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
using Robust.Shared.Audio;

namespace Content.Server.DeadSpace.MonkeyKing.Components;

[RegisterComponent]
public sealed partial class MonkeyKingComponent : Component
{
    [DataField("actionArmy", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ActionArmy = "ActionArmy";

    [DataField]
    public EntityUid? ActionArmyEntity;

    [DataField("actionKingBuff", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ActionKingBuff = "ActionKingBuff";

    [DataField]
    public EntityUid? ActionKingBuffEntity;

    [DataField("actionGiveIntelligence", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ActionGiveIntelligence = "ActionGiveIntelligence";

    [DataField]
    public EntityUid? ActionGiveIntelligenceEntity;

    [DataField("servantMonkeyProto", customTypeSerializer: typeof(PrototypeIdSerializer<EntityPrototype>))]
    public string ServantMonkeyProto = "MobMonkeyServant";

    [DataField]
    public SoundSpecifier? ArmySound = null;

    [DataField]
    public SoundSpecifier? KingBuffSound = null;

    [DataField]
    public SoundSpecifier? GiveIntelligenceSound = null;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public List<string> WeaponList = new List<string>
    {
        "RollingPin",
        "Cane",
        "WeaponRevolverInspector",
        "Stunbaton",
        "GrenadeDummy",
        "SpearBone",
        "Spear",
        "BaseBallBat",
        "KitchenKnife",
        "RitualDagger",
        "Scalpel",
        "Wrench",
        "WelderMini",
        "Saw",
        "Nettle",
        "WeaponPistolMk58",
        "MopItem",
        "ToolboxSyndicate",
        "Bola",
        "Shovel",
        "ThrowingKnife",
        "Shiv",
        "WeaponLaserSvalinn",
        "FireExtinguisher",
        "HydroponicsToolHatchet",
        "Shovel",
        "Crowbar",
        "OxygenTankFilled"
    };

    [DataField]
    public float RangeBuff = 15f;

    [DataField]
    public float BuffDuration = 10f;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public float SpeedBuff = 1.5f;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public float GetDamageBuff = 0.5f;

    [DataField]
    public float GiveIntelligenceDuration = 2f;
}
