using System.Collections.Generic;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public abstract bool SupportsDepth(); // Determines if the shape uses depth

    protected int sides;
    protected float radius;
    protected int index;
    protected int centerIndex;

    protected Vector3 position;
    protected Quaternion rotation;

    protected bool isCap;
    protected bool isFlipped;

    [Header(" Data ")]
    protected List<Vector3> vertices;
    protected List<int> triangles;

    public void Initialize(int sides, float radius, int index, Vector3 position, Quaternion rotation, bool isCap, bool isFlipped)
    {
        this.sides = sides;
        this.radius = radius;
        this.index = index;
        this.centerIndex = index * (sides + 1);
        this.position = position;
        this.rotation = rotation;
        this.isCap = isCap;
        this.isFlipped = isFlipped;

        vertices = new List<Vector3>();
        triangles = new List<int>();

        CreateVertices();
        if (isCap)
        {
            CreateTriangles();
        }
    }

    public Vector3[] GetVertices()
    {
        return vertices.ToArray();
    }

    public int[] GetTriangles()
    {
        return triangles.ToArray();
    }

    public int GetCenterIndex()
    {
        return centerIndex;
    }

    protected abstract void CreateVertices();

    protected void CreateTriangles()
    {
        for (int i = centerIndex; i < sides - 1 + centerIndex; i++)
        {
            triangles.Add(centerIndex);

            if (!isFlipped)
            {
                triangles.Add(i + 2);
                triangles.Add(i + 1);
            }
            else
            {
                triangles.Add(i + 1);
                triangles.Add(i + 2);
            }
        }
        triangles.Add(centerIndex);

        if (!isFlipped)
        {
            triangles.Add(centerIndex + 1);
            triangles.Add(sides + centerIndex);
        }
        else
        {
            triangles.Add(sides + centerIndex);
            triangles.Add(centerIndex + 1);
        }
    }
}