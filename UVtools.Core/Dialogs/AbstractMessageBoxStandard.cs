﻿/*
 *                     GNU AFFERO GENERAL PUBLIC LICENSE
 *                       Version 3, 19 November 2007
 *  Copyright (C) 2007 Free Software Foundation, Inc. <https://fsf.org/>
 *  Everyone is permitted to copy and distribute verbatim copies
 *  of this license document, but changing it is not allowed.
 */

using System;
using System.Threading.Tasks;

namespace UVtools.Core.Dialogs;

public abstract class AbstractMessageBoxStandard
{
    public enum MessageButtons
    {
        Ok,
        YesNo,
        OkCancel,
        OkAbort,
        YesNoCancel,
        YesNoAbort,
    }

    [Flags]
    public enum MessageButtonResult
    {
        Ok = 0,
        Yes = 1,
        No = 2,
        Abort = No | Yes, // 0x00000003
        Cancel = 4,
        None = Cancel | Yes, // 0x00000005
    }

    protected AbstractMessageBoxStandard()
    { }

    public abstract Task<MessageButtonResult> ShowDialog(string? title, string message, MessageButtons buttons = MessageButtons.Ok);
    public Task<MessageButtonResult> ShowDialog(string message, MessageButtons buttons = MessageButtons.Ok) => ShowDialog(null, message, buttons);
}