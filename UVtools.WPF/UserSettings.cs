﻿/*
 *                     GNU AFFERO GENERAL PUBLIC LICENSE
 *                       Version 3, 19 November 2007
 *  Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
 *  Everyone is permitted to copy and distribute verbatim copies
 *  of this license document, but changing it is not allowed.
 */

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Avalonia.Media;
using JetBrains.Annotations;
using UVtools.Core;
using UVtools.Core.Objects;
using Color=UVtools.WPF.Structures.Color;

namespace UVtools.WPF
{
    [Serializable]
    public sealed class UserSettings : BindableBase
    {
        #region Constants
        public const ushort SETTINGS_VERSION = 5;
        #endregion

        #region Sub classes

        #region General
        [Serializable]
        public sealed class GeneralUserSettings : BindableBase
        {
            private bool _startMaximized = true;
            private bool _checkForUpdatesOnStartup = true;
            private bool _loadDemoFileOnStartup = true;
            private int _maxDegreeOfParallelism = -1;

            private bool _windowsCanResize;
            private bool _windowsTakeIntoAccountScreenScaling = true;
            private ushort _windowsHorizontalMargin = 100;
            private ushort _windowsVerticalMargin = 60;
            private bool _expandDescriptions = true;
            private byte _defaultOpenFileExtensionIndex;
            private string _defaultDirectoryOpenFile;
            private string _defaultDirectorySaveFile;
            private string _defaultDirectoryExtractFile;
            private string _defaultDirectoryConvertFile;
            private string _defaultDirectoryScripts;
            private bool _promptOverwriteFileSave = true;
            private string _fileSaveNamePrefix;
            private string _fileSaveNameSuffix = "_copy";


            public bool StartMaximized
            {
                get => _startMaximized;
                set => RaiseAndSetIfChanged(ref _startMaximized, value);
            }

            public bool CheckForUpdatesOnStartup
            {
                get => _checkForUpdatesOnStartup;
                set => RaiseAndSetIfChanged(ref _checkForUpdatesOnStartup, value);
            }

            public bool LoadDemoFileOnStartup
            {
                get => _loadDemoFileOnStartup;
                set => RaiseAndSetIfChanged(ref _loadDemoFileOnStartup, value);
            }

            /// <summary>
            /// Gets or sets the maximum number of concurrent tasks enabled by a ParallelOptions instance.
            /// </summary>
            public int MaxDegreeOfParallelism
            {
                get => _maxDegreeOfParallelism;
                set => RaiseAndSetIfChanged(ref _maxDegreeOfParallelism, Math.Min(value, Environment.ProcessorCount));
            }

            public bool WindowsCanResize
            {
                get => _windowsCanResize;
                set => RaiseAndSetIfChanged(ref _windowsCanResize, value);
            }

            public bool WindowsTakeIntoAccountScreenScaling
            {
                get => _windowsTakeIntoAccountScreenScaling;
                set => RaiseAndSetIfChanged(ref _windowsTakeIntoAccountScreenScaling, value);
            }

            public ushort WindowsHorizontalMargin
            {
                get => _windowsHorizontalMargin;
                set => RaiseAndSetIfChanged(ref _windowsHorizontalMargin, value);
            }

            public ushort WindowsVerticalMargin
            {
                get => _windowsVerticalMargin;
                set => RaiseAndSetIfChanged(ref _windowsVerticalMargin, value);
            }

            public bool ExpandDescriptions
            {
                get => _expandDescriptions;
                set => RaiseAndSetIfChanged(ref _expandDescriptions, value);
            }

            public byte DefaultOpenFileExtensionIndex
            {
                get => _defaultOpenFileExtensionIndex;
                set => RaiseAndSetIfChanged(ref _defaultOpenFileExtensionIndex, value);
            }

            public string DefaultDirectoryOpenFile
            {
                get => _defaultDirectoryOpenFile;
                set => RaiseAndSetIfChanged(ref _defaultDirectoryOpenFile, value);
            }

            public string DefaultDirectorySaveFile
            {
                get => _defaultDirectorySaveFile;
                set => RaiseAndSetIfChanged(ref _defaultDirectorySaveFile, value);
            }

            public string DefaultDirectoryExtractFile
            {
                get => _defaultDirectoryExtractFile;
                set => RaiseAndSetIfChanged(ref _defaultDirectoryExtractFile, value);
            }

            public string DefaultDirectoryConvertFile
            {
                get => _defaultDirectoryConvertFile;
                set => RaiseAndSetIfChanged(ref _defaultDirectoryConvertFile, value);
            }

            public string DefaultDirectoryScripts
            {
                get => _defaultDirectoryScripts;
                set => RaiseAndSetIfChanged(ref _defaultDirectoryScripts, value);
            }


            public bool PromptOverwriteFileSave
            {
                get => _promptOverwriteFileSave;
                set => RaiseAndSetIfChanged(ref _promptOverwriteFileSave, value);
            }

            public string FileSaveNamePrefix
            {
                get => _fileSaveNamePrefix;
                set => RaiseAndSetIfChanged(ref _fileSaveNamePrefix, value);
            }

            public string FileSaveNameSuffix
            {
                get => _fileSaveNameSuffix;
                set => RaiseAndSetIfChanged(ref _fileSaveNameSuffix, value);
            }

            public GeneralUserSettings() { }

            public GeneralUserSettings Clone()
            {
                
                var clone = MemberwiseClone() as GeneralUserSettings;
                return clone;
            }
        }
        #endregion

        #region Layer Preview
        [Serializable]
        public sealed class LayerPreviewUserSettings : BindableBase
        {
            private Color _tooltipOverlayBackgroundColor = new(210, 255, 255, 192);
            private bool _tooltipOverlay = true;
            private Color _volumeBoundsOutlineColor = new(255, 0, 255, 0);
            private byte _volumeBoundsOutlineThickness = 3;
            private bool _volumeBoundsOutline = true;
            private Color _layerBoundsOutlineColor = new(255, 45, 150, 45);
            private byte _layerBoundsOutlineThickness = 3;
            private bool _layerBoundsOutline = false;
            private Color _contourBoundsOutlineColor = new(255, 50, 100, 50);
            private byte _contourBoundsOutlineThickness = 2;
            private bool _contourBoundsOutline = false;
            private Color _hollowOutlineColor = new(255, 255, 165, 0);
            private sbyte _hollowOutlineLineThickness = 5;
            private bool _hollowOutline = false;
            private Color _centroidOutlineColor = new(255, 255, 0, 0);
            private byte _centroidOutlineDiameter = 8;
            private bool _centroidOutlineHollow = false;
            private bool _centroidOutline = true;
            private Color _maskOutlineColor = new(255, 42, 157, 244);
            private sbyte _maskOutlineLineThickness = 10;
            private bool _maskClearRoiAfterSet = true;
            private Color _previousLayerDifferenceColor = new(255, 81, 131, 82);
            private Color _nextLayerDifferenceColor = new(255, 81, 249, 252);
            private Color _bothLayerDifferenceColor = new(255, 246, 240, 216);
            private bool _showLayerDifference = false;
            private bool _layerDifferenceHighlightSimilarityInstead = false;
            private Color _islandColor = new(255, 255, 255, 0); 
            private Color _islandHighlightColor = new(255, 255, 215, 0);
            private Color _overhangColor = new(255, 255, 105, 180);
            private Color _overhangHighlightColor = new(255, 255, 20, 147);
            private Color _resinTrapColor = new(255, 255, 165, 0);
            private Color _resinTrapHighlightColor = new(255, 244, 164, 96); 
            private Color _touchingBoundsColor = new(255, 255, 0, 0);
            private Color _crosshairColor = new(255, 255, 0, 0);
            private bool _zoomToFitPrintVolumeBounds = true;
            private byte _zoomLockLevelIndex = 7;
            private bool _zoomIssues = true;
            private bool _crosshairShowOnlyOnSelectedIssues = false;
            private byte _crosshairFadeLevelIndex = 5;
            private uint _crosshairLength = 20;
            private byte _crosshairMargin = 5;
            private bool _autoRotateLayerBestView = true;
            private bool _autoFlipLayerIfMirrored = true;
            private bool _layerZoomToFitOnLoad = true;
            private bool _showBackgroudGrid;

            public Color TooltipOverlayBackgroundColor
            {
                get => _tooltipOverlayBackgroundColor;
                set
                {
                    RaiseAndSetIfChanged(ref _tooltipOverlayBackgroundColor, value);
                    RaisePropertyChanged(nameof(TooltipOverlayBackgroundBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush TooltipOverlayBackgroundBrush
            {
                get => new(_tooltipOverlayBackgroundColor.ToAvalonia());
                set => TooltipOverlayBackgroundColor = new Color(value);
            }

            public bool TooltipOverlay
            {
                get => _tooltipOverlay;
                set => RaiseAndSetIfChanged(ref _tooltipOverlay, value);
            }

            public Color VolumeBoundsOutlineColor
            {
                get => _volumeBoundsOutlineColor;
                set
                {
                    RaiseAndSetIfChanged(ref _volumeBoundsOutlineColor, value);
                    RaisePropertyChanged(nameof(VolumeBoundsOutlineBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush VolumeBoundsOutlineBrush
            {
                get => new(_volumeBoundsOutlineColor.ToAvalonia());
                set => VolumeBoundsOutlineColor = new Color(value);
            }

            public byte VolumeBoundsOutlineThickness
            {
                get => _volumeBoundsOutlineThickness;
                set => RaiseAndSetIfChanged(ref _volumeBoundsOutlineThickness, value);
            }

            public bool VolumeBoundsOutline
            {
                get => _volumeBoundsOutline;
                set => RaiseAndSetIfChanged(ref _volumeBoundsOutline, value);
            }

            public Color LayerBoundsOutlineColor
            {
                get => _layerBoundsOutlineColor;
                set
                {
                    RaiseAndSetIfChanged(ref _layerBoundsOutlineColor, value);
                    RaisePropertyChanged(nameof(LayerBoundsOutlineBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush LayerBoundsOutlineBrush
            {
                get => new(_layerBoundsOutlineColor.ToAvalonia());
                set => LayerBoundsOutlineColor = new Color(value);
            }

            public byte LayerBoundsOutlineThickness
            {
                get => _layerBoundsOutlineThickness;
                set => RaiseAndSetIfChanged(ref _layerBoundsOutlineThickness, value);
            }

            public bool LayerBoundsOutline
            {
                get => _layerBoundsOutline;
                set => RaiseAndSetIfChanged(ref _layerBoundsOutline, value);
            }

            public Color ContourBoundsOutlineColor
            {
                get => _contourBoundsOutlineColor;
                set
                {
                    RaiseAndSetIfChanged(ref _contourBoundsOutlineColor, value);
                    RaisePropertyChanged(nameof(ContourBoundsOutlineBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush ContourBoundsOutlineBrush
            {
                get => new(_contourBoundsOutlineColor.ToAvalonia());
                set => ContourBoundsOutlineColor = new Color(value);
            }

            public byte ContourBoundsOutlineThickness
            {
                get => _contourBoundsOutlineThickness;
                set => RaiseAndSetIfChanged(ref _contourBoundsOutlineThickness, value);
            }

            public bool ContourBoundsOutline
            {
                get => _contourBoundsOutline;
                set => RaiseAndSetIfChanged(ref _contourBoundsOutline, value);
            }

            public Color HollowOutlineColor
            {
                get => _hollowOutlineColor;
                set
                {
                    RaiseAndSetIfChanged(ref _hollowOutlineColor, value);
                    RaisePropertyChanged(nameof(HollowOutlineBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush HollowOutlineBrush
            {
                get => new(_hollowOutlineColor.ToAvalonia());
                set => HollowOutlineColor = new Color(value);
            }

            public sbyte HollowOutlineLineThickness
            {
                get => _hollowOutlineLineThickness;
                set => RaiseAndSetIfChanged(ref _hollowOutlineLineThickness, value);
            }

            public bool HollowOutline
            {
                get => _hollowOutline;
                set => RaiseAndSetIfChanged(ref _hollowOutline, value);
            }

            public Color CentroidOutlineColor
            {
                get => _centroidOutlineColor;
                set
                {
                    RaiseAndSetIfChanged(ref _centroidOutlineColor, value);
                    RaisePropertyChanged(nameof(CentroidOutlineBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush CentroidOutlineBrush
            {
                get => new(_centroidOutlineColor.ToAvalonia());
                set => CentroidOutlineColor = new Color(value);
            }

            public byte CentroidOutlineDiameter
            {
                get => _centroidOutlineDiameter;
                set => RaiseAndSetIfChanged(ref _centroidOutlineDiameter, value);
            }

            public bool CentroidOutlineHollow
            {
                get => _centroidOutlineHollow;
                set => RaiseAndSetIfChanged(ref _centroidOutlineHollow, value);
            }

            public bool CentroidOutline
            {
                get => _centroidOutline;
                set => RaiseAndSetIfChanged(ref _centroidOutline, value);
            }

            public Color MaskOutlineColor
            {
                get => _maskOutlineColor;
                set
                {
                    RaiseAndSetIfChanged(ref _maskOutlineColor, value);
                    RaisePropertyChanged(nameof(MaskOutlineBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush MaskOutlineBrush
            {
                get => new(_maskOutlineColor.ToAvalonia());
                set => MaskOutlineColor = new Color(value);
            }

            public sbyte MaskOutlineLineThickness
            {
                get => _maskOutlineLineThickness;
                set => RaiseAndSetIfChanged(ref _maskOutlineLineThickness, value);
            }

            public bool MaskClearROIAfterSet
            {
                get => _maskClearRoiAfterSet;
                set => RaiseAndSetIfChanged(ref _maskClearRoiAfterSet, value);
            }

            public Color PreviousLayerDifferenceColor
            {
                get => _previousLayerDifferenceColor;
                set
                {
                    RaiseAndSetIfChanged(ref _previousLayerDifferenceColor, value);
                    RaisePropertyChanged(nameof(PreviousLayerDifferenceBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush PreviousLayerDifferenceBrush
            {
                get => new(_previousLayerDifferenceColor.ToAvalonia());
                set => PreviousLayerDifferenceColor = new Color(value);
            }

            public Color NextLayerDifferenceColor
            {
                get => _nextLayerDifferenceColor;
                set
                {
                    RaiseAndSetIfChanged(ref _nextLayerDifferenceColor, value);
                    RaisePropertyChanged(nameof(NextLayerDifferenceBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush NextLayerDifferenceBrush
            {
                get => new(_nextLayerDifferenceColor.ToAvalonia());
                set => NextLayerDifferenceColor = new Color(value);
            }

            public Color BothLayerDifferenceColor
            {
                get => _bothLayerDifferenceColor;
                set
                {
                    RaiseAndSetIfChanged(ref _bothLayerDifferenceColor, value);
                    RaisePropertyChanged(nameof(BothLayerDifferenceBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush BothLayerDifferenceBrush
            {
                get => new(_bothLayerDifferenceColor.ToAvalonia());
                set => BothLayerDifferenceColor = new Color(value);
            }

            public bool ShowLayerDifference
            {
                get => _showLayerDifference;
                set => RaiseAndSetIfChanged(ref _showLayerDifference, value);
            }

            public bool LayerDifferenceHighlightSimilarityInstead
            {
                get => _layerDifferenceHighlightSimilarityInstead;
                set => RaiseAndSetIfChanged(ref _layerDifferenceHighlightSimilarityInstead, value);
            }

            public Color IslandColor
            {
                get => _islandColor;
                set
                {
                    RaiseAndSetIfChanged(ref _islandColor, value);
                    RaisePropertyChanged(nameof(IslandBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush IslandBrush
            {
                get => new(_islandColor.ToAvalonia());
                set => IslandColor = new Color(value);
            }

            public Color IslandHighlightColor
            {
                get => _islandHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _islandHighlightColor, value);
                    RaisePropertyChanged(nameof(IslandHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush IslandHighlightBrush
            {
                get => new(_islandHighlightColor.ToAvalonia());
                set => IslandHighlightColor = new Color(value);
            }

            public Color OverhangColor
            {
                get => _overhangColor;
                set
                {
                    RaiseAndSetIfChanged(ref _overhangColor, value);
                    RaisePropertyChanged(nameof(OverhangBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush OverhangBrush
            {
                get => new(_overhangColor.ToAvalonia());
                set => OverhangColor = new Color(value);
            }

            public Color OverhangHighlightColor
            {
                get => _overhangHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _overhangHighlightColor, value);
                    RaisePropertyChanged(nameof(OverhangHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush OverhangHighlightBrush
            {
                get => new(_overhangHighlightColor.ToAvalonia());
                set => OverhangHighlightColor = new Color(value);
            }

            public Color ResinTrapColor
            {
                get => _resinTrapColor;
                set
                {
                    RaiseAndSetIfChanged(ref _resinTrapColor, value);
                    RaisePropertyChanged(nameof(ResinTrapBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush ResinTrapBrush
            {
                get => new(_resinTrapColor.ToAvalonia());
                set => ResinTrapColor = new Color(value);
            }

            public Color ResinTrapHighlightColor
            {
                get => _resinTrapHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _resinTrapHighlightColor, value);
                    RaisePropertyChanged(nameof(ResinTrapHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush ResinTrapHighlightBrush
            {
                get => new(_resinTrapHighlightColor.ToAvalonia());
                set => ResinTrapHighlightColor = new Color(value);
            }
            public Color TouchingBoundsColor
            {
                get => _touchingBoundsColor;
                set
                {
                    RaiseAndSetIfChanged(ref _touchingBoundsColor, value);
                    RaisePropertyChanged(nameof(TouchingBoundsBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush TouchingBoundsBrush
            {
                get => new(_touchingBoundsColor.ToAvalonia());
                set => TouchingBoundsColor = new Color(value);
            }

            public Color CrosshairColor
            {
                get => _crosshairColor;
                set
                {
                    RaiseAndSetIfChanged(ref _crosshairColor, value);
                    RaisePropertyChanged(nameof(CrosshairBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush CrosshairBrush
            {
                get => new(_crosshairColor.ToAvalonia());
                set => CrosshairColor = new Color(value);
            }

            public bool ZoomToFitPrintVolumeBounds
            {
                get => _zoomToFitPrintVolumeBounds;
                set => RaiseAndSetIfChanged(ref _zoomToFitPrintVolumeBounds, value);
            }

            public byte ZoomLockLevelIndex
            {
                get => _zoomLockLevelIndex;
                set => RaiseAndSetIfChanged(ref _zoomLockLevelIndex, value);
            }

            public bool ZoomIssues
            {
                get => _zoomIssues;
                set => RaiseAndSetIfChanged(ref _zoomIssues, value);
            }

            public bool CrosshairShowOnlyOnSelectedIssues
            {
                get => _crosshairShowOnlyOnSelectedIssues;
                set => RaiseAndSetIfChanged(ref _crosshairShowOnlyOnSelectedIssues, value);
            }

            public byte CrosshairFadeLevelIndex
            {
                get => _crosshairFadeLevelIndex;
                set => RaiseAndSetIfChanged(ref _crosshairFadeLevelIndex, value);
            }

            public uint CrosshairLength
            {
                get => _crosshairLength;
                set => RaiseAndSetIfChanged(ref _crosshairLength, value);
            }

            public byte CrosshairMargin
            {
                get => _crosshairMargin;
                set => RaiseAndSetIfChanged(ref _crosshairMargin, value);
            }

            public bool AutoRotateLayerBestView
            {
                get => _autoRotateLayerBestView;
                set => RaiseAndSetIfChanged(ref _autoRotateLayerBestView, value);
            }

            public bool AutoFlipLayerIfMirrored
            {
                get => _autoFlipLayerIfMirrored;
                set => RaiseAndSetIfChanged(ref _autoFlipLayerIfMirrored, value);
            }

            public bool LayerZoomToFitOnLoad
            {
                get => _layerZoomToFitOnLoad;
                set => RaiseAndSetIfChanged(ref _layerZoomToFitOnLoad, value);
            }

            public bool ShowBackgroudGrid
            {
                get => _showBackgroudGrid;
                set => RaiseAndSetIfChanged(ref _showBackgroudGrid, value);
            }

            public LayerPreviewUserSettings Clone()
            {
                return MemberwiseClone() as LayerPreviewUserSettings;
            }

        }
        #endregion

        #region Issues
        [Serializable]
        public sealed class IssuesUserSettings : BindableBase
        {
            private bool _computeIssuesOnLoad;
            private bool _autoRepairIssuesOnLoad;
            private bool _computeIssuesOnClickTab = true;
            private bool _computeIslands = true;
            private bool _computeOverhangs = true;
            private bool _computeResinTraps = true;
            private bool _computeTouchingBounds = true;
            private bool _computePrintHeight = true;
            private bool _computeEmptyLayers = true;
            private bool _islandEnhancedDetection = true;
            private bool _islandAllowDiagonalBonds;
            private byte _islandBinaryThreshold;
            private byte _islandRequiredAreaToProcessCheck = 1;
            private byte _islandRequiredPixelBrightnessToProcessCheck = 1;
            private byte _islandRequiredPixelsToSupport = 10;
            private decimal _islandRequiredPixelsToSupportMultiplier = 0.25m;
            private byte _islandRequiredPixelBrightnessToSupport = 150;
            private bool _overhangIndependentFromIslands = true;
            private byte _overhangErodeIterations = 49;
            private byte _resinTrapBinaryThreshold = 127;
            private byte _resinTrapRequiredAreaToProcessCheck = 17;
            private byte _resinTrapRequiredBlackPixelsToDrain = 10;
            private byte _resinTrapMaximumPixelBrightnessToDrain = 30;
            private byte _touchingBoundMinimumPixelBrightness = 127;
            private byte _touchingBoundMarginLeft = 5;
            private byte _touchingBoundMarginTop = 5;
            private byte _touchingBoundMarginRight = 5;
            private byte _touchingBoundMarginBottom = 5;
            private bool _touchingBoundSyncMargins = true;
            private decimal _printHeightOffset;

            public bool ComputeIssuesOnLoad
            {
                get => _computeIssuesOnLoad;
                set => RaiseAndSetIfChanged(ref _computeIssuesOnLoad, value);
            }

            public bool AutoRepairIssuesOnLoad
            {
                get => _autoRepairIssuesOnLoad;
                set => RaiseAndSetIfChanged(ref _autoRepairIssuesOnLoad, value);
            }

            public bool ComputeIssuesOnClickTab
            {
                get => _computeIssuesOnClickTab;
                set => RaiseAndSetIfChanged(ref _computeIssuesOnClickTab, value);
            }

            public bool ComputeIslands
            {
                get => _computeIslands;
                set => RaiseAndSetIfChanged(ref _computeIslands, value);
            }

            public bool ComputeOverhangs
            {
                get => _computeOverhangs;
                set => RaiseAndSetIfChanged(ref _computeOverhangs, value);
            }

            public bool ComputeResinTraps
            {
                get => _computeResinTraps;
                set => RaiseAndSetIfChanged(ref _computeResinTraps, value);
            }

            public bool ComputeTouchingBounds
            {
                get => _computeTouchingBounds;
                set => RaiseAndSetIfChanged(ref _computeTouchingBounds, value);
            }

            public bool ComputePrintHeight
            {
                get => _computePrintHeight;
                set => RaiseAndSetIfChanged(ref _computePrintHeight, value);
            }

            public bool ComputeEmptyLayers
            {
                get => _computeEmptyLayers;
                set => RaiseAndSetIfChanged(ref _computeEmptyLayers, value);
            }

            public bool IslandEnhancedDetection
            {
                get => _islandEnhancedDetection;
                set => RaiseAndSetIfChanged(ref _islandEnhancedDetection, value);
            }

            public bool IslandAllowDiagonalBonds
            {
                get => _islandAllowDiagonalBonds;
                set => RaiseAndSetIfChanged(ref _islandAllowDiagonalBonds, value);
            }

            public byte IslandBinaryThreshold
            {
                get => _islandBinaryThreshold;
                set => RaiseAndSetIfChanged(ref _islandBinaryThreshold, value);
            }

            public byte IslandRequiredAreaToProcessCheck
            {
                get => _islandRequiredAreaToProcessCheck;
                set => RaiseAndSetIfChanged(ref _islandRequiredAreaToProcessCheck, value);
            }

            public decimal IslandRequiredPixelsToSupportMultiplier
            {
                get => _islandRequiredPixelsToSupportMultiplier;
                set => RaiseAndSetIfChanged(ref _islandRequiredPixelsToSupportMultiplier, value);
            }

            public byte IslandRequiredPixelsToSupport
            {
                get => _islandRequiredPixelsToSupport;
                set => RaiseAndSetIfChanged(ref _islandRequiredPixelsToSupport, value);
            }
            
            public byte IslandRequiredPixelBrightnessToProcessCheck
            {
                get => _islandRequiredPixelBrightnessToProcessCheck;
                set => RaiseAndSetIfChanged(ref _islandRequiredPixelBrightnessToProcessCheck, value);
            }

            public byte IslandRequiredPixelBrightnessToSupport
            {
                get => _islandRequiredPixelBrightnessToSupport;
                set => RaiseAndSetIfChanged(ref _islandRequiredPixelBrightnessToSupport, value);
            }

            public bool OverhangIndependentFromIslands
            {
                get => _overhangIndependentFromIslands;
                set => RaiseAndSetIfChanged(ref _overhangIndependentFromIslands, value);
            }

            public byte OverhangErodeIterations
            {
                get => _overhangErodeIterations;
                set => RaiseAndSetIfChanged(ref _overhangErodeIterations, value);
            }
            
            public byte ResinTrapBinaryThreshold
            {
                get => _resinTrapBinaryThreshold;
                set => RaiseAndSetIfChanged(ref _resinTrapBinaryThreshold, value);
            }

            public byte ResinTrapRequiredAreaToProcessCheck
            {
                get => _resinTrapRequiredAreaToProcessCheck;
                set => RaiseAndSetIfChanged(ref _resinTrapRequiredAreaToProcessCheck, value);
            }

            public byte ResinTrapRequiredBlackPixelsToDrain
            {
                get => _resinTrapRequiredBlackPixelsToDrain;
                set => RaiseAndSetIfChanged(ref _resinTrapRequiredBlackPixelsToDrain, value);
            }

            public byte ResinTrapMaximumPixelBrightnessToDrain
            {
                get => _resinTrapMaximumPixelBrightnessToDrain;
                set => RaiseAndSetIfChanged(ref _resinTrapMaximumPixelBrightnessToDrain, value);
            }

            public byte TouchingBoundMinimumPixelBrightness
            {
                get => _touchingBoundMinimumPixelBrightness;
                set => RaiseAndSetIfChanged(ref _touchingBoundMinimumPixelBrightness, value);
            }

            public byte TouchingBoundMarginLeft
            {
                get => _touchingBoundMarginLeft;
                set
                {
                    if(!RaiseAndSetIfChanged(ref _touchingBoundMarginLeft, value)) return;
                    if (_touchingBoundSyncMargins)
                    {
                        TouchingBoundMarginRight = value;
                    }
                }
            }

            public byte TouchingBoundMarginTop
            {
                get => _touchingBoundMarginTop;
                set
                {
                    if (!RaiseAndSetIfChanged(ref _touchingBoundMarginTop, value)) return;
                    if (_touchingBoundSyncMargins)
                    {
                        TouchingBoundMarginBottom = value;
                    }
                }
            }

            public byte TouchingBoundMarginRight
            {
                get => _touchingBoundMarginRight;
                set
                {
                    if(!RaiseAndSetIfChanged(ref _touchingBoundMarginRight, value)) return;
                    if (_touchingBoundSyncMargins)
                    {
                        TouchingBoundMarginLeft = value;
                    }
                }
            }

            public byte TouchingBoundMarginBottom
            {
                get => _touchingBoundMarginBottom;
                set
                {
                    if(!RaiseAndSetIfChanged(ref _touchingBoundMarginBottom, value)) return;
                    if (_touchingBoundSyncMargins)
                    {
                        TouchingBoundMarginTop = value;
                    }
                }
            }

            public bool TouchingBoundSyncMargins
            {
                get => _touchingBoundSyncMargins;
                set => RaiseAndSetIfChanged(ref _touchingBoundSyncMargins, value);
            }

            public decimal PrintHeightOffset
            {
                get => _printHeightOffset;
                set => RaiseAndSetIfChanged(ref _printHeightOffset, value);
            }

            public IssuesUserSettings Clone()
            {
                return MemberwiseClone() as IssuesUserSettings;
            }

        }
        #endregion

        #region Pixel Editor
        [Serializable]
        public sealed class PixelEditorUserSettings : BindableBase
        {
            private Color _addPixelColor = new(255, 144, 238, 144);
            private Color _addPixelHighlightColor = new(255, 0, 255, 0);
            private Color _removePixelColor = new(255, 219, 112, 147);
            private Color _removePixelHighlightColor = new(255, 139, 0, 0);
            private Color _supportsColor = new(255, 0, 255, 255);
            private Color _supportsHighlightColor = new(255, 0, 139, 139);
            private Color _drainHoleColor = new(255, 142, 69, 133);
            private Color _drainHoleHighlightColor = new(255, 159, 0, 197);
            private Color _cursorColor = new(150, 52, 152, 219);
            private bool _partialUpdateIslandsOnEditing = true;
            private bool _closeEditorOnApply;

            public Color AddPixelColor
            {
                get => _addPixelColor;
                set
                {
                    RaiseAndSetIfChanged(ref _addPixelColor, value);
                    RaisePropertyChanged(nameof(AddPixelBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush AddPixelBrush
            {
                get => new(_addPixelColor.ToAvalonia());
                set => AddPixelColor = new Color(value);
            }

            public Color AddPixelHighlightColor
            {
                get => _addPixelHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _addPixelHighlightColor, value);
                    RaisePropertyChanged(nameof(AddPixelHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush AddPixelHighlightBrush
            {
                get => new(_addPixelHighlightColor.ToAvalonia());
                set => AddPixelHighlightColor = new Color(value);
            }

            public Color RemovePixelColor
            {
                get => _removePixelColor;
                set
                {
                    RaiseAndSetIfChanged(ref _removePixelColor, value);
                    RaisePropertyChanged(nameof(RemovePixelBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush RemovePixelBrush
            {
                get => new(_removePixelColor.ToAvalonia());
                set => RemovePixelColor = new Color(value);
            }

            public Color RemovePixelHighlightColor
            {
                get => _removePixelHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _removePixelHighlightColor, value);
                    RaisePropertyChanged(nameof(RemovePixelHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush RemovePixelHighlightBrush
            {
                get => new(_removePixelHighlightColor.ToAvalonia());
                set => RemovePixelHighlightColor = new Color(value);
            }

            public Color SupportsColor
            {
                get => _supportsColor;
                set
                {
                    RaiseAndSetIfChanged(ref _supportsColor, value);
                    RaisePropertyChanged(nameof(SupportsBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush SupportsBrush
            {
                get => new(_supportsColor.ToAvalonia());
                set => SupportsColor = new Color(value);
            }

            public Color SupportsHighlightColor
            {
                get => _supportsHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _supportsHighlightColor, value);
                    RaisePropertyChanged(nameof(SupportsHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush SupportsHighlightBrush
            {
                get => new(_supportsHighlightColor.ToAvalonia());
                set => SupportsHighlightColor = new Color(value);
            }

            public Color DrainHoleColor
            {
                get => _drainHoleColor;
                set
                {
                    RaiseAndSetIfChanged(ref _drainHoleColor, value);
                    RaisePropertyChanged(nameof(DrainHoleBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush DrainHoleBrush
            {
                get => new(_drainHoleColor.ToAvalonia());
                set => DrainHoleColor = new Color(value);
            }

            public Color DrainHoleHighlightColor
            {
                get => _drainHoleHighlightColor;
                set
                {
                    RaiseAndSetIfChanged(ref _drainHoleHighlightColor, value);
                    RaisePropertyChanged(nameof(DrainHoleHighlightBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush DrainHoleHighlightBrush
            {
                get => new(_drainHoleHighlightColor.ToAvalonia());
                set => DrainHoleHighlightColor = new Color(value);
            }

            public Color CursorColor
            {
                get => _cursorColor;
                set
                {
                    RaiseAndSetIfChanged(ref _cursorColor, value);
                    RaisePropertyChanged(nameof(CursorBrush));
                }
            }

            [XmlIgnore]
            public SolidColorBrush CursorBrush
            {
                get => new(_cursorColor.ToAvalonia());
                set => CursorColor = new Color(value);
            }

            public bool PartialUpdateIslandsOnEditing
            {
                get => _partialUpdateIslandsOnEditing;
                set => RaiseAndSetIfChanged(ref _partialUpdateIslandsOnEditing, value);
            }

            public bool CloseEditorOnApply
            {
                get => _closeEditorOnApply;
                set => RaiseAndSetIfChanged(ref _closeEditorOnApply, value);
            }

            public PixelEditorUserSettings Clone()
            {
                return MemberwiseClone() as PixelEditorUserSettings;
            }
        }
        #endregion

        #region Layer Repair
        [Serializable]
        public sealed class LayerRepairUserSettings : BindableBase
        {
            private bool _repairIslands = true;
            private bool _repairResinTraps = true;
            private bool _removeEmptyLayers = true;
            private ushort _removeIslandsBelowEqualPixels = 5;
            private ushort _removeIslandsRecursiveIterations = 4;
            private ushort _attachIslandsBelowLayers = 2;
            private byte _closingIterations = 2;
            private byte _openingIterations = 0;

            public bool RepairIslands
            {
                get => _repairIslands;
                set => RaiseAndSetIfChanged(ref _repairIslands, value);
            }

            public bool RepairResinTraps
            {
                get => _repairResinTraps;
                set => RaiseAndSetIfChanged(ref _repairResinTraps, value);
            }

            public bool RemoveEmptyLayers
            {
                get => _removeEmptyLayers;
                set => RaiseAndSetIfChanged(ref _removeEmptyLayers, value);
            }

            public ushort RemoveIslandsBelowEqualPixels
            {
                get => _removeIslandsBelowEqualPixels;
                set => RaiseAndSetIfChanged(ref _removeIslandsBelowEqualPixels, value);
            }

            public ushort RemoveIslandsRecursiveIterations
            {
                get => _removeIslandsRecursiveIterations;
                set => RaiseAndSetIfChanged(ref _removeIslandsRecursiveIterations, value);
            }

            public ushort AttachIslandsBelowLayers
            {
                get => _attachIslandsBelowLayers;
                set => RaiseAndSetIfChanged(ref _attachIslandsBelowLayers, value);
            }

            public byte ClosingIterations
            {
                get => _closingIterations;
                set => RaiseAndSetIfChanged(ref _closingIterations, value);
            }

            public byte OpeningIterations
            {
                get => _openingIterations;
                set => RaiseAndSetIfChanged(ref _openingIterations, value);
            }

            public LayerRepairUserSettings Clone()
            {
                return MemberwiseClone() as LayerRepairUserSettings;
            }
        }
        #endregion

        #region Tools

        [Serializable]
        public sealed class ToolsUserSettings : BindableBase
        {
            private bool _restoreLastUsedSettings;
            private bool _lastUsedSettingsKeepOnCloseFile = true;
            private bool _lastUsedSettingsPriorityOverDefaultProfile = true;

            public bool RestoreLastUsedSettings
            {
                get => _restoreLastUsedSettings;
                set => RaiseAndSetIfChanged(ref _restoreLastUsedSettings, value);
            }

            public bool LastUsedSettingsKeepOnCloseFile
            {
                get => _lastUsedSettingsKeepOnCloseFile;
                set => RaiseAndSetIfChanged(ref _lastUsedSettingsKeepOnCloseFile, value);
            }

            public bool LastUsedSettingsPriorityOverDefaultProfile
            {
                get => _lastUsedSettingsPriorityOverDefaultProfile;
                set => RaiseAndSetIfChanged(ref _lastUsedSettingsPriorityOverDefaultProfile, value);
            }
        }

        #endregion

        #region Automations

        [Serializable]
        public sealed class AutomationsUserSettings : BindableBase
        {
            private bool _saveFileAfterModifications = true;
            private bool _autoConvertFiles = true;
            private Enumerations.LightOffDelaySetMode _lightOffDelayDelaySetMode = Enumerations.LightOffDelaySetMode.UpdateWithExtraDelay;
            private bool _changeOnlyLightOffDelayIfZero = true;
            private decimal _lightOffDelay = 2.5m;
            private decimal _bottomLightOffDelay = 3m;

            public bool SaveFileAfterModifications
            {
                get => _saveFileAfterModifications;
                set => RaiseAndSetIfChanged(ref _saveFileAfterModifications, value);
            }

            public bool AutoConvertFiles
            {
                get => _autoConvertFiles;
                set => RaiseAndSetIfChanged(ref _autoConvertFiles, value);
            }

            public Enumerations.LightOffDelaySetMode LightOffDelaySetMode
            {
                get => _lightOffDelayDelaySetMode;
                set
                {
                    if(!RaiseAndSetIfChanged(ref _lightOffDelayDelaySetMode, value)) return;
                    RaisePropertyChanged(nameof(ChangeOnlyLightOffDelayIfZeroVisible));
                    RaisePropertyChanged(nameof(LightOffDelayExtraTimeVisible));
                }
            }

            public bool ChangeOnlyLightOffDelayIfZero
            {
                get => _changeOnlyLightOffDelayIfZero;
                set => RaiseAndSetIfChanged(ref _changeOnlyLightOffDelayIfZero, value);
            }

            public bool ChangeOnlyLightOffDelayIfZeroVisible =>
                _lightOffDelayDelaySetMode 
                    is not Enumerations.LightOffDelaySetMode.NoAction 
                    and not Enumerations.LightOffDelaySetMode.SetToZero;


            public decimal LightOffDelay
            {
                get => _lightOffDelay;
                set => RaiseAndSetIfChanged(ref _lightOffDelay, value);
            }

            public decimal BottomLightOffDelay
            {
                get => _bottomLightOffDelay;
                set => RaiseAndSetIfChanged(ref _bottomLightOffDelay, value);
            }

            public bool LightOffDelayExtraTimeVisible => _lightOffDelayDelaySetMode
                is Enumerations.LightOffDelaySetMode.UpdateWithExtraDelay;
        }

        #endregion

        #endregion

        #region Singleton

        public static string SettingsFolder
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), About.Software);
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

        /// <summary>
        /// Default filepath for store <see cref="UserSettings"/>
        /// </summary>
        private static string FilePath => Path.Combine(SettingsFolder, "usersettings.xml");


        private static UserSettings _instance;
        /// <summary>
        /// Instance of <see cref="UserSettings"/> (singleton)
        /// </summary>
        public static UserSettings Instance
        {
            get => _instance ??= new UserSettings();
            internal set => _instance = value;
        }
        #endregion

        #region Properties

        private GeneralUserSettings _general;
        private LayerPreviewUserSettings _layerPreview;
        private IssuesUserSettings _issues;
        private PixelEditorUserSettings _pixelEditor;
        private LayerRepairUserSettings _layerRepair;
        private ToolsUserSettings _tools;
        private AutomationsUserSettings _automations;
        private ushort _settingsVersion = SETTINGS_VERSION;
        private string _appVersion;
        private uint _savesCount;
        private DateTime _modifiedDateTime;


        public GeneralUserSettings General
        {
            get => _general ??= new GeneralUserSettings();
            set => _general = value;
        }

        public LayerPreviewUserSettings LayerPreview
        {
            get => _layerPreview ??= new LayerPreviewUserSettings();
            set => _layerPreview = value;
        }

        
        public IssuesUserSettings Issues
        {
            get => _issues ??= new IssuesUserSettings();
            set => _issues = value;
        }

        
        public PixelEditorUserSettings PixelEditor
        {
            get => _pixelEditor ??= new PixelEditorUserSettings();
            set => _pixelEditor = value;
        }



        public LayerRepairUserSettings LayerRepair
        {
            get => _layerRepair ??= new LayerRepairUserSettings();
            set => _layerRepair = value;
        }

        public ToolsUserSettings Tools
        {
            get => _tools ??= new ToolsUserSettings();
            set => _tools = value;
        }

        public AutomationsUserSettings Automations
        {
            get => _automations ??= new AutomationsUserSettings();
            set => _automations = value;
        }

        /*
        /// <summary>
        /// Gets or sets the number of times this file has been reset to defaults
        /// </summary>
        public uint ResetCount { get; set; }
        */

        public ushort SettingsVersion
        {
            get => _settingsVersion;
            set => RaiseAndSetIfChanged(ref _settingsVersion,  value);
        }

        /// <summary>
        /// Gets or sets the last running version of UVtools with these settings
        /// </summary>
        [NotNull]
        public string AppVersion
        {
            get => _appVersion ??= App.Version.ToString();
            set => RaiseAndSetIfChanged(ref _appVersion, value);
        }

        /// <summary>
        /// Gets or sets the number of times this file has been saved
        /// </summary>
        public uint SavesCount
        {
            get => _savesCount;
            set => RaiseAndSetIfChanged(ref _savesCount, value);
        }

        /// <summary>
        /// Gets or sets the last time this file has been modified
        /// </summary>
        public DateTime ModifiedDateTime
        {
            get => _modifiedDateTime;
            set => RaiseAndSetIfChanged(ref _modifiedDateTime, value);
        }

        #endregion

        #region Constructor

        private UserSettings()
        {
            if (OperatingSystem.IsMacOS()) // Fix macOS scaling information
            {
                var monjave = new Version(10, 14, 6);
                if (Environment.OSVersion.Version.CompareTo(monjave) >= 0)
                {
                    General.WindowsTakeIntoAccountScreenScaling = false;
                }
            }
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Reset settings to defaults
        /// </summary>
        /// <param name="save">True to save settings on file, otherwise false</param>
        public static void Reset(bool save = false)
        {
            _instance = new UserSettings();
            if (save) Save();
        }

        /// <summary>
        /// Load settings from file
        /// </summary>
        public static void Load()
        {
            if (!File.Exists(FilePath))
            {
                return;
            }

            var serializer = new XmlSerializer(typeof(UserSettings));
            try
            {
                using var myXmlReader = new StreamReader(FilePath);
                _instance = (UserSettings)serializer.Deserialize(myXmlReader);
                if (_instance.General.MaxDegreeOfParallelism <= 0)
                {
                    _instance.General.MaxDegreeOfParallelism = -1;
                }
                else
                {
                    _instance.General.MaxDegreeOfParallelism = Math.Min(_instance.General.MaxDegreeOfParallelism, Environment.ProcessorCount);
                }
                    

                if (_instance.SettingsVersion < SETTINGS_VERSION)
                {
                    // Upgrade
                    if (_instance.SettingsVersion <= 4)
                    {
                        _instance.General.MaxDegreeOfParallelism = -1;
                    }
                    _instance.SettingsVersion = SETTINGS_VERSION;
                }

                CoreSettings.MaxDegreeOfParallelism = _instance.General.MaxDegreeOfParallelism;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Reset();
            }
        }


        /// <summary>
        /// Save settings to file
        /// </summary>
        public static void Save()
        {
            Instance.SavesCount++;
            _instance.ModifiedDateTime = DateTime.Now;
            CoreSettings.MaxDegreeOfParallelism = _instance.General.MaxDegreeOfParallelism;
            var serializer = new XmlSerializer(_instance.GetType());
            try
            {
                using var myXmlWriter = new StreamWriter(FilePath);
                serializer.Serialize(myXmlWriter, _instance);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public static void SetVersion()
        {
            Instance.AppVersion = App.Version.ToString();
        }

        public static object[] PackObjects => 
            new object[]
            {
                Instance.General,
                Instance.LayerPreview,
                Instance.Issues,
                Instance.PixelEditor,
                Instance.LayerRepair,
                Instance.Automations
            };
        #endregion

        #region Methods

        public UserSettings Clone()
        {
            var clone = MemberwiseClone() as UserSettings;
            clone.General = clone.General.Clone();
            clone.LayerPreview = clone.LayerPreview.Clone();
            clone.Issues = clone.Issues.Clone();
            clone.PixelEditor = clone.PixelEditor.Clone();
            clone.LayerRepair = clone.LayerRepair.Clone();
            return clone;
        }

        #endregion
    }
}
