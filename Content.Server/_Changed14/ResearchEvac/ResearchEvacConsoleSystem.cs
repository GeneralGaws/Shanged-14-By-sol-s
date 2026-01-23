using Content.Server._Changed14.ResearchEvac.Components;
// using Content.Client._Changed14.ResearchEvac;
using Content.Server.Power.EntitySystems;
using Content.Server.Station.Components;
using Content.Shared._Changed14.ResearchEvac;
using Content.Shared.CCVar;
using Content.Shared.Materials;
using Content.Shared.Radio;
using Robust.Shared.Audio;
using Content.Shared.Physics;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Components;
using Content.Shared.Power;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Content.Server.Research.Systems;
using Content.Server.Research.TechnologyDisk.Components;
using Content.Shared.UserInterface;
using Content.Shared.Research;
using Content.Shared.Research.Components;
using Robust.Server.Audio;
using Robust.Server.GameObjects;
using Robust.Shared.Timing;
using Content.Server.Chat.Systems;

namespace Content.Server._Changed14.ResearchEvac;

public sealed class ResearchEvacSystem : EntitySystem
{
    [Dependency] private readonly SharedMapSystem _mapSystem = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly ChatSystem _chat = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ResearchEvacConsoleComponent, BoundUIOpenedEvent>(OnGeneratorBUIOpened);
        SubscribeLocalEvent<ResearchEvacConsoleComponent, ResearchEvacButtonPressedEvent>(OnCallEvacButtonPressed);
    }

    private void OnGeneratorBUIOpened(EntityUid uid, ResearchEvacConsoleComponent component, BoundUIOpenedEvent args)
    {
        UpdateGeneratorUi(uid, component);
    }

    private void OnCallEvacButtonPressed(EntityUid uid, ResearchEvacConsoleComponent component, ResearchEvacButtonPressedEvent message)
    {

        UpdateGeneratorUi(uid, component);
        _chat.DispatchGlobalAnnouncement(Loc.GetString("ЕБИТЕ ФУРРИ"), Loc.GetString("ИИСУС"), false, null, colorOverride: Color.Crimson);

    }

    private void UpdateGeneratorUi(EntityUid uid, ResearchEvacConsoleComponent component)
    {
        var canCall = (TryComp<ResearchEvacConsoleComponent>(uid, out var printing));
        var isConsole = (TryComp<ResearchEvacConsoleComponent>(uid, out var console));
        var state = new ResearchEvacConsoleBoundUserInterfaceState(canCall, isConsole);
        _ui.SetUiState(uid, ResearchEvacConsoleUiKey.Key, state);
        _chat.DispatchGlobalAnnouncement(Loc.GetString("ЕБИТЕ ФУРРИ"), Loc.GetString("ИИСУС"), false, null, colorOverride: Color.Crimson);
        OnGeneratingFinished(uid, component);

    }

    private void TryGeneratorCreateAnomaly(EntityUid uid, ResearchEvacConsoleComponent component)
    {
        // if (!Resolve(uid, ref component))
        //     return;

        // if (!this.IsPowered(uid, EntityManager))
        //     return;


        UpdateGeneratorUi(uid, component);
    }

    private void OnGeneratingFinished(EntityUid uid, ResearchEvacConsoleComponent component)
    {
        var message = Loc.GetString("anomaly-generator-announcement");
        _chat.DispatchGlobalAnnouncement(Loc.GetString("У ВАС ЕСТЬ 2 МИНУТЫ ЧТОБЫ УБИТЬ ДРУГ ДРУГА!"), Loc.GetString("Бог-император"), false, null, colorOverride: Color.Crimson);
    }

    // private void UpdateGenerator()
    // {
    //     var query = EntityQueryEnumerator<ResearchEvacConsoleComponent>();
    //     while (query.MoveNext(out var ent, out var gen))
    //     {
    //         OnGeneratingFinished(ent, gen);
    //     }
    // }

}
