using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveGenerator : MonoBehaviour
{
    [SerializeField] private float heightChangeForCurvePoint = 0.005f;
    [SerializeField] private float rotationSpeed = 0.1f;
    [SerializeField] private Vector3[] localPoints;

    [Header("TEST")]
    [SerializeField] private Transform from, to;
    [SerializeField] private float distance = 0.08f;

    public Vector3[] LocalPoints => localPoints;

    private void Update() {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    [ContextMenu("Calculate Test")]
    public void Test() {
        SimulateLine(from.position, to.position, distance);
    }

    public void SimulateLine(Vector3 from, Vector3 to, float distancePerPoint) {
        float distance = Vector3.Distance(from, to);
        int numberOfPoint = (int)(distance / distancePerPoint) + 1; // always +1 in case the gap at the last point

        transform.position = from;
        transform.LookAt(to, Vector3.up);

        Vector3 startPoint = Vector3.zero;
        Vector3 lastPoint = transform.InverseTransformPoint(to);

        localPoints = new Vector3[numberOfPoint];
        localPoints[0] = startPoint;
        localPoints[numberOfPoint - 1] = lastPoint;

        float increaseZPerPoint = distancePerPoint;
        float currentZ = 0f;

        for (int i = 1; i < numberOfPoint - 1; i++) {
            currentZ += increaseZPerPoint;

            localPoints[i] = new Vector3(0f, 0f, currentZ);
        }

        SimulateBezier();
    }

    struct PointAnchor {
        public int index;
        public Vector3 anchor1;
        public Vector3 point;
        public Vector3 anchor2;
    }

    private void SimulateBezier() {
        int numberOfPoint = localPoints.Length / 5;

        PointAnchor[] pointAnchors = new PointAnchor[numberOfPoint + 2];
        pointAnchors[0] = new() { index = 0, point = localPoints[0] };
        pointAnchors[^1] = new() { index = localPoints.Length - 1, point = localPoints[localPoints.Length - 1] };
        int increased = localPoints.Length / (numberOfPoint + 1);
        
        for (int i = 1; i < pointAnchors.Length - 1; i++) {
            int current = increased * i;

            localPoints[current] = new Vector3(localPoints[current].x, localPoints[current].y + Random.Range(-heightChangeForCurvePoint, heightChangeForCurvePoint), localPoints[current].z);

            pointAnchors[i].index = current;
            pointAnchors[i].point = localPoints[current];
        }

        if (pointAnchors.Length == 1)
            return;

        for (int i = 0; i < pointAnchors.Length; i++) {
            if (i < pointAnchors.Length - 1) {
                float anchorZ = (pointAnchors[i].point.z + pointAnchors[i + 1].point.z) / 2f;
                pointAnchors[i].anchor2 = new Vector3(pointAnchors[i].point.x, pointAnchors[i].point.y, anchorZ);
            }

            if (i > 0) {
                pointAnchors[i].anchor1 = new Vector3(pointAnchors[i].point.x, pointAnchors[i].point.y, pointAnchors[i - 1].anchor2.z);
            }

            Debug.Log(pointAnchors[i].anchor1 + " , " + pointAnchors[i].point + " , " + pointAnchors[i].anchor2);
        }

        for (int i = 0; i < pointAnchors.Length - 1; i++) {
            int start = pointAnchors[i].index;
            int end = pointAnchors[i + 1].index;
            int length = end - start;

            for (int j = start + 1; j < end; j++) {
                float t = (float)(j - start) / length;
                localPoints[j] = CubicBezierLerpFunction(t, pointAnchors[i].point, pointAnchors[i].anchor2, pointAnchors[i + 1].anchor1, pointAnchors[i + 1].point);
            }
        }
    }

    private Vector3 CubicBezierLerpFunction(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }

    public void LookAt(Vector3 target) {
        Vector3 directionToTarget = target - transform.position;

        // Calculate the rotation angles using trigonometry (use Mathf.Atan2)
        float angleY = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;
        float angleX = Mathf.Atan2(directionToTarget.y, Mathf.Sqrt(directionToTarget.x * directionToTarget.x + directionToTarget.z * directionToTarget.z)) * Mathf.Rad2Deg;

        // Smoothly interpolate the current rotation towards the target rotation
        Quaternion targetRotation = Quaternion.Euler(new Vector3(angleX, angleY, transform.rotation.eulerAngles.z));
        transform.rotation = targetRotation;
    }

    private void OnDrawGizmos() {
        Vector3[] globalLine = new Vector3[localPoints.Length];
        transform.TransformPoints(localPoints, globalLine);

        for (int i = 0; i < globalLine.Length - 1; i++) {
            Gizmos.DrawLine(globalLine[i], globalLine[i + 1]);
        }
    }
}
