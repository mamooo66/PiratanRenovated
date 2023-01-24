using SimpleSQL;

[System.Serializable]
public class ShipData
{
    [Default(0)]public int maxHealth;
    [Default(0)]public int cannonCount;
    [Default(0)]public float attackCooldown;
    [Default(0)]public float maxDistance;
    [Default(0)]public float speed;
    [Default(0)]public int hitRate;
}