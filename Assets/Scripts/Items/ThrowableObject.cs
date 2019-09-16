using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody), typeof(VelocityEstimator))]
public class ThrowableObject : GrabableObject
{

    public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;
    [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
    public float releaseVelocityTimeOffset = -0.011f;
    public float scaleReleaseVelocity = 1.1f;


    protected VelocityEstimator velocityEstimator;
    private bool attached = false;
    private Rigidbody rigidBody;
    protected float attachTime;
    protected Vector3 attachPosition;

    private void Awake()
    {
        interactable = GetComponentInChildren<Interactable>();
        rigidBody = GetComponentInChildren<Rigidbody>();
        velocityEstimator = GetComponent<VelocityEstimator>();
    }
    private void OnDetachedFromHand(Hand hand)
    {
        attached = false;

        //onDetachFromHand.Invoke();

        hand.HoverUnlock(null);

        //rigidBody.interpolation = hadInterpolation;

        Vector3 velocity;
        Vector3 angularVelocity;

        GetReleaseVelocities(hand, out velocity, out angularVelocity);

        rigidBody.velocity = velocity;
        rigidBody.angularVelocity = angularVelocity;
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        //Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());

        //hadInterpolation = this.rigidbody.interpolation;

        attached = true;

        //onPickUp.Invoke();

        //hand.HoverLock(null);

        // rigidBody.interpolation = RigidbodyInterpolation.None;

        velocityEstimator.BeginEstimatingVelocity();

        //attachTime = Time.time;
        //attachPosition = transform.position;
        // attachRotation = transform.rotation;

    }
    public virtual void GetReleaseVelocities(Hand hand, out Vector3 velocity, out Vector3 angularVelocity)
    {
        if (hand.noSteamVRFallbackCamera && releaseVelocityStyle != ReleaseStyle.NoChange)
            releaseVelocityStyle = ReleaseStyle.ShortEstimation; // only type that works with fallback hand is short estimation.

        switch (releaseVelocityStyle)
        {
            case ReleaseStyle.ShortEstimation:
                velocityEstimator.FinishEstimatingVelocity();
                velocity = velocityEstimator.GetVelocityEstimate();
                angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
                break;
            case ReleaseStyle.AdvancedEstimation:
                hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                break;
            case ReleaseStyle.GetFromHand:
                velocity = hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset);
                angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
                break;
            default:
            case ReleaseStyle.NoChange:
                velocity = rigidBody.velocity;
                angularVelocity = rigidBody.angularVelocity;
                break;
        }

        if (releaseVelocityStyle != ReleaseStyle.NoChange)
            velocity *= scaleReleaseVelocity;
    }

}
