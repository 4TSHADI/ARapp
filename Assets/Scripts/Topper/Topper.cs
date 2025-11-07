using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Topper : MonoBehaviour
{
    public GameObject cake; // Assign the cylinder (cake) GameObject in the Inspector
    public GameObject topperStick;
    public Font textFont;   // Assign a font for the cake topper text
    public InputField inputField;    // Assign the Input Field UI element in the Inspector
    Vector3 cakeCenter; 
    public float topperScale = 0.1f; // Scale for the text
    private GameObject cakeObject; // Parent object for the cake mesh
    public GameObject CAKE;
    private string userInput = "";

    public void InsertTopper()
    {
        // Input field for user text
        string input = inputField.text;
        cakeObject = CAKE.transform.Find("CakeBread")?.gameObject;
        // Validate input
        if (string.IsNullOrWhiteSpace(input))
        {
            Debug.LogError("Input cannot be empty.");
            return;
        }

        var words = input.Split(' ').Where(word => !string.IsNullOrWhiteSpace(word)).ToArray();
        if (words.Length > 2)
        {
            Debug.LogError("Input cannot exceed 2 words.");
            return;
        }

        if (input.Length > 20)
        {
            Debug.LogError("Input cannot exceed 20 characters.");
            return;
        }



        // Create the new topper
        cakeCenter = new Vector3(cake.transform.position.x, cake.transform.position.y+4f, cake.transform.position.z);
        GameObject topper = new GameObject("CakeTopper");
        topper.transform.SetParent(cakeObject.transform);
        topper.transform.localPosition = cakeCenter;

        // Add text mesh
        TextMesh textMesh = topper.AddComponent<TextMesh>();
        textMesh.text = input;
        textMesh.font = textFont;
        textMesh.characterSize = 0.1f;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.fontStyle = FontStyle.Normal;  // Prevent underlines
        textMesh.richText = false;  // Disable special text effects
        // Adjust scale and position
        topper.transform.localScale = Vector3.one * topperScale;
        topperStick.SetActive(true);

        topperStick.transform.position = new Vector3(cake.transform.position.x, cake.transform.position.y + 4.5f, cake.transform.position.z);

    }
    public void DeleteTopper()
    {
        topperStick.SetActive(false);
        //Destroy any existing topper
        Transform existingTopper = cake.transform.Find("CakeTopper");
        if (existingTopper != null)
        {
            Destroy(existingTopper.gameObject);
        }
    }
}
