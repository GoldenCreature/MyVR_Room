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

    private bool isNearCanvas = false;

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
        // 캔버스 오브젝트 찾기
        GameObject canvas = GameObject.FindWithTag("Canvas");
        linesParent = new GameObject("DrawnLines");

        // 캔버스가 있으면 자식으로 넣기
        if (canvas != null)
            linesParent.transform.SetParent(canvas.transform);
    }

    void Update()
    {
        // 그리는 중이면 매 프레임 점 추가
        if (isDrawing && currentLine != null && isNearCanvas)
        {
            // 월드 좌표를 linesParent 로컬 좌표로 변환
            Vector3 tipPos = linesParent.transform.InverseTransformPoint(brushTip.position);

            if (currentPoints.Count == 0 ||
                Vector3.Distance(currentPoints[currentPoints.Count - 1], tipPos) > 0.001f)
            {
                currentPoints.Add(tipPos);
                currentLine.positionCount = currentPoints.Count;
                currentLine.SetPositions(currentPoints.ToArray());
            }
        }
    }

    public void SetNearCanvas(bool value)
    {
        isNearCanvas = value;
    }

    public void SetCanvas(GameObject newCanvas)
    {
        // 이미 이 캔버스에 그리고 있으면 무시
        if (linesParent != null && linesParent.transform.parent == newCanvas.transform)
            return;

        // 새 DrawnLines 생성
        linesParent = new GameObject("DrawnLines");
        linesParent.transform.SetParent(newCanvas.transform);
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
        currentLine.useWorldSpace = false;

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