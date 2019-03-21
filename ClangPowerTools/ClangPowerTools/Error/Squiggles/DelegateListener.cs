using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClangPowerTools.Error.Squiggles
{
  public class DelegateListener<TMessage> : IListener<TMessage>
  {
    private Action<TMessage> _action;

    public DelegateListener(Action<TMessage> action)
    {
      _action = action;
    }

    public void Handle(TMessage message)
    {
      _action?.Invoke(message);
    }
  }
}
