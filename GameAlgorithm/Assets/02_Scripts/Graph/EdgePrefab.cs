using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgePrefab : MonoBehaviour
{
    private LineRenderer lineRenderer;
    void Start()
    {
        // �̹� �ִ� LineRenderer ������
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.gray;
        lineRenderer.endColor = Color.gray;
        lineRenderer.positionCount = 2;
    }
}
