using UnityEngine;

public abstract class NonPlayerCharacterScript : MonoBehaviour, ICharacterScript 
{
    public int[] Position { get; set; }
    public int MoveSpeed { get; set; }

    public abstract void MoveTo(int[] position);
}
