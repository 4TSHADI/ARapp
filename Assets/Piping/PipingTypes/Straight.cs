using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines; // Add this namespace for Spline functionality

public class Straight : Piping
{
    private List<GameObject> createdObjects = new List<GameObject>();
    int pipingResolution = 20;
    int sides = 80;
    private Spline spline; // The actual spline
    public int numKnots = 8; // Number of knots to make the circle smoother
    private bool isGenerating = false;
    private Color newColor;
    private bool editingStraight = false;
    private string selectedShapeLocalStraight;
    private GameObject cakeObject; // Parent object for the cake mesh
    public GameObject CAKE;

    protected override void Start()
    {
        base.Start(); // Call the base class Start() if it exists
        spline = new Spline();
        // Create a circular spline on top of the cake
        CreateCircularSpline();
    }
    protected override void Update()
    {
        base.Update();
        if (editingStraight)
        {
            cakeObject = CAKE.transform.Find("CakeBread")?.gameObject;
            newColor = ColorSelectionManager.Instance.GetSelectedColor();
            // Ensure listeners are only added once
            Generate();
        }
    }


    public override void Generate()
    {

        deletePiping();
        isGenerating = true;

        editingStraight = true;
        selectedShapeLocalStraight = "" + shapeAndTypeHandler.selectedShape;
        Debug.Log(selectedShapeLocalStraight);


        // Generate the straight piping
        CreateCircularSpline();
        GenerateStraightPiping();
    }

    public void deletePiping()
    {
        foreach (GameObject child in createdObjects)
        {
            Destroy(child);
        }
        isGenerating = false;
        editingStraight = false;

    }
    private void GenerateStraightPiping()
    {
        Vector3 cakePosition = cakeObject.transform.position;
        float cakeHeight = cakeObject.transform.localScale.y;
        float cakeRadius = cakeObject.transform.localScale.x * 0.5f;

        Vector3 basePosition = new Vector3(
            cakePosition.x,
            cakePosition.y  - cakeHeight,
            cakePosition.z
        );

        GameObject pipingObject = new GameObject("StraightPiping");
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
        Debug.Log("To Create Shapes ");
        Debug.Log(basePosition);

        vertices.Clear();
        triangles.Clear();
        shapes.Clear();
        CreateShapesAlongSpline(shapes);
        LinkShapes(shapes, triangles);
        UpdateData(shapes, vertices, triangles);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshRenderer.material.color = newColor;
        meshRenderer.material.renderQueue = 3001;
    }

    //private void CreateShapes(Vector3 basePosition, List<Shape> shapes)
    //{
    //    Debug.Log("Create Shapes entered");
    //    Debug.Log(pipingResolution);
    //    float angleStep = 360f / pipingResolution;
    //    Debug.Log(angleStep);

    //    for (int i = 0; i < pipingResolution; i++)
    //    {
    //        float cakeRadius = (cakeObject.transform.localScale.x) / 2;

    //        Shape newShape;
    //        Vector3 newPosition = new Vector3(
    //            basePosition.x + cakeRadius * Mathf.Cos(Mathf.Deg2Rad * i * angleStep) * (1 - pipingPosition),
    //            basePosition.y,
    //            basePosition.z + cakeRadius * Mathf.Sin(Mathf.Deg2Rad * i * angleStep) * (1 - pipingPosition)
    //        );

    //        Quaternion rotation = Quaternion.Euler(0, -(angleStep * i), 0);
    //        bool isCap = (i == 0 || i == pipingResolution - 1);
    //        bool isFlipped = (i == pipingResolution - 1);
    //        Debug.Log("-------Selected Shape" + i);

    //        if (selectedShapeLocalStraight == "Star")
    //        {
    //            Debug.Log(selectedShapeLocalStraight);
    //            Debug.Log("Selected Shape" + i);
    //            newShape = new Star();
    //            ((Star)newShape).Initialize(sides, radius, radius * (1 - depth), i, newPosition, rotation, isCap, isFlipped);
    //        }
    //        else
    //        {
    //            newShape = new Circle();
    //            ((Circle)newShape).Initialize(sides, radius, i, newPosition, rotation, isCap, isFlipped);
    //        }

    //        shapes.Add(newShape);
    //    }
    //}

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
    private void CreateCircularSpline()
    {
        // Clear existing knots
        spline.Clear();
        MeshFilter meshFilterCake = cakeObject.GetComponent<MeshFilter>();

        Vector3 cakePosition = cakeObject.transform.position;
        Bounds bounds = meshFilterCake.mesh.bounds;
        float cakeHeight = bounds.size.y * cakeObject.transform.localScale.y;
        float cakeRadius = Mathf.Max(bounds.size.x, bounds.size.z) * cakeObject.transform.localScale.x / 2;

        // Create a circular spline with 4 knots (adjust as needed)
        for (int i = 0; i < numKnots; i++)
        {
            Debug.Log("position---" + pipingPosition);

            float angle = (i / (float)numKnots) * 2 * Mathf.PI; // Distribute points evenly around the circle
            Vector3 knotPosition = new Vector3(
                (cakePosition.x + cakeRadius * Mathf.Cos(angle)) * (1 - pipingPosition),
                cakePosition.y + cakeHeight / 2,
                (cakePosition.z + cakeRadius * Mathf.Sin(angle)) * (1 - pipingPosition)
            );
            Quaternion knotRotation = Quaternion.Euler(0, -(90 * i), 0);

            BezierKnot knot = new BezierKnot(knotPosition);
            knot.Rotation = knotRotation;
            spline.Add(knot);
        }

        // Close the spline loop
        spline.Closed = true;
        // Create a GameObject to hold the spline
        GameObject splineObject = new GameObject("Spline_Straight");
        splineObject.transform.SetParent(cakeObject.transform); // Set as child of the cakeObject
        SplineContainer splineContainer = splineObject.AddComponent<SplineContainer>();
        splineContainer.Spline = spline; // Assign the spline to the container
    }
    private void CreateShapesAlongSpline(List<Shape> shapes)
    {
        Debug.Log("Create Shapes Along Spline entered");
        float angleStep = 360f / pipingResolution;

        for (int i = 0; i < pipingResolution; i++)
        {
            // Get the position along the spline
            float t = (float)i / (pipingResolution - 1);
            Vector3 positionOnSpline = spline.EvaluatePosition(t);

            Shape newShape;
            Quaternion rotation = Quaternion.Euler(0, -(angleStep * i), 0);

            bool isCap = (i == 0 || i == pipingResolution - 1);
            bool isFlipped = (i == pipingResolution - 1);
            Debug.Log(selectedShapeLocalStraight + "-----dfgjtfjkg" + depth);

            if (selectedShapeLocalStraight == "Star")
            {
                Debug.Log(selectedShapeLocalStraight);
                newShape = new Star();
                ((Star)newShape).Initialize(sides, radius, radius * (1 - depth), i, positionOnSpline, rotation, isCap, isFlipped);
            }
            else
            {
                newShape = new Circle();
                ((Circle)newShape).Initialize(sides, radius, i, positionOnSpline, rotation, isCap, isFlipped);
            }
            if (selectedShapeLocalStraight == "Star")
            {
                newShape = new Star();
                ((Star)newShape).Initialize(sides, radius, radius * (1 - depth), i, positionOnSpline, rotation, isCap, isFlipped);
            }
            else if (selectedShapeLocalStraight == "FrenchStar")
            {
                int sidesFrench = 80;
                newShape = new Star();
                ((Star)newShape).Initialize(sidesFrench, radius, i, positionOnSpline, rotation, isCap, isFlipped);
            }
            else if (selectedShapeLocalStraight == "Petal")
            {
                newShape = new Petal();
                ((Petal)newShape).Initialize(sides, radius, i, positionOnSpline, rotation, isCap, isFlipped);
            }
            else
            {
                newShape = new Circle();
                ((Circle)newShape).Initialize(sides, radius, i, positionOnSpline, rotation, isCap, isFlipped);
            }
            shapes.Add(newShape);
        }
    }
    public void StartEditing()
    {
        editingStraight = true;
    }

    public void StopEditing()
    {
        editingStraight = false;
    }
}