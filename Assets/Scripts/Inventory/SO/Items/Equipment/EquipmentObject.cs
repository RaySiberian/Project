public abstract class EquipmentObject :ItemObject
{
    public Buff Buff;
    public EquipmentType EquipmentType;
    protected override void Awake()
    {
        base.Awake();
        Type = ItemObjectType.Equipment;
        StackAble = false;
        MaxStuckSize = -1;
        
        Buff.AttackBonus = 0;
        Buff.AttackRangeBonus = 0;
        Buff.MineMultiplyBonus = 0;
        Buff.MineRangeBonus = 0;
    }
}

public enum EquipmentType
{
    Head = 0,
    Torso = 1,
    Lags = 2,
    Feet = 3,
    Hands = 4,
    Fingers = 5,
    Neck = 6,
    Weapon = 7,
    Shield = 8,
    All = 9
}