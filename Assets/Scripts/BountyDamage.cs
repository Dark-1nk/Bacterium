using UnityEngine;

public class BountySystem : MonoBehaviour
{
    public GameObject bountyTarget;
    public int bonusDamage = 20;

    public void ApplyBonusDamage()
    {
        if (bountyTarget != null)
        {
            PlayerUnit player = GetComponent<PlayerUnit>();
            EnemyUnit bounty = gameObject.GetComponent<EnemyUnit>();

            if (bounty != null)
            {
                bounty.TakeDamage(bonusDamage);
            }
        }
    }
}
