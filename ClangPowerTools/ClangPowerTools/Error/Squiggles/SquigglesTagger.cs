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

    private ITextView _textView;
    private ITextBuffer _buffer;
    private IEventAggregator _eventAggregator;
    private readonly string _codyDocsFilename;
    private readonly DelegateListener<DocumentationAddedEvent> _listener;



    public SquigglesTagger(ITextView textView, ITextBuffer buffer, IEventAggregator eventAggregator)
    {
      _textView = textView;
      _buffer = buffer;
      _eventAggregator = eventAggregator;
      var filename = GetFileName(buffer);
      _codyDocsFilename = filename + Consts.CODY_DOCS_EXTENSION;
      _listener = new DelegateListener<DocumentationAddedEvent>
           (OnDocumentationAdded);
      _eventAggregator.AddListener<DocumentationAddedEvent>(_listener);

    }


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


    //public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
    //{
    //  foreach (IMappingTagSpan<AsmTokenTag> myTokenTag in this._aggregator.GetTags(spans))
    //  {
    //    SnapshotSpan tagSpan = myTokenTag.Span.GetSpans(this._sourceBuffer)[0];
    //    yield return new TagSpan<IErrorTag>(tagSpan, new ErrorTag(PredefinedErrorTypeNames.SyntaxError, "some info about the error"));
    //  }
    //}
  }
}
