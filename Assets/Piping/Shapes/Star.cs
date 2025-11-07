using UnityEngine;

public class Star : Shape
{
    private float innerRadius;
    private float adjustedRadius;
    private float adjustedInnerRadius;
    private int i;
    private Vector3 newPosition;

    public Star()
    {
    }

    public Star(int sides, float adjustedRadius, float adjustedInnerRadius, int i, Vector3 newPosition, Quaternion rotation, bool isCap, bool isFlipped)
    {
        this.sides = sides;
        this.adjustedRadius = adjustedRadius;
        this.adjustedInnerRadius = adjustedInnerRadius;
        this.i = i;
        this.newPosition = newPosition;
        this.rotation = rotation;
        this.isCap = isCap;
        this.isFlipped = isFlipped;
    }

    public void Initialize(int sides, float radius, float innerRadius, int index, Vector3 position, Quaternion rotation, bool isCap, bool isFlipped)
    {
        this.innerRadius = innerRadius;
        base.Initialize(sides, radius, index, position, rotation, isCap, isFlipped);
    }

    public override bool SupportsDepth()
    {
        return true; // Star uses Depth
    }

    protected override void CreateVertices()
    {
        Vector3 center = position;
        vertices.Add(center);

        float angleStep = 360f / sides;
        for (int i = 0; i < sides; i++)
        {
            Vector3 vertex = center;
            float currentRadius = (i % 2 == 0) ? radius : innerRadius;
            float currentAngle = Mathf.Deg2Rad * (i * angleStep);
            vertex.x += Mathf.Cos(currentAngle) * currentRadius;
            vertex.y += Mathf.Sin(currentAngle) * currentRadius;

            vertex = rotation * (vertex - center) + center;
            vertices.Add(vertex);
        }
    }
}