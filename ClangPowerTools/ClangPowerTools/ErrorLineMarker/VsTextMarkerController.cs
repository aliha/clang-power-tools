using ClangPowerTools.Events;
using ClangPowerTools.Services;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;

namespace ClangPowerTools.ErrorLineMarker
{
  internal class VsTextMarkerController : IVsTextMarkerClient, IDisposable
  {
    #region Members

    private IVsTextView mVsTextView;

    private IVsTextLines mVsTextLines;

    private IVsTextLineMarker[] mVsTextLineMarkers;

    private Dictionary<string, List<TaskErrorModel>> mErrors;

    #endregion


    #region Public Methods

    public void Initialize()
    {
      var textManager = VsServiceProvider.GetService(typeof(SVsTextManager)) as IVsTextManager;
      textManager.GetActiveView(1, null, out mVsTextView);
      mVsTextView.GetBuffer(out mVsTextLines);
      mVsTextLineMarkers = new IVsTextLineMarker[1];
    }

    public void OnErrorDetected(IEnumerable<TaskErrorModel> aErrorList)
    {
      mErrors = GroupErrorsAfterFileName(aErrorList);
      CreateErrorMarkers();
    }



    public void OnErrorDetected(object sender, ErrorDetectedEventArgs e)
    {
      mErrors = GroupErrorsAfterFileName(e.ErrorList);
      CreateErrorMarkers();
    }

    #region IVsTextMarkerClient Implementation

    public void MarkerInvalidated()
    {
    }

    public int GetTipText(IVsTextMarker pMarker, string[] pbstrText)
    {
      return VSConstants.S_OK;
    }

    public void OnBufferSave(string pszFileName)
    {
    }

    public void OnBeforeBufferClose()
    {
    }

    public int GetMarkerCommandInfo(IVsTextMarker pMarker, int iItem, string[] pbstrText, uint[] pcmdf)
    {
      return VSConstants.S_OK;
    }

    public int ExecMarkerCommand(IVsTextMarker pMarker, int iItem)
    {
      return VSConstants.S_OK;
    }

    public void OnAfterSpanReload()
    {
    }

    public int OnAfterMarkerChange(IVsTextMarker pMarker)
    {
      return VSConstants.S_OK;
    }

    #endregion

    #region IDisposable Implementation

    public void Dispose()
    {
      mVsTextLineMarkers[0].Invalidate();
      mVsTextLineMarkers[0].UnadviseClient();
      mVsTextLineMarkers[0] = null;

      mErrors.Clear();
      mErrors = null;
      mVsTextView = null;
      mVsTextLines = null;

      GC.SuppressFinalize(this);
    }

    #endregion

    #endregion

    #region Private Methods

    /// <summary>
    /// Create a red or green wavy underline text marker for the detected clang errors
    /// </summary>
    private void CreateErrorMarkers()
    {
      if (0 == mErrors.Count)
        return;

      var activeDocument = DocumentsHandler.GetActiveDocument();
      if (null == activeDocument)
        return;

      if (!mErrors.ContainsKey(activeDocument.FullName))
        return;

      foreach (var error in mErrors[activeDocument.FullName])
      {
        var marker = new VsTextMarkerModel
        {
          Type = TaskErrorCategory.Error == error.ErrorCategory ? 5 : 4,
          StartLine = error.Line,
          StartIndex = error.Column - 1,
          EndLine = error.Line,
          EndIndex = error.Column + 1
        };
        Add(marker);
      }

    }

    /// <summary>
    /// Creates a marker of a given type over the specified region
    /// </summary>
    /// <param name="aMarker">The Marker model which contains all the necessary data to create a new marker</param>
    private void Add(VsTextMarkerModel aMarker)
    {
      mVsTextLines.CreateLineMarker(aMarker.Type, aMarker.StartLine,
        aMarker.StartIndex, aMarker.EndLine, aMarker.EndIndex, this, mVsTextLineMarkers);

      mVsTextLines.CreateLineMarker(aMarker.Type, 1,
        1, 1, 7, this, mVsTextLineMarkers);
    }

    private Dictionary<string, List<TaskErrorModel>> GroupErrorsAfterFileName(IEnumerable<TaskErrorModel> aErrorList)
    {
      var errors = new Dictionary<string, List<TaskErrorModel>>();
      foreach (var error in aErrorList)
      {
        if (TaskErrorCategory.Message == error.ErrorCategory)
          continue;

        if (!errors.ContainsKey(error.Document))
          errors.Add(error.Document, new List<TaskErrorModel> { error });
        else
          errors[error.Document].Add(error);
      }
      return errors;
    }

    #endregion

  }
}
