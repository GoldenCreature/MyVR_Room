using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    public Color color; // 인스펙터에서 색상 지정

    private void OnTriggerEnter(Collider other)
    {
        // 닿은 오브젝트에서 BrushController 찾기
        BrushController brush = other.GetComponentInParent<BrushController>();

        if (brush != null)
        {
            brush.SetColor(color);
            Debug.Log("색상 변경: " + color);
        }
    }
}