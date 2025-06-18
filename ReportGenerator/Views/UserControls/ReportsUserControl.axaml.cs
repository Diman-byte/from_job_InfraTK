using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReportGenerator.Models;
using ReportGenerator.ViewModels;
using ReportGenerator.ViewModels.UserControls;
using System.Reactive;
using System.Threading.Tasks;
//using ReportGenerator.ViewModels.UserControls;

namespace ReportGenerator.Views.UserControls;

public partial class ReportsUserControl : ReactiveUserControl<ReportsUserControlViewModel>
{
    public ReportsUserControl()
    {
        InitializeComponent();

        // ��� �������� ����������� ���� � ����������� ������
        this.WhenActivated(action =>
                action(ViewModel!.ShowAddReportDialog.RegisterHandler(DoShowDialogAsync)));
       
        // ��� ��������� ������� ������
        this.WhenActivated(action =>
                action(ViewModel!.OpenFileTemplateDialog.RegisterHandler(async interaction =>
                {
                    var topLevel = TopLevel.GetTopLevel(this);

                    var files = await topLevel.StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
                    {
                        Title = "�������� ���� �������",
                        AllowMultiple = false
                    });
                    // IStorageFile � ��������� � ������������ Avalonia.Platform.Storage, ������� ������������� ������ � ������

                    if(files.Count >= 1)
                    {
                        var uri = files[0];
                        string path = uri.TryGetLocalPath();
                        interaction.SetOutput(path);
                    }
                    else
                    {
                        interaction.SetOutput(string.Empty);
                    }

                })));

        // ��� ��������� ����� ������ �������
        this.WhenActivated(action =>
            action(ViewModel!.OpenFolderReportDialog.RegisterHandler(async interaction =>
            {
                var topLevel = TopLevel.GetTopLevel(this);

                var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new Avalonia.Platform.Storage.FolderPickerOpenOptions
                {
                    Title = "�������� ����� ���������� �������",
                    AllowMultiple = false
                });

                if (folder.Count >= 1)
                {
                    var result = folder[0].TryGetLocalPath();
                    interaction.SetOutput(result);
                }
                else
                {
                    interaction.SetOutput(string.Empty);
                }
            })));
    }

    private async Task DoShowDialogAsync(IInteractionContext<AddReportWindowViewModel, AddReportDialogResult?> interaction)
    {
        var dialog = new AddReportWindow();
        dialog.DataContext = interaction.Input;

        var result = await dialog.ShowDialog<AddReportDialogResult?>(GetMainWindow());
        interaction.SetOutput(result);
    }

    private Window GetMainWindow()
    {
        return (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
    }


}