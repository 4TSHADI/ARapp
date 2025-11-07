using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class starCake : MonoBehaviour
{
    public Slider cakeSizeSlider;      // Reference to the slider
    public Text sizeText;              // Reference to the Text element displaying the size
    public Slider cakeHeightSlider;    // Reference to the slider
    public Text heightText;            // Reference to the Text element displaying the height
    public GameObject CAKE;             // Parent object
    private GameObject cakeBread;       // Holds the generated cake

    private readonly int[] cakeSizes = { 6, 8, 10, 12, 14 }; // Diameter options
    private readonly float[] cakeHeights = { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f }; // Height options

    private MeshFilter cakeMeshFilter;
    private MeshRenderer cakeMeshRenderer;
    private bool isEditing;

    private void Start()
    {
        cakeSizeSlider.onValueChanged.AddListener(delegate { UpdateCakeSize(); });
        cakeHeightSlider.onValueChanged.AddListener(delegate { UpdateCakeSize(); });
    }

    private void InitializeCakeMesh()
    {
        cakeBread = new GameObject("CakeBread");
        cakeBread.transform.SetParent(CAKE.transform);
        cakeBread.transform.position = Vector3.zero;

        cakeMeshFilter = cakeBread.AddComponent<MeshFilter>();
        cakeMeshRenderer = cakeBread.AddComponent<MeshRenderer>();
        cakeMeshRenderer.material = Resources.Load<Material>("Materials/CakeBread");
    }

    private void UpdateCakeSize()
    {
        if (isEditing)
        {
            DestroyImmediate(cakeBread); // Destroy old cake when editing
            InitializeCakeMesh();
            StopEditing();
        }

        int xsliderValue = (int)cakeSizeSlider.value;
        int cakeDiameter = cakeSizes[xsliderValue];
        int ysliderValue = (int)cakeHeightSlider.value;
        float cakeHeight = cakeHeights[ysliderValue];

        sizeText.text = "Cake Size: " + cakeDiameter + "\"";
        heightText.text = "Cake Layers: " + cakeHeight * 2;

        GenerateCakeCube(cakeDiameter, cakeHeight);

        cakeBread.transform.position = new Vector3(0, -4.5f + (0.25f * ysliderValue), 0);
    }

    private void GenerateCakeCube(float diameter, float height)
    {
        // Create cube vertices based on width and height
        float width = diameter * 0.4f;  // Adjust scaling factor as needed
        float depth = diameter * 0.4f;
        float cubeHeight = height;

        Vector3[] vertices = new Vector3[]
        {
            // Bottom vertices
            new Vector3(-width/2, 0, -depth/2),
            new Vector3(width/2, 0, -depth/2),
            new Vector3(width/2, 0, depth/2),
            new Vector3(-width/2, 0, depth/2),
            // Top vertices
            new Vector3(-width/2, cubeHeight, -depth/2),
            new Vector3(width/2, cubeHeight, -depth/2),
            new Vector3(width/2, cubeHeight, depth/2),
            new Vector3(-width/2, cubeHeight, depth/2),
        };

        int[] triangles = new int[]
        {
            // Bottom
            0, 2, 1,
            0, 3, 2,
            // Top
            4, 5, 6,
            4, 6, 7,
            // Front
            0, 1, 5,
            0, 5, 4,
            // Back
            2, 3, 7,
            2, 7, 6,
            // Left
            3, 0, 4,
            3, 4, 7,
            // Right
            1, 2, 6,
            1, 6, 5
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
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
