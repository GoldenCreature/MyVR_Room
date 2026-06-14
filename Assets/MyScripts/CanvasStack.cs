using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CanvasStack : MonoBehaviour
{
    public GameObject canvasPrefab; // 복사할 캔버스 프리팹

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // 원본 놓기
        grabInteractable.interactionManager.SelectExit(
            (IXRSelectInteractor)args.interactorObject,
            grabInteractable
        );

        // 복사본 생성
        GameObject newCanvas = Instantiate(canvasPrefab, transform.position, transform.rotation);

        // 복사본 잡기
        var newGrab = newCanvas.GetComponent<XRGrabInteractable>();
        if (newGrab != null)
        {
            grabInteractable.interactionManager.SelectEnter(
                (IXRSelectInteractor)args.interactorObject,
                newGrab
            );
        }
    }
}