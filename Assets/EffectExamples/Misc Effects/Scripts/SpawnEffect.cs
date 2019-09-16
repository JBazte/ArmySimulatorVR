using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEffect : MonoBehaviour
{

    public float spawnEffectTime = 2;
    public float pause = 1;
    public AnimationCurve fadeIn;

    ParticleSystem ps;
    float timer = 0;
    public Renderer _renderer;
    public Material mat;
    int shaderProperty;
    bool hasStarted;

    void Start()
    {
        shaderProperty = Shader.PropertyToID("_cutoff");
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();
        //ps = GetComponentInChildren<ParticleSystem>();

        //  var main = ps.main;
        //main.duration = spawnEffectTime;

        //ps.Play();

    }

    public float StartDisolve()
    {
        // ps.Play();
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
            mat.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
        }

    }
}
