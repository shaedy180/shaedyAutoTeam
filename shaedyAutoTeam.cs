using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using System;

namespace shaedyAutoTeam;

public class AutoTeamJoin : BasePlugin
{
    public override string ModuleName => "shaedy AutoTeam";
    public override string ModuleVersion => "1.1.0";
    public override string ModuleAuthor => "shaedy";

    public override void Load(bool hotReload)
    {
        RegisterEventHandler<EventPlayerConnectFull>(OnPlayerConnectFull);
    }

    private HookResult OnPlayerConnectFull(EventPlayerConnectFull @event, GameEventInfo info)
    {
        var player = @event.Userid;

        if (player == null || !player.IsValid || player.IsHLTV || player.IsBot)
            return HookResult.Continue;

        // Small delay so the client is ready for the team switch
        AddTimer(1.0f, () =>
        {
            if (!player.IsValid) return;

            if (player.TeamNum == (byte)CsTeam.Spectator || player.TeamNum == (byte)CsTeam.None)
            {
                var targetTeam = (CsTeam)Random.Shared.Next(2, 4);
                player.ChangeTeam(targetTeam);

                // Force respawn so the player doesn't get stuck as dead
                player.Respawn();

                Console.WriteLine($"[AutoTeam] {player.PlayerName} -> {targetTeam} (Respawned)");
            }
        });

        return HookResult.Continue;
    }
}