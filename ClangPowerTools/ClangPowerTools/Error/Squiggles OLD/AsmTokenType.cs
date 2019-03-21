using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClangPowerTools.Error.Squiggles
{
  public enum AsmTokenType
  {
    Mnemonic, Register, Remark, Directive, Constant, Jump, Label, LabelDef, Misc, UserDefined1, UserDefined2, UserDefined3, UNKNOWN
  }
}
