using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClangPowerTools.Error.Squiggles
{
  public class AsmTokenTag : ITag
  {
    public static readonly string MISC_KEYWORD_PROTO = "PROTO";

    public AsmTokenType Type { get; private set; }
    public string Misc { get; private set; }
    public AsmTokenTag(AsmTokenType type)
    {
      this.Type = type;
    }
    public AsmTokenTag(AsmTokenType type, string misc)
    {
      this.Type = type;
      this.Misc = misc;
    }
  }
}
