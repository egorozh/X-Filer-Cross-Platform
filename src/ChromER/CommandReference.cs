using Avalonia;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace ChromER
{
    public class CommandReference : AvaloniaObject, ICommand
    {
        #region Dependency Properties

        public static readonly StyledProperty<ICommand> CommandProperty = AvaloniaProperty.Register
            <CommandReference, ICommand>(nameof(Command));
        
        public static readonly StyledProperty<object> CommandParameterProperty = AvaloniaProperty.Register
            <CommandReference, object>(nameof(CommandParameter));
        
        #endregion

        #region Public Properties

        public ICommand? Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        #region ICommand Members

        public bool CanExecute(object? parameter)
            => Command != null && Command.CanExecute(GetParameter(parameter, CommandParameter));

        public void Execute(object? parameter)
            => Command?.Execute(GetParameter(parameter, CommandParameter));

        public event EventHandler? CanExecuteChanged;

        #endregion

        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == CommandProperty)
            {
                var oldCommand = e.OldValue.Value as ICommand;
                var newCommand = e.NewValue.Value as ICommand;

                OnCommandChanged(oldCommand, newCommand);
            }
        }

        #region Private Methods

        private static object? GetParameter(object? mainParameter, object? referenceParameter)
        {
            if (mainParameter == null)
                return referenceParameter;

            if (referenceParameter == null)
                return mainParameter;

            List<object> mergesParameters = new();

            if (referenceParameter is object[] arrayParams2)
                mergesParameters.AddRange(arrayParams2);
            else
                mergesParameters.Add(referenceParameter);

            if (mainParameter is object[] arrayParams)
                mergesParameters.AddRange(arrayParams);
            else
                mergesParameters.Add(mainParameter);

            return mergesParameters.ToArray();
        }

        private void OnCommandChanged(ICommand? oldCommand, ICommand? newCommand)
        {
            if (oldCommand != null)
                oldCommand.CanExecuteChanged -= OldCommandOnCanExecuteChanged;

            if (newCommand != null)
                newCommand.CanExecuteChanged += OldCommandOnCanExecuteChanged;
        }

        private void OldCommandOnCanExecuteChanged(object? sender, EventArgs e) => RaiseCanExecuteChanged();

        private void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        #endregion

        #region Freezable

        //protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

        #endregion
    }
}