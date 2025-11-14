using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class FogEffect : MonoBehaviour
{
    public Volume postProcessVolume;
    private ColorAdjustments colorAdjustments;
    public ParticleSystem fogParticle;

    private void Start()
    {
        if (postProcessVolume.profile.TryGet(out colorAdjustments))
        { colorAdjustments.colorFilter.value = Color.white; }
        
        PlayFogEffect();
    }

    public void PlayFogEffect()
    {
        DOTween.To(() => colorAdjustments.colorFilter.value, 
                   x => colorAdjustments.colorFilter.value = x, 
                   new Color(0.3f, 1f, 0.3f), 0.5f)
               .OnComplete(() =>
               {
                   DOTween.To(() => colorAdjustments.colorFilter.value,
                              x => colorAdjustments.colorFilter.value = x,
                              Color.white, 1f);
               });

        fogParticle.Play();
    }
}