using System;
using System.Security;
using System.Windows;
using System.Windows.Input;

namespace ThemesLib.InStat.Helpers
{
    internal static class CommandHelpers
    {
        internal static bool CanExecuteCommandSource(ICommandSource commandSource)
        {
            ICommand command = commandSource.Command;
            if (command == null)
                return false;
            object commandParameter = commandSource.CommandParameter;
            IInputElement target = commandSource.CommandTarget;
            RoutedCommand routedCommand = command as RoutedCommand;
            if (routedCommand == null)
                return command.CanExecute(commandParameter);
            if (target == null)
                target = commandSource as IInputElement;
            return routedCommand.CanExecute(commandParameter, target);
        }

    }
}