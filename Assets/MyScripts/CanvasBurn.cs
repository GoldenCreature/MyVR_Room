using UnityEngine;

public class CanvasBurn : MonoBehaviour
{
    public ParticleSystem smokeEffect;
    public Material canvasTransparentMaterial; // Transparent Material 저장 필드

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Canvas"))
        {
            if (smokeEffect != null)
                smokeEffect.Play();

            CanvasFade fade = other.gameObject.GetComponent<CanvasFade>();
            if (fade == null)
                fade = other.gameObject.AddComponent<CanvasFade>();

            fade.SetSmokeEffect(smokeEffect);
            fade.StartFade(canvasTransparentMaterial); // 머티리얼 전달
        }
    }
}