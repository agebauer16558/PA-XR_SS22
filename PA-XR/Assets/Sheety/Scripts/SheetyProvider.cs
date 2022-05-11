using System.Collections;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SheetyProvider : MonoBehaviour
{
    // Singleton-Pattern
    public static SheetyProvider Instance;

    // Pfad zum Sheety-Projekt
    public string path;

    // Name des Tabellen-Blattes
    private string sheetName;
    
    // Test Daten - Objekt
    private TestDataRecord currentTestData;

    // Startzeitpunkt
    private Stopwatch stopWatch;

    void Awake()
    {
        if (Instance == null) 
        {
            Instance = this;
        }
    }

    void Start()
    {
        // Kein Pfad angegeben
        if(string.IsNullOrEmpty(path))
        {
            print("ACHTUNG: Es wurde kein Pfad zur Sheety-API angegeben!");
        }
        else
        {
            // Namen des Tabellen-Blattes aus Pfad extrahieren
            int slashIndex = path.LastIndexOf("/");
            sheetName = path.Substring(slashIndex + 1, path.Length - slashIndex - 1);
        }
    }

    /// <summary>
    /// Daten über die Sheety-API versenden
    /// </summary>
    /// <param name="jsonString">Erzeugter JSON-String</param>
    /// <returns></returns>
    public IEnumerator Post(string jsonString) 
    {
        //  Pfad angegeben?
        if(!string.IsNullOrEmpty(path))
        {
            // Pfad mit Sheet-Namen verbinden
            var request = new UnityWebRequest(path, "POST");
            
            byte[] bodyRaw = Encoding.UTF8.GetBytes(FormatJsonData(jsonString));
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            // Keine Fehlermeldung erhalten
            if (string.IsNullOrEmpty(request.error)) 
            {
                print("Daten erfolgreich übertragen");
            } 
            else 
            {
                print("Es ist ein Fehler aufgetreten: " + request.error);
            }   
        } 
        else 
        {
            print("Um Daten zu übertragen müssen Sie einen Pfad angeben!");
        }    
    }

    /// <summary>
    /// Wandelt den übergebenen JSON-String in einen
    /// für die Sheety-API passenden JSON-String um.
    /// (Kapsulung in ein weiteres JSON-Objekt notwendig)
    /// </summary>
    /// <returns>{sheety-Prefix + { ursp. JSON-String }}</returns>
    private string FormatJsonData(string jsonString) 
    {
        string sheetEntryName = sheetName.Substring(0, sheetName.Length - 1);        
        var serializedData = "{\"" + sheetEntryName + "\" : " + jsonString + "}";
        print(serializedData);
        return serializedData;
    }
}
