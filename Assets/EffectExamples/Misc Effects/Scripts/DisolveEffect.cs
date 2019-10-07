using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisolveEffect : MonoBehaviour
{

    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;

    ParticleSystem ps;
    float timer = 0;
    public Renderer[] renderers;

    int shaderProperty;
    bool hasStarted;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        StartDisolve();

        //ps = GetComponentInChildren<ParticleSystem>();

        //  var main = ps.main;
        //main.duration = spawnEffectTime;

        //ps.Play();


    }

    public float StartDisolve()
    {
        // ps.Play();
        renderers = GetComponentsInChildren<Renderer>();
        hasStarted = true;
        return spawnEffectTime;
    }

    public void ResetDisolve()
    {
        timer = 0;
        hasStarted = false;
    }

    void Update()
    {
        if (hasStarted)
        {
            if (timer < spawnEffectTime + pause)
            {
                timer += Time.deltaTime;
            }
            float value = fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer));
            foreach (var r in renderers)
            {
                foreach (var mat in r.materials)
                {
                    mat.SetFloat(shaderProperty, value);
                }

            }

        }

    }
}
