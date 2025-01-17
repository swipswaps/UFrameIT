using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

/// <summary>
/// Keeps track of all available and current <see cref="Stage"/>
/// </summary>
public static class StageStatic
{
    /// <summary>
    /// - <c>Key</c>: stage name
    /// - <c>Value</c>: stages created by KWARC
    /// </summary>
    public static Dictionary<string, Stage> StageOfficial;

    /// <summary>
    /// - <c>Key</c>: stage name
    /// - <c>Value</c>: stages created by local user
    /// </summary>
    public static Dictionary<string, Stage> StageLocal;

    /// <summary>
    /// Used to map <see cref="StageOfficial"/> <see cref="Stage.category">categories</see> into a ordered list for the StageMenue.
    /// </summary>
    public static Dictionary<string, int> Category = new Dictionary<string, int> {
        { "", -1 },
        { "Demo Category", 0 },
    };

    /// <summary>
    /// <see cref="Stage.name"/> of current <see cref="stage"/> or one to be loaded.
    /// <seealso cref="LoadInitStage(bool, GameObject)"/>
    /// </summary>
    public static string current_name;

    /// <summary>
    /// !<see cref="Stage.use_install_folder"/> of current <see cref="stage"/> or one to be loaded.
    /// <seealso cref="LoadInitStage(bool, GameObject)"/>
    /// </summary>
    public  static bool local_stage;

    /// <summary>
    /// Current <see cref="Mode"/>
    /// </summary>
    public static Mode mode;

    /// <summary>
    /// Loadable world scenes
    /// </summary>
    public static readonly List<string> Worlds = GenerateWorldList();

    /// <summary>
    /// Current <see cref="Stage"/>
    /// </summary>
    public static Stage stage {
        get {
            return (local_stage ? StageLocal : StageOfficial)[current_name];
        }
        set {
            current_name = value.name;
            local_stage = !value.use_install_folder;

            (local_stage ? StageLocal : StageOfficial).Remove(current_name);
            (local_stage ? StageLocal : StageOfficial).Add(current_name, value);

            value.store();
        }
    }

    /// <summary>
    /// TODO: set when encountering an error
    /// </summary>
    public static StageErrorStruct last_error {
        get;
        private set;
    }

    // TODO! generate at buildtime
    /// <summary>
    /// Extracts all loadable scenes for <see cref="Worlds"/>.
    /// </summary>
    /// <returns><see cref="Worlds"/></returns>
    private static List<string> GenerateWorldList()
    {

#if UNITY_EDITOR

        List<string> _Worlds = new List<string>();

        string world = "World";
        string ending = ".unity";
        foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                string name = new System.IO.FileInfo(scene.path).Name;
                name = name.Substring(0, name.Length - ending.Length);

                if (0 == string.Compare(name, name.Length - world.Length, world, 0, world.Length))
                {
                    _Worlds.Add(name);
                }
            }
        }
#else
        List<string> _Worlds = new List<string> {"TreeWorld", "RiverWorld"};
        Debug.Log("WorldList might be incomplete or incorrect!");
#endif

        return _Worlds;
    }

    /// <summary>
    /// Available Modes a <see cref="Stage"/> to be selected and/ or loaded in.
    /// </summary>
    public enum Mode
    {
        Play,
        Create,
    }

    /// <summary>
    /// Created when an error (may) occures while a <see cref="Stage"/> is being created, because of incompatible variables.
    /// </summary>
    public struct StageErrorStruct
    {
        /// <summary> set iff <see cref="Stage.category"/> is incompatible </summary>
        public bool category    { get { return state[0]; } set { state[0] = value; } }

        /// <summary> set iff <see cref="Stage.number"/> is incompatible </summary>
        public bool id          { get { return state[1]; } set { state[1] = value; } }

        /// <summary> set iff <see cref="Stage.name"/> is incompatible </summary>
        public bool name        { get { return state[2]; } set { state[2] = value; } }

        /// <summary> set iff <see cref="Stage.description"/> is incompatible </summary>
        public bool description { get { return state[3]; } set { state[3] = value; } }

        /// <summary> set iff <see cref="Stage.scene"/> is incompatible </summary>
        public bool scene       { get { return state[4]; } set { state[4] = value; } }

        /// <summary> set iff !<see cref="Stage.use_install_folder"/> is incompatible </summary>
        public bool local       { get { return state[5]; } set { state[5] = value; } }

        /// <summary> set iff <see cref="Stage.path"/> was not found </summary>
        public bool load        { get { return state[6]; } set { state[6] = value; } }


        /// <summary>
        /// stores all boolish members, to iterate over
        /// </summary>
        private bool[] state;

        /// <summary>
        /// <see langword="true"/> iff no error occures.
        /// </summary>
        public bool pass
        {
            get { return state.Aggregate(true, (last, next) => last && !next); }
        }

        public readonly static StageErrorStruct
            InvalidFolder = new StageErrorStruct(false, false, false, false, false, true, false),
            NotLoadable   = new StageErrorStruct(false, false, false, false, false, false, true);

        /// <summary>
        /// Initiator <br/>
        /// canonical
        /// </summary>
        public StageErrorStruct(bool category, bool id, bool name, bool description, bool scene, bool local, bool load)
        {
            state = new bool[7];

            this.category = category;
            this.id = id;
            this.name = name;
            this.description = description;
            this.scene = scene;
            this.local = local;
            this.load = load;
        }
    }

    /// <summary>
    /// sets <see cref="mode"/> and en-/ disables children of <paramref name="gameObject"/> with certain Tags, only available in certain <see cref="Mode">Modes</see> (e.g. in Def_Stage)
    /// </summary>
    /// <param name="mode"><see cref="Mode"/> to set</param>
    /// <param name="gameObject"> which children will be checked</param>
    public static void SetMode(Mode mode, GameObject gameObject = null)
    {
        gameObject ??= new GameObject();

        // handle StageStatic.mode
        switch (StageStatic.mode = mode)
        {
            case Mode.Play:
                gameObject.UpdateTagActive("CreatorMode", false);
                break;
            case Mode.Create:
                gameObject.UpdateTagActive("CreatorMode", true);
                break;
        }

        // handle stage mode
        switch (mode)
        {
            case Mode.Play:
            case Mode.Create:
                if (ContainsKey(current_name, local_stage))
                    stage.SetMode(mode == Mode.Create);
                break;
        }

    }

    public static StageErrorStruct Validate(string category, int id, string name, string description, string scene, bool local = true)
    {   
        return new StageErrorStruct(
            category.Length == 0,
            ContainsNumber(category, id, true),
            name.Length == 0 || ContainsKey(name, true) || ContainsKey(name, false),
            false,
            !Worlds.Contains(scene),
            local == false,
            false
            );
    }

    public static StageErrorStruct LoadNewStage(string category, int id, string name, string description, string scene)
    {
        StageErrorStruct ret = Validate(category, id, name, description, scene, true);
        if (!ret.pass)
            return ret;

        stage = new Stage(category, id, name, description, scene, true)
        {
            creatorMode = true
        };
        stage.store();

        LoadCreate();
        return ret;
    }

    /// <summary>
    /// Load current <see cref="stage"/> in <see cref="Mode.Create"/>
    /// </summary>
    public static void LoadCreate()
    {
        SetMode(Mode.Create);
        Loader.LoadScene(stage.scene);
    }

    /// <summary>
    /// Finds first unused <see cref="Stage.number"/> in a certain <paramref name="category"/>.
    /// </summary>
    /// <param name="local">which kind of stage we are looking at</param>
    /// <param name="category">the category in question</param>
    /// <returns>first unused <see cref="Stage.number"/> in a certain <paramref name="category"/></returns>
    public static int NextNumber(bool local, string category)
    {
        var numbers = (local ? StageLocal : StageOfficial).Values.Where(s => s.category == category).Select(s => s.number).ToList();

        if (0 == numbers.Count)
            return 1;

        numbers.Sort();
        int last = numbers[0];
        foreach (int i in numbers)
        {
            if (i > last && i != last + 1)
                return last + 1;
            last = i;
        }

        return last + 1;
    }

    /// <summary>
    /// Looks wether an <see cref="Stage.number"/> <paramref name="i"/> exists within a certain <see cref="Stage.category"/> <paramref name="category"/> in local saves (<paramref name="local"/> == <see langword="true"/>) or install path.
    /// </summary>
    /// <param name="category">to look in</param>
    /// <param name="i">to look for</param>
    /// <param name="local">where to look</param>
    /// <returns></returns>
    public static bool ContainsNumber(string category, int i, bool local)
    {
        return (local ? StageLocal : StageOfficial).Values
            .Where(s => s.category == category)
            .Select(s => s.number)
            .Contains(i);
    }

    /// <summary>
    /// Looks for and initial loads (see <see cref="Stage.ShallowLoad(ref Stage, string)"/>) <see cref="Stage">Stages</see> in <see cref="local_stage"/> and !<see cref="local_stage"/>.
    /// </summary>
    public static void ShallowLoadStages()
    {
        StageOfficial = Stage.Grup(null, true);
        StageLocal = Stage.Grup(null, false);
    }

    /// <summary>
    /// Sets parameters, defining what to load in <see cref="LoadInitStage(bool, GameObject)"/> and <see cref="LoadInitStage(string, bool, bool, GameObject)"/>.
    /// </summary>
    /// <param name="name">sets <see cref="current_name"/></param>
    /// <param name="local">sets <see cref="local_stage"/></param>
    public static void SetStage(string name, bool local)
    {
        local_stage = local;
        current_name = name;
    }

    /// <summary>
    /// Returns a <see cref="Stage"/> or throws <see cref="Exception"/> if not found.
    /// </summary>
    /// <param name="name"><see cref="Stage.name"/></param>
    /// <param name="local">where to look</param>
    /// <returns><c>(local ? StageLocal : StageOfficial)[name];</c></returns>
    public static Stage GetStage(string name, bool local)
    {
        return (local ? StageLocal : StageOfficial)[name];
    }

    /// <summary>
    /// Deletes a <see cref="Stage"/> and all its associated files (including save games).
    /// <seealso cref="Stage.delete(bool)"/>
    /// </summary>
    /// <param name="stage">to be deleted</param>
    public static void Delete(Stage stage)
    {
        GetStage(stage.name, !stage.use_install_folder).delete(true);
        (!stage.use_install_folder ? StageLocal : StageOfficial).Remove(stage.name);
    }

    /// <summary>
    /// Wrapps <see cref="LoadInitStage(bool, GameObject)"/> with extra parameters.
    /// Loads and initiates <see cref="Stage"/> defined by <paramref name="name"/> and <paramref name="local"/>.
    /// </summary>
    /// <param name="name">sets <see cref="current_name"/> iff succeedes</param>
    /// <param name="local">sets <see cref="current_name"/> iff succeedes</param>
    /// <param name="restore_session">wether to restore last (loaded) player session (<see langword="true"/>) or start from scratch (<see langword="false"/>).</param>
    /// <param name="gameObject">(e.g. UI/ Def_Stage) toggles recursively children with tag "DevelopingMode" to <see cref="mode"/> == <see cref="Mode.Create"/>.</param>
    /// <returns><see langword="false"/> iff <see cref="Stage"/> defined by <paramref name="name"/> and <paramref name="local"/> could not be *found* or *loaded*.</returns>
    public static bool LoadInitStage(string name, bool local = false, bool restore_session = true, GameObject gameObject = null)
    {
        bool old_l = local_stage;
        string old_n = current_name;
        SetStage(name, local);

        if (!LoadInitStage(restore_session, gameObject))
        {
            local_stage = old_l;
            current_name = old_n;
            return false;
        }

        return true;
    }

    /// <summary>
    /// Loads and initiates <see cref="Stage"/> defined by <see cref="current_name"/> and <see cref="local_stage"/>.
    /// </summary>
    /// <param name="restore_session">wether to restore last (loaded) player session (<see langword="true"/>) or start from scratch (<see langword="false"/>).</param>
    /// <param name="gameObject">(e.g. UI/ Def_Stage) toggles recursively children with tag "DevelopingMode" to <see cref="mode"/> == <see cref="Mode.Create"/>.</param>
    /// <returns><see langword="false"/> iff <see cref="Stage"/> defined by <see cref="current_name"/> and <see cref="local_stage"/> could not be *found* or *loaded*.</returns>
    public static bool LoadInitStage(bool restore_session, GameObject gameObject = null)
    {
        if (!ContainsKey(current_name, local_stage) || !stage.DeepLoad())
            return false;

        if (restore_session)
        {
            stage.factState.invoke = true;
            stage.factState.Draw();
        }
        else
        {
            stage.ResetPlay();
            if(mode == Mode.Create) // block saving "player" progress
                stage.player_record.seconds = -1;
        }

        if(gameObject != null)
            gameObject.UpdateTagActive("DevelopingMode", mode == Mode.Create);
        SetMode(stage.creatorMode ? Mode.Create : Mode.Play);
        return true;
    }

    /// <summary>
    /// Wrapps <see cref="ContainsKey(string, bool)"/>; defaulting local to <see cref="local_stage"/>
    /// </summary>
    public static bool ContainsKey(string key)
    {
        return ContainsKey(key, local_stage);
    }

    /// <summary>
    /// Looks for a <see cref="Stage"/> <paramref name="key"/> in <see cref="StageLocal"/> (<paramref name="local"/>==<see langword="true"/>) or <see cref="StageOfficial"/> (<paramref name="local"/>==<see langword="false"/>).
    /// </summary>
    /// <returns><c>(local ? StageLocal : StageOfficial).ContainsKey(key)</c></returns>
    public static bool ContainsKey(string key, bool local)
    {
        return (local ? StageLocal : StageOfficial).ContainsKey(key);
    }
}