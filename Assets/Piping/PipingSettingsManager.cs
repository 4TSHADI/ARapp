using System.Collections.Generic;
using UnityEngine;
using static ShapeAndTypeHandler;

public class PipingSettingsManager : MonoBehaviour
{
    public static PipingSettingsManager Instance { get; private set; }

    // Use a tuple (PipingType, ShapeType) as the key
    private Dictionary<(PipingType, ShapeType), PipingSettings> settingsDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        settingsDictionary = new Dictionary<(PipingType, ShapeType), PipingSettings>();
    }

    public void SaveSettings(PipingType pipingType, ShapeType shapeType, PipingSettings settings)
    {
        // Use a tuple as the key
        settingsDictionary[(pipingType, shapeType)] = settings;
    }

    public PipingSettings LoadSettings(PipingType pipingType, ShapeType shapeType)
    {
        // Use a tuple as the key
        if (settingsDictionary.ContainsKey((pipingType, shapeType)))
        {
            return settingsDictionary[(pipingType, shapeType)];
        }
        return new PipingSettings(); // Return default settings if not found
    }
}