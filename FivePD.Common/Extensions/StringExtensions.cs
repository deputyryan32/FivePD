// Copyright (c) GTAPoliceMods LLC. All Rights Reserved.
//
// All code and assets contained in this file are STRICTLY
// the intellectual property of GTAPoliceMods unless otherwise
// noted by a license provided. Do not copy or redistribute.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FivePD.Common.Extensions
{
    /// <summary>
    /// Holds extension methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex ParamRegex = new Regex(@"{{(?<key>[^{}()]+)(?:\((?<value>[^{}()]+)\))?}}", RegexOptions.Compiled);

        /// <summary>
        /// Replaces parameter identifiers with their values depending on its template.
        /// The template {{key}} will be converted to formatting + key.
        /// The template {{key(value)}} will be converted to formatting + value.
        /// More in-depth example: https://github.com/GTAPoliceMods/FivePD/issues/46#issuecomment-1100093724.
        /// </summary>
        /// <param name="text">The text that's parameters needs to be replaced.</param>
        /// <param name="parameters">A dictionary containing the key + formatting pairs.</param>
        /// <param name="formattingReset">This string will be placed after the replaced value to reset its formatting.</param>
        /// <returns>The new string with the replaced parameters.</returns>
        public static string ReplaceParams(this string text, Dictionary<string, string> parameters, string formattingReset = "")
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Count == 0)
            {
                return text;
            }

            return ParamRegex.Replace(text, match =>
            {
                var key = match.Groups["key"].Value;
                if (!parameters.TryGetValue(key, out var replacement))
                {
                    return match.Value;
                }

                var valueGroup = match.Groups["value"];
                var suffix = valueGroup.Success ? valueGroup.Value : string.Empty;
                return replacement + suffix + formattingReset;
            });
        }
    }
}