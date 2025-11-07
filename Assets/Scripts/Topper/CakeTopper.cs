using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class CakeTopper : MonoBehaviour
{
    public InputField inputField; // Input field for user text
    public GameObject cake;            // Reference to the cake object in the scene
    public GameObject stick; // Prefab of the stick object
    public TMP_FontAsset tfont; // ChillerSDF is the font I want for the button label text.

    Vector3 cakeCenter;
    public void DeleteTopper() 
    {
        // Clear previous toppers if any
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void GenerateCakeTopper()
    {
        Debug.LogWarning("Generating topper");
        float cakeHeight = cake.transform.localScale.y;
        float stickLength = cakeHeight ; // Stick height is half of the cake height

        float numx = cake.transform.position.x;
        float numy = cake.transform.position.y;
        float numz = cake.transform.position.z;

        cakeCenter = new Vector3(numx, numy + cakeHeight / 2, numz);
        Vector3 stickStart = new Vector3(numx, numy + (cakeHeight - (0.2f *cakeHeight)), numz); // Stick starts at the center of the cake
        Vector3 textCenter = stickStart + new Vector3(0, stickLength/2, 0); // Text is positioned above the stick

        Debug.Log("  " + textCenter + "   " + stickStart + "   " + cakeHeight + cakeCenter + "   " + stickLength);
        DeleteTopper();
        topperPosition(textCenter, cakeHeight);
        stickPosition(stickStart, stickLength);
    }
    void topperPosition(Vector3 textCenter, float cakeHeight) {
        // Get the user input text
        string inputText = inputField.text;
        if (string.IsNullOrWhiteSpace(inputText))
        {
            Debug.LogWarning("Input text is empty or invalid.");
            return;
        }
        // Validate input text length and word count
        if (inputText.Length > 20)
        {
            Debug.LogWarning("Input text exceeds 20 characters.");
            return;
        }
        string[] words = inputText.Split(' ');
        if (words.Length > 3)
        {
            Debug.LogWarning("Input text contains more than 3 words.");
            return;
        }

        float lineHeight = 0.4f; // Distance between lines
        for (int i = 0; i < words.Length; i++)
        {
            GameObject textObject = new GameObject($"CakeTopperText_{i}");
            textObject.transform.SetParent(transform);

            // Add and configure TextMeshPro component
            TextMeshPro textMeshPro = textObject.AddComponent<TextMeshPro>();
            textMeshPro.text = words[i];
            textMeshPro.font = tfont;
            textMeshPro.fontSize = 5 + (0.5f* cakeHeight);  // cake height acts as a scale
            textMeshPro.fontWeight = FontWeight.Bold;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.enableWordWrapping = false;
            textMeshPro.color = Color.black;
            // Position each word on a new line
            textObject.transform.position = textCenter + new Vector3(0, (words.Length -i) * lineHeight, 0);
            textObject.transform.localRotation = Quaternion.identity;
            textObject.transform.localScale = Vector3.one;
            Debug.Log(" " + textObject.transform.position);

        }

        Debug.Log("Cake topper generated successfully with words on separate lines.");
    }



    void stickPosition(Vector3 stickStart, float stickLength)
    {
        // Add a stick below the text
        stick.transform.localScale = new Vector3(0.04f, stickLength, 0.04f);
        stick.transform.localPosition = stickStart;
        // Scale and align the stick
        //stick.transform.localScale = new Vector3(0.2f, 1, 0.2f); // Adjust stick dimensions as needed
        //stick.transform.localRotation = Quaternion.identity;
        Debug.Log("cake's location." + cakeCenter);

    }
}

