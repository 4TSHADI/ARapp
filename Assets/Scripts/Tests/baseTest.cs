//using UnityEngine;
//using UnityEngine.TestTools;
//using NUnit.Framework;
//using System.Collections;

//public class baseIntegration
//{
//    private GameObject cakePrefab;
//    private GameObject cakeInstance;
//    private const int MinExpectedVertices = 100;

//    [UnitySetUp]
//    public IEnumerator SetUp()
//    {
//        // Load the base cake prefab from the Resources folder
//        cakePrefab = Resources.Load<GameObject>("Prefabs/CakeBase");
//        Assert.NotNull(cakePrefab, "CakeBase prefab not found in Resources/Prefabs.");

//        // Instantiate the base cake in the scene
//        cakeInstance = GameObject.Instantiate(cakePrefab);
//        cakeInstance.name = "TestCake";
//        yield return null;
//    }

//    [UnityTest]
//    public IEnumerator BaseCake_ShouldHaveMeshFilterAndRenderer()
//    {
//        // Verify base cake has required components
//        var meshFilter = cakeInstance.GetComponent<MeshFilter>();
//        var meshRenderer = cakeInstance.GetComponent<MeshRenderer>();
//        Assert.NotNull(meshFilter, "Base cake is missing MeshFilter.");
//        Assert.NotNull(meshRenderer, "Base cake is missing MeshRenderer.");
//        Assert.Greater(meshFilter.mesh.vertexCount, 0, "Base cake mesh is empty.");
//        yield return null;
//    }

//    [UnityTest]
//    public IEnumerator PipingIntegration_ShouldGenerateNonEmptyMesh()
//    {
//        // Attach the Straight piping component
//        var straight = cakeInstance.AddComponent<Straight>();
//        straight.CAKE = cakeInstance;               // assign parent cake
//        straight.numKnots = 8;                      // test parameters
//        straight.pipingResolution = 20;
//        straight.sides = 16;

//        // Trigger generation
//        straight.Generate();
//        yield return new WaitForSeconds(0.1f);      // allow mesh build

//        // Locate the generated piping GameObject
//        var pipingGO = GameObject.Find("StraightPiping");
//        Assert.NotNull(pipingGO, "StraightPiping GameObject not created.");

//        // Validate mesh content
//        var pipeMF = pipingGO.GetComponent<MeshFilter>();
//        Assert.NotNull(pipeMF, "Piping object missing MeshFilter.");
//        Assert.Greater(pipeMF.mesh.vertexCount, MinExpectedVertices,
//                      $"Piping mesh has too few vertices: {pipeMF.mesh.vertexCount}.");

//        yield return null;
//    }

//    [UnityTest]
//    public IEnumerator ShapeSwitching_ShouldNotLeaveOrphansOrErrors()
//    {
//        var handler = cakeInstance.AddComponent<ShapeAndTypeHandler>();
//        var straight = cakeInstance.GetComponent<Straight>();

//        // Generate initial shape
//        handler.SelectShape(ShapeType.Circle);
//        straight.Generate();
//        yield return new WaitForSeconds(0.1f);

//        // Switch to a different shape
//        handler.SelectShape(ShapeType.Star);
//        straight.Generate();
//        yield return new WaitForSeconds(0.1f);

//        // Ensure only one piping root exists
//        var allPipings = GameObject.FindObjectsOfType<Straight>();
//        Assert.AreEqual(1, allPipings.Length, "Multiple Straight components found after shape switch.");

//        // Ensure no orphaned child objects
//        var pipingGO = GameObject.Find("StraightPiping");
//        Assert.AreEqual(straight.transform.parent, pipingGO.transform.parent,
//                        "Piping GameObject parent changed unexpectedly.");

//        yield return null;
//    }

//    [UnityTearDown]
//    public IEnumerator TearDown()
//    {
//        Object.Destroy(cakeInstance);
//        yield return null;
//    }
//}
