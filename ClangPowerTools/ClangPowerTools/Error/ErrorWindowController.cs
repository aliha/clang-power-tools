using ClangPowerTools.Error.Tags;
using ClangPowerTools.Events;
using ClangPowerTools.Handlers;
using ClangPowerTools.Services;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace ClangPowerTools
{
  public class ErrorWindowController : ErrorListProvider
  {
    #region Members

    private static ErrorTaggerProvider mErrorTaggerProvider = new ErrorTaggerProvider();

    private static List<ErrorTagger> tagger = new List<ErrorTagger>();

    //private static ErrorTagger tagger = new ErrorTagger();

    #endregion


    #region Constructor

    /// <summary>
    /// Instance Constructor
    /// </summary>
    /// <param name="aServiceProvider"></param>
    public ErrorWindowController(IServiceProvider aIServiceProvider) : base(aIServiceProvider)
    {
    }

    #endregion


    #region Public Methods


    public void OnErrorDetected(object sender, ErrorDetectedEventArgs e)
    {
      UIUpdater.Invoke(() =>
      {
        SuspendRefresh();

        foreach (TaskErrorModel error in e.ErrorList)
        {
          error.Navigate += ErrorTaskNavigate;
          Tasks.Add(error);
        }

        ResumeRefresh();
        BringToFront();


        var dte = (DTE2)VsServiceProvider.GetService(typeof(DTE));
        var docs = dte.Documents;



        tagger.Clear();

        mErrorTaggerProvider.Errors = e.ErrorList;


        foreach(var error in mErrorTaggerProvider.Errors)
        {
          Document doc = null;
          foreach(Document d in docs)
          {
            if(d.FullName.ToLower() == error.Document.ToLower())
            {
              doc = d;
              break;
            }
          }

          if (doc == null)
            continue;

          tagger.Add(mErrorTaggerProvider.CreateTagger<IErrorTag>(DocumentsHandler.GetDocumentTextBuffer(doc.FullName.ToLower()), doc.FullName.ToLower()));

        }

      });

    }


    public void RemoveErrors(IVsHierarchy aHierarchy)
    {
      UIUpdater.Invoke(() =>
      {
        SuspendRefresh();

        for (int i = Tasks.Count - 1; i >= 0; --i)
        {
          var errorTask = Tasks[i] as ErrorTask;
          aHierarchy.GetCanonicalName(Microsoft.VisualStudio.VSConstants.VSITEMID_ROOT, out string nameInHierarchy);

          if (null == errorTask.HierarchyItem)
            return;

          errorTask.HierarchyItem.GetCanonicalName(Microsoft.VisualStudio.VSConstants.VSITEMID_ROOT, out string nameErrorTaskHierarchy);
          if (nameInHierarchy == nameErrorTaskHierarchy)
          {
            errorTask.Navigate -= ErrorTaskNavigate;
            Tasks.Remove(errorTask);
          }
        }

        ResumeRefresh();
      });
    }


    public void Clear()
    {
      UIUpdater.Invoke(() =>
      {
        Tasks.Clear();
      });
    }

    public void OnClangCommandBegin(object sender, ClearErrorListEventArgs e)
    {
      Clear();
    }

    public void OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
    {
      Clear();
    }

    #endregion


    #region Private Methods

    private void ErrorTaskNavigate(object sender, EventArgs e)
    {
      ErrorTask objErrorTask = (ErrorTask)sender;
      objErrorTask.Line += 1;
      bool bResult = Navigate(objErrorTask, new Guid(EnvDTE.Constants.vsViewKindCode));
      objErrorTask.Line -= 1;
    }

    #endregion

  }
}
