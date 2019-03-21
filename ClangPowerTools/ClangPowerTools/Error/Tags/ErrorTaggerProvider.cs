//using Microsoft.VisualStudio.Text;
//using Microsoft.VisualStudio.Text.Tagging;
//using System.Collections.Generic;

//namespace ClangPowerTools.Error.Tags
//{
//  internal class ErrorTaggerProvider : ITaggerProvider
//  {
//    public IEnumerable<TaskErrorModel> Errors { get; set; }



//    public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
//    {
//      var doc = GetBufferProperty<ITextDocument>(buffer);

//      if (doc != null)
//      {
//        var fileName = doc.FilePath;

//        return new ErrorTagger(buffer, Errors, fileName) as ITagger<T>;


//        //        return buffer.Properties.GetOrCreateSingletonProperty(() => (ITagger<T>)new ErrorTagger(buffer, Errors, fileName));


//        //if (JSLint.CanLint(fileName))
//        //{
//        //  return buffer.Properties.GetOrCreateSingletonProperty(() => (ITagger<T>)new JSLintTagger(buffer, this.ErrorListProvider, fileName));
//        //}
//      }

//      return null;
//    }



//    private static TValue GetBufferProperty<TValue>(ITextBuffer buffer)
//        where TValue : class
//    {
//      var key = typeof(TValue);
//      var properties = buffer.Properties;

//      if (properties.ContainsProperty(key))
//      {
//        return properties[key] as TValue;
//      }

//      return null;
//    }

//  }
//}
