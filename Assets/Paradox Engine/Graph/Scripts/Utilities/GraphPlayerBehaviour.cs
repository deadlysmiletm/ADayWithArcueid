using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ParadoxEngine;
using ParadoxEngine.Utilities;
using ParadoxEngine.Localization;
using ParadoxFramework;

public class GraphPlayerBehaviour : MonoBehaviour
{
    public EngineGraph graph;
    public Text textContainer;
    public Text characterNameContainer;
    public ParadoxSessionCache cache;
    public DSetting settings;

    public PoolManager poolManager;
    public LocalizationManager localizationManager;
    public Transform characterContainer;
    public Dictionary<DCharacter, Image> CharactersInScene;

    public int layer = 0;
    public Image[] backgrounds = new Image[2];

    public int channel = 0;
    public AudioSource[] musicChannel = new AudioSource[2];
    public AudioSource soundChannel;
    public AudioSource voiceChannel;

    private EngineGraph _currentGraph;
    private NTemplate _actualNode;
    private bool _isPlaying;

    public bool IsPlaying { get => _isPlaying; set => _isPlaying = value; }
    public Canvas Canvas;


    private void Awake()
    {
        poolManager = GetComponent<PoolManager>();
        Canvas = GetComponentInParent<Canvas>();

        if (graph == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        CharactersInScene = new Dictionary<DCharacter, Image>();

        if (DialogueDatabase.parameters == null)
            DialogueDatabase.parameters = graph.parameters;
    }

    void Start()
    {
        AssignBehaviour(graph);
        Play();
    }

    void Update ()
    {
        if (_isPlaying && _currentGraph != null)
            Playing();
	}


    public void Play()
    {
        DialogueDatabase.activeGraphPlayer = this;
        this.gameObject.SetActive(true);

        backgrounds[0].gameObject.SetActive(true);
        backgrounds[1].gameObject.SetActive(false);

        _isPlaying = true;
    }

    void Playing() => _actualNode.Execute();

    public void Stop()
    {
        _isPlaying = false;
        _actualNode = _currentGraph.nodes[0];
        DialogueDatabase.activeGraphPlayer = null;
        this.gameObject.SetActive(false);
    }

    #region Behaviour Settings
    public void AssignBehaviour(EngineGraph grapho)
    {
        if (localizationManager)
            GraphLocalizationSetter.TranslateGraph(grapho, localizationManager);

        _currentGraph = grapho;
        _actualNode = grapho.nodes.Where(x => x.nodeType == EnumNodeType.Start).First();
    }

    public void ChangeNode(NTemplate node)
    {
        node.EndState();
        _actualNode = node;
    }
    #endregion


    #region Nodes Setters
    public void ShowDialogueBox(bool isActive) => characterNameContainer.transform.parent.gameObject.SetActive(isActive);
    public void ShowTextContainer(bool isActive) => textContainer.transform.parent.gameObject.SetActive(isActive);

    public void ChangeText(string text)
    {
        if (!textContainer.gameObject.activeInHierarchy)
            ShowTextContainer(true);

        textContainer.text = text;
    }

    public string GetText() => textContainer.text;

    public void ChangeNameDialogueText(string text)
    {
        if (!characterNameContainer.gameObject.activeInHierarchy)
            ShowDialogueBox(true);

        characterNameContainer.text = text;
    }
    #endregion

    #region Button Pool
    public Button TakeButtonPool()
    {
        var temp = poolManager.GetPoolObject("Button").GetComponent<Button>();
        temp.GetComponent<ButtonGUI>().fxSource = soundChannel;

        return temp;
    }

    public void ReturnButtonPool(Button button) => poolManager.DisposePoolObject("Button", button.gameObject);
    #endregion

    #region Characters Pool
    public Image TakeCharacterPool(DCharacter chara)
    {
        if (CharactersInScene.ContainsKey(chara))
            return CharactersInScene[chara];

        var img = poolManager.GetPoolObject("Character").GetComponent<Image>();
        CharactersInScene.Add(chara, img);

        return img;
    }

    public void ReturnCharacterPool(DCharacter chara)
    {
        var img = CharactersInScene[chara];
        CharactersInScene.Remove(chara);

        poolManager.DisposePoolObject("Character", img.gameObject);
    }
    #endregion
}