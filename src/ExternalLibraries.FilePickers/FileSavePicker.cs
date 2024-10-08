﻿using ExternalLibraries.Pickers.Classes;
using ExternalLibraries.Pickers.Enums;

namespace ExternalLibraries.Pickers;

/// <summary>
/// Class responsible for file pick dialog.
/// </summary>
public class FileSavePicker
{
    /// <summary>
    /// Window handle where dialog should appear.
    /// </summary>
    private readonly IntPtr _windowHandle;

    /// <summary>
    /// File pick dialog.
    /// </summary>
    /// <param name="windowHandle">Window handle where dialog should appear.</param>
    public FileSavePicker(IntPtr windowHandle)
    {
        _windowHandle = windowHandle;
    }

    /// <summary>
    /// Shows file pick dialog.
    /// </summary>
    /// <param name="typeFilters">List of extensions applied on dialog.</param>
    /// <param name="defaultName">Default name for the file.</param>
    /// <returns>Path to selected file or empty string.</returns>
    public string Show(List<string>? typeFilters = null, string defaultName = "")
    {
        FOS options = FOS.FOS_FORCEFILESYSTEM | FOS.FOS_OVERWRITEPROMPT;
        return Helper.ShowSave(_windowHandle, options, typeFilters, defaultName);
    }
}
