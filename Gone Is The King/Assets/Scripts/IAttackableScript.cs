using UnityEngine;

public interface IAttackableScript
{
    public double Health {get;set;}
    public double Strength {get;set;}
    public ArmourScript Armour {get;set;}
}
