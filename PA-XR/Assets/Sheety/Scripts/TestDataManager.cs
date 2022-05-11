using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Diese Klasse steuert den Testprozess,
/// die relevanten Daten werden gespeichert und 
/// zum Versenden an den SheetyProvider übergeben
/// </summary>
public class TestDataManager : MonoBehaviour
{
    // Container (GameObject) unter dem alle Panels
    // hängen müssen
    public GameObject panelContainer;

    // Daten-Objekt für Testdaten
    private TestDataRecord currentTestData;

    // Stoppuhr für die Testdauer
    private Stopwatch stopWatch;

    // Alle Panels (GameObjects) die Durchgeschaltet
    // werden können
    private List<GameObject> panels;

    // Index des gerade aktiven Panels
    private int currentPanelIndex;

    void Start()
    {
        // Panels vorhanden? 
        if (panelContainer.transform.childCount > 0)
        {
            panels = new List<GameObject>();
            for (int i = 0; i < panelContainer.transform.childCount; i++)
            {
                panels.Add(panelContainer.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            print("ACHTUNG: Sie haben keine Panels angelegt!");
        }

        // Fragebogen in Ausgangszustand bringen 
        // (= Erstes Panel angeschaltet, andere ausschalten)
        currentPanelIndex = 0;
        panels[0].SetActive(true);
        for (int i = 1; i < panels.Count; i++)
        {
            panels[i].SetActive(false);
        }
    }

    /// <summary>
    /// Daten für den Test vorbereiten, 
    /// z.B. Daten-Objekt initialisieren,
    /// Stoppuhr starten, etc.
    /// </summary>
    public void StartTest()
    {
        // Neues Test-Daten-Objekt erzeugen
        currentTestData = new TestDataRecord();

        // Startzeitpunkt merken
        currentTestData.date = DateTime.Now.ToString();

        // Verwendung des Szenennames als Name für den Test
        currentTestData.testName = SceneManager.GetActiveScene().name;

        // Stoppuhr initialisieren und starten
        stopWatch = new Stopwatch();
        stopWatch.Start();
    }

    /// <summary>
    /// Test beenden: Daten aus dem UI lesen und speichern,
    /// Daten in JSON-Format serialisieren und zum Versenden geben
    /// </summary>
    public void StopTest()
    {
        // Stoppuhr anhalten
        stopWatch.Stop();
        
        // Daten aus dem UI holen
        GatherData();

        // Daten-Objekt in JSON-String serialisieren
        string jsonString = JsonUtility.ToJson(currentTestData);

        // JSON-String mit Sheety versenden
        StartCoroutine(SheetyProvider.Instance.Post(jsonString));
    }

    public void NextQuestion()
    {
        // Index hochzählen
        currentPanelIndex++;
        // Index auf gültigen Bereich beschränken
        currentPanelIndex = Mathf.Clamp(currentPanelIndex, 0, panels.Count-1);
        for (int i = 0; i < panels.Count; i++)
        {
            if(i == currentPanelIndex)
            {
                panels[i].SetActive(true);
            } 
            else
            {
                panels[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Alle notwendigen Daten im Test-Daten-Objekt speichern
    /// </summary>
    private void GatherData()
    {            
        // Vergangene Zeit in Text umwandeln und formatieren
        TimeSpan ts = stopWatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

        // Dauer im Test-Daten-Objekt eintragen
        currentTestData.duration = elapsedTime;

        // Daten aus UI: Zurodnung der Panels zum Testdaten-Objekt
        // currentTestData.questionCanteen = GetLabelOfSelectedToggle(panels[1]);
        // currentTestData.questionLibrary = GetIndexOfSelectedToggle(panels[2]);
    }


    /// <summary>
    /// Liefert die gewählte Option (Toggle) für eine Toogle-Group
    /// und gibt den Text der gewählten Option zurück
    /// </summary>
    private string GetLabelOfSelectedToggle(GameObject panel)
    {
        // Liegt eine Toggle Group auf dem Panel?
        ToggleGroup group = panel.GetComponent<ToggleGroup>();

        // Toogle Group gefunden?
        if (group != null)
        {
            Toggle[] toggles = group.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].isOn)
                {
                    return toggles[i].GetComponentInChildren<Text>().text;
                }
            }
        }

        // Keine Toogle-Group gefunden oder kein aktiver Toogle
        return String.Empty;
    }

    /// <summary>
    /// Liefert die gewählte Option (Toggle) für eine Toogle-Group
    /// und gibt den Index (Bei 0 beginnend) der gewählten Option zurück
    /// </summary>
    private int GetIndexOfSelectedToggle(GameObject panel)
    {
        // Liegt eine Toggle Group auf dem Panel?
        ToggleGroup group = panel.GetComponent<ToggleGroup>();

        // Toogle Group gefunden?
        if (group != null)
        {
            Toggle[] toggles = group.GetComponentsInChildren<Toggle>();

            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].isOn)
                {
                    return i;
                }
            }
        }

        // Keine Toogle-Group gefunden oder kein aktiver Toogle
        return -1;
    }
}
