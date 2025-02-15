﻿/*
 *                     GNU AFFERO GENERAL PUBLIC LICENSE
 *                       Version 3, 19 November 2007
 *  Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
 *  Everyone is permitted to copy and distribute verbatim copies
 *  of this license document, but changing it is not allowed.
 */

using Emgu.CV.Cuda;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UVtools.Core.Layers;
using UVtools.Core.Operations;

namespace UVtools.Core;

public static class CoreSettings
{
    #region Members
    private static int _maxDegreeOfParallelism = -1;

    #endregion

    #region Properties

    public static CultureInfo OptimalCultureInfo
    {
        get
        {
            var cultureInfo = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            cultureInfo.NumberFormat.CurrencyDecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.CurrencyDecimalSeparator;
            cultureInfo.NumberFormat.CurrencyGroupSeparator = CultureInfo.InvariantCulture.NumberFormat.CurrencyGroupSeparator;
            cultureInfo.NumberFormat.NumberDecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator;
            cultureInfo.NumberFormat.NumberGroupSeparator = CultureInfo.InvariantCulture.NumberFormat.NumberGroupSeparator;
            cultureInfo.NumberFormat.PercentDecimalSeparator = CultureInfo.InvariantCulture.NumberFormat.PercentDecimalSeparator;
            cultureInfo.NumberFormat.PercentGroupSeparator = CultureInfo.InvariantCulture.NumberFormat.PercentGroupSeparator;
            return cultureInfo;
        }
    }

    /// <summary>
    /// Gets or sets the maximum number of concurrent tasks enabled by this ParallelOptions instance.
    /// Less or equal to 0 will set to auto number
    /// 1 = Single thread
    /// n = Multi threads
    /// </summary>
    public static int MaxDegreeOfParallelism
    {
        get => _maxDegreeOfParallelism;
        set => _maxDegreeOfParallelism = value > 0 ? Math.Min(value, Environment.ProcessorCount) : -1;
    }

    /// <summary>
    /// Gets the ParallelOptions with <see cref="MaxDegreeOfParallelism"/> set
    /// </summary>
    public static ParallelOptions ParallelOptions => new() {MaxDegreeOfParallelism = _maxDegreeOfParallelism};

    /// <summary>
    /// Gets the ParallelOptions with <see cref="MaxDegreeOfParallelism"/> set to 1 for debug purposes
    /// </summary>
    public static ParallelOptions ParallelDebugOptions => new() { MaxDegreeOfParallelism = 1 };


    /// <summary>
    /// Gets the ParallelOptions with <see cref="MaxDegreeOfParallelism"/> and the <see cref="CancellationToken"/> set
    /// </summary>
    public static ParallelOptions GetParallelOptions(CancellationToken token = default)
    {
        var options = ParallelOptions;
        options.CancellationToken = token;
        return options;
    }

    /// <summary>
    /// Gets the ParallelOptions with <see cref="MaxDegreeOfParallelism"/> and the <see cref="CancellationToken"/> set
    /// </summary>
    public static ParallelOptions GetParallelOptions(OperationProgress progress) => GetParallelOptions(progress.Token);

    /// <summary>
    /// Gets or sets if operations run via CUDA when possible
    /// </summary>
    public static bool EnableCuda { get; set; }

    /// <summary>
    /// Gets if we can use cuda on operations
    /// </summary>
    public static bool CanUseCuda => EnableCuda && CudaInvoke.HasCuda;

    /// <summary>
    /// Gets or sets the default compression type for layers
    /// </summary>
    public static LayerCompressionCodec DefaultLayerCompressionCodec { get; set; } = LayerCompressionCodec.Lz4;

    /// <summary>
    /// <para>The average resin 1000ml bottle cost, to use when bottle cost is not available.</para>
    /// <para>Use 0 to disable.</para>
    /// </summary>
    public static float AverageResin1000MlBottleCost = 60f;

    /// <summary>
    /// Gets the default folder to save the settings
    /// </summary>
    public static string DefaultSettingsFolder
    {
        get
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            if (string.IsNullOrWhiteSpace(folder)) folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrWhiteSpace(folder)) folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            if (string.IsNullOrWhiteSpace(folder)) folder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var path = Path.Combine(folder, About.Software);
            return path;
        }
    }

    /// <summary>
    /// Gets the default folder to save the settings
    /// </summary>
    public static string DefaultSettingsFolderAndEnsureCreation
    {
        get
        {
            var path = DefaultSettingsFolder;
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return path;
        }
    }

    #endregion
}