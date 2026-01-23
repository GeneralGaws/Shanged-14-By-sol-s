using Content.Server._Changed14.ResearchEvac.Components;
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
        OnGeneratingFinished(uid, component);
    }

    private void UpdateGeneratorUi(EntityUid uid, ResearchEvacConsoleComponent component)
    {
        var canCall = (TryComp<ResearchEvacConsoleComponent>(uid, out var printing));
        var state = new ResearchEvacConsoleBoundUserInterfaceState(canCall);
        _ui.SetUiState(uid, ResearchEvacConsoleUiKey.Key, state);


    }

    private void OnGeneratingFinished(EntityUid uid, ResearchEvacConsoleComponent component)
    {
        _chat.DispatchGlobalAnnouncement(Loc.GetString("ЕБИТЕ ФУРРИ"), Loc.GetString("ИИСУС"), false, null, colorOverride: Color.Crimson);
    }

}
