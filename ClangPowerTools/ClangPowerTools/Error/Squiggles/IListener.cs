namespace ClangPowerTools.Error.Squiggles
{

  /// <summary>
  /// Specifies a class that would like to receive particular messages.
  /// </summary>
  /// <typeparam name="TMessage">The type of message object to subscribe to.</typeparam>
  public interface IListener<in TMessage>
  {
    /// <summary>
    /// This will be called every time a TMessage is published through the event aggregator
    /// </summary>
    void Handle(TMessage message);

  }
}
