using System.Diagnostics;
using System.Windows.Input;

namespace EPIC.ClearView.Utilities.Commands
{
    // Token: 0x0200006B RID: 107
    public class RelayCommand : ICommand
    {
        // Token: 0x0600033A RID: 826 RVA: 0x0001ADDC File Offset: 0x00018FDC
        public RelayCommand(Action<object> execute, Predicate<object>? canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            this._execute = execute;
            this._canExecute = canExecute;
        }

        // Token: 0x0600033B RID: 827 RVA: 0x0001AE18 File Offset: 0x00019018
        [DebuggerStepThrough]
        public bool CanExecute(object? parameter)
        {
            return this._canExecute == null || this._canExecute(parameter);
        }

        // Token: 0x1400000B RID: 11
        // (add) Token: 0x0600033C RID: 828 RVA: 0x0001AE42 File Offset: 0x00019042
        // (remove) Token: 0x0600033D RID: 829 RVA: 0x0001AE4C File Offset: 0x0001904C
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        // Token: 0x0600033E RID: 830 RVA: 0x0001AE56 File Offset: 0x00019056
        public void Execute(object? parameter)
        {
            this._execute(parameter);
        }

        // Token: 0x04000187 RID: 391
        private readonly Action<object> _execute;

        // Token: 0x04000188 RID: 392
        private readonly Predicate<object> _canExecute;
    }
}
