using Microsoft.VisualStudio.TextManager.Interop;
using System;

namespace ClangPowerTools.ErrorLineMarker
{
  public class VsTextMarkerModel
  {
    #region Members

    public int Type { get; set; }

    public int StartLine { get; set; }

    public int StartIndex { get; set; }

    public int EndLine { get; set; }

    public int EndIndex { get; set; }

    //public IVsTextMarkerClient TextMarkerClient { get; set; }

    //public IVsTextLineMarker[] VsTextLineMarker { get; set; }

    #endregion

  }
}
