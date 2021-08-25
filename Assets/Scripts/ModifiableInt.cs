using System;
using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();
[Serializable]
public class ModifiableInt
{
    public List<IModifier> Modifiers = new List<IModifier>();
    [SerializeField] private int baseValue;
    [SerializeField] private int modifiedValue;
    
    public int ModifiedValue
    {
        get => modifiedValue;
        private set => modifiedValue = value;
    }
    public int BaseValue
    {
        get => baseValue;
        set { baseValue = value; UpdateModifiedValue();}
    }

    public event ModifiedEvent ValueModified;

    public ModifiableInt(ModifiedEvent method = null)
    {
        modifiedValue = baseValue;
        if (method != null)
        {
            ValueModified += method;
        }
    }

    public void RegisterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }
    
    public void UnRegisterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }

    public void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        for (int i = 0; i < Modifiers.Count; i++)
        {
            Modifiers[i].AddValue(ref valueToAdd);
        }
        ModifiedValue = baseValue + valueToAdd;
        ValueModified?.Invoke();
    }

    public void AddModifier(IModifier modifier)
    {
        Modifiers.Add(modifier);
        UpdateModifiedValue();
    }
    
    public void RemoveModifier(IModifier modifier)
    {
        Modifiers.Remove(modifier);
        UpdateModifiedValue();
    }
}
