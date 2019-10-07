using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Selectable
{
    Renderer[] meshRenderers;
    [SerializeField]
    Transform grx;
    private Dictionary<Renderer, Material[]> originalMaterials = new Dictionary<Renderer, Material[]>();
    public int EnemyID
    {
        get
        {
            return enemyID;
        }
        set
        {
            if (enemyID == int.MinValue && value != int.MinValue)
            {
                enemyID = value;
            }
        }
    }
    public void ResetMaterials()
    {
        foreach (KeyValuePair<Renderer, Material[]> pair in originalMaterials)
        {
            pair.Key.materials = pair.Value;
        }
    }
    public void ChangeToNewMaterial(Material newMaterial)
    {
        //meshRenderers = grx.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in meshRenderers)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j] = newMaterial;
            }
            rend.materials = mats;
        }
    }

    public void ChangeToNewColor(Color newColor)
    {
       //meshRenderers = grx.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in meshRenderers)
        {
            var mats = new Material[rend.materials.Length];
            for (var j = 0; j < rend.materials.Length; j++)
            {
                mats[j].color = newColor;
            }
            rend.materials = mats;
        }
    }

    private void OnEnable()
    {
        //children is a reference to the renderers

        //meshRenderers = GetComponentsInChildren<Renderer>();
        meshRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in meshRenderers)
        {
            //make array of all materials in renderer
            Material[] materials = rend.materials;
            //add to dictionary renderer and material
            originalMaterials[rend] = materials;
        }
    }


    public EnemyFactory OriginFactory
    {
        get
        {
            return originFactory;
        }

        set
        {
            if (originFactory == null)
            {
                originFactory = value;
            }
            else
            {
                Debug.LogError("Can't Set Two different Factories to an Enemy");
            }
        }

    }

    public override SelectableTypes Type
    {
        get
        {
            return SelectableTypes.Enemy;
        }
    }

    private int enemyID = int.MinValue;
    private EnemyFactory originFactory;
    public void Recycle()
    {
        if (originFactory == null)
        {
            Debug.LogWarning(gameObject.name + "Doesn't have a origin factory");
            Destroy(gameObject);
        }
        ResetMaterials();
        originFactory.Reclaim(this);
    }



    public override void OnSelected()
    {
        // Show Info
        // Show Options
    }

    public override void Diselected()
    {
        // UnShow Info

    }

}
