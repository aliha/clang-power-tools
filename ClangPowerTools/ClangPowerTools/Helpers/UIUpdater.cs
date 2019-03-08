using Microsoft.VisualStudio.Shell;
using System;

namespace ClangPowerTools.Handlers
{
  public class UIUpdater
  {
    #region Public Methods

    public async static System.Threading.Tasks.Task BeginInvokeAsync(Action aAction)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      aAction.BeginInvoke(aAction.EndInvoke, null);
    }

    public async static System.Threading.Tasks.Task InvokeAsync(Action aAction)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
      aAction.Invoke();
    }

    #endregion

  }
}
