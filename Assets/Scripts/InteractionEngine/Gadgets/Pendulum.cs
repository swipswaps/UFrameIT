﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CommunicationEvents;

public class Pendulum : Gadget
    //Acts as a Pendulum starting at a Point
{
    //Attributes for simulating the drawing of a line
    private bool lineDrawingActivated;
    public LayerMask LayerPendulumHits;
    public LineRenderer lineRenderer;
    private List<Vector3> linePositions = new List<Vector3>();
    public Material linePreviewMaterial;

    new void Awake()
    {
        base.Awake();
        this.UiName = "Pendulum";
        if (MaxRange == 0)
            MaxRange = GlobalBehaviour.GadgetLaserDistance;
    }

    new void OnEnable()
    {
        base.OnEnable();
        this.ResetGadget();
        ActivateLineDrawing();
    }

    void OnDisable()
    {
        this.ResetGadget();
    }

    public override void OnHit(RaycastHit hit)
    {

        if (!this.isActiveAndEnabled) return;

        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Point"))
        {
            PointFact tempFact = StageStatic.stage.factState[hit.transform.GetComponent<FactObject>().URI] as PointFact;

            //Raycast downwoard
            RaycastHit ground;
            if(Physics.Raycast(tempFact.Point, Vector3.down, out ground, Mathf.Infinity, this.LayerPendulumHits.value))
            {
                FactManager.AddPointFact(ground);
            }
        }
    }
    void Update()
    {
        if (!this.isActiveAndEnabled)
            return;

        if (this.lineDrawingActivated)
            UpdateLineDrawing();
    }

    private void ResetGadget()
    {
        DeactivateLineDrawing();
    }

    private void ActivateLineDrawing()
    {
        this.lineRenderer.enabled = true;
        this.lineRenderer.positionCount = 2;
        this.lineRenderer.material = this.linePreviewMaterial;

        lineRenderer.startWidth = 0.095f;
        lineRenderer.endWidth = 0.095f;
        //Set LineDrawing activated
        this.lineDrawingActivated = true;

        //initiate linePositions-Array
        linePositions.Add(this.Cursor.transform.position);
        linePositions.Add(this.Cursor.transform.position);

        UpdateLineDrawing();
    }

    //Updates the points of the Lines when baseLine was selected in LineMode
    private void UpdateLineDrawing()
    {
        this.linePositions[0] = this.Cursor.transform.position;

        //Raycast downwoard
        RaycastHit ground;
        if (Physics.Raycast(this.linePositions[0], Vector3.down, out ground, Mathf.Infinity, this.LayerPendulumHits.value))
            this.linePositions[1] = ground.point;
        else
            this.linePositions[1] = this.linePositions[0];

        this.lineRenderer.SetPosition(0, this.linePositions[0]);
        this.lineRenderer.SetPosition(1, this.linePositions[1]);
    }

    //Deactivate LineDrawing so that no Line gets drawn when Cursor changes
    private void DeactivateLineDrawing()
    {
        this.lineRenderer.positionCount = 0;
        this.linePositions = new List<Vector3>();
        this.lineDrawingActivated = false;
        this.lineRenderer.enabled = false;
    }

}
