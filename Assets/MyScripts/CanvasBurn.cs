using UnityEngine;

public class CanvasBurn : MonoBehaviour
{
    public ParticleSystem smokeEffect; // SmokeEffect 연결

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Canvas"))
        {
            if (smokeEffect != null)
                smokeEffect.Play();

            CanvasFade fade = other.gameObject.GetComponent<CanvasFade>();
            if (fade == null)
                fade = other.gameObject.AddComponent<CanvasFade>();

            fade.SetSmokeEffect(smokeEffect); // 연기 파티클 전달
            fade.StartFade();
        }
    }
}