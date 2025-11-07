using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icing : MonoBehaviour
{
    public GameObject CAKE;
    [SerializeField] private Toggle isGlossy;
    private Material icing;
    private bool editingIcing = false;
    private GameObject cakeObject; // Parent object for the cake mesh

    private void Start()
    {

    }

    private void Update()
    {
        if (editingIcing)
        {

            cakeObject = CAKE?.transform.Find("CakeBread")?.gameObject;

            Color newColor = ColorSelectionManager.Instance.GetSelectedColor();
            UpdateIcingColor(newColor);
            Debug.Log("Updated Icing Color: " + newColor);
        }
    }

    public void UpdateIcingColor(Color newColor)
    {

        // Load the correct material BEFORE setting the color
        if (isGlossy != null && isGlossy.isOn)
        {
            icing = Resources.Load<Material>("Materials/Glossy");
        }
        else
        {
            icing = Resources.Load<Material>("Materials/Fondant");
        }

        if (icing == null)
        {
            Debug.LogError("Failed to load icing material!");
            return;
        }

        // Set the new color
        icing.color = newColor;

        // Assign material to cake renderer
        Renderer renderer = cakeObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = icing;
        }
        else
        {
            Debug.LogError("No Renderer component found on the cake object.");
        }
    }

    public void startEditingIcing() => editingIcing = true;
    public void stopEditingIcing() => editingIcing = false;
}
