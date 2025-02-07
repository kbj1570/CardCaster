using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Inst;
    
    public GameObject waterEffect;
    public GameObject fireEffect;
    public GameObject earthEffect;
    public GameObject windEffect;
    public GameObject lightEffect;
    public GameObject darkEffect;

    private Dictionary<EServentAttribute, GameObject> effectDict;

    void Awake()
    {
        Inst = this;
        effectDict = new Dictionary<EServentAttribute, GameObject>
        {
            { EServentAttribute.Water, waterEffect },
            { EServentAttribute.Fire, fireEffect },
            { EServentAttribute.Earth, earthEffect },
            { EServentAttribute.Wind, windEffect },
            { EServentAttribute.Light, lightEffect },
            { EServentAttribute.Dark, darkEffect }
        };
    }

    public void SpawnSummonEffect(EServentAttribute element, Vector3 position)
    {
        if (effectDict.TryGetValue(element, out GameObject effectPrefab))
        {
            GameObject effectInstance = Instantiate(effectPrefab, position - new UnityEngine.Vector3(0,2,0), Quaternion.identity);
            float delay = 0.3f; // 파티클이 0.5초 동안 유지된 후 멈추게 함
            StartCoroutine(StopAndDestroyParticle(effectInstance.GetComponent<ParticleSystem>(), delay));
        }
    }

    IEnumerator StopAndDestroyParticle(ParticleSystem ps, float delay)
    {
        yield return new WaitForSeconds(delay);
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting); // 파티클 방출 중단

        // 파티클이 완전히 사라진 후 삭제
        yield return new WaitForSeconds(ps.main.startLifetime.constantMax);
    }
}
