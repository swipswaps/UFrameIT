﻿using System.Collections;
using UnityEngine;
using static CommunicationEvents;
using TMPro;
using static Tape;

public class MenuScript : MonoBehaviour
{

    public enum MenuStates {None,One,Two,Three,Four,FactGenerating,Five};
    public MenuStates currentState;

    public GameObject pageOne;
    public GameObject FactGenering;
    public GameObject pageTwo;
    public GameObject pageThree;
    public GameObject pageFour;
    public GameObject pageFive;
    public Problemobject problem;
    public GameObject charakterPrefab;
    public GameObject charakterPrefab2;
    public GameObject player;
    public GameObject walkAroundPoint;
    public GameObject success_slot;
    public GameObject failure_slot;
    public GameObject npc_slot;
    public GameObject problemPrefab;
    public GameObject problemObject;

    //npc counter
    public static int npc_counter = 1;
    

    //nur für test
    public GameObject testsuccessReaction;
    public int debugcounter;

    public static string desired_problemfact;

    public KeyCode MenuKey = KeyCode.F2;
    public KeyCode NPCKey;
    public KeyCode NPCKey2;

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController CamControl;
    public MeshRenderer CursorRenderer;
    // integer check whether frameitUI is active or not
    public static int tabcounter;

    // "press key to talk" - reference
    


    // Start is called before the first frame update
    void Start()
    {
        currentState = MenuStates.None;
        //charakterPrefab = Instantiate(charakterPrefab2);
        //problemObject = Instantiate(problemPrefab);
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().problemObject = problemObject;
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key = NPCKey;
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key2 = NPCKey2;
        //charakterPrefab.GetComponentInChildren<CharacterDialog>().Key = NPCKey;
        tabcounter = 0;

        //für debug
        debugcounter = 0;

        


    }

    // Update is called once per frame
    void Update()
    {
        //for debugging
        /*if (Input.GetKeyDown(KeyCode.E) && debugcounter%2 == 0)
        {
            testsuccessReaction.SetActive(true);
            for(int i = 0; i<3; i++)
            {
                testsuccessReaction.transform.GetChild(i).transform.position = new Vector3(charakterPrefab2.transform.position.x, 40, charakterPrefab2.transform.position.z);
                ParticleSystem ps = testsuccessReaction.transform.GetChild(i).gameObject.GetComponent<ParticleSystem>();
                var em = ps.emission;
                em.enabled = true;
            }
            Debug.Log("testsuccessReaction gestartet " + debugcounter.ToString());
            debugcounter++;
        }
        else if (Input.GetKeyDown(KeyCode.E) && debugcounter % 2 == 1)
        {
            testsuccessReaction.SetActive(false);
            Debug.Log("testsuccessReaction gestoppt " + debugcounter.ToString());
            debugcounter++;
        }*/

        //checks whether tab was pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            tabcounter++;
        }

        if (Input.GetKeyDown(MenuKey) && currentState != MenuStates.Three)
        {
            if(currentState == MenuStates.None)
            {
                currentState = MenuStates.One;
            }
            else
            {
                currentState = MenuStates.None;
            }
        }

            switch (currentState)
        {
            case MenuStates.One:
                pageOne.SetActive(true);
                FactGenering.SetActive(false);
                pageTwo.SetActive(false);
                pageThree.SetActive(false);
                pageFour.SetActive(false);
                pageFive.SetActive(false);
                CamControl.enabled = false;
                CursorRenderer.enabled = false;
                Time.timeScale = 0f;
                break;

            case MenuStates.Two:
                pageOne.SetActive(false);
                FactGenering.SetActive(false);
                pageTwo.SetActive(true);
                pageThree.SetActive(false);
                pageFour.SetActive(false);
                pageFive.SetActive(false);
                CamControl.enabled = false;
                CursorRenderer.enabled = false;
                Time.timeScale = 0f;
                break;

            case MenuStates.Three:
                pageOne.SetActive(false);
                FactGenering.SetActive(false);
                pageTwo.SetActive(false);
                pageThree.SetActive(true);
                pageFour.SetActive(false);
                pageFive.SetActive(false);
                CamControl.enabled = false;
                CursorRenderer.enabled = false;
                Time.timeScale = 0f;
                break;

            case MenuStates.Four:
                pageOne.SetActive(false);
                FactGenering.SetActive(false);
                pageTwo.SetActive(false);
                pageThree.SetActive(false);
                pageFour.SetActive(true);
                pageFive.SetActive(false);
                CamControl.enabled = false;
                CursorRenderer.enabled = false;
                Time.timeScale = 0f;
                break;

            case MenuStates.Five:
                pageOne.SetActive(false);
                FactGenering.SetActive(false);
                pageTwo.SetActive(false);
                pageThree.SetActive(false);
                pageFour.SetActive(false);
                pageFive.SetActive(true);
                if (tabcounter % 2 == 0)
                {
                    CamControl.enabled = false;
                    CursorRenderer.enabled = false;
                }
                else
                {
                    CamControl.enabled = true;
                    CursorRenderer.enabled = true;
                }
                Time.timeScale = 1f;
                break;

            case MenuStates.None:
                pageOne.SetActive(false);
                FactGenering.SetActive(false);
                pageTwo.SetActive(false);
                pageThree.SetActive(false);
                pageFour.SetActive(false);
                pageFive.SetActive(false);
                if(tabcounter % 2 == 0)
                {
                    CamControl.enabled = false;
                    CursorRenderer.enabled = false;
                }
                else
                {
                    CamControl.enabled = true;
                    CursorRenderer.enabled = true;
                }
                Time.timeScale = 1f;
                break;

            case MenuStates.FactGenerating:
                pageOne.SetActive(false);
                FactGenering.SetActive(true);
                pageTwo.SetActive(false);
                pageThree.SetActive(false);
                pageFour.SetActive(false);
                pageFive.SetActive(false);
                if (tabcounter % 2 == 0)
                {
                    CamControl.enabled = false;
                    CursorRenderer.enabled = false;
                }
                else
                {
                    CamControl.enabled = true;
                    CursorRenderer.enabled = true;
                }
                Time.timeScale = 1f;
                break;
        }
        
    }

    public void OnAngle()
    {
        Tape.creatormodeON = true;
        currentState = MenuStates.FactGenerating;
        GetChildByName(pageTwo, "Actual desired scrolls").GetComponent<TMP_Text>().text = "SupplementaryAngles or AngleSum";
        GetChildByName(pageTwo, "Actual desired gadgets").GetComponent<TMP_Text>().text = "Pointer-Gadget, Angle-Gadget, Line-Gadget";
        desired_problemfact = "Angle";
        //problem = new Problemobject();

        //Character Instantiieren
        charakterPrefab = Instantiate(charakterPrefab2);
        problemObject = Instantiate(problemPrefab);
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().problemObject = problemObject;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key = NPCKey;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key2 = NPCKey2;
    }

    public void OnPoint()
    {
        Tape.creatormodeON = true;
        currentState = MenuStates.FactGenerating;
        GetChildByName(pageTwo, "Actual desired scrolls").GetComponent<TMP_Text>().text = "MidPoint";
        GetChildByName(pageTwo, "Actual desired gadgets").GetComponent<TMP_Text>().text = "Pointer-Gadget";
        desired_problemfact = "Point";
        //problem = new Problemobject();

        //Character Instantiieren
        charakterPrefab = Instantiate(charakterPrefab2);
        problemObject = Instantiate(problemPrefab);
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().problemObject = problemObject;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key = NPCKey;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key2 = NPCKey2;
    }

    public void OnDistance()
    {
        Tape.creatormodeON = true;
        currentState = MenuStates.FactGenerating;
        GetChildByName(pageTwo, "Actual desired scrolls").GetComponent<TMP_Text>().text = "OppositeLen";
        GetChildByName(pageTwo, "Actual desired gadgets").GetComponent<TMP_Text>().text = "Pointer-Gadget, Angle-Gadget, Line-Gadget";
        desired_problemfact = "Distance";
        //problem = new Problemobject();

        //Character Instantiieren
        charakterPrefab = Instantiate(charakterPrefab2);
        problemObject = Instantiate(problemPrefab);
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().problemObject = problemObject;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key = NPCKey;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().Key2 = NPCKey2;
    }

    public void OnContinue()
    {
        Tape.creatormodeON = false;
        currentState = MenuStates.Two;
    }

    public void OnApply2()
    {
        // bei go-around-point den tatsächlichen goaround point so machen dass er am boden ist, damit npc nicht in der luft rumlaeuft
        // if two facts are in slots then go to next page and save the tho facts in the problemobject otherwise show error warning
        Fact problemFact = GetChildByName(pageTwo, "Slot 1").GetComponent<DropHandling>().currentFact;
        Fact goAroundFact = GetChildByName(pageTwo, "Slot 2").GetComponent<DropHandling>().currentFact;
        if(problemFact != null && goAroundFact != null && goAroundFact is PointFact)
        {
            switch (desired_problemfact)
            {
                case "Point":
                    if(problemFact is PointFact)
                    {
                        currentState = MenuStates.Three;
                        problemObject.GetComponent<Problemobject>().problemFact = problemFact;
                        problemObject.GetComponent<Problemobject>().goAroundPoint = goAroundFact;
                    }
                    else
                    {
                        Debug.Log("you need to select a Point Fact for the problem object!");
                    }
                    break;

                case "Angle":
                    if(problemFact is AngleFact)
                    {
                        currentState = MenuStates.Three;
                        problemObject.GetComponent<Problemobject>().problemFact = problemFact;
                        problemObject.GetComponent<Problemobject>().goAroundPoint = goAroundFact;
                    }
                    else
                    {
                        Debug.Log("you need to select a Angle Fact for the problem object!");
                    }
                    break;

                case "Distance":
                    if(problemFact is LineFact)
                    {
                        currentState = MenuStates.Three;
                        problemObject.GetComponent<Problemobject>().problemFact = problemFact;
                        problemObject.GetComponent<Problemobject>().goAroundPoint = goAroundFact;
                    }
                    else
                    {
                        Debug.Log("you need to select a Line Fact for the problem object!");
                    }
                    break;
            }
        }
        else
        {
            Debug.Log("Please make sure that both slots are filled and the go-around-object is a point fact!");

        }
        

    }

    public void OnApply3()
    {
        currentState = MenuStates.Four;
        GameObject inputField1 = GetChildByName(pageThree, "InputField (1)");
        GameObject inputField2 = GetChildByName(pageThree, "InputField (2)");
        GameObject inputField3 = GetChildByName(pageThree, "InputField (3)");
        if (inputField1 == null)
        {
            Debug.Log("inputfield1 nicht gefunden");
        }
        else
        {
            Debug.Log("inputfield gefunden");
        }

        string rawtext1 = inputField1.GetComponent<TMP_InputField>().text;
        string rawtext2 = inputField2.GetComponent<TMP_InputField>().text;
        string rawtext3 = inputField3.GetComponent<TMP_InputField>().text;
        problemObject.GetComponent<Problemobject>().npcProblemText = rawtext1;
        problemObject.GetComponent<Problemobject>().npcSuccessText = rawtext2;
        problemObject.GetComponent<Problemobject>().npcFailureText = rawtext3;
        int amountOfSentences1 = GetAmountOfSentences(rawtext1);

        Debug.Log("text aus eingabe ist: " + rawtext1);
        Debug.Log("Anzahl der Sätze ist: " + GetAmountOfSentences(rawtext1));


        // Setze die Saetze vom Taskcharakter
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().sentences = new string[amountOfSentences1 + 2];
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().sentences[amountOfSentences1] = rawtext2;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().sentences[amountOfSentences1+1] = rawtext3;
        SetCharakterSentences(rawtext1, amountOfSentences1, charakterPrefab.GetComponentInChildren<MyCharacterDialog>().sentences);
        //charakterPrefab.GetComponentInChildren<CharacterDialog>().sentences = new string[amountOfSentences1 + 1];
        //SetCharakterSentences(rawtext1, amountOfSentences1, charakterPrefab.GetComponentInChildren<CharacterDialog>().sentences);
        //charakterPrefab.GetComponentInChildren<CharacterDialog>().sentences[amountOfSentences1] = rawtext2;
        


        

        //Debug.Log("Anzahl Elemente von sentences ist " + charakterPrefab.GetComponentInChildren<CharacterDialog>().sentences.Length);
        /*for(int i = 0; i<amountOfSentences1+1; i++)
        {
            if (charakterPrefab.GetComponentInChildren<CharacterDialog>().sentences[i] != null)
            {
                Debug.Log("sentences[" + i + "] = " + charakterPrefab.GetComponentInChildren<CharacterDialog>().sentences[i].ToString());
            }
            else
            {
                Debug.Log("sentences[" + i + "] = null");
            }
        }*/

        //Get text from UI input
       // GameObject inputField1 = GetChildByName(pageThree, "InputField (1)");
        //string rawtext = inputField1.GetComponent<TextMesh>().text;

    }

    public void onApply4()
    {
        if(success_slot.GetComponent<ItemSlot>().itemOnSlot && failure_slot.GetComponent<ItemSlot>().itemOnSlot && npc_slot.GetComponent<ItemSlot>().itemOnSlot)
        {
            currentState = MenuStates.Five;
            //Konvertiere Goaround pointfact zu Gameobject um es Taskcharakter zu uebergeben (y auf null damit er auf boden ist)
            walkAroundPoint = new GameObject();
            float x = problemObject.GetComponent<Problemobject>().goAroundPoint.Representation.transform.position.x;
            float y = 0;
            float z = problemObject.GetComponent<Problemobject>().goAroundPoint.Representation.transform.position.z;
            walkAroundPoint.transform.position = new Vector3(x, y, z);

            problemObject.GetComponent<Problemobject>().successReaction = success_slot.GetComponent<ItemSlot>().animationprefab;
            problemObject.GetComponent<Problemobject>().failureReaction = failure_slot.GetComponent<ItemSlot>().animationprefab;
            charakterPrefab.GetComponentInChildren<MyCharacterDialog>().successreaction = success_slot.GetComponent<ItemSlot>().animationprefab;
            //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().successreaction.SetActive(false);
            charakterPrefab.GetComponentInChildren<MyCharacterDialog>().failurereaction = failure_slot.GetComponent<ItemSlot>().animationprefab;
            //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().failurereaction.SetActive(false);

            //TODO: npc reaction einbauen
            problemObject.GetComponent<Problemobject>().npcReaction = npc_slot.GetComponent<ItemSlot>().npcReaction;

            //Standardmäßig circle run, nur wenn man crouch walk wählt, soll der controller geändert werden
            if(npc_slot.GetComponent<ItemSlot>().npcReaction == "HumanoidCrouchWalkLeft")
            {
                charakterPrefab.GetComponent<Animator>().runtimeAnimatorController = npc_slot.GetComponent<ItemSlot>().npcController;
            }

            problemObject.GetComponent<Problemobject>().npcController = charakterPrefab.GetComponent<Animator>().runtimeAnimatorController;

        }
        

    }

    public void OnFinish()
    {

        //Fenster wird nichtmehr angezeigt
        currentState = MenuStates.None;

        //Anzahl der NPCs wird erhöht
        npc_counter++;

        //Setze Taskcharacteranimation von npc
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().CharakterAnimation.id = npc_counter;
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().npc_id = npc_counter;
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().CharakterAnimation = new TaskCharakterAnimation();
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().CharakterAnimation.walkAroundObject = walkAroundPoint;
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().CharakterAnimation.player = player;
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().CharakterAnimation.radiusAroundObject = 10;
        //charakterPrefab.GetComponentInChildren<MyCharacterDialog>().CharakterAnimation.talkingZoneDistance = 10;

        //Setze WalkaroundObject von Taskcharakter
        charakterPrefab.GetComponent<TaskCharakterAnimation>().walkAroundObject = walkAroundPoint;

        //Setze Player von Taskcharakter
        charakterPrefab.GetComponent<TaskCharakterAnimation>().player = player;

        //Instantiate(charakterPrefab, new Vector3(x + (float)0.5, y + (float)0.5, z + (float)0.5), Quaternion.identity);
        float x = walkAroundPoint.transform.position.x;
        float y = 0;
        float z = walkAroundPoint.transform.position.z;
        Debug.Log("x = " + x.ToString() + ", y = " + y.ToString() + ", z = " + z.ToString());
        //charakterPrefab = Instantiate(charakterPrefab, new Vector3(x, y, z), Quaternion.identity);
        Vector3 temp = new Vector3(x, y, z);
        charakterPrefab.transform.position = temp;
        Debug.Log("charakterPrefabs position = (" + charakterPrefab.transform.position.x.ToString() + "/" + charakterPrefab.transform.position.y.ToString() + "/" + charakterPrefab.transform.position.z.ToString() + ")");
        //GameObject realNewNpc = Instantiate(charakterPrefab, new Vector3(x, y, z), Quaternion.identity);

        //new NPC "press key to talk"-instantiation
        Transform myContinueText = charakterPrefab.transform.Find("DialogManager/DialogText/ContinueText");
        GameObject myDialogManager = GetChildByName(charakterPrefab, "DialogManager");
        myDialogManager.GetComponent<CharacterDialog>().continueText = myContinueText.gameObject;

        //new NPC Talking Zone initialization
        charakterPrefab.GetComponent<TaskCharakterAnimation>().talkingZoneDistance = 10;

        // debug fix of first sentence of new npc
        charakterPrefab.transform.Find("DialogManager/DialogText").gameObject.GetComponent<TMP_Text>().text = charakterPrefab.GetComponentInChildren<MyCharacterDialog>().sentences[0];

        //delete all previously generated facts (so that solution is not in the current factlist


        //CreateProblem();
        // Setze Problem im NPC
        charakterPrefab.GetComponentInChildren<MyCharacterDialog>().problemObject = problemObject;

        //debug what problemobject looks like

    }

    public void OnBack1()
    {
        this.currentState = MenuStates.None;
        Debug.Log("Back Button on page 1 pressed");
        Debug.Log("Current state = " + this.currentState.ToString());
    }

    public void OnBackFactGenering()
    {
        currentState = MenuStates.One;
    }

    public void OnBack2()
    {
        currentState = MenuStates.FactGenerating;
    }

    public void OnBack3()
    {
        currentState = MenuStates.Two;
    }

    public void OnBack4()
    {
        currentState = MenuStates.Three;
    }

    public void OnBack5()
    {
        currentState = MenuStates.Four;
    }


    private T GetChildComponentByName<T>(string name, GameObject parent) where T : Component
    {
        foreach (T component in parent.GetComponentsInChildren<T>(true))
        {
            if (component.gameObject.name == name)
            {
                return component;
            }
        }
        return null;
    }

    private GameObject GetChildByName(GameObject parent, string name)
    {
        GameObject goal;
        int childrenLength = parent.transform.childCount;
        for(int i = 0; i<childrenLength; i++)
        {
            if(parent.transform.GetChild(i).gameObject.name == name)
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    private int GetAmountOfSentences(string text)
    {
        char[] textarray = text.ToCharArray();
        int amount = 0;
        for(int i = 0; i<textarray.Length; i++)
        {
            if(textarray[i] == '.' || textarray[i] == '?' || textarray[i] == '!')
            {
                amount++;
            }
        }
        return amount;
    }

    private void SetCharakterSentences(string text, int amountOfSentences, string[] sentences)
    { 
        char[] textarray = text.ToCharArray();
        //Debug.Log("textarray = " + textarray.ArrayToString() + "; laenge von textarray = " + textarray.Length);
        int pointCounter = 0;
        for(int i = 0; i<textarray.Length; i++)
        {
          if(textarray[i] != '.' && textarray[i] != '?' && textarray[i] != '!')
          {
                sentences[pointCounter] += textarray[i];
                //Debug.Log("pointcounter = " + pointCounter);
                //Debug.Log("textarray[" + i + "] = " + textarray[i]);
                
          }
          else
          {
                sentences[pointCounter] += textarray[i];
                pointCounter++;
          }

        }
        //Debug.Log("pointcounter = " + pointCounter);
    }

    private void showProblemInstances()
    {
        if(problem.problemFact != null)
        {
            Debug.Log("problemFact = " + problem.problemFact.ToString());
        }
        else
        {
            Debug.Log("problemFact = null");
        }
    }

    /*private bool FrameIT_UI_Active()
    {
        return frameitUI.GetComponent<HideUI>.
    }*/
}