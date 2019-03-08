using System.Threading.Tasks;

namespace ClangPowerTools.Builder
{
  public interface IAsyncBuilder<T>
  {
    Task BuildAsync();

    T GetAsyncResult();
  }
}
