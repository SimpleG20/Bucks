using System;

[Serializable]
public class UserDataRaw
{
    public float AvailableMoney { get; set; }
    public float Income {  get; set; }
    public float Limit {  get; set; }
    public int LastDay {  get; set; }

    public float AutoSave { get; set; }

    public string[] Names {  get; set; }
    /// <summary>
    /// 0 - Value<br></br>1 - Item Type<br></br>2 - Amount Parceled<br></br>3 - Day<br></br>4 - Month<br></br>5 - Year<br></br>6 - Show Total
    /// <br></br>7 - Subscription<br></br>8 - Credited<br></br>
    /// </summary>
    public float[,] MatrixItems {  get; set; }
}
