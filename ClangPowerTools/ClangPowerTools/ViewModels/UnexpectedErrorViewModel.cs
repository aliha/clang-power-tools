namespace ClangPowerTools.ViewModels
{
  public class UnexpectedErrorViewModel
  {
    private string mMessage;

    public string Message
    {
      get { return mMessage; }
      set { mMessage = value; }
    }

    public UnexpectedErrorViewModel(string message) => mMessage = message;

  }
}
