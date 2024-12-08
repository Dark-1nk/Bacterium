using UnityEngine;

public class BountyDamage : MonoBehaviour
{
    public GameObject bountyTargetPrefab; // The specific prefab to deal extra damage to
    public int normalDamage = 10; // Default damage dealt to normal targets
    public int bountyDamage = 20; // Extra damage dealt to the bounty target

    public void DealDamage(GameObject target)
    {
        // Check if the target matches the bounty target prefab
        if (target.CompareTag(bountyTargetPrefab.tag))
        {
            ApplyDamage(target, bountyDamage);
        }
        else
        {
            ApplyDamage(target, normalDamage);
        }
    }

    private void ApplyDamage(GameObject target, int damage)
    {
        UnitHealth enemyUnit = target.GetComponent<UnitHealth>();
        if (enemyUnit != null)
        {
            enemyUnit.TakeDamage(damage);
        }
    }
}
