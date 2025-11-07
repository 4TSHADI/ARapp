using UnityEngine;

public class Petal : Shape
{
    private float adjustedRadius;
    private int i;
    private Vector3 newPosition;

    public Petal()
    {
    }

    public Petal(int sides, float adjustedRadius, int i, Vector3 newPosition, Quaternion rotation, bool isCap, bool isFlipped)
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
        return false; // Petal does NOT use Depth
    }

    protected override void CreateVertices()
    {
        Vector3 center = position;
        vertices.Add(center + Vector3.up * radius); // Tip of the cone (top of the petal)

        float angleStep = 360f / sides;
        for (int i = 0; i < sides; i++)
        {
            Vector3 vertex = center;
            float currentAngle = Mathf.Deg2Rad * (i * angleStep);

            // Create the circular base of the cone
            vertex.x += Mathf.Cos(currentAngle) * radius;
            vertex.z += Mathf.Sin(currentAngle) * radius; // Use Z instead of Y for the base
            vertex.y -= radius; // Move the base down to form the cone shape

            // Apply rotation and position
            vertex = rotation * (vertex - center) + center;
            vertices.Add(vertex);
        }
    }

}