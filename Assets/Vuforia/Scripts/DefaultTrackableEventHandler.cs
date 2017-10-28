/*==============================================================================
Copyright (c) 2017 PTC Inc. All Rights Reserved.

Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System.Collections.Generic;
using UnityEngine;
using Vuforia;

/// <summary>
///     A custom handler that implements the ITrackableEventHandler interface.
/// </summary>
public class DefaultTrackableEventHandler : MonoBehaviour, ITrackableEventHandler
{
    #region PRIVATE_MEMBER_VARIABLES

    protected TrackableBehaviour mTrackableBehaviour;

    #endregion // PRIVATE_MEMBER_VARIABLES

    #region UNTIY_MONOBEHAVIOUR_METHODS

    protected virtual void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    #endregion // UNTIY_MONOBEHAVIOUR_METHODS

    #region PUBLIC_METHODS

    /// <summary>
    ///     Implementation of the ITrackableEventHandler function called when the
    ///     tracking state changes.
    /// </summary>
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
            OnTrackingFound();
        }
        else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
                 newStatus == TrackableBehaviour.Status.NOT_FOUND)
        {
            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
            OnTrackingLost();
        }
        else
        {
            // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
            // Vuforia is starting, but tracking has not been lost or found yet
            // Call OnTrackingLost() to hide the augmentations
            OnTrackingLost();
        }
    }

    #endregion // PUBLIC_METHODS

    #region PRIVATE_METHODS

    public static string objName;

    protected virtual void OnTrackingFound()
    {
        //time scale on
        //Time.timeScale = 1f;

        //Текущий го который проверяется
        GameObject curGO = new GameObject();
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        var queue = new Queue<GameObject>();
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();

        foreach (Transform t in ts)
        {
            if (t != null && t.gameObject != null)
                queue.Enqueue(t.gameObject);
        }

        while (queue.Count > 0)
        {
            curGO = queue.Dequeue();
            if (curGO.GetComponent<Renderer>() != null)
                curGO.GetComponent<Renderer>().enabled = true;
            if (curGO.GetComponent<Collider>() != null)
                curGO.GetComponent<Collider>().enabled = true;
            if (curGO.GetComponent<Canvas>() != null)
                curGO.GetComponent<Canvas>().enabled = true;
            if (curGO.GetComponent<Animator>() != null)
                curGO.GetComponent<Animator>().enabled = true;
            if (curGO.transform.childCount > 0)
            {

                //Оттут поправ коунт на коунт-1
                for (int i = 0; i < curGO.transform.childCount; i++)
                {
                    queue.Enqueue(curGO.transform.GetChild(i).gameObject);
                }

            }
        }
        /*
        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
            */
    }


    protected virtual void OnTrackingLost()
    {
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);
        var animatorComponents = GetComponentInChildren<Animator>(true);

        //time scale off
        //Time.timeScale = 0f;
        objName = transform.GetChild(0).name;

        Debug.Log(TakeAniTime());

        if (animatorComponents != null)
            animatorComponents.enabled = false;

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }

    #endregion // PRIVATE_METHODS


    #region TAKE_ANIMATION_TIME

    private float TakeAniTime()
    {
        //var obj = transform.GetChild(0);
        //Animator objectAnim = GetComponentInChildren<Animator>();
        //if (objectAnim != null)
        //{

        //    obj.animation.

        //}
        //else
        return -1f;
    }



    #endregion //TAKE_ANIMATION_TIME
}
