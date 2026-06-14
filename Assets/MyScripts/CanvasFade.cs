using UnityEngine;

public class CanvasFade : MonoBehaviour
{
    // 외부에서 Transparent 머티리얼을 받아올 필드
    private Material transparentMaterial;
    private float fadeDuration = 5f;
    private float timer = 0f;
    private bool isFading = false;
    private Renderer[] renderers;
    private ParticleSystem smokeEffect;

    public void StartFade(Material transMat)
    {
        // 외부에서 머티리얼 받아오기
        transparentMaterial = transMat;
        renderers = GetComponentsInChildren<Renderer>();

        // 기존 머티리얼 대신 Transparent 머티리얼로 교체
        foreach (Renderer r in renderers)
        {
            // 기존 색상 먼저 저장
            Color[] originalColors = new Color[r.materials.Length];
            for (int i = 0; i < r.materials.Length; i++)
            {
                originalColors[i] = r.materials[i].color;
            }

            // 머티리얼 교체
            Material[] mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(transparentMaterial);
                mats[i].color = originalColors[i]; // 저장해둔 색상 적용
            }
            r.materials = mats;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = true;

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

    public void SetSmokeEffect(ParticleSystem ps)
    {
        smokeEffect = ps;
    }
}