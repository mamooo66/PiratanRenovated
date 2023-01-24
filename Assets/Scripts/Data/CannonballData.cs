using SimpleSQL;
[System.Serializable]
public class CannonballData
{
    [PrimaryKey, NotNull, Unique]
    public int CannonballID{ get; set; }
    [Default(0)]
    public int isHealler{ get; set; }
    [Default(20), NotNull]
    public int Effect{ get; set; }
    [Unique, NotNull, Default("name")]
    public string CannonballName{ get; set; }
    [Default(10000), NotNull]
    public int Cost{ get; set; }
}