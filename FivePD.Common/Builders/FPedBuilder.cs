// Copyright (c) GTAPoliceMods LLC. All Rights Reserved.
//
// All code and assets contained in this file are STRICTLY
// the intellectual property of GTAPoliceMods unless otherwise
// noted by a license provided. Do not copy or redistribute.

using System;
using System.Collections.Generic;
using System.Linq;
using FivePD.Common.ConfigModels;
using FivePD.Common.Models;
using Newtonsoft.Json;

#pragma warning disable CS1591
#pragma warning disable SA1600

namespace FivePD.Common.Builders
{
    /// <summary>
    /// Used for the construction of <see cref="FPed"/> objects.
    /// </summary>
    public class FPedBuilder
    {
        private readonly Random _random = new Random(Guid.NewGuid().GetHashCode());

        /// <summary>
        /// Initializes a new instance of the <see cref="FPedBuilder"/> class.
        /// </summary>
        /// <param name="networkId">The network identifier of the ped.</param>
        public FPedBuilder(int networkId)
        {
            this.NetworkId = networkId;
        }

        public int NetworkId { get; private set; }

        [JsonProperty]
        public FGender Gender { get; private set; }

        public string Firstname { get; private set; }

        public string Lastname { get; private set; }

        public Date Birthdate { get; private set; }

        public List<Item> Items { get; private set; }

        public FPedBuilder WithFirstname(string name)
        {
            this.Firstname = name;
            return this;
        }

        public FPedBuilder WithFirstname(List<string> names)
        {
            if (names == null || names.Count == 0)
            {
                throw new ArgumentException("Name collection cannot be null or empty", nameof(names));
            }

            this.Firstname = names[this._random.Next(names.Count)].Trim();
            return this;
        }

        public FPedBuilder WithLastname(string name)
        {
            this.Lastname = name;
            return this;
        }

        public FPedBuilder WithLastname(List<string> names)
        {
            if (names == null || names.Count == 0)
            {
                throw new ArgumentException("Name collection cannot be null or empty", nameof(names));
            }

            this.Lastname = names[this._random.Next(names.Count)].Trim();
            return this;
        }

        public FPedBuilder WithGender(FGender gender)
        {
            this.Gender = gender;
            return this;
        }

        public FPedBuilder WithBirthdate()
        {
            int year = this._random.Next(1960, 2001);
            int month = this._random.Next(1, 13);
            int day = this._random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            this.Birthdate = new Date(year, month, day);
            return this;
        }

        public FPedBuilder WithBirthdate(Date birthdate)
        {
            this.Birthdate = birthdate;
            return this;
        }

        public FPedBuilder WithItems(List<Item> items)
        {
            if (items == null || items.Count == 0)
            {
                this.Items = new List<Item>();
                return this;
            }

            this.Items = items
                .Where(item => item.Location == Item.ItemLocation.Ped || item.Location == Item.ItemLocation.Everywhere)
                .OrderBy(_ => Guid.NewGuid())
                .Take(this._random.Next(0, 6))
                .ToList();
            return this;
        }

        /// <summary>
        /// Initializes a new <see cref="FPed"/> instance.
        /// </summary>
        /// <returns>Returns the newly created <see cref="FPed"/> instance.</returns>
        public FPed Build()
        {
            return new FPed(this);
        }
    }
}