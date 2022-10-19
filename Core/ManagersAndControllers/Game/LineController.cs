using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private List<TimePointF> points;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        points = new List<TimePointF>();
    }

    public void SetUpLine(List<TimePointF> points)
    {
        if (lineRenderer == null)
            return;

        lineRenderer.positionCount = points.Count;
        this.points = points;
    }

    public void RefreshLine(List<TimePointF> points)
    {
        if (lineRenderer == null)
            return;

        if (lineRenderer.positionCount == 0)
        {
            SetUpLine(points);
        }

        for (int i = 0; i < this.points.Count; i++)
        {
            lineRenderer.SetPosition(i, new Vector2(points[i].X, points[i].Y));
        }
    }
}

