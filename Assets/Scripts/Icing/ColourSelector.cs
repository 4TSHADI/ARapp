using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectionManager : MonoBehaviour
{
    public static ColorSelectionManager Instance;
    public List<Button> colorButtons;
    public Dictionary<string, (double H, double S, double V)> colors;
    public Action<Color> OnColorChanged;
    public GameObject colourPanel;
    public Button toggleButton;
    private Color selectedColor;
    private int doublepress;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        colors = new Dictionary<string, (double, double, double)>
        {
            { "Coral", (15, 0.6, 1.0) },        
            { "Pink", (340, 0.8, 0.85) },   
            { "Maroon", (350, 0.9, 0.5) },      
            { "Rust", (20, 0.8, 0.6) },        
            { "Gold", (50, 0.7, 0.85) },
            { "Marigold", (40, 1.0, 1.0) },     
            { "Warm Gold", (35, 0.9, 0.7) },   
            { "Chartreuse", (70, 1.0, 1.0) },    
            { "Jade", (140, 0.9, 0.7) },         
            { "Misty Green", (160, 0.3, 0.6) }, 
            { "Avocado", (90, 0.5, 0.5) },      
            { "Moss Green", (80, 0.5, 0.45) },   
            { "Hunter Green", (160, 1.0, 0.3) },
            { "Periwinkle", (240, 0.5, 0.9) },   
            { "Aqua", (180, 0.8, 1.0) },        
            { "Turquoise", (180, 0.9, 0.8) },   
            { "Teal", (180, 1.0, 0.5) },        
            { "Navy Blue", (220, 1.0, 0.4) },    
            { "Lavender", (275, 0.4, 0.85) },    
            { "Mauve", (290, 0.3, 0.7) },        
            { "Mulberry", (320, 0.6, 0.6) },
            { "Purple", (300, 0.8, 0.4) },
            { "Gray", (0, 0, 0.5) },
            { "White", (60, 0.3, 0.98) },
            { "Black", (0, 0, 0) }              
        };


        foreach (var button in colorButtons)
        {
            string colorName = button.name;
            if (colors.ContainsKey(colorName))
            {
                button.onClick.AddListener(() => SelectColor(colorName));
                Debug.Log(colorName + ": " + button.name);
            }
        }
        // Add listener for the toggle button
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleColourPanel);
        }
    }

    void SelectColor(string colorName)
    {
        double H = colors[colorName].H/360.0;
        double S = colors[colorName].S;
        double V = colors[colorName].V;
        selectedColor = Color.HSVToRGB((float)H, (float)S, (float)V);
        OnColorChanged?.Invoke(selectedColor);
        Debug.Log("Selected color: " + selectedColor);

    }

    public Color GetSelectedColor()
    {
        return selectedColor;
    }
    public void ToggleColourPanel()
    {
        bool isActive = colourPanel.activeSelf;
        Debug.Log("Panel was: " + (isActive ? "Active" : "Inactive"));

        colourPanel.SetActive(!isActive); // Toggle panel visibility

        Debug.Log("Panel now: " + (colourPanel.activeSelf ? "Active" : "Inactive"));



    }
}
