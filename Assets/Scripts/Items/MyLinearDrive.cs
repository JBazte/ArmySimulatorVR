//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Drives a linear mapping based on position between 2 positions
//
//=============================================================================

using UnityEngine;
using System.Collections;
using Valve.VR.InteractionSystem;

//-------------------------------------------------------------------------
[RequireComponent(typeof(Interactable))]
public class MyLinearDrive : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;
    public LinearMapping linearMapping;
    public bool repositionGameObject = false;
    public bool maintainMomemntum = false;
    public float momemtumDampenRate = 5.0f;

    protected Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.DetachFromOtherHand;

    protected float initialMappingOffset;
    protected int numMappingChangeSamples = 5;
    protected float[] mappingChangeSamples;
    protected float prevMapping = 0.0f;
    protected float mappingChangeRate;
    protected int sampleCount = 0;

    protected Interactable interactable;
    public Quaternion startingRotation;
    private Weapon attachedWeapon;
    private Magazine magazine;

    protected virtual void Awake()
    {
        mappingChangeSamples = new float[numMappingChangeSamples];
        interactable = GetComponent<Interactable>();
        magazine = GetComponent<Magazine>();
    }

    protected virtual void Start()
    {
        /*
        if (linearMapping == null)
        {
            linearMapping = GetComponentInParent<LinearMapping>();
        }

        if (linearMapping == null)
        {
            linearMapping = gameObject.AddComponent<LinearMapping>();
        }
         */

        //initialMappingOffset = linearMapping.value;

        if (repositionGameObject)
        {
            UpdateLinearMapping(transform);
        }
    }


    public void OnActivated(LinearMapping lm, Weapon attachedWeapon)
    {
        linearMapping = lm;
        transform.SetParent(attachedWeapon.transform);
        this.attachedWeapon = attachedWeapon;
        initialMappingOffset = linearMapping.value - CalculateLinearMapping(magazine.attachedHand.transform);
        sampleCount = 0;
        mappingChangeRate = 0.0f;
        repositionGameObject = true;
        GetComponentInChildren<Collider>().isTrigger = true;

    }



    protected virtual void OnDetachedFromHand(Hand hand)
    {
        CalculateMappingChangeRate();
        if (repositionGameObject)
        {
            magazine.OnDettachedFromWeapon();
            repositionGameObject = false;
            attachedWeapon.EndLinearDrive();
        }
    }


    protected void CalculateMappingChangeRate()
    {
        //Compute the mapping change rate
        mappingChangeRate = 0.0f;
        int mappingSamplesCount = Mathf.Min(sampleCount, mappingChangeSamples.Length);
        if (mappingSamplesCount != 0)
        {
            for (int i = 0; i < mappingSamplesCount; ++i)
            {
                mappingChangeRate += mappingChangeSamples[i];
            }
            mappingChangeRate /= mappingSamplesCount;
        }
    }

    protected void UpdateLinearMapping(Transform updateTransform)
    {

        if (repositionGameObject)
        {
            prevMapping = linearMapping.value;
            linearMapping.value = Mathf.Clamp01(initialMappingOffset + CalculateLinearMapping(updateTransform));

            mappingChangeSamples[sampleCount % mappingChangeSamples.Length] = (1.0f / Time.deltaTime) * (linearMapping.value - prevMapping);
            sampleCount++;
            transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMapping.value);
            transform.rotation = startingRotation;
        }
    }

    protected float CalculateLinearMapping(Transform updateTransform)
    {
        Vector3 direction = endPosition.position - startPosition.position;
        float length = direction.magnitude;
        direction.Normalize();

        Vector3 displacement = updateTransform.position - startPosition.position;

        return Vector3.Dot(displacement, direction) / length;
    }


    protected virtual void Update()
    {
        if (repositionGameObject)
        {
            if (maintainMomemntum && mappingChangeRate != 0.0f)
            {
                //Dampen the mapping change rate and apply it to the mapping
                mappingChangeRate = Mathf.Lerp(mappingChangeRate, 0.0f, momemtumDampenRate * Time.deltaTime);
                linearMapping.value = Mathf.Clamp01(linearMapping.value + (mappingChangeRate * Time.deltaTime));
                // Debug.Log(linearMapping.value + "2");


                transform.position = Vector3.Lerp(startPosition.position, endPosition.position, linearMapping.value);
            }
        }

        if (repositionGameObject)
        {
            //Debug.Log(linearMapping.value + "1");
            UpdateLinearMapping(interactable.attachedToHand.transform);
            if (linearMapping.value == 0)
            {
                repositionGameObject = false;
                GetComponentInChildren<Collider>().isTrigger = false;
                attachedWeapon.EndLinearDrive();
                linearMapping.RestoreOriginalValue();
            }
            if (linearMapping.value == 1)
            {
                repositionGameObject = false;
                attachedWeapon.EndLinearDrive();
                attachedWeapon.AttachMagazine(magazine);
                linearMapping.RestoreInvertedOriginalValue();
            }
        }

    }
}

