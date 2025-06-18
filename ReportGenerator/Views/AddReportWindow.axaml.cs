using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using ReportGenerator.ViewModels;
using System;

namespace ReportGenerator.Views;

public partial class AddReportWindow : ReactiveWindow<AddReportWindowViewModel>
{
    public AddReportWindow()
    {
        InitializeComponent();

        // код для закрытия окна
        //this.WhenActivated(action => action(ViewModel!.CommandOk.Subscribe(_ => Close())));
        this.WhenActivated(action => action(ViewModel!.CommandOk.Subscribe(Close)));
        this.WhenActivated(action => action(ViewModel!.CancelCommand.Subscribe(_ => Close())));
    }
}   