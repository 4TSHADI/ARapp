using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SwitchToMain : MonoBehaviour
{
    public Button backButton; // Assign this in the Inspector

    private void Start()
    {
        // Add a listener to the button
        backButton.onClick.AddListener(OnBackButtonnClicked);
    }

 

    private void OnBackButtonnClicked()
    {


        // Load the AR scene
        SceneManager.LoadScene("Design"); // Replace "ARScene" with the name of your AR scene
    }

}