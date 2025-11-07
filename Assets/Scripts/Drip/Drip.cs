using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Drip : MonoBehaviour
{
    [Header("Drip Settings")]
    public Slider thicknessSlider;
    public Slider lengthSlider;
    public Slider segmentSlider;
    private GameObject cake; // Parent object for the cake mesh
    public GameObject CAKE;

    [SerializeField] private Toggle coverTop;
    private int isGenerating = 0;
    private Mesh mesh;
    private Color newColor;
    private bool editingDrip;
    private GameObject dripObject;
    public float zFightOffset = 0.01f;

    private void Start()
    {
        DeleteDrip();
        thicknessSlider.onValueChanged.AddListener(delegate { GenerateDrip(); });
        lengthSlider.onValueChanged.AddListener(delegate { GenerateDrip(); });
        segmentSlider.onValueChanged.AddListener(delegate { GenerateDrip(); });
    }

    private void Update()
    {
        if (editingDrip)
        {
            if (isGenerating == 0) // Only create once
            {
                isGenerating = 1;
                CreateDripObject();
            }

            cake = CAKE.transform.Find("CakeBread")?.gameObject;
            newColor = ColorSelectionManager.Instance.GetSelectedColor();
            GenerateDrip();
        }
    }

    private void CreateDripObject()
    {
        if (dripObject == null)
        {
            dripObject = new GameObject("Drip");
            dripObject.transform.SetParent(CAKE.transform);
            dripObject.transform.localPosition = Vector3.zero;

            MeshFilter meshFilter = dripObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = dripObject.AddComponent<MeshRenderer>();

            meshRenderer.material = Resources.Load<Material>("Materials/Drip");

            mesh = new Mesh();
            meshFilter.mesh = mesh;
        }
        isGenerating = 0;
    }
    void GenerateDrip()
    {
        editingDrip = true;
        if (cake == null) return;

        // Ensure dripObject exists
        if (dripObject == null)
        {
            CreateDripObject();
        }

        // Get MeshFilter and MeshRenderer from the existing object
        MeshFilter meshFilter = dripObject.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = dripObject.GetComponent<MeshRenderer>();

        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Fetch cake properties
        float dripThickness = thicknessSlider.value;
        float dripLength = lengthSlider.value;
        int dripSegments = Mathf.Max(3, (int)segmentSlider.value); // Ensure minimum segments

        MeshFilter meshFilterCake = cake.GetComponent<MeshFilter>();
        Bounds bounds = meshFilterCake.mesh.bounds;

        float cakeHeight = bounds.size.y * cake.transform.localScale.y;
        float cakeRadius = Mathf.Max(bounds.size.x, bounds.size.z) * cake.transform.localScale.x / 2;
        cakeRadius += zFightOffset;  // Prevent z-fighting

        float cakeTopY = cake.transform.position.y + (cakeHeight / 2); // Ensure we start at the top of CakeBread

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float angleStep = 360f / dripSegments;
        float noiseScale = 0.2f;

        // Top cover (optional)
        if (coverTop.isOn)
        {
            vertices.Add(new Vector3(0, cakeTopY, 0)); // Center vertex
            uvs.Add(new Vector2(0.5f, 0.5f));

            for (int i = 0; i <= dripSegments; i++)
            {
                float angle = Mathf.Deg2Rad * (i * angleStep);
                float x = Mathf.Cos(angle) * cakeRadius;
                float z = Mathf.Sin(angle) * cakeRadius;
                vertices.Add(new Vector3(x, cakeTopY, z));  // Adjusted to start from the top of the cake
                uvs.Add(new Vector2((x / cakeRadius + 1) * 0.5f, (z / cakeRadius + 1) * 0.5f));
            }

            for (int i = 1; i < dripSegments; i++)
            {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i);
            }
            triangles.Add(0);
            triangles.Add(1);
            triangles.Add(dripSegments);
        }

        // Drip vertices
        int dripVertexOffset = coverTop.isOn ? dripSegments + 2 : 0;
        for (int i = 0; i <= dripSegments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * cakeRadius;
            float z = Mathf.Sin(angle) * cakeRadius;

            vertices.Add(new Vector3(x, cakeTopY, z)); // Ensure drips start at the top of CakeBread
            uvs.Add(new Vector2(i / (float)dripSegments, 1));

            float noiseValue = Mathf.PerlinNoise(i * noiseScale, Time.time * 0.1f);
            float dripOffset = Mathf.Lerp(dripLength * 0.7f, dripLength, noiseValue);

            vertices.Add(new Vector3(x, (cakeTopY - dripOffset), z)); // Adjusted for correct drip placement
            uvs.Add(new Vector2(i / (float)dripSegments, 0));
        }

        // Create triangles
        for (int i = 0; i < dripSegments; i++)
        {
            int top1 = dripVertexOffset + i * 2;
            int top2 = dripVertexOffset + ((i + 1) % dripSegments) * 2;
            int bottom1 = top1 + 1;
            int bottom2 = top2 + 1;

            triangles.Add(top1);
            triangles.Add(bottom2);
            triangles.Add(bottom1);

            triangles.Add(top1);
            triangles.Add(top2);
            triangles.Add(bottom2);
        }

        // Assign mesh data
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        // Update material color
        if (meshRenderer.material.color != newColor)
        {
            meshRenderer.material.color = newColor;
        }
        meshRenderer.material.renderQueue = 3001;
    }



    public void DeleteDrip()
    {
        if (dripObject != null)
        {
            Destroy(dripObject);
            dripObject = null;
        }
        StopEditingDrip();
    }


    public void StartEditingDrip() => editingDrip = true;

    public void StopEditingDrip() => editingDrip = false;


}