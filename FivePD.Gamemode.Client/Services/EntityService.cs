// Copyright (c) GTAPoliceMods LLC. All Rights Reserved.
//
// All code and assets contained in this file are STRICTLY
// the intellectual property of GTAPoliceMods unless otherwise
// noted by a license provided. Do not copy or redistribute.

using System;
using System.Threading.Tasks;
using CitizenFX.Core;
using FivePD.Common;
using FivePD.Gamemode.Client.Interfaces;
using Cfx = CitizenFX.Core.Native;

namespace FivePD.Gamemode.Client.Services
{
    /// <inheritdoc />
    public class EntityService : IEntityService
    {
        private const int DefaultTimeoutMs = 5000;
        private readonly ILoggerService _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityService"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerService"/> to use for logging.</param>
        public EntityService(ILoggerService logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this._logger = logger;
        }

        /// <inheritdoc />
        public async Task<Ped> SpawnPed(uint model, Vector3 location, float heading)
        {
            Cfx.API.RequestModel(model);

            var timeout = BaseScript.Delay(DefaultTimeoutMs);
            while (!Cfx.API.HasModelLoaded(model))
            {
                if (await Task.WhenAny(BaseScript.Delay(100), timeout).ConfigureAwait(false) == timeout)
                {
                    this._logger.Warn("SpawnPed timed out for model {0}", model);
                    throw new TimeoutException("Timed out waiting for ped model to load");
                }
            }

            var handle = Cfx.API.CreatePed(26, model, location.X, location.Y, location.Z, heading, true, false);
            return new Ped(handle);
        }

        /// <inheritdoc />
        public async Task<Vehicle> SpawnVehicle(uint model, Vector3 location, float heading)
        {
            var tcs = new TaskCompletionSource<int>();
            BaseScript.TriggerServerEvent(Events.EntityManagement.RequestVehicle, model, location, heading, new Action<int>(id =>
            {
                tcs.TrySetResult(id);
            }));

            var completed = await Task.WhenAny(tcs.Task, BaseScript.Delay(DefaultTimeoutMs)).ConfigureAwait(false);
            if (completed != tcs.Task)
            {
                this._logger.Warn("SpawnVehicle timed out for model {0}", model);
                throw new TimeoutException("Timed out waiting for vehicle spawn response");
            }

            var entity = Entity.FromNetworkId(tcs.Task.Result);
            if (!(entity is Vehicle vehicle))
            {
                throw new InvalidOperationException("Spawned entity was not a vehicle");
            }

            return vehicle;
        }

        /// <inheritdoc />
        public async Task<Prop> SpawnProp(int model, Vector3 location)
        {
            uint uintModel = (uint)model;
            Cfx.API.RequestModel(uintModel);

            var timeout = BaseScript.Delay(DefaultTimeoutMs);
            while (!Cfx.API.HasModelLoaded(uintModel))
            {
                if (await Task.WhenAny(BaseScript.Delay(100), timeout).ConfigureAwait(false) == timeout)
                {
                    this._logger.Warn("SpawnProp timed out for model {0}", model);
                    throw new TimeoutException("Timed out waiting for prop model to load");
                }
            }

            var handle = Cfx.API.CreateObject(model, location.X, location.Y, location.Z, true, false, false);
            return new Prop(handle);
        }
    }
}