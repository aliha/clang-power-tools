using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ClangPowerTools.Error.Tags
{
  internal class ErrorTagger
  {

    #region Members

    private static readonly Regex WordBoundaryPattern = new Regex(@"[^\$\w]", RegexOptions.Compiled);

    private ITextBuffer buffer;

    private IEnumerable<TaskErrorModel> errors;

    private string fileName;

    private IList<ErrorTag> tags;

    #endregion


    #region Constructor

    public ErrorTagger()
    {

    }

    public ErrorTagger(ITextBuffer buffer, IEnumerable<TaskErrorModel> errorListProvider, string fileName)
    {
      this.buffer = buffer;
      this.errors = errorListProvider;
      this.fileName = fileName;

      this.tags = new List<ErrorTag>();
      this.PopulateTags();

      //this.errorListProvider.ErrorListChange += this.OnErrorListChange;
    }


    //public JSLintTagger(ITextBuffer buffer, IJSLintErrorListProvider errorListProvider, string fileName)
    //{
    //  this.buffer = buffer;
    //  this.errorListProvider = errorListProvider;
    //  this.fileName = fileName;

    //  this.tags = new List<JSLintTag>();
    //  this.PopulateTags();

    //  this.errorListProvider.ErrorListChange += this.OnErrorListChange;
    //}

    #endregion



    public event EventHandler<SnapshotSpanEventArgs> TagsChanged;


    public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection snapshotSpans)
    {
      var list = new List<TagSpan<IErrorTag>>();

      if (this.tags.Count > 0)
      {
        foreach (var snapshotSpan in snapshotSpans)
        {
          foreach (ErrorTag tag in this.tags)
          {
            var snapshot = snapshotSpan.Snapshot;
            var span = tag.TrackingSpan.GetSpan(snapshot);

            if (span.IntersectsWith(snapshotSpan))
            {
              var tagSpan = new TagSpan<IErrorTag>(new SnapshotSpan(snapshot, span), tag);

              list.Add(tagSpan);
            }
          }
        }
      }

      return list;
    }


    private static Span GetErrorSpan(ITextSnapshot snapshot, TaskErrorModel error)
    {
      var line = snapshot.GetLineFromLineNumber(error.Line);
      var text = line.GetText();

      if (text.Length < error.Column)
      {
        return new Span(line.End.Position, 1);
      }

      var start = line.Start.Position + error.Column;
      var length = line.End.Position - start;
      var match = WordBoundaryPattern.Match(text, error.Column);

      if (match.Success)
      {
        length = match.Index - error.Column;
      }

      return new Span(start, length);
    }



    //private void OnErrorListChange(object sender, ErrorListChangeEventArgs e)
    //{
    //  if (this.IsRelevant(e))
    //  {
    //    this.PopulateTags();

    //    var handler = this.TagsChanged;

    //    if (handler != null)
    //    {
    //      var snapshot = this.buffer.CurrentSnapshot;
    //      var span = new SnapshotSpan(snapshot, new Span(0, snapshot.Length));

    //      handler(this, new SnapshotSpanEventArgs(span));
    //    }
    //  }
    //}



    //private bool IsRelevant(ErrorListChangeEventArgs e)
    //{
    //  switch (e.Action)
    //  {
    //    case ErrorListAction.ClearFile:
    //    case ErrorListAction.AddFile:
    //      return e.ContainsFile(this.fileName);
    //    case ErrorListAction.ClearAll:
    //      return true;
    //  }

    //  return false;
    //}



    private void PopulateTags()
    {
      this.tags.Clear();

      //var document = DocumentsHandler.GetActiveDocument();

      //foreach(var err in this.errors)
      //{
      //  var doc1 = err.Document;
      //  var doc2 = document.FullName;

      //}


      var errorss = this.errors.Where(err => err.Document.ToLower() == this.fileName.ToLower());
      var snapshot = this.buffer.CurrentSnapshot;

      foreach (var error in errorss)
      {
        if (error.Line > snapshot.LineCount)
        {
          continue;
        }

        var errorSpan = GetErrorSpan(snapshot, error);
        var trackingSpan = snapshot.CreateTrackingSpan(errorSpan, SpanTrackingMode.EdgeInclusive);

        this.tags.Add(new ErrorTag(trackingSpan, error.Text));
      }
    }

  }
}
