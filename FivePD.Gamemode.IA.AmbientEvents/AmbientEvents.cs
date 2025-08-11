// Copyright (c) GTAPoliceMods LLC. All Rights Reserved.
//
// All code and assets contained in this file are STRICTLY
// the intellectual property of GTAPoliceMods unless otherwise
// noted by a license provided. Do not copy or redistribute.

using System;
using System.Threading.Tasks;
using FivePD.Gamemode.Server.API;
using FivePD.Gamemode.Server.Interfaces;

namespace FivePD.Gamemode.IA.AmbientEvents
{
    /**
     * <summary>Responsible for creating and management various ambient
     * events around players (e.x.: carjackings or speeders).</summary>
     */
    [AddonMetadata("AmbientEventProvider", "1.0.0", 100)]
    public class AmbientEvents : IGamemodeAddon
    {
        private readonly IAddonLoggerService _logger;

        public AmbientEvents(IAddonLoggerService logger)
        {
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /**
         * <inheritdoc />
         */
        public Task OnStarted()
        {
            try
            {
                this._logger.Information("I started!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AmbientEvents start logging failed: {ex}");
            }

            return Task.CompletedTask;
        }

        /**
         * <inheritdoc />
         */
        public Task OnStopped()
        {
            try
            {
                this._logger.Information("I stopped!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AmbientEvents stop logging failed: {ex}");
            }

            return Task.CompletedTask;
        }
    }
}
