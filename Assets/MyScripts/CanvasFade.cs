using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    private float fadeDuration = 5f;
    private float timer = 0f;
    private bool isFading = false;
    private Renderer[] renderers;

    public void StartFade()
    {
        renderers = GetComponentsInChildren<Renderer>();

        // 머티리얼을 Transparent로 변경
        foreach (Renderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                mat.SetFloat("_Surface", 1); // URP Transparent
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
            }
        }

        // Rigidbody 중력 켜서 떨어지게
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }

        // XR Grab 비활성화 (타는 중에 못 잡게)
        var grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.XRGrabInteractable>();
        if (grab != null)
            grab.enabled = false;

        isFading = true;
    }

    void Update()
    {
        if (!isFading) return;

        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

        foreach (Renderer r in renderers)
        {
            foreach (Material mat in r.materials)
            {
                Color c = mat.color;
                c.a = alpha;
                mat.color = c;
            }
        }

        if (timer >= fadeDuration)
        {
            smokeEffect?.Stop();
            Destroy(gameObject);
        }
    }

    private ParticleSystem smokeEffect;

    public void SetSmokeEffect(ParticleSystem ps)
    {
        smokeEffect = ps;
    }
}