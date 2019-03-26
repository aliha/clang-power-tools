using Microsoft.Practices.Composite.Events;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace ClangPowerTools.Error.Squiggles
{
  internal sealed class SquigglesTagger : ITagger<SquiggleTag>
  {

    private readonly ITextBuffer _sourceBuffer;
    private readonly ITagAggregator<AsmTokenTag> _aggregator;
    private readonly ErrorListProvider _errorListProvider;
    private readonly LabelGraph _labelGraph;
    private readonly AsmSimulator _asmSimulator;
    private readonly Brush _foreground;
    private object _updateLock = new object();
    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;



    internal SquigglesTagger(
            ITextBuffer buffer,
            IBufferTagAggregatorFactoryService aggregatorFactory,
            LabelGraph labelGraph,
            AsmSimulator asmSimulator)
    {
      //AsmDudeToolsStatic.Output_INFO("SquigglesTagger: constructor");
      this._sourceBuffer = buffer;
      this._aggregator = AsmDudeToolsStatic.GetOrCreate_Aggregator(buffer, aggregatorFactory);
      this._errorListProvider = AsmDudeTools.Instance.Error_List_Provider;
      this._foreground = AsmDudeToolsStatic.GetFontColor();

      this._labelGraph = labelGraph;
      if (this._labelGraph.Enabled)
      {
        this._labelGraph.Reset_Done_Event += (o, i) => {
          this.Update_Squiggles_Tasks_Async().ConfigureAwait(false);
          this.Update_Error_Tasks_Labels_Async().ConfigureAwait(false);
        };
        this._labelGraph.Reset();
      }

      this._asmSimulator = asmSimulator;
      if (this._asmSimulator.Enabled)
      {
        this._asmSimulator.Line_Updated_Event += (o, e) =>
        {
          //AsmDudeToolsStatic.Output_INFO("SquigglesTagger:Handling _asmSimulator.Line_Updated_Event: event from " + o + ". Line " + e.LineNumber + ": "+e.Message);
          this.Update_Squiggles_Tasks_Async(e.LineNumber).ConfigureAwait(false);
          this.Update_Error_Task_AsmSimAsync(e.LineNumber, e.Message).ConfigureAwait(false);
        };
        this._asmSimulator.Reset_Done_Event += (o, e) =>
        {
          AsmDudeToolsStatic.Output_INFO("SquigglesTagger:Handling _asmSimulator.Reset_Done_Event: event from " + o);
          //this.Update_Error_Tasks_AsmSim_Async();
        };
        this._asmSimulator.Reset();
      }
    }


    //public SquigglesTagger(ITextView textView, ITextBuffer buffer, IEventAggregator eventAggregator)
    //{
    //  _textView = textView;
    //  _buffer = buffer;
    //  _eventAggregator = eventAggregator;
    //  var filename = GetFileName(buffer);
    //  _codyDocsFilename = filename + Consts.CODY_DOCS_EXTENSION;
    //  _listener = new DelegateListener<DocumentationAddedEvent>
    //       (OnDocumentationAdded);
    //  _eventAggregator.AddListener<DocumentationAddedEvent>(_listener);

    //}


    //private readonly ITextBuffer _sourceBuffer;
    //private readonly ITagAggregator<AsmTokenTag> _aggregator;

    //public event EventHandler<SnapshotSpanEventArgs> TagsChanged;



    //internal SquigglesTagger(ITextBuffer buffer, IBufferTagAggregatorFactoryService aggregatorFactory)
    //{
    //  this._sourceBuffer = buffer;
    //  ITagAggregator<AsmTokenTag> sc()
    //  {
    //    return aggregatorFactory.CreateTagAggregator<AsmTokenTag>(buffer);
    //  }
    //  this._aggregator = buffer.Properties.GetOrCreateSingletonProperty(sc);
    //}


    public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    {
      foreach (IMappingTagSpan<AsmTokenTag> myTokenTag in this._aggregator.GetTags(spans))
      {
        SnapshotSpan tagSpan = myTokenTag.Span.GetSpans(this._sourceBuffer)[0];
        yield return new TagSpan<IErrorTag>(tagSpan, new ErrorTag(PredefinedErrorTypeNames.SyntaxError, "some info about the error"));
      }
    }
  }
}
