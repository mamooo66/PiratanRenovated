using SimpleSQL;

[System.Serializable]
public class PlayerData
{
    [PrimaryKey]
    public string playerID{ get; set; }
    [Default(0)]
    public int Gold{ get; set; }
    [Default(0)]
    public int Experience{ get; set; }
    [Default(0)]
    public int Pearl{ get; set; }
    [Default(100)]
    public int requiredExperience{ get; set; }
    [NotNull]
    public string playerName{ get; set; }
    [Default(0)]
    public int shipNo{ get; set; }
    [Default(1)]
    public int health{ get; set; }
    [Default(1)]
    public int level{ get; set; }
    [Default("")]
    public string cannonballData{get; set; }

}