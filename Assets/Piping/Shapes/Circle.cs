using UnityEngine;

public class Circle : Shape
{
    private float adjustedRadius;
    private int i;
    private Vector3 newPosition;

    public Circle()
    {
    }

    public Circle(int sides, float adjustedRadius, int i, Vector3 newPosition, Quaternion rotation, bool isCap, bool isFlipped)
    {
        this.sides = sides;
        this.adjustedRadius = adjustedRadius;
        this.i = i;
        this.newPosition = newPosition;
        this.rotation = rotation;
        this.isCap = isCap;
        this.isFlipped = isFlipped;
    }

    public void Initialize(int sides, float radius, int index, Vector3 position, Quaternion rotation, bool isCap, bool isFlipped)
    {
        base.Initialize(sides, radius, index, position, rotation, isCap, isFlipped);
    }

    public override bool SupportsDepth()
    {
        return false; // Circle does NOT use Depth
    }

    protected override void CreateVertices()
    {
        Vector3 center = position;
        vertices.Add(center);

        float angleStep = 360f / sides;
        for (int i = 0; i < sides; i++)
        {
            Vector3 vertex = center;
            float currentAngle = Mathf.Deg2Rad * (i * angleStep);
            vertex.x += Mathf.Cos(currentAngle) * radius;
            vertex.y += Mathf.Sin(currentAngle) * radius;

            vertex = rotation * (vertex - center) + center;
            vertices.Add(vertex);
        }
    }
}