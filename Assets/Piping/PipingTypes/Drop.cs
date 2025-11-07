using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;
using static ShapeAndTypeHandler;

public class Drop : Piping
{
    private List<GameObject> createdObjects = new List<GameObject>();
    int pipingResolution = 20;
    int sides = 20;
    public Slider freqSlider;
    public Slider heightSlider;
    public int loops = 4;
    private bool isGenerating = false;
    private Color newColor;
    private bool editingDrop = false;
    private GameObject cakeObject; // Parent object for the cake mesh
    public GameObject CAKE;

    float heightVal;
    int freqVal;
    private bool isDirty = false;
    private bool isLoaded = false;
    private bool isSaved = false;
    private string selectedShapeLocalDrop;

    // Cache variables for change detection
    private float cachedfreqVal;
    private float cachedheightVal;
    List<Spline> splines = new List<Spline>();

    public int numKnots = 20; // Number of knots to make the circle smoother

    //private PipingSettingsManager settingsManager;

    protected override void Start()
    {
        base.Start(); // Call the base class Start() if it exists
        //settingsManager = FindObjectOfType<PipingSettingsManager>();
        // Add listeners to sliders
        freqSlider.onValueChanged.AddListener(OnfreqValChanged);
        heightSlider.onValueChanged.AddListener(OnheightValChanged);

        // Load saved settings
        //LoadSliderValues();
        // Create a circular spline on top of the cake
    }
    protected override void Update()
    {
        base.Update();
        if (editingDrop)
        {
            cakeObject = CAKE?.transform.Find("CakeBread")?.gameObject;

            newColor = ColorSelectionManager.Instance.GetSelectedColor();
            // Ensure listeners are only added once
            Generate();
        }
    }
    private void OnheightValChanged(float value)
    {
        heightVal = value;
        Generate();
    }
    private void OnfreqValChanged(float value)
    {
        freqVal = (int)value;
        Generate();
    }
    public void ResetSliders()
    {
        //SaveSliderValues();

        freqSlider.value = freqSlider.minValue;
        heightSlider.value = heightSlider.minValue;
        pipingPositionSlider.value = pipingPositionSlider.minValue;
        radiusSlider.value = radiusSlider.minValue; ;
        depthSlider.value = depthSlider.minValue;
    }

    public override void Generate()
    {
        deletePiping();
        isGenerating = true;

        editingDrop = true;
        // Remove existing piping objects
        selectedShapeLocalDrop = "" + shapeAndTypeHandler.selectedShape;


        // Generate the straight piping
        GenerateDropPiping();
        isSaved = false;
    }
    public void deletePiping()
    {
        foreach (GameObject child in createdObjects)
        {
            Destroy(child);
        }
        isGenerating = false;
        editingDrop = false;

    }
    private void GenerateDropPiping()
    {
        freqVal = (int)freqSlider.value;
        heightVal = heightSlider.value;
        float angleStep = 360f / freqVal; // Angle step between each Drop

        MeshFilter meshFilterCake = cakeObject.GetComponent<MeshFilter>();

        Vector3 cakePosition = cakeObject.transform.position;
        Bounds bounds = meshFilterCake.mesh.bounds;
        float cakeHeight = bounds.size.y * cakeObject.transform.localScale.y;
        float cakeRadius = Mathf.Max(bounds.size.x, bounds.size.z) * cakeObject.transform.localScale.x / 2;

        for (int i = 0; i < freqVal; i++)
        {
            float currentAngle = Mathf.Deg2Rad * (i * angleStep);

            Vector3 basePosition = new Vector3(
                (cakePosition.x + cakeRadius * Mathf.Cos(currentAngle)) * (1 - pipingPositionSlider.value),
                cakePosition.y + (cakeHeight/2) + 0.01f,
                (cakePosition.z + cakeRadius * Mathf.Sin(currentAngle)) * (1 - pipingPositionSlider.value)
            );

            GameObject pipingObject = new GameObject("Drop_" + i);
            pipingObject.transform.SetParent(cakeObject.transform.parent);
            createdObjects.Add(pipingObject);

            MeshFilter meshFilter = pipingObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = pipingObject.AddComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Standard"));

            Mesh mesh = new Mesh();
            meshFilter.mesh = mesh;

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<Shape> shapes = new List<Shape>();
            CreateDropShapes(basePosition, shapes);

            LinkShapes(shapes, triangles);
            UpdateData(shapes, vertices, triangles);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            meshRenderer.material.color = newColor;
            meshRenderer.material.renderQueue = 3001;
        }
    }
    private void CreateDropShapes(Vector3 basePosition, List<Shape> shapes)
    {
        Debug.Log("Create Shapes entered");
        Debug.Log(pipingResolution);
        float angleStep = loops * 360f / pipingResolution;
        Debug.Log(angleStep);
        float step = heightVal / (pipingResolution - 1); // Proper height scaling

        for (int i = 0; i < pipingResolution; i++)
        {

            float progress = 1 - ((float)i / (pipingResolution - 1));


            float adjustedRadius = progress * radius;
            float StarInnerRadiusVal = radius * (1 - depth);
            // Inner radius reduces to 0 at the last star
            float adjustedInnerRadius = progress * StarInnerRadiusVal;
            Shape newShape;

            float yPosition = basePosition.y + i * heightVal;
            // Calculate the position along the Drop relative to the base position
            Vector3 newPosition = new Vector3(
                basePosition.x + adjustedRadius * Mathf.Cos(Mathf.Deg2Rad * i * angleStep),
                yPosition,
                basePosition.z + adjustedRadius * Mathf.Sin(Mathf.Deg2Rad * i * angleStep)
            );

            // Drop rotation for this segment
            Quaternion rotation = Quaternion.Euler(0, -(angleStep * i), 0);

            // Define if this is a cap or flipped section
            bool isCap = (i == 0 || i == pipingResolution - 1);
            bool isFlipped = (i == pipingResolution - 1);
            Debug.Log(selectedShape);
            Debug.Log("Selected Shape" + i);



            if (selectedShapeLocalDrop == "Star")
            {
                newShape = new Star();
                ((Star)newShape).Initialize(sides, radius, radius * (1 - depth), i, newPosition, rotation, isCap, isFlipped);
            }
            else
            {
                newShape = new Circle();
                ((Circle)newShape).Initialize(sides, radius, i, newPosition, rotation, isCap, isFlipped);
            }

            shapes.Add(newShape);
        }
    }

    private void LinkShapes(List<Shape> shapes, List<int> triangles)
    {
        Debug.Log("Link Shapes entered");

        for (int i = 0; i < shapes.Count - 1; i++)
        {
            LinkShape(shapes[i], shapes[i + 1], triangles);
        }
    }

    private void LinkShape(Shape c0, Shape c1, List<int> triangles)
    {
        Debug.Log("Link Shape entered");

        int c0Center = c0.GetCenterIndex();
        int c1Center = c1.GetCenterIndex();

        for (int i = 0; i < sides - 1; i++)
        {
            triangles.Add(c0Center + i + 1);
            triangles.Add(c1Center + i + 2);
            triangles.Add(c1Center + i + 1);

            triangles.Add(c0Center + i + 1);
            triangles.Add(c0Center + i + 2);
            triangles.Add(c1Center + i + 2);
        }
    }

    private void UpdateData(List<Shape> shapes, List<Vector3> vertices, List<int> triangles)
    {

        for (int i = 0; i < shapes.Count; i++)
        {
            vertices.AddRange(shapes[i].GetVertices());
            triangles.AddRange(shapes[i].GetTriangles());
        }
    }
    public void StartEditing()
    {
        editingDrop = true;
    }

    public void StopEditing()
    {
        editingDrop = false;
    }
}