using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class trialShapes : MonoBehaviour
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

    private void GenerateFancyPrism(float diameter, float height, string shapeType = "star")
    {
        int points = 5;
        float outerRadius = diameter * 0.2f;
        float innerRadius = outerRadius * 0.5f;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int side = 0; side <= 1; side++)
        {
            float y = (side == 0) ? 0 : height;
            for (int i = 0; i < points * 2; i++)
            {
                float angle = i * Mathf.PI / points;
                float radius = outerRadius;

                if (shapeType == "star")
                {
                    radius = (i % 2 == 0) ? outerRadius : innerRadius;
                }
                else if (shapeType == "flower")
                {
                    radius = outerRadius * (1f + 0.3f * Mathf.Sin(5 * angle));
                }
                else if (shapeType == "polygon")
                {
                    radius = outerRadius; // no inner radius alternation
                }
                else if (shapeType == "wavy")
                {
                    radius = outerRadius * (1f + 0.2f * Mathf.Sin(8 * angle));
                }

                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                vertices.Add(new Vector3(x, y, z));
            }
        }

        vertices.Add(new Vector3(0, 0, 0));      // Bottom center
        vertices.Add(new Vector3(0, height, 0)); // Top center

        int bottomCenterIndex = vertices.Count - 2;
        int topCenterIndex = vertices.Count - 1;

        for (int i = 0; i < points * 2; i++)
        {
            int next = (i + 1) % (points * 2);

            triangles.Add(bottomCenterIndex);
            triangles.Add(next);
            triangles.Add(i);

            triangles.Add(topCenterIndex);
            triangles.Add(points * 2 + i);
            triangles.Add(points * 2 + next);
        }

        for (int i = 0; i < points * 2; i++)
        {
            int next = (i + 1) % (points * 2);
            int bottomCurrent = i;
            int bottomNext = next;
            int topCurrent = points * 2 + i;
            int topNext = points * 2 + next;

            triangles.Add(bottomCurrent);
            triangles.Add(topCurrent);
            triangles.Add(topNext);

            triangles.Add(bottomCurrent);
            triangles.Add(topNext);
            triangles.Add(bottomNext);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        cakeMeshFilter.mesh = mesh;
    }

}
