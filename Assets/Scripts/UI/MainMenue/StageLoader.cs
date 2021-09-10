using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: SE: Split for Stage/Local
public class StageLoader : ListLoader<Stage>
{
    protected bool local = false;
    private string NoOfficial = "No Entry found, please check directory!";

    protected new void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        StageStatic.SetStage("", local);
        StageStatic.ShallowLoadStages();

        Dictionary<string, Stage> dict = StageStatic.StageOfficial;
        ListButtons(dict.Values.OrderByDescending((v) => v.number).ToList());

        scroll.verticalScrollbar.numberOfSteps = dict.Count;
    }

    protected override void Default()
    {
        var def = Instantiate(Entry);
        def.transform.SetParent(List.transform, false);

        WriteInChildText(def.transform.GetChild(2).gameObject, NoOfficial);
    }

    protected override void ListButtonsWrapped(List<Stage> list)
    {
        foreach (var stage in list)
        {
            GameObject prefab = Instantiate(Entry);

            prefab.transform.SetParent(List.transform, false);
            prefab.transform.SetAsFirstSibling();

            WriteInChildText(prefab.transform.GetChild(0).gameObject, stage.number.ToString());
            WriteInChildText(prefab.transform.GetChild(1).gameObject, stage.name);
            WriteInChildText(prefab.transform.GetChild(2).gameObject, stage.description);

            // TODO: handle unable to load
            prefab.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { Loader.LoadStage(stage.name, local, true); });
        }
    }
}
