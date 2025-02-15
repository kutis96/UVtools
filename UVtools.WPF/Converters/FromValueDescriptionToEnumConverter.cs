﻿/*
 *                     GNU AFFERO GENERAL PUBLIC LICENSE
 *                       Version 3, 19 November 2007
 *  Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
 *  Everyone is permitted to copy and distribute verbatim copies
 *  of this license document, but changing it is not allowed.
 */

using Avalonia.Data.Converters;
using System;
using System.Linq;
using UVtools.Core.Extensions;

namespace UVtools.WPF.Converters;

public class FromValueDescriptionToEnumConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        var list = EnumExtensions.GetAllValuesAndDescriptions(value.GetType());
        return list.FirstOrDefault(vd => vd.Value.Equals(value));
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is null) return null;
        var list = EnumExtensions.GetAllValuesAndDescriptions(targetType);
        return list.FirstOrDefault(vd => vd.Description == value.ToString())?.Value;
    }
}