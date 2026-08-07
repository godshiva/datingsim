using DatingSim.Scripts.App;
using DatingSim.Scripts.Save;
using Godot;

namespace DatingSim.Scripts.UI;

public partial class MainMenuController : Control
{
    private Button _newGameButton = null!;
    private Button _continueButton = null!;
    private Button _quitButton = null!;
    private Label _subtitleLabel = null!;

    public override void _Ready()
    {
        _newGameButton = GetNode<Button>("%NewGameButton");
        _continueButton = GetNode<Button>("%ContinueButton");
        _quitButton = GetNode<Button>("%QuitButton");
        _subtitleLabel = GetNode<Label>("%SubtitleLabel");

        _newGameButton.Pressed += OnNewGame;
        _continueButton.Pressed += OnContinue;
        _quitButton.Pressed += OnQuit;

        _continueButton.Disabled = !GodotSaveService.HasSave();
        _subtitleLabel.Text = GodotSaveService.HasSave()
            ? "A save waits in the annex."
            : "Attendance required. Bring a pen — and better judgment.";
    }

    private void OnNewGame()
    {
        GameSession.StartNew("You");
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

    private void OnContinue()
    {
        if (!GodotSaveService.HasSave())
        {
            return;
        }

        GameSession.Continue(GodotSaveService.Load());
        GetTree().ChangeSceneToFile("res://Scenes/Game.tscn");
    }

    private void OnQuit() => GetTree().Quit();
}
