//using Microsoft.VisualStudio.Text;
//using Microsoft.VisualStudio.Text.Adornments;
//using Microsoft.VisualStudio.Text.Tagging;

//namespace ClangPowerTools.Error.Tags
//{
//  internal class ErrorTag : IErrorTag
//  {

//    #region Members

//    private string mTooltip;

//    public ErrorTag()
//    {
//    }

//    #endregion


//    #region Constructor

//    public ErrorTag(ITrackingSpan aTrackingSpan, string aToolTip)
//    {
//      TrackingSpan = aTrackingSpan;
//      mTooltip = aToolTip;
//    }

//    #endregion


//    #region Properties

//    public ITrackingSpan TrackingSpan { get; private set; }

//    public string ErrorType => PredefinedErrorTypeNames.CompilerError;

//    public object ToolTipContent => mTooltip;

//    #endregion

//  }
//}
