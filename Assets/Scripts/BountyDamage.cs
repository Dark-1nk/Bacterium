using UnityEngine;

public class BountySystem : MonoBehaviour
{
    public string targetTag = "SpecialEnemy";
    public int bonusDamage = 20;

    public int GetDamage(string target)
    {
        return target == targetTag ? bonusDamage : 0;
    }
}
