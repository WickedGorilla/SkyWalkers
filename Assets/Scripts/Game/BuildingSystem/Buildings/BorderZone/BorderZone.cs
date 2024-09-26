using UnityEngine;

public class BorderZone : MonoBehaviour
{
    [SerializeField] private Vector2 _size;
    [SerializeField] private Vector2 _center;

    public Vector2 Center => _center;

    public Vector2 Size => _size;

    public Vector2 BorderRight
        => GetBorderPointFromAxis(transform.lossyScale.x, _size.x, Vector2.right);

    public Vector2 BorderLeft
        => GetBorderPointFromAxis(transform.lossyScale.x, -_size.x, Vector2.left);

    public Vector2 BorderUp
        => GetBorderPointFromAxis(transform.lossyScale.y, _size.y, Vector2.up);
    
    public Vector2 BorderDown
        => GetBorderPointFromAxis(transform.lossyScale.y, -_size.y, Vector2.down);
    
    private Vector2 OffsetCenter => Vector2.Scale(_center, transform.lossyScale);

    private Vector2 GetBorderPointFromAxis(float scale, float size, Vector2 axis)
    {
        float distanceToCenter = Mathf.Abs(scale * size / 2);
        Vector2 direction = distanceToCenter * axis;
        Vector2 line = transform.rotation * (direction + OffsetCenter);

        return (Vector2)transform.position + line;
    }

    private Vector2[] GetVertices()
    {
        Vector2 center = (Vector2)(transform.rotation * OffsetCenter) + (Vector2)transform.position;
        Vector2 size = Vector2.Scale(transform.lossyScale, _size);

        Vector2[] point = new Vector2[4];

        Vector2 firstPoint1 = -size / 2;
        point[0] = firstPoint1;
        point[1] = firstPoint1 + new Vector2(size.x, 0);
        point[2] = firstPoint1 + new Vector2(0, size.y);

        Vector2 firstPoint2 = size / 2;
        point[3] = firstPoint2;

        for (int i = 0; i < point.Length; i++)
        {
            point[i] = transform.rotation * point[i];
            point[i] += center;
        }

        return point;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector2[] vertices = GetVertices();

        foreach (Vector2 vertex in vertices)
            Gizmos.DrawSphere(vertex, 0.1f);

        Vector2 center = transform.rotation * OffsetCenter + transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, BorderUp);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(center, BorderRight);
    }
#endif
}