using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTipDetector : MonoBehaviour
{
    private BrushController brushController;

    void Start()
    {
        // 부모에서 BrushController 찾기
        brushController = GetComponentInParent<BrushController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Canvas"))
            brushController.SetNearCanvas(true);
            brushController.SetCanvas(other.gameObject);    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Canvas"))
            brushController.SetNearCanvas(false);
    }
}
