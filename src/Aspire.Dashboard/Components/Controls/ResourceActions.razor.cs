// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Dashboard.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Aspire.Dashboard.Components;

public partial class ResourceActions : ComponentBase
{
    private static readonly Icon s_viewDetailsIcon = new Icons.Regular.Size20.Info();
    private static readonly Icon s_consoleLogsIcon = new Icons.Regular.Size20.SlideText();

    private AspireMenuButton? _menuButton;

    [Inject]
    public required IStringLocalizer<Resources.Resources> Loc { get; set; }

    [Parameter]
    public required IList<CommandViewModel> Commands { get; set; }

    [Parameter]
    public EventCallback<CommandViewModel> CommandSelected { get; set; }

    [Parameter]
    public EventCallback<string> OnViewDetails { get; set; }

    [Parameter]
    public EventCallback OnConsoleLogs { get; set; }

    [Parameter]
    public string? MenuButtonId { get; set; }

    private readonly List<MenuButtonItem> _menuItems = new();

    protected override void OnParametersSet()
    {
        _menuItems.Clear();

        _menuItems.Add(new MenuButtonItem
        {
            Title = Loc[nameof(Resources.Resources.ResourceActionViewDetailsText)],
            Icon = s_viewDetailsIcon.Content,
            OnClick = () => OnViewDetails.InvokeAsync(_menuButton?.MenuButtonId)
        });
        _menuItems.Add(new MenuButtonItem
        {
            Title = Loc[nameof(Resources.Resources.ResourceActionConsoleLogsText)],
            Icon = s_consoleLogsIcon.Content,
            OnClick = OnConsoleLogs.InvokeAsync
        });

        var menuCommands = Commands.Where(c => !c.IsHighlighted && c.State != CommandViewModelState.Hidden).ToList();
        if (menuCommands.Count > 0)
        {
            _menuItems.Add(new MenuButtonItem { IsDivider = true });

            foreach (var command in menuCommands)
            {
                _menuItems.Add(new MenuButtonItem
                {
                    Title = command.DisplayName,
                    Icon = command.IconContent,
                    OnClick = () => CommandSelected.InvokeAsync(command)
                });
            }
        }
    }
}
