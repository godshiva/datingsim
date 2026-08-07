using DatingSim.Core.Characters;
using DatingSim.Core.Endings;
using DatingSim.Core.State;

namespace DatingSim.Core.Narrative;

/// <summary>
/// Verdant University — The After Hours Club.
/// Creative writing circle in the old annex library. Attendance is mandatory, so everyone keeps colliding.
/// </summary>
public static class StoryCatalog
{
    public static IReadOnlyDictionary<string, SceneDef> Build()
    {
        var scenes = new Dictionary<string, SceneDef>(StringComparer.Ordinal);
        foreach (var scene in CreateScenes())
        {
            scenes[scene.Id] = scene;
        }

        return scenes;
    }

    private static IEnumerable<SceneDef> CreateScenes()
    {
        yield return Scene(
            "act1_arrival",
            "Act I — Annex Lights",
            1,
            [
                Line(null, "Verdant University keeps its oldest building half-asleep. The annex library smells like dust, toner, and rain on brick."),
                Line(null, "A handwritten flyer on the door: AFTER HOURS CLUB — attendance required. Stories, critiques, and 'no ghosts, probably.'"),
                Line(CharacterId.Akari, "There you are! I'm Akari — president, snack allocator, and the person who will guilt you if you skip."),
                Line(CharacterId.Sora, "Oooh, new blood! I'm Sora. If you sit by me I'll share gummies. If you don't, I'll still share gummies. Fair warning."),
                Line(CharacterId.Mika, "...Hi. I'm Mika. I, um. I draw during critiques. It helps me listen."),
                Line(CharacterId.Rin, "Rin. Don't make this a welcome ceremony."),
                Line(CharacterId.Akari, "Ignore Rin. She's part of the furniture. Charming furniture! Anyway — first night tradition: pick someone to show you the annex."),
            ],
            [
                C("a1_go_rin", "Ask Rin, even though she looks annoyed.", "act1_rin").Aff(CharacterId.Rin, 8),
                C("a1_go_mika", "Accept Mika's quiet offer to show the studio.", "act1_mika").Aff(CharacterId.Mika, 10),
                C("a1_go_akari", "Let President Akari give the official tour.", "act1_akari").Aff(CharacterId.Akari, 10),
                C("a1_go_sora", "Follow Sora toward the vending machines.", "act1_sora").Aff(CharacterId.Sora, 10),
                C("a1_rare_listen", "Linger by the stairwell and listen before choosing.", "act1_rare_overhear")
                    .Flag(GameFlags.RareSecretMeeting)
                    .Rare("Requires noticing the annex instead of chasing a smile."),
            ]);

        yield return Scene(
            "act1_rare_overhear",
            "Act I — Stairwell Echo",
            1,
            [
                Line(null, "You stay still long enough to hear two voices below the landing."),
                Line(CharacterId.Akari, "If anyone digs into the roster again, we shut it down. Quietly."),
                Line(CharacterId.Sora, "Relax. People love candy more than paperwork."),
                Line(null, "Footsteps scatter. When you step into the hall, all four of them are smiling like nothing happened."),
                Line(CharacterId.Rin, "You hear something you shouldn't, you either ask me later — or you pretend you're stupid. Choose carefully."),
            ],
            [
                C("a1_rare_to_rin", "Ask Rin later, privately.", "act1_rin")
                    .Aff(CharacterId.Rin, 6)
                    .Flag(GameFlags.RinInsight),
                C("a1_rare_ignore", "Pretend you heard nothing and rejoin the tour.", "act1_akari")
                    .Aff(CharacterId.Akari, 4),
            ]);

        yield return Scene(
            "act1_rin",
            "Act I — Rin",
            1,
            [
                Line(CharacterId.Rin, "Fine. Annex map: books upstairs, studio through the fire door, roof access is locked because someone was dramatic."),
                Line(CharacterId.Rin, "If Akari calls us a family again, leave the room. That's my free advice."),
                Line(null, "She talks like every sentence is being charged rent. Still — she steers you around a loose floorboard without commenting."),
            ],
            [
                C("a1_rin_pity", "Softly: 'You don't have to push everyone away.'", "act1_wrap").Aff(CharacterId.Rin, -4),
                C("a1_rin_respect", "Match her tone: 'Noted. Show me the fire door.'", "act1_wrap")
                    .Aff(CharacterId.Rin, 12)
                    .Flag(GameFlags.RinNotPity),
                C("a1_rin_probe_family", "Ask why she really joined the club.", "act1_rin_lie").Aff(CharacterId.Rin, 2),
            ]);

        yield return Scene(
            "act1_rin_lie",
            "Act I — Rin's Tidy Story",
            1,
            [
                Line(CharacterId.Rin, "I like quiet rooms. That's it. Don't write a tragedy about me in your head."),
                Line(null, "Her answer is too clean. The kind of clean people use when the truth has teeth."),
            ],
            [
                C("a1_rin_lie_accept", "Let it go.", "act1_wrap").Aff(CharacterId.Rin, 4),
                C("a1_rin_lie_see", "Say you won't invent a tragedy — but you won't buy the brochure either.", "act1_wrap")
                    .Aff(CharacterId.Rin, 10)
                    .Flag(GameFlags.RinInsight, GameFlags.RinNotPity),
            ]);

        yield return Scene(
            "act1_mika",
            "Act I — Mika",
            1,
            [
                Line(CharacterId.Mika, "The studio is nicer at night. Less... eyes."),
                Line(CharacterId.Mika, "I could sketch you, if you want. Just the silhouette. You have a calm outline."),
                Line(null, "A wooden drawer under the supply table wears a small padlock that doesn't match anything else in the room."),
            ],
            [
                C("a1_mika_flatter", "Let her sketch you.", "act1_wrap")
                    .Aff(CharacterId.Mika, 14)
                    .Flag(GameFlags.MikaObsessed),
                C("a1_mika_drawer", "Casually ask about the locked drawer.", "act1_mika_lie").Aff(CharacterId.Mika, 2),
                C("a1_mika_ex", "Ask if she always painted alone — or if someone used to join her.", "act1_mika_ex_lie")
                    .Aff(CharacterId.Mika, 4),
            ]);

        yield return Scene(
            "act1_mika_lie",
            "Act I — Old Supplies",
            1,
            [
                Line(CharacterId.Mika, "Oh — that? Just old supplies. Turpentine, rags. It smells weird. Better locked."),
                Line(null, "She smiles with her mouth only. Her pencil hand has gone still."),
            ],
            [
                C("a1_mika_lie_drop", "Drop it and compliment her sketches instead.", "act1_wrap")
                    .Aff(CharacterId.Mika, 10)
                    .Flag(GameFlags.MikaObsessed),
                C("a1_mika_lie_note", "File the lie away behind your eyes.", "act1_wrap")
                    .Aff(CharacterId.Mika, 6)
                    .Flag(GameFlags.MikaFoundDrawerHint),
            ]);

        yield return Scene(
            "act1_mika_ex_lie",
            "Act I — Grad School",
            1,
            [
                Line(CharacterId.Mika, "Someone used to. They moved away for grad school. It's fine. People leave. That's normal."),
                Line(null, "The word 'normal' lands like a practiced stitch over a tear."),
            ],
            [
                C("a1_mika_ex_comfort", "Offer comfort.", "act1_wrap")
                    .Aff(CharacterId.Mika, 12)
                    .Flag(GameFlags.MikaObsessed, GameFlags.MikaPushedEx),
                C("a1_mika_ex_press", "Ask where they went to school.", "act1_wrap")
                    .Aff(CharacterId.Mika, 3)
                    .Flag(GameFlags.MikaPushedEx, GameFlags.MikaFoundDrawerHint),
            ]);

        yield return Scene(
            "act1_akari",
            "Act I — Akari",
            1,
            [
                Line(CharacterId.Akari, "Welcome to the family! We critique gently, celebrate loudly, and keep the annex looking respectable."),
                Line(CharacterId.Akari, "If anyone tells you a member left under weird circumstances — rumor control is part of leadership."),
                Line(null, "She laughs like a camera flash: bright, brief, leaving an afterimage."),
            ],
            [
                C("a1_akari_loyal", "Say you're glad someone keeps things stable.", "act1_wrap").Aff(CharacterId.Akari, 12),
                C("a1_akari_rival", "Ask about the member who left.", "act1_akari_lie").Aff(CharacterId.Akari, 2),
                C("a1_akari_scholarship", "Ask how she juggles presidency and her scholarship load.", "act1_akari_lie")
                    .Aff(CharacterId.Akari, 2),
            ]);

        yield return Scene(
            "act1_akari_lie",
            "Act I — A Misunderstanding",
            1,
            [
                Line(CharacterId.Akari, "Oh, that? A misunderstanding we already resolved as a club. Growth! Boundaries! Very healthy."),
                Line(CharacterId.Akari, "And the scholarship is fine. Perfect record, perfect timing. Please don't worry your pretty curiosity about it."),
            ],
            [
                C("a1_akari_lie_buy", "Buy the sunshine.", "act1_wrap").Aff(CharacterId.Akari, 10),
                C("a1_akari_lie_doubt", "Smile back, and doubt her anyway.", "act1_wrap")
                    .Aff(CharacterId.Akari, 4)
                    .Flag(GameFlags.AkariQuestionedRival),
            ]);

        yield return Scene(
            "act1_sora",
            "Act I — Sora",
            1,
            [
                Line(CharacterId.Sora, "Vending machines are the real club officers. Also, do you want a charm? I make them. Side hustle!"),
                Line(CharacterId.Sora, "Don't tell Akari I said the machines are in charge. She'll schedule a meeting about hierarchy."),
                Line(null, "Her wallet is thick with cash for someone who 'just sells charms.'"),
            ],
            [
                C("a1_sora_fun", "Play along. Buy a charm.", "act1_wrap").Aff(CharacterId.Sora, 12),
                C("a1_sora_money", "Ask how the charm business pays this well.", "act1_sora_lie").Aff(CharacterId.Sora, 2),
                C("a1_sora_errands", "Ask about the late-night 'errands' she mentioned on the flyer comments.", "act1_sora_lie")
                    .Aff(CharacterId.Sora, 2),
            ]);

        yield return Scene(
            "act1_sora_lie",
            "Act I — Tutoring Rich Kids",
            1,
            [
                Line(CharacterId.Sora, "Tutoring! Rich kids tip like maniacs. And charms. And, uh, recycling metal. Totally cute capitalism."),
                Line(CharacterId.Sora, "Anyway — gummy? Gummy ends interrogations. Club rule I just invented."),
            ],
            [
                C("a1_sora_lie_eat", "Take the gummy.", "act1_wrap").Aff(CharacterId.Sora, 10),
                C("a1_sora_lie_note", "Decline the gummy and keep the question in your pocket.", "act1_wrap")
                    .Aff(CharacterId.Sora, 4)
                    .Flag(GameFlags.SoraMoneyLie),
            ]);

        yield return Scene(
            "act1_wrap",
            "Act I — Critique Circle",
            1,
            [
                Line(CharacterId.Akari, "Alright, family — first prompt: write about someone you trust. Keep it short. Keep it honest. Or entertainingly dishonest!"),
                Line(CharacterId.Rin, "If honesty gets you hurt, don't be honest. Next."),
                Line(CharacterId.Mika, "I wrote about... outlines. People as outlines. It's safer."),
                Line(CharacterId.Sora, "I wrote about a heist but it's a metaphor. Or isn't! Surprise me."),
                Line(null, "The night ends with tea, rain, and the sense that every smile in the room has a second draft."),
            ],
            [
                C("a1_to_act2", "Come back for the next meeting.", "act2_start")
                    .Flag(GameFlags.MetClub, GameFlags.Act1Complete)
                    .Act(2),
            ]);

        yield return Scene(
            "act2_start",
            "Act II — Soft Knives",
            2,
            [
                Line(null, "Week two. The annex feels smaller. Everyone already has a seat that looks accidental and isn't."),
                Line(CharacterId.Akari, "Partner work tonight! Choose a critique buddy. Choose wisely — stories get personal fast."),
            ],
            [
                C("a2_partner_rin", "Partner with Rin.", "act2_rin").Aff(CharacterId.Rin, 8),
                C("a2_partner_mika", "Partner with Mika.", "act2_mika").Aff(CharacterId.Mika, 8),
                C("a2_partner_akari", "Partner with Akari.", "act2_akari").Aff(CharacterId.Akari, 8),
                C("a2_partner_sora", "Partner with Sora.", "act2_sora").Aff(CharacterId.Sora, 8),
            ]);

        yield return Scene(
            "act2_rin",
            "Act II — Rin",
            2,
            [
                Line(CharacterId.Rin, "Your draft is fine. Stop apologizing in the margins. It's needy."),
                Line(null, "Later, you catch her rewriting a junior member's critique so Akari won't humiliate them in front of everyone."),
                Line(CharacterId.Rin, "What? I hate wasted paper. Don't make eyes at me."),
            ],
            [
                C("a2_rin_expose", "Tease her for being kind.", "act2_mid").Aff(CharacterId.Rin, -6),
                C("a2_rin_quiet", "Say nothing — just nod once, like a secret kept.", "act2_mid")
                    .Aff(CharacterId.Rin, 14)
                    .Flag(GameFlags.RinSawKindness, GameFlags.RinTrusted),
                C("a2_rin_push_past", "Push again about her family.", "act2_rin_shut").Aff(CharacterId.Rin, -2),
            ]);

        yield return Scene(
            "act2_rin_shut",
            "Act II — Door Closed",
            2,
            [
                Line(CharacterId.Rin, "I told you. Quiet rooms. If you need a sob story, interview Mika. She collects them."),
                Line(null, "A lie, or a redirection sharp enough to cut. Either way, the door is shut."),
            ],
            [
                C("a2_rin_shut_ok", "Back off.", "act2_mid").Aff(CharacterId.Rin, 4).Flag(GameFlags.RinNotPity),
            ]);

        yield return Scene(
            "act2_mika",
            "Act II — Mika",
            2,
            [
                Line(CharacterId.Mika, "I like writing with you nearby. It feels like the room has a heartbeat."),
                Line(CharacterId.Mika, "Don't leave in the middle of a sentence, okay? People did that before."),
            ],
            [
                C("a2_mika_promise", "Promise you'll stay.", "act2_mid")
                    .Aff(CharacterId.Mika, 16)
                    .Flag(GameFlags.MikaObsessed),
                C("a2_mika_boundary", "Say you'll stay for the scene — not forever.", "act2_mid")
                    .Aff(CharacterId.Mika, 4)
                    .Flag(GameFlags.TriedToLeaveMika),
                C("a2_mika_drawer2", "Ask to borrow something from the locked drawer.", "act2_mika_drawer")
                    .Requires(GameFlags.MikaFoundDrawerHint),
            ]);

        yield return Scene(
            "act2_mika_drawer",
            "Act II — Padlock",
            2,
            [
                Line(CharacterId.Mika, "No. Please. It's... supplies. Why does everyone ask about supplies?"),
                Line(null, "For a second her voice isn't soft. It's absolute."),
            ],
            [
                C("a2_mika_drawer_soothe", "Apologize and change the subject.", "act2_mid")
                    .Aff(CharacterId.Mika, 10)
                    .Flag(GameFlags.MikaObsessed),
                C("a2_mika_drawer_persist", "Say locked things invite stories.", "act2_mid")
                    .Aff(CharacterId.Mika, 2)
                    .Flag(GameFlags.MikaFoundDrawerHint, GameFlags.TriedToLeaveMika),
            ]);

        yield return Scene(
            "act2_akari",
            "Act II — Akari",
            2,
            [
                Line(CharacterId.Akari, "Between us — leadership means editing reality until it photographs well."),
                Line(CharacterId.Akari, "If you stick with me, I'll make sure the club — and you — look impeccable."),
            ],
            [
                C("a2_akari_deal", "Accept her protection.", "act2_mid")
                    .Aff(CharacterId.Akari, 14)
                    .Flag(GameFlags.AkariCoveredForYou),
                C("a2_akari_refuse_edit", "Say you'd rather keep the unedited version.", "act2_mid")
                    .Aff(CharacterId.Akari, 2)
                    .Flag(GameFlags.AkariQuestionedRival),
                C("a2_akari_leverage", "Mention the stairwell conversation you overheard.", "act2_akari_pressure")
                    .Requires(GameFlags.RareSecretMeeting)
                    .Rare("Only if you overheard Act I's stairwell secret."),
            ]);

        yield return Scene(
            "act2_akari_pressure",
            "Act II — Soft Threat",
            2,
            [
                Line(CharacterId.Akari, "Ah. Ears in the stairwell. Cute. Dangerous. Let's call it leverage and keep smiling."),
                Line(CharacterId.Akari, "Help me keep the club clean, and I won't have to invent a reason you were never here."),
            ],
            [
                C("a2_akari_fold", "Agree to help her 'keep things clean.'", "act2_mid")
                    .Aff(CharacterId.Akari, 12)
                    .Flag(GameFlags.AkariLeverage, GameFlags.AkariCoveredForYou),
                C("a2_akari_defy", "Refuse to be a prop in her minutes.", "act2_mid")
                    .Aff(CharacterId.Akari, -4)
                    .Flag(GameFlags.AkariLeverage, GameFlags.AkariQuestionedRival),
            ]);

        yield return Scene(
            "act2_sora",
            "Act II — Sora",
            2,
            [
                Line(CharacterId.Sora, "Okay bestie hypothetical: if a door 'accidentally' opened and a safe was 'already unlocked,' is that a crime or a plot twist?"),
                Line(CharacterId.Sora, "Asking for a short story. Or a long night. Same difference!"),
            ],
            [
                C("a2_sora_laugh", "Laugh it off as fiction.", "act2_mid").Aff(CharacterId.Sora, 10),
                C("a2_sora_interested", "Ask what the 'safe' is in real life.", "act2_sora_pitch")
                    .Aff(CharacterId.Sora, 6)
                    .Flag(GameFlags.SoraInvitedCrime),
                C("a2_sora_callout", "Call the money story fake to her face.", "act2_sora_pitch")
                    .Requires(GameFlags.SoraMoneyLie)
                    .Aff(CharacterId.Sora, 2)
                    .Flag(GameFlags.SoraInvitedCrime),
            ]);

        yield return Scene(
            "act2_sora_pitch",
            "Act II — Side Hustle",
            2,
            [
                Line(CharacterId.Sora, "Fine. Cards on the gummy table. Campus office has donation cash that 'falls through cracks.' I collect cracks."),
                Line(CharacterId.Sora, "Come with me Friday. Easy. Fun. Or don't — and maybe the cracks start looking like your student ID. Joke! Unless?"),
            ],
            [
                C("a2_sora_accept", "Agree to go.", "act2_mid")
                    .Aff(CharacterId.Sora, 16)
                    .Flag(GameFlags.SoraAcceptedCrime, GameFlags.SoraInvitedCrime),
                C("a2_sora_refuse", "Refuse.", "act2_mid")
                    .Aff(CharacterId.Sora, -2)
                    .Flag(GameFlags.SoraRefusedCrime, GameFlags.SoraInvitedCrime),
            ]);

        yield return Scene(
            "act2_mid",
            "Act II — Crosswinds",
            2,
            [
                Line(null, "The meeting breaks into pockets of laughter and quiet. You can feel routes forming like weather."),
                Line(CharacterId.Akari, "Before you go — next week we do final readings. Choose who you're writing toward. Hearts are plot devices!"),
            ],
            [
                C("a2_to_act3", "Return for final readings.", "act3_lock")
                    .Flag(GameFlags.Act2Complete)
                    .Act(3),
            ]);

        yield return Scene(
            "act3_lock",
            "Act III — Who You're Writing Toward",
            3,
            [
                Line(null, "The annex lights hum. Four doors, one hallway. Your notes have already picked a direction — even if you haven't said it aloud."),
            ],
            [
                C("a3_lock_rin", "Write toward Rin.", "act3_rin").Aff(CharacterId.Rin, 10).Lock(CharacterId.Rin).Act(3),
                C("a3_lock_mika", "Write toward Mika.", "act3_mika").Aff(CharacterId.Mika, 10).Lock(CharacterId.Mika).Act(3),
                C("a3_lock_akari", "Write toward Akari.", "act3_akari").Aff(CharacterId.Akari, 10).Lock(CharacterId.Akari).Act(3),
                C("a3_lock_sora", "Write toward Sora.", "act3_sora").Aff(CharacterId.Sora, 10).Lock(CharacterId.Sora).Act(3),
                C("a3_lock_none", "Write toward no one. Keep the page blank.", "ending_lonely")
                    .Ending(EndingId.LonelyNight)
                    .Act(4)
                    .Rare("Avoid locking a route entirely."),
            ]);

        yield return Scene(
            "act3_rin",
            "Act III — Rin",
            3,
            [
                Line(CharacterId.Rin, "If you're here to fix me, leave. If you're here because the annex is less ugly with you in it... stay. Quietly."),
                Line(null, "Outside, rain stitches the roof. For once she doesn't look like she's counting exits."),
            ],
            [
                C("a3_rin_true", "Stay — without pity, without a speech.", "ending_rin")
                    .Requires(GameFlags.RinInsight, GameFlags.RinNotPity, GameFlags.RinSawKindness)
                    .Aff(CharacterId.Rin, 20)
                    .Ending(EndingId.RinTrueLove)
                    .Act(4)
                    .Rare("Needs insight, no-pity, and witnessing her hidden kindness."),
                C("a3_rin_fail", "Tell her you'll save her from her loneliness.", "ending_lonely")
                    .Ending(EndingId.LonelyNight)
                    .Act(4),
            ]);

        yield return Scene(
            "act3_mika",
            "Act III — Mika",
            3,
            [
                Line(CharacterId.Mika, "I finished your portrait. All of you. Even the parts you don't show people."),
                Line(CharacterId.Mika, "You won't leave, right? Say you won't leave."),
            ],
            [
                C("a3_mika_stay", "Promise forever.", "ending_mika")
                    .Aff(CharacterId.Mika, 20)
                    .Ending(EndingId.MikaDeath)
                    .Act(4),
                C("a3_mika_leave", "Admit you need air — space — an unlocked door.", "ending_mika")
                    .Aff(CharacterId.Mika, 2)
                    .Flag(GameFlags.TriedToLeaveMika)
                    .Ending(EndingId.MikaDeath)
                    .Act(4),
            ]);

        yield return Scene(
            "act3_akari",
            "Act III — Akari",
            3,
            [
                Line(CharacterId.Akari, "There's a disciplinary packet with a blank name. Scandal needs a face. The club needs a survivor."),
                Line(CharacterId.Akari, "You've been so supportive. Be supportive one more time — by being useful."),
            ],
            [
                C("a3_akari_trust", "Trust the president.", "ending_akari")
                    .Ending(EndingId.AkariRuin)
                    .Act(4),
                C("a3_akari_fight", "Refuse to be her scapegoat.", "ending_akari")
                    .Requires(GameFlags.AkariQuestionedRival)
                    .Ending(EndingId.AkariRuin)
                    .Act(4),
            ]);

        yield return Scene(
            "act3_sora",
            "Act III — Sora",
            3,
            [
                Line(CharacterId.Sora, "Friday's still on. Hoodie up. Cameras are decorative, trust me."),
                Line(CharacterId.Sora, "Or we can stay home and let the story write itself with your fingerprints. I'm flexible!"),
            ],
            [
                C("a3_sora_go", "Go with her.", "ending_sora")
                    .Flag(GameFlags.SoraAcceptedCrime)
                    .Ending(EndingId.SoraCrime)
                    .Act(4),
                C("a3_sora_no", "Refuse again.", "ending_sora")
                    .Flag(GameFlags.SoraRefusedCrime)
                    .Ending(EndingId.SoraCrime)
                    .Act(4),
            ]);

        yield return EndingScene(
            "ending_rin",
            "Ending — Rain on the Annex Roof",
            [
                Line(CharacterId.Rin, "I'm not a project. You're not a spare heart. We're just... here."),
                Line(null, "The rain keeps time on the annex roof. For the first time, the After Hours Club feels like a room you can leave — and return to."),
                Line(null, "HAPPY ENDING — the only one."),
            ]);

        yield return EndingScene(
            "ending_mika",
            "Ending — The Locked Drawer",
            [
                Line(CharacterId.Mika, "I told you people leave. I fixed that."),
                Line(null, "The padlock opens. The studio smells like turpentine and copper. Her sketches of you stop having eyes."),
                Line(null, "BAD END — Mika."),
            ]);

        yield return EndingScene(
            "ending_akari",
            "Ending — Minutes of the Meeting",
            [
                Line(CharacterId.Akari, "Thank you for protecting the club. Truly. I'll send a care package to whoever handles your hearing."),
                Line(null, "By morning your name is typed neatly into every form. Akari's smile is unchanged on the campus site."),
                Line(null, "BAD END — Akari."),
            ]);

        yield return EndingScene(
            "ending_sora",
            "Ending — Errand After Midnight",
            [
                Line(CharacterId.Sora, "See? Plot twist! You're the main character of the police report."),
                Line(null, "Whether you held the bag or dropped it, the evidence prefers you. Sora's charms are already for sale off-campus."),
                Line(null, "BAD END — Sora."),
            ]);

        yield return EndingScene(
            "ending_lonely",
            "Ending — Lights Out in the Annex",
            [
                Line(null, "You never chose hard enough for anyone to keep you — or kill you."),
                Line(null, "The club dissolves into group chats that go quiet. The annex lights click off."),
                Line(null, "BITTER END — Alone."),
            ]);
    }

    private static ChoiceBuilder C(string id, string text, string next) =>
        ChoiceBuilder.Make(id, text, next);

    private static SceneDef Scene(
        string id,
        string title,
        int act,
        DialogueLine[] lines,
        ChoiceDef[] choices) =>
        new()
        {
            Id = id,
            Title = title,
            Act = act,
            Lines = lines,
            Choices = choices,
        };

    private static SceneDef EndingScene(string id, string title, DialogueLine[] lines) =>
        new()
        {
            Id = id,
            Title = title,
            Act = 4,
            Lines = lines,
            Choices = [],
            IsEndingScene = true,
        };

    private static DialogueLine Line(CharacterId? speaker, string text) =>
        new()
        {
            Speaker = speaker ?? CharacterId.None,
            Text = text,
        };
}
