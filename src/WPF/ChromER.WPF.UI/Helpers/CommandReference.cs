using System;
using System.Windows;
using System.Windows.Input;

namespace ChromER.WPF.UI
{
    public class CommandReference : Freezable, ICommand
    {
        #region Dependency Properties

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(CommandReference),
            new PropertyMetadata(OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(
            "CommandParameter", typeof(object), typeof(CommandReference),
            new PropertyMetadata(default(object)));

        #endregion

        #region Public Properties

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }


        public object CommandParameter
        {
            get => (object) GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion
        
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            var param = CommandParameter ?? parameter;

            if (Command != null)
                return Command.CanExecute(param);
            return false;
        }

        public void Execute(object parameter)
        {
            var param = CommandParameter ?? parameter;

            Command.Execute(param);
        }

        public event EventHandler CanExecuteChanged;

        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandReference commandReference = d as CommandReference;
            ICommand oldCommand = e.OldValue as ICommand;
            ICommand newCommand = e.NewValue as ICommand;

            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= commandReference.CanExecuteChanged;
            }

            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += commandReference.CanExecuteChanged;
            }
        }

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}