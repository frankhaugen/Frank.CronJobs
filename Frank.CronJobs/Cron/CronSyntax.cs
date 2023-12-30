/*
 * MIT License
 *
 * Copyright (c) 2018 Marx J. Moura
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Text.RegularExpressions;

namespace Frank.CronJobs.Cron;

internal sealed partial class CronSyntax(IEnumerable<string> expressions)
{
    private const string Asterisk = @"^\*$"; // Matches: *
    private const string Dash = @"^\d{1,2}-\d{1,2}$"; // Matches: 00-00
    private const string Hash = @"^\d{1,2}#[1-5]$"; // Matches: 00#0
    private const string LAndW = @"^(\d)?L$|L-\d{1,2}|^LW$|^\d{1,2}W$"; // Matches: L, 0L, L-00, LW or 00W
    private const string Slash = @"^(\*|\d{1,2}(-\d{1,2})?)/\d{1,2}$"; // Matches: */00, 00/00 or 00-00/00
    private const string ListValue = @"\d{1,2}|\d{1,2}-\d{1,2}|\d{1,2}(-\d{1,2})?/\d{1,2}"; // Matches: 00, 00-00, 00/00 or 00-00/00

    private const int SecondPosition = 0;
    private const int MinutePosition = 1;
    private const int HourPosition = 2;
    private const int DayPosition = 3;
    private const int MonthPosition = 4;
    private const int DayOfWeekPosition = 5;

    public bool IsValid()
    {
        if (expressions.Count() != 6)
            return false;

        if (!AllowedCharacters("*-,/", expressions.ElementAt(SecondPosition))) return false;
        if (!AllowedCharacters("*-,/", expressions.ElementAt(MinutePosition))) return false;
        if (!AllowedCharacters("*-,/", expressions.ElementAt(HourPosition))) return false;
        if (!AllowedCharacters("*-,/LW", expressions.ElementAt(DayPosition))) return false;
        if (!AllowedCharacters("*-,/", expressions.ElementAt(MonthPosition))) return false;
        if (!AllowedCharacters("*-,/L#", expressions.ElementAt(DayOfWeekPosition))) return false;

        return IsWellFormed();
    }

    private static bool AllowedCharacters(string allowedCharacters, string subExpression)
    {
        var characters = AllowedCharactersRegex().Replace(subExpression, string.Empty);
        return characters.Length == 0 || characters.All(allowedCharacters.Contains);
    }

    private bool IsWellFormed()
    {
        const string list = $@"^{ListValue}(,({ListValue}))*$";
        const string pattern = $@"{Asterisk}|{Dash}|{Hash}|{Slash}|{LAndW}|{list}";
        // var regex = IsWellFormedRegex();
        var regex = new Regex(pattern, RegexOptions.Compiled);
        
        return expressions.All(exp => regex.IsMatch(exp));
    }

    [GeneratedRegex(@"\d")]
    private static partial Regex AllowedCharactersRegex();
    
    [GeneratedRegex(@"^\*$|^\d{1,2}-\d{1,2}$|^\d{1,2}#[1-5]$|^(\*|\d{1,2}(-\d{1,2})?)/\d{1,2}$|^(\d)?L$|L-\d{1,2}|^LW$|^\d{1,2}W$|^\d{1,2}|\d{1,2}-\d{1,2}|\d{1,2}(-\d{1,2})?/\d{1,2}(,(\d{1,2}|\d{1,2}-\d{1,2}|\d{1,2}(-\d{1,2})?/\d{1,2}))*$", RegexOptions.Compiled)]
    private static partial Regex IsWellFormedRegex();
}
