﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static CommunicationEvents;

public class DisplayFacts : MonoBehaviour
{
    public Dictionary<Type, GameObject> prefabDictionary;

    public Dictionary<string, GameObject> displayedFacts = new Dictionary<string, GameObject>();

    public GameObject prefab_Point;
    public GameObject prefab_Distance;
    public GameObject prefab_Angle;
    public GameObject prefab_Default;
    public GameObject prefab_OnLine;
    public GameObject prefab_Line;

    public int x_Start;
    public int y_Start;
    public int X_Pacece_Between_Items;
    public int y_Pacece_Between_Items;
    public int number_of_Column;

    //Start is called before the first frame update
    void Start()
    {
        prefabDictionary = new Dictionary<Type, GameObject>() {
            {typeof(PointFact), prefab_Point},
            {typeof(LineFact), prefab_Distance},
            {typeof(RayFact), prefab_Line},
            {typeof(AngleFact), prefab_Angle},
            {typeof(OnLineFact), prefab_OnLine}
        };

        var rect = GetComponent<RectTransform>();
        x_Start = (int)(rect.rect.x + X_Pacece_Between_Items * .5f);
        y_Start = (int)(-rect.rect.y - y_Pacece_Between_Items * .5f);//);
        number_of_Column = Mathf.Max(1, (int)(rect.rect.width / prefab_Point.GetComponent<RectTransform>().rect.width) - 1);

        AddFactEvent.AddListener(AddFact);
        RemoveFactEvent.AddListener(RemoveFact);
        AnimateExistingFactEvent.AddListener(AnimateFact);
    }

    public void AddFact(Fact fact) {
        var fid = fact.Id;
        var obj = CreateDisplay(transform, fact);
        obj.GetComponent<RectTransform>().localPosition = GetPosition(displayedFacts.Count);
        displayedFacts.Add(fact.Id, obj);
    }

    public void RemoveFact(Fact fact)
    {
        GameObject.Destroy(displayedFacts[fact.Id]);
        displayedFacts.Remove(fact.Id);
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        int i = 0;
        foreach (var element in displayedFacts)
            element.Value.GetComponent<RectTransform>().localPosition = GetPosition(i++);
    }

    public void AnimateFact(Fact fact) {
        var factIcon = displayedFacts[fact.Id];
        factIcon.GetComponentInChildren<ImageHintAnimation>().AnimationTrigger();
    }

    private GameObject CreateDisplay(Transform transform, Fact fact)
    {
        return fact.instantiateDisplay(prefabDictionary[fact.GetType()], transform);
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(x_Start + (X_Pacece_Between_Items * (i % number_of_Column)), y_Start + (-y_Pacece_Between_Items * (i / number_of_Column)), 0f);
    }

}
