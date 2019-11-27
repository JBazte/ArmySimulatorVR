using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSquareSpawner : MonoBehaviour
{

    [SerializeField]
    WaveController waveController;
    [SerializeField]
    bool surfaceOnly;
    [SerializeField]
    bool bidimensional;
    [SerializeField]
    Box[] boxesInstances = null;
    public Vector3 SpawnPoint
    {
        get
        {
            Vector3 p;

            p.x = Random.Range(-.5f, .5f);
            p.y = Random.Range(-.5f, .5f);
            p.z = Random.Range(-.5f, .5f);

            if (surfaceOnly)
            {
                int axis = Random.Range(0, 3);
                p[axis] = p[axis] < 0f ? -.5f : .5f;
            }
            if (bidimensional)
            {
                p.y = -0.5f;
            }
            return transform.TransformPoint(p);
        }

    }

    public void SpawnBox()
    {

        int factoryIndex = Random.Range(0, boxesInstances.Length);
        Box box = Instantiate(boxesInstances[factoryIndex], SpawnPoint, Quaternion.identity);
        box.transform.position = SpawnPoint;
        //if (box.)
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    void Start()
    {
        waveController.OnWaveStart += SpawnBox;
    }


}


