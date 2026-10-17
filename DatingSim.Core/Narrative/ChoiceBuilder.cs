using DatingSim.Core.Characters;
using DatingSim.Core.Endings;

namespace DatingSim.Core.Narrative;

public sealed class ChoiceBuilder
{
    private readonly string _id;
    private readonly string _text;
    private readonly string _nextSceneId;
    private CharacterId _affectionTarget = CharacterId.None;
    private int _affectionDelta;
    private readonly List<string> _setFlags = [];
    private readonly List<string> _requires = [];
    private readonly List<string> _forbids = [];
    private CharacterId _lockRoute = CharacterId.None;
    private EndingId _ending = EndingId.None;
    private int? _act;
    private bool _rare;
    private string? _hint;

    private ChoiceBuilder(string id, string text, string nextSceneId)
    {
        _id = id;
        _text = text;
        _nextSceneId = nextSceneId;
    }

    public static ChoiceBuilder Make(string id, string text, string nextSceneId) =>
        new(id, text, nextSceneId);

    public ChoiceBuilder Aff(CharacterId id, int delta)
    {
        _affectionTarget = id;
        _affectionDelta = delta;
        return this;
    }

    public ChoiceBuilder Flag(params string[] flags)
    {
        _setFlags.AddRange(flags);
        return this;
    }

    public ChoiceBuilder Requires(params string[] flags)
    {
        _requires.AddRange(flags);
        return this;
    }

    public ChoiceBuilder Forbids(params string[] flags)
    {
        _forbids.AddRange(flags);
        return this;
    }

    public ChoiceBuilder Lock(CharacterId route)
    {
        _lockRoute = route;
        return this;
    }

    public ChoiceBuilder Ending(EndingId ending)
    {
        _ending = ending;
        return this;
    }

    public ChoiceBuilder Act(int act)
    {
        _act = act;
        return this;
    }

    public ChoiceBuilder Rare(string? hint = null)
    {
        _rare = true;
        _hint = hint;
        return this;
    }

    public ChoiceDef Build() =>
        new()
        {
            Id = _id,
            Text = _text,
            NextSceneId = _nextSceneId,
            RequiresFlags = _requires,
            ForbidsFlags = _forbids,
            IsRare = _rare,
            Hint = _hint,
            Effect = new ChoiceEffect
            {
                AffectionTarget = _affectionTarget,
                AffectionDelta = _affectionDelta,
                SetFlags = _setFlags,
                LockRoute = _lockRoute,
                SetEnding = _ending,
                SetAct = _act,
            },
        };

    public static implicit operator ChoiceDef(ChoiceBuilder builder) => builder.Build();
}
