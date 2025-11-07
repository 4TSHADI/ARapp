using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CakeSizeSelector : MonoBehaviour
{
    public Slider cakeSizeSlider;      // Reference to the slider
    public Text sizeText;              // Reference to the Text element displaying the size
    public Slider cakeHeightSlider;    // Reference to the slider
    public Text heightText;            // Reference to the Text element displaying the size
    public GameObject CAKE;
    private GameObject cakeBread;  // Holds the generated cake

    // Define cake sizes corresponding to each slider step
    private readonly int[] cakeSizes = { 6, 8, 10, 12, 14 };
    private readonly float[] cakeHeights = { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f };

    private MeshFilter cakeMeshFilter;
    private MeshRenderer cakeMeshRenderer;
    private bool isEditing;

    private void Start()
    {


        // Set up the slider to call UpdateCakeSize every time it's changed
        cakeSizeSlider.onValueChanged.AddListener(delegate { UpdateCakeSize(); });
        cakeHeightSlider.onValueChanged.AddListener(delegate { UpdateCakeSize(); });

    }
    private void InitializeCakeMesh()
    {
        // Create a parent object called "CakeBread" in the scene
        cakeBread = new GameObject("CakeBread");
        cakeBread.transform.SetParent(CAKE.transform);
        cakeBread.transform.position = Vector3.zero;
        cakeMeshRenderer.material = Resources.Load<Material>("Materials/CakeBread");

        // Add MeshFilter and MeshRenderer components to the parent object
        cakeMeshFilter = cakeBread.AddComponent<MeshFilter>();
        cakeMeshRenderer = cakeBread.AddComponent<MeshRenderer>();
    }
    private void UpdateCakeSize()
    {
        if (isEditing)
        {
            // Create a parent object called "CakeBread" in the scene
            cakeBread = new GameObject("CakeBread");
            cakeBread.transform.SetParent(CAKE.transform);
            cakeBread.transform.position = Vector3.zero;

            // Add MeshFilter and MeshRenderer components to the parent object
            cakeMeshFilter = cakeBread.AddComponent<MeshFilter>();
            cakeMeshRenderer = cakeBread.AddComponent<MeshRenderer>();
            cakeMeshRenderer.material = Resources.Load<Material>("Materials/CakeBread");

            StopEditing();
        }
        // Get the integer slider value (0 to 4) and map it to a cake size
        int xsliderValue = (int)cakeSizeSlider.value;
        int cakeDiameter = cakeSizes[xsliderValue];
        int ysliderValue = (int)cakeHeightSlider.value;
        float cakeHeight = cakeHeights[ysliderValue];

        // Update the text to show the selected cake size
        sizeText.text = "Cake Size: " + cakeDiameter + "\"";
        heightText.text = "Cake layers: " + cakeHeight * 2;

        // Generate the cake mesh
        GenerateCakeMesh(cakeDiameter, cakeHeight);

        // Adjust the position of the cake based on the height
        cakeBread.transform.position = new Vector3(0, -4.5f + (0.25f * ysliderValue), 0);
    }

    private void GenerateCakeMesh(float diameter, float height)
    {
        int segments = 36; // Number of subdivisions for smoothness
        float radius = diameter * 0.2f;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Top and bottom center points
        Vector3 topCenter = new Vector3(0, height * 0.1f, 0);
        Vector3 bottomCenter = new Vector3(0, -height * 0.1f, 0);

        vertices.Add(topCenter);    // First vertex (Top center)
        vertices.Add(bottomCenter); // Second vertex (Bottom center)

        // Generate side and cap vertices
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * Mathf.PI * 2 / segments;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            // Top and bottom ring vertices
            vertices.Add(new Vector3(x, height * 0.5f, z));
            vertices.Add(new Vector3(x, -height * 0.5f, z));

            if (i > 0)
            {
                // Side triangles (each step forms a quad split into 2 triangles)
                int start = 2 + (i - 1) * 2;
                int next = start + 2;

                triangles.Add(start);
                triangles.Add(next);
                triangles.Add(start + 1);

                triangles.Add(start + 1);
                triangles.Add(next);
                triangles.Add(next + 1);

                // Top cap triangles
                triangles.Add(0);
                triangles.Add(next);
                triangles.Add(start);

                // Bottom cap triangles
                triangles.Add(1);
                triangles.Add(start + 1);
                triangles.Add(next + 1);
            }
        }

        // Create and assign mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        cakeMeshFilter.mesh = mesh;
    }
    public void StartEditing()
    {
        isEditing = true;
    }

    public void StopEditing()
    {
        isEditing = false;
    }
}
