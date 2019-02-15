using ClangPowerTools.Services;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.IO;

namespace ClangPowerTools
{
  public class DocumentsHandler
  {
    #region Public Methods

    /// <summary>
    /// Get active documents
    /// </summary>
    /// <returns>Active documents</returns>
    public static Documents GetActiveDocuments()
    {
      return VsServiceProvider.TryGetService(typeof(DTE), out object dte) ?
        (dte as DTE).Documents : null;
    }

    /// <summary>
    /// Get the active document
    /// </summary>
    /// <returns>Active document</returns>
    public static Document GetActiveDocument()
    {
      return VsServiceProvider.TryGetService(typeof(DTE), out object dte) ?
        (dte as DTE).ActiveDocument : null;
    }

    /// <summary>
    /// Get the name of the active document
    /// </summary>
    public static List<string> GetDocumentsToIgnore()
    {
      List<string> documentsToIgnore = new List<string>();
      DTE vsServiceProvider = VsServiceProvider.TryGetService(typeof(DTE), out object dte) ?
          (dte as DTE) : null;

      Document activeDocument = vsServiceProvider.ActiveDocument;
      SelectedItems selectedDocuments = vsServiceProvider.SelectedItems;

      if (selectedDocuments.Count == 1 && selectedDocuments.Item(1).Name == activeDocument.Name)
      {
        documentsToIgnore.Add(activeDocument.Name);
        return documentsToIgnore;
      }

      if (selectedDocuments.Count > 0)
      {
        for (int i = 1; i <= selectedDocuments.Count; i++)
        {
          documentsToIgnore.Add(selectedDocuments.Item(i).Name);
        }
      }
      return documentsToIgnore;
    }

    /// <summary>
    /// Save all the active documents
    /// </summary>
    public static void SaveActiveDocuments()
    {
      var activeDocuments = GetActiveDocuments();
      if (null != activeDocuments && 0 < activeDocuments.Count)
      activeDocuments.SaveAll();
    }

    /// <summary>
    /// Save all the active documents
    /// </summary>
    public static void SaveActiveDocument()
    {
      var activeDocument = GetActiveDocument();
      if (null != activeDocument)
        activeDocument.Save();
    }


    public static ITextBuffer GetDocumentTextBuffer()
    {
      var document = GetActiveDocument();
      var openWindowPath = Path.Combine(document.Path, document.Name);
      return GetBufferAt(openWindowPath);
    }


    #endregion


    #region Private Methods

    private static ITextBuffer GetBufferAt(string filePath)
    {
      var componentModel = (IComponentModel)VsServiceProvider.GetService(typeof(SComponentModel));
      var editorAdapterFactoryService = componentModel.GetService<IVsEditorAdaptersFactoryService>();

      IVsTextView view = Vsix.GetVsTextViewFrompPath(filePath);
      IVsTextLines lines;
      if (view.GetBuffer(out lines) == 0)
      {
        var buffer = lines as IVsTextBuffer;
        if (buffer != null)
          return editorAdapterFactoryService.GetDataBuffer(buffer);
      }
      return null;


      //var componentModel = (IComponentModel) VsServiceProvider.GetService(typeof(SComponentModel));
      //var editorAdapterFactoryService = componentModel.GetService<IVsEditorAdaptersFactoryService>();

      //var dte2 = (EnvDTE80.DTE2)Package.GetGlobalService(typeof(SDTE));
      //var sp = (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)dte2;
      //var serviceProvider = new ServiceProvider(sp);

      //IVsUIHierarchy uiHierarchy;
      //uint itemID;
      //IVsWindowFrame windowFrame;

      //if (VsShellUtilities.IsDocumentOpen(
      //  serviceProvider,
      //  filePath,
      //  Guid.Empty,
      //  out uiHierarchy,
      //  out itemID,
      //  out windowFrame))
      //{
      //  IVsTextView view = Vsix.GetVsTextViewFrompPath(filePath) VsShellUtilities.GetTextView(windowFrame);
      //  IVsTextLines lines;
      //  if (view.GetBuffer(out lines) == 0)
      //  {
      //    var buffer = lines as IVsTextBuffer;
      //    if (buffer != null)
      //      return editorAdapterFactoryService.GetDataBuffer(buffer);
      //  }
      //}

      //return null;
    }

    #endregion


  }
}
