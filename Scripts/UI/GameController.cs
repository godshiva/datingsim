using DatingSim.Core.Characters;
using DatingSim.Core.Endings;
using DatingSim.Core.Narrative;
using DatingSim.Core.State;
using DatingSim.Scripts.App;
using DatingSim.Scripts.Save;
using Godot;

namespace DatingSim.Scripts.UI;

public partial class GameController : Control
{
    private Label _actLabel = null!;
    private Label _titleLabel = null!;
    private Label _speakerLabel = null!;
    private RichTextLabel _dialogueLabel = null!;
    private VBoxContainer _choiceBox = null!;
    private Button _advanceButton = null!;
    private Button _saveButton = null!;
    private Button _menuButton = null!;
    private ColorRect _backdrop = null!;
    private ColorRect _accentBar = null!;
    private Label _endingBanner = null!;

    private StoryEngine _engine = null!;
    private GameState _state = null!;
    private SceneDef _scene = null!;
    private int _lineIndex;
    private bool _awaitingChoice;
    private bool _finished;

    public override void _Ready()
    {
        _actLabel = GetNode<Label>("%ActLabel");
        _titleLabel = GetNode<Label>("%TitleLabel");
        _speakerLabel = GetNode<Label>("%SpeakerLabel");
        _dialogueLabel = GetNode<RichTextLabel>("%DialogueLabel");
        _choiceBox = GetNode<VBoxContainer>("%ChoiceBox");
        _advanceButton = GetNode<Button>("%AdvanceButton");
        _saveButton = GetNode<Button>("%SaveButton");
        _menuButton = GetNode<Button>("%MenuButton");
        _backdrop = GetNode<ColorRect>("%Backdrop");
        _accentBar = GetNode<ColorRect>("%AccentBar");
        _endingBanner = GetNode<Label>("%EndingBanner");

        _advanceButton.Pressed += OnAdvance;
        _saveButton.Pressed += OnSave;
        _menuButton.Pressed += OnMenu;

        if (GameSession.State is null)
        {
            GameSession.StartNew();
        }

        _engine = GameSession.Engine;
        _state = GameSession.State!;
        LoadScene(_state.CurrentSceneId);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (_finished || _awaitingChoice)
        {
            return;
        }

        if (@event.IsActionPressed("ui_accept") ||
            (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left }))
        {
            OnAdvance();
            GetViewport().SetInputAsHandled();
        }
    }

    private void LoadScene(string sceneId)
    {
        _scene = _engine.GetScene(sceneId);
        _state.CurrentSceneId = sceneId;
        _state.Act = _scene.Act;
        _lineIndex = 0;
        _awaitingChoice = false;
        _finished = _scene.IsEndingScene && _scene.Lines.Count == 0;

        ClearChoices();
        _endingBanner.Visible = false;
        _actLabel.Text = _scene.IsEndingScene ? "Finale" : $"Act {_scene.Act}";
        _titleLabel.Text = _scene.Title;
        UpdateBackdropForScene();
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (_lineIndex >= _scene.Lines.Count)
        {
            PresentChoicesOrEnd();
            return;
        }

        var line = _scene.Lines[_lineIndex];
        var speaker = line.ResolveSpeakerName();
        _speakerLabel.Text = string.IsNullOrEmpty(speaker) ? " " : speaker;
        _dialogueLabel.Text = line.Text;
        ApplySpeakerAccent(line.Speaker);
        _advanceButton.Disabled = false;
        _advanceButton.Text = _lineIndex >= _scene.Lines.Count - 1 && HasChoicesAhead()
            ? "Choose"
            : "Continue";
    }

    private bool HasChoicesAhead() =>
        !_scene.IsEndingScene && _engine.GetAvailableChoices(_state, _scene).Count > 0;

    private void OnAdvance()
    {
        if (_finished || _awaitingChoice)
        {
            return;
        }

        if (_lineIndex < _scene.Lines.Count - 1)
        {
            _lineIndex++;
            ShowCurrentLine();
            return;
        }

        if (_lineIndex == _scene.Lines.Count - 1)
        {
            _lineIndex++;
        }

        PresentChoicesOrEnd();
    }

    private void PresentChoicesOrEnd()
    {
        if (_scene.IsEndingScene)
        {
            FinishEnding();
            return;
        }

        var choices = _engine.GetAvailableChoices(_state, _scene);
        if (choices.Count == 0)
        {
            _dialogueLabel.Text = "There's nowhere left to go.";
            _advanceButton.Disabled = true;
            return;
        }

        _awaitingChoice = true;
        _advanceButton.Disabled = true;
        _speakerLabel.Text = " ";
        _dialogueLabel.Text = "What do you do?";
        ClearChoices();

        foreach (var choice in choices)
        {
            var button = new Button
            {
                Text = choice.IsRare ? $"◆ {choice.Text}" : choice.Text,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
            };
            button.AddThemeFontSizeOverride("font_size", 18);
            var captured = choice.Id;
            button.Pressed += () => OnChoice(captured);
            _choiceBox.AddChild(button);
        }
    }

    private void OnChoice(string choiceId)
    {
        ClearChoices();
        _awaitingChoice = false;
        var next = _engine.ApplyChoice(_state, choiceId);
        LoadScene(next.Id);
    }

    private void FinishEnding()
    {
        _finished = true;
        _awaitingChoice = false;
        _advanceButton.Disabled = true;
        ClearChoices();

        if (_state.Ending == EndingId.None)
        {
            _state.Ending = _engine.ResolveEnding(_state);
        }

        var ending = EndingCatalog.Get(_state.Ending);
        _endingBanner.Visible = true;
        _endingBanner.Text = ending.IsHappy
            ? $"★ {ending.Title}"
            : ending.Title;
        _dialogueLabel.Text = ending.Summary;
        _saveButton.Disabled = false;

        var back = new Button
        {
            Text = "Return to Title",
            SizeFlagsHorizontal = SizeFlags.ExpandFill,
        };
        back.Pressed += OnMenu;
        _choiceBox.AddChild(back);
    }

    private void OnSave()
    {
        GodotSaveService.Save(_state);
        _titleLabel.Text = $"{_scene.Title}  ·  Saved";
    }

    private void OnMenu()
    {
        GetTree().ChangeSceneToFile("res://Scenes/MainMenu.tscn");
    }

    private void ClearChoices()
    {
        foreach (var child in _choiceBox.GetChildren())
        {
            child.QueueFree();
        }
    }

    private void UpdateBackdropForScene()
    {
        // Night annex: cool ink wash, warmer on endings.
        _backdrop.Color = _scene.IsEndingScene
            ? new Color(0.12f, 0.09f, 0.11f, 1f)
            : new Color(0.08f, 0.11f, 0.16f, 1f);
    }

    private void ApplySpeakerAccent(CharacterId speaker)
    {
        if (speaker == CharacterId.None || !CharacterRegistry.TryGet(speaker, out var def))
        {
            _accentBar.Color = new Color(0.75f, 0.72f, 0.65f, 1f);
            return;
        }

        _accentBar.Color = Color.FromString(def.AccentColor, new Color(0.75f, 0.72f, 0.65f));
    }
}
