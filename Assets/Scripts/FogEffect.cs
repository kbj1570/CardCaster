using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class FogEffect : MonoBehaviour
{
    public Volume postProcessVolume; // Post Processing Volume
    private ColorAdjustments colorAdjustments;
    public ParticleSystem fogParticle; // 안개 파티클 시스템

    private void Start()
    {
        // Post Processing 설정
        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        {
            colorAdjustments.colorFilter.value = Color.white; // 기본 색상
        }

        PlayFogEffect();
    }

    public void PlayFogEffect()
    {
        // 초록색으로 변경 (0.5초 동안)
        DOTween.To(() => colorAdjustments.colorFilter.value, 
                   x => colorAdjustments.colorFilter.value = x, 
                   new Color(0.3f, 1f, 0.3f), 0.5f)
               .OnComplete(() =>
               {
                   // 원래 색으로 복귀 (1초 후)
                   DOTween.To(() => colorAdjustments.colorFilter.value,
                              x => colorAdjustments.colorFilter.value = x,
                              Color.white, 1f);
               });

        // 파티클 시스템 실행
        fogParticle.Play();
    }
}