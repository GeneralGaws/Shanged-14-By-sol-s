// LIGHT TODO: лорные название вербов
// HARD TODO: анимации
// POSSIBLE FUTURE: цвет зависит от фурри, голос от пола
using Content.Server.DoAfter;
using Content.Shared.DoAfter;
using Content.Shared.Verbs;
using Content.Shared.Changed14.Fuckable;
using Robust.Shared.Audio.Systems;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Changed14.Furry;
using Content.Server.Stunnable;
using Content.Shared.Interaction;
using Content.Shared.Popups;


namespace Content.Server.Changed14.Fuckable;

public sealed class FuckableSystem : EntitySystem
{
    [Dependency] private readonly DoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private readonly StunSystem _stun = default!;
    [Dependency] private readonly SharedInteractionSystem _interaction = default!;
    [Dependency] private readonly SharedPopupSystem _popupSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<FuckableComponent, GetVerbsEvent<ActivationVerb>>(OnActivationVerb);
        SubscribeLocalEvent<FuckableComponent, FuckDoAfterEvent>(OnFuckDoAfter);
    }

    private void OnFuckDoAfter(EntityUid uid, FuckableComponent comp, ref FuckDoAfterEvent args)
    {

        if (args.Cancelled)
            return;

        if (!_solutionContainer.TryGetInjectableSolution(uid, out var injectable, out _))
            return;

        _solutionContainer.TryAddReagent(injectable.Value, comp.ReagentId, 5);

        return;
    }

    private void OnActivationVerb(EntityUid uid, FuckableComponent comp, ref GetVerbsEvent<ActivationVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract)
            return;

        if (!HasComp<FurryComponent>(args.User))
            return;

        var user = args.User;

        var verb = new ActivationVerb()
        {
            Act = () => HandleFuck(uid, user, comp),
            Text = Loc.GetString("changed-fuck-verb"),
            Message = Loc.GetString("changed-fuck-desc"),
        };

        args.Verbs.Add(verb);
    }

    private void HandleFuck(EntityUid uid, EntityUid user, FuckableComponent comp)
    {
        var doAfterArgs = new DoAfterArgs(EntityManager, user, TimeSpan.FromSeconds(3), new FuckDoAfterEvent(), uid, uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = false,
            DistanceThreshold = 0.5f,
            MovementThreshold = 0.15f,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);

        if (_interaction.InRangeUnobstructed(user, uid, 0.5f, Shared.Physics.CollisionGroup.Impassable))
        {
            _audio.PlayPvs(comp.FuckableCumSound, uid);
            _stun.TryKnockdown(uid, TimeSpan.FromSeconds(5), true);
        }
        else
        {
            _popupSystem.PopupEntity(Loc.GetString("changed-fuck-range"), user, user);
        }
    }
}
