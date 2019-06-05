using ClangPowerTools.ViewModels;
using System.Windows;

namespace ClangPowerTools.Views
{
  /// <summary>
  /// Interaction logic for UnexpectedErrorView.xaml
  /// </summary>
  public partial class UnexpectedErrorView : Window
  {
    public UnexpectedErrorView(string message)
    {
      InitializeComponent();
      var errorViewModel = new UnexpectedErrorViewModel(message);
      DataContext = errorViewModel;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }
  }
}
