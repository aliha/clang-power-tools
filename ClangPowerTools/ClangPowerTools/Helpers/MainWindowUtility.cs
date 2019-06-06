using ClangPowerTools.Services;
using EnvDTE;
using System;

namespace ClangPowerTools.Helpers
{
  public class MainWindowUtility
  {
    public static IntPtr GetPointer()
    {
      var dte = (DTE)VsServiceProvider.GetService(typeof(DTE));
      return dte.MainWindow != null ? (IntPtr)dte.MainWindow.HWnd : IntPtr.Zero;
    }
  }
}
