// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics;
using Aspire.Dashboard.Extensions;
using Google.Protobuf.WellKnownTypes;

namespace Aspire.Dashboard.Model;

[DebuggerDisplay("Name = {Name}, ResourceType = {ResourceType}, State = {State}, Properties = {Properties.Count}")]
public sealed class ResourceViewModel
{
    public required string Name { get; init; }
    public required string ResourceType { get; init; }
    public required string DisplayName { get; init; }
    public required string Uid { get; init; }
    public required string? State { get; init; }
    public required string? StateStyle { get; init; }
    public required DateTime? CreationTimeStamp { get; init; }
    public required ImmutableArray<EnvironmentVariableViewModel> Environment { get; init; }
    public required ImmutableArray<UrlViewModel> Urls { get; init; }
    public required FrozenDictionary<string, Value> Properties { get; init; }
    public required ImmutableArray<CommandViewModel> Commands { get; init; }
    public KnownResourceState? KnownState { get; init; }

    internal bool MatchesFilter(string filter)
    {
        // TODO let ResourceType define the additional data values we include in searches
        return Name.Contains(filter, StringComparisons.UserTextSearch);
    }

    public static string GetResourceName(ResourceViewModel resource, IDictionary<string, ResourceViewModel> allResources)
    {
        var count = 0;
        foreach (var (_, item) in allResources)
        {
            if (item.IsHiddenState())
            {
                continue;
            }

            if (item.DisplayName == resource.DisplayName)
            {
                count++;
                if (count >= 2)
                {
                    // There are multiple resources with the same display name so they're part of a replica set.
                    // Need to use the name which has a unique ID to tell them apart.
                    return resource.Name;
                }
            }
        }

        return resource.DisplayName;
    }
}

[DebuggerDisplay("CommandType = {CommandType}, DisplayName = {DisplayName}")]
public sealed class CommandViewModel
{
    public string CommandType { get; }
    public CommandViewModelState State { get; }
    public string DisplayName { get; }
    public string? ConfirmationMessage { get; }
    public Value? Parameter { get; }
    public bool IsHighlighted { get; }
    public string? IconContent { get; }

    public CommandViewModel(string commandType, CommandViewModelState state, string displayName, string? confirmationMessage, Value? parameter, bool isHighlighted, string? iconContent)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandType);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        CommandType = commandType;
        State = state;
        DisplayName = displayName;
        ConfirmationMessage = confirmationMessage;
        Parameter = parameter;
        IsHighlighted = isHighlighted;
        IconContent = iconContent;
    }
}

public enum CommandViewModelState
{
    Enabled,
    Disabled,
    Hidden
}

[DebuggerDisplay("Name = {Name}, Value = {Value}, FromSpec = {FromSpec}, IsValueMasked = {IsValueMasked}")]
public sealed class EnvironmentVariableViewModel
{
    public string Name { get; }
    public string? Value { get; }
    public bool FromSpec { get; }

    public bool IsValueMasked { get; set; } = true;

    public EnvironmentVariableViewModel(string name, string? value, bool fromSpec)
    {
        // Name should always have a value, but somehow an empty/whitespace name can reach this point.
        // Better to allow the dashboard to run with an env var with no name than break when loading resources.
        // https://github.com/dotnet/aspire/issues/5309
        Name = name;
        Value = value;
        FromSpec = fromSpec;
    }
}

[DebuggerDisplay("Name = {Name}, Url = {Url}, IsInternal = {IsInternal}")]
public sealed class UrlViewModel
{
    public string Name { get; }
    public Uri Url { get; }
    public bool IsInternal { get; }

    public UrlViewModel(string name, Uri url, bool isInternal)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(url);

        Name = name;
        Url = url;
        IsInternal = isInternal;
    }
}
