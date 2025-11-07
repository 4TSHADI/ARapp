//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.TestTools;

//public class DecorationUnitTests
//{
//    // Data class matching JSON structure
//    [System.Serializable]
//    public class TestVector
//    {
//        public int PipingResolution;
//        public int Sides;
//        public float PositionOffset;
//        public int ExpectedVertexCount;
//    }

//    [System.Serializable]
//    private class TestVectorWrapper
//    {
//        public TestVector[] vectors;
//    }

//    // Helper to load test vectors from JSON file
//    private static IEnumerable<TestVector> LoadTestVectors(string relativePath)
//    {
//        var fullPath = Path.Combine(Application.dataPath, relativePath);
//        Assert.IsTrue(File.Exists(fullPath), $"Test data file not found: {fullPath}");
//        var json = File.ReadAllText(fullPath);
//        var wrapper = JsonUtility.FromJson<TestVectorWrapper>(json);
//        return wrapper.vectors;
//    }

//    // Provide test cases to the parameterized test
//    private static IEnumerable DecorationTestCases
//    {
//        get
//        {
//            foreach (var vec in LoadTestVectors("Tests/Data/DecorationTestVectors.json"))
//                yield return new TestCaseData(vec).SetName(
//                    $"GenerateMesh_{vec.PipingResolution}Segs_{vec.Sides}Sides");
//        }
//    }

//    [UnityTest]
//    [TestCaseSource(nameof(DecorationTestCases))]
//    public IEnumerator GenerateMesh_WithValidParameters_ProducesExpectedVertexCount(TestVector vector)
//    {
//        // Arrange
//        var sceneGO = new GameObject("TestSceneRoot");
//        var cakeGO = new GameObject("CakeBase");
//        cakeGO.transform.SetParent(sceneGO.transform);
//        // Add a dummy MeshFilter so spline can read bounds
//        var dummyMesh = new Mesh();
//        dummyMesh.vertices = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up };
//        dummyMesh.triangles = new int[] { 0, 1, 2 };
//        cakeGO.AddComponent<MeshFilter>().mesh = dummyMesh;

//        // Attach and configure piping component
//        var straight = cakeGO.AddComponent<Straight>();
//        straight.CAKE = cakeGO;
//        straight.pipingResolution = vector.PipingResolution;
//        straight.sides = vector.Sides;
//        straight.pipingPosition = vector.PositionOffset;

//        // Act
//        straight.Generate();
//        // Wait a frame for mesh construction
//        yield return null;

//        // Assert
//        var pipingGO = GameObject.Find("StraightPiping");
//        Assert.IsNotNull(pipingGO, "StraightPiping root was not created.");
//        var mf = pipingGO.GetComponent<MeshFilter>();
//        Assert.IsNotNull(mf, "MeshFilter missing on piping GameObject.");
//        Assert.AreEqual(vector.ExpectedVertexCount, mf.mesh.vertexCount,
//            $"Expected {vector.ExpectedVertexCount} vertices, but got {mf.mesh.vertexCount}.");

//        // Cleanup
//        Object.DestroyImmediate(sceneGO);
//    }
//}
