using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public int petalCount = 6; // Number of petals
    public float petalRadius = 1.0f; // Radius of each petal
    public float flowerRadius = 2.0f; // Radius of the flower

    private List<Shape> petals = new List<Shape>();

    void Start()
    {
        GenerateFlower();
        CombineMeshes();
    }

    void GenerateFlower()
    {
        float angleStep = 360f / petalCount;

        for (int i = 0; i < petalCount; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            Vector3 position = new Vector3(Mathf.Cos(angle) * flowerRadius, 0, Mathf.Sin(angle) * flowerRadius);
            Quaternion rotation = Quaternion.Euler(0, -i * angleStep, 0);

            Petal petal = new Petal();
            petal.Initialize(16, petalRadius, i, position, rotation, true, false);
            petals.Add(petal);
        }
    }

    void CombineMeshes()
    {
        List<Vector3> allVertices = new List<Vector3>();
        List<int> allTriangles = new List<int>();

        foreach (var petal in petals)
        {
            int vertexOffset = allVertices.Count;
            allVertices.AddRange(petal.GetVertices());

            foreach (int triangle in petal.GetTriangles())
            {
                allTriangles.Add(triangle + vertexOffset);
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = allVertices.ToArray();
        mesh.triangles = allTriangles.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshFilter.mesh = mesh;
        meshRenderer.material = new Material(Shader.Find("Standard")); // Default material
    }
}