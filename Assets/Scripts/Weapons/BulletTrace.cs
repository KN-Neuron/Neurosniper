using UnityEngine;

public class BulletTrace : MonoBehaviour
{
    private LineRenderer lr;

    public void Setup(Vector3 start, Vector3 end, float duration)
    {
        if (lr == null) lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Destroy(gameObject, duration);
    }
}
