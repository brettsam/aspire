// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;

namespace Aspire.Hosting.ApplicationModel;

#pragma warning disable RS0016 // Add public types and members to the declared API
/// <summary>
/// Represents a command annotation for a resource.
/// </summary>
[DebuggerDisplay("Type = {GetType().Name,nq}, Source = {Source}, Target = {Target}")]
public sealed class ResourceCommandAnnotation : IResourceAnnotation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceCommandAnnotation"/> class.
    /// </summary>
    public ResourceCommandAnnotation(string type, string displayName, Func<CustomResourceSnapshot, Task<bool>> visible, Func<string, IServiceProvider, Task> invokeCommand, string? iconContent, bool isHighlighted)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(displayName);

        Type = type;
        DisplayName = displayName;
        Visible = visible;
        InvokeCommand = invokeCommand;
        IconContent = iconContent;
        IsHighlighted = isHighlighted;
    }

    /// <summary>
    /// Gets the source of the bind mount or name if a volume. Can be <c>null</c> if the mount is an anonymous volume.
    /// </summary>
    public string Type { get; }

    /// <summary>
    /// Gets the target of the mount.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// 
    /// </summary>
    public Func<CustomResourceSnapshot, Task<bool>> Visible { get; }

    /// <summary>
    /// 
    /// </summary>
    public Func<string, IServiceProvider, Task> InvokeCommand { get; }

    /// <summary>
    /// 
    /// </summary>
    public string? IconContent { get; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsHighlighted { get; }
}
#pragma warning restore RS0016 // Add public types and members to the declared API
