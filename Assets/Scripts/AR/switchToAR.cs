using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToAR : MonoBehaviour
{
    public Button switchButton; // Assign this in the Inspector
    private static GameObject savedCakeObject; // Static variable to save the cake object

    private void Start()
    {
        // Add a listener to the button
        switchButton.onClick.AddListener(OnSwitchButtonClicked);
    }

    private void OnSwitchButtonClicked()
    {
        // Save the cake object and its children
        SaveCakeObject();

        // Load the AR scene
        SceneManager.LoadScene("ARScene"); // Replace "ARScene" with the name of your AR scene
    }


    private void SaveCakeObject()
    {
        // Find the cake object in the current scene
        GameObject cakeObject = GameObject.Find("CAKE"); 

        if (cakeObject != null)
        {
            //show cakeobject in ar window
            savedCakeObject = cakeObject;
            DontDestroyOnLoad(savedCakeObject); // Ensure the cake object persists across scenes
        }
        else
        {
            Debug.LogError("Cake object not found in the current scene!");
        }
    }
    public static GameObject GetSavedCakeObject()
    {
        return savedCakeObject;
    }
}