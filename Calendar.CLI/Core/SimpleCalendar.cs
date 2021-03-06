﻿namespace Calendar.CLI.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Enumerations;
    using Extensions;
    using Themes.Abstractions;
    using Themes.Components;
    using Translations.Abstractions;
    using Translations.Components;

    /// <summary>
    ///     Represents a calendar layout implementation
    /// </summary>
    public sealed class SimpleCalendar
    {
        private Theme SelectedTheme { get; }
        private Language SelectedLanugage { get; }
        private ConsoleUtility Formatter { get; }
        
        public SimpleCalendar(ThemeID themeId, LanguageID lanugageId, ConsoleUtility formatter)
        {
            var themes = new List<Theme>
            {
                new DarkTheme(), new LightTheme()
            };

            var languages = new List<Language>
            {
                new Polish(), new English()
            };

            Formatter = formatter;
            SelectedTheme = themes.Find(x => x.ThemeId == themeId);
            SelectedLanugage = languages.Find(x => x.LanguageId == lanugageId);
        }

        public SimpleCalendar SetupTheme()
        {
            SelectedTheme.SetTheme();
            return this;
        }

        public SimpleCalendar DisplayCurrentDate()
        {
            var time = DateTime.Now;
            var currentDate = $"Today is {time.Day}/{SelectedLanugage.Months[time.Month - 1]}/{time.Year}";

            Formatter
                .Clear()
                .BreakLine()
                .DisplayTextInColumn(currentDate, $"{{0, {Console.WindowWidth / 2 + currentDate.Length / 2}}}")
                .BreakLine();

            return this;
        }

        public SimpleCalendar DisplayCalendar()
        {
            var index = 0;
            var identificator = 1;
            var time = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(time.Year, time.Month);
            var firstDayOfMonth = (int)new DateTime(time.Year, time.Month, 1).DayOfWeek;
            var identificatorsOfDays = new string[35];

            while (index++ < identificatorsOfDays.Length)
                if (index >= firstDayOfMonth && identificator <= daysInMonth)
                    identificatorsOfDays[index - 1] = $"{identificator++}";

            var final = SelectedLanugage.GetDaysShortcuts(3).Concat(identificatorsOfDays).ToList();

            for (var i = 0; i < 6; i++)
                Formatter.DisplayTextInRow(i == 0 ? final.Take(7).Append("\n") : final.Skip(7 * i).Take(7).Append("\n"), $"{{0, 14}}");

            return this;
        }
    }
}
