using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class BrushController : MonoBehaviour
{
    public Renderer brushTipRenderer; // 붓 촉 렌더러
    public Transform brushTip; // 붓 촉 위치 기준점
    public float brushWidth = 0.01f; // 선 굵기

    private Color currentColor = Color.white;
    private XRGrabInteractable grabInteractable;

    private bool isDrawing = false;
    private LineRenderer currentLine; // 현재 그리고 있는 선
    private List<Vector3> currentPoints = new List<Vector3>(); // 현재 선의 점들
    private GameObject linesParent; // 선들을 담을 부모 오브젝트

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.activated.AddListener(OnActivate);
            grabInteractable.deactivated.AddListener(OnDeactivate);
        }

        // 선들 담을 빈 오브젝트 생성
        linesParent = new GameObject("DrawnLines");
    }

    void Update()
    {
        // 그리는 중이면 매 프레임 점 추가
        if (isDrawing && currentLine != null)
        {
            Vector3 tipPos = brushTip.position;

            // 마지막 점과 거리가 일정 이상일 때만 추가 (너무 촘촘하면 성능 저하)
            if (currentPoints.Count == 0 ||
                Vector3.Distance(currentPoints[currentPoints.Count - 1], tipPos) > 0.001f)
            {
                currentPoints.Add(tipPos);
                currentLine.positionCount = currentPoints.Count;
                currentLine.SetPositions(currentPoints.ToArray());
            }
        }
    }

    private void OnActivate(ActivateEventArgs args)
    {
        isDrawing = true;
        CreateNewLine(); // 새 선 생성
    }

    private void OnDeactivate(DeactivateEventArgs args)
    {
        isDrawing = false;
        currentLine = null;
        currentPoints.Clear();
    }

    private void CreateNewLine()
    {
        // 새 오브젝트에 LineRenderer 추가
        GameObject lineObj = new GameObject("Line");
        lineObj.transform.SetParent(linesParent.transform);

        currentLine = lineObj.AddComponent<LineRenderer>();
        currentLine.material = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
        currentLine.material.SetColor("_BaseColor", currentColor);
        currentLine.startWidth = brushWidth;
        currentLine.endWidth = brushWidth;
        currentLine.positionCount = 0;
        currentLine.useWorldSpace = true; // 월드 좌표 기준

        currentPoints.Clear();
    }

    public void SetColor(Color newColor)
    {
        currentColor = newColor;

        if (brushTipRenderer != null)
            brushTipRenderer.material.color = newColor;
    }

    public Color GetColor()
    {
        return currentColor;
    }
}