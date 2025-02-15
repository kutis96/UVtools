﻿using Avalonia.Markup.Xaml;
using UVtools.Core.Operations;
using UVtools.WPF.Windows;

namespace UVtools.WPF.Controls.Tools;

public class ToolMoveControl : ToolControl
{
    private bool _isMiddleCenterChecked = true;
    public OperationMove Operation => BaseOperation as OperationMove;

    public bool IsMiddleCenterChecked
    {
        get => _isMiddleCenterChecked;
        set => RaiseAndSetIfChanged(ref _isMiddleCenterChecked, value);
    }

    public ToolMoveControl()
    {
        BaseOperation = new OperationMove(SlicerFile);
        if (!ValidateSpawn()) return;
        InitializeComponent();
            

        Operation.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName.Equals(nameof(Operation.IsWithinBoundary)))
            {
                ParentWindow.ButtonOkEnabled = Operation.IsWithinBoundary;
            }
        };
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void Callback(ToolWindow.Callbacks callback)
    {
        switch (callback)
        {
            case ToolWindow.Callbacks.Init:
            case ToolWindow.Callbacks.AfterLoadProfile:
                Operation.ROI = App.MainWindow.ROI.IsEmpty ? SlicerFile.BoundingRectangle : App.MainWindow.ROI;
                break;
            case ToolWindow.Callbacks.ClearROI:
                Operation.ROI = SlicerFile.BoundingRectangle;
                Operation.Reset();
                break;
        }
    }
}