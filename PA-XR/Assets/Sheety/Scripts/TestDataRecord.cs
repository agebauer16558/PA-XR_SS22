/// <summary>
/// Diese Klasse dient als Speicher für die Testdaten.
/// Die Bezeichnungen der einzelnen Eigenschaften müssen mit
/// den Bezeichnungen der Spalten im Spreadsheet übereinstimmen
/// </summary>
[System.Serializable]
public class TestDataRecord
{
    public string testName;
    public string date;
    public string duration;
    public string questionCanteen;
    public int questionLibrary;
}
