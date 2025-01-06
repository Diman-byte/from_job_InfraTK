using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Platform;
using Splat;
using System;
using System.IO;
using Workstation.Common.MsgLog;



namespace Workstation;

public partial class MessageWindow : Window
{
    public MessageWindow()
    {
        InitializeComponent();
        //FuncOtladka();
    }

    MsgLogClass msgLog;
    bool fullMessage = false;

    public MessageWindow(MsgLogClass msgLog, bool fullMessage = false)
    {
        InitializeComponent();

        DatailsCB.IsCheckedChanged += DetailsCB_Click; // Привязка события Checked к чек боксу

        // В WPF или старой версии авалонии обработчики событий можно было указывать в XAML как строки (например, Click="OK_Click"),
        // и это работало благодаря встроенному механизму привязки событий. В Avalonia такой подход не поддерживается.
        // Все события нужно подключать через код.
        var button_ok = this.FindControl<Button>("Button_OK");
        button_ok.Click += OK_Click;

        this.msgLog = msgLog;
        this.fullMessage = fullMessage;
        UpdateView();
        ChangeDetailsView();
    }

    private void UpdateView()
    {
        // Если msgLog.LogDetails пуст, скрывает флажок DatailsCB.
        if (string.IsNullOrEmpty(msgLog.LogDetails))
        {
            DatailsCB.IsVisible = false;
        }
        else
        {
            DatailsCB.IsVisible = true;
        }

        if (fullMessage)
        {
            MessageTB.Text = msgLog.TimeStamp + "\n" + msgLog.SourceLog + "\n" + msgLog.LogText;
        }

        MessageTB.Text = msgLog.LogText;
        DatailsTB.Text = msgLog.LogDetails;

        if (msgLog.TypeLog == LogLevel.Info)
        {
            MessageIcon.Source = LoadBitmap("avares://Workstation/Assets/Icons/Info.png");
            // avares:// — специальный префикс для работы с ресурсами Avalonia.
            Title = "Сообщение";
        }
        if (msgLog.TypeLog == LogLevel.Warn)
        {
            MessageIcon.Source = LoadBitmap("avares://Workstation/Assets/Icons/Warning.png");
            Title = "Предупреждение";
        }
        if (msgLog.TypeLog == LogLevel.Error)
        {
            MessageIcon.Source = LoadBitmap("avares://Workstation/Assets/Icons/Alarm.png");
            Title = "Тревога";
        }

        static Bitmap LoadBitmap(string path)
        {
            return new Bitmap(AssetLoader.Open(new System.Uri(path)));
            // new Bitmap(stream) - Метод принимает поток(Stream) и создаёт из него объект Bitmap — изображение,
            // которое можно отобразить в элементах Avalonia.

            // new Uri(path) создаёт объект класса Uri, который описывает местоположение ресурса.

            // AssetLoader — это глобальный сервис для загрузки ресурсов в Avalonia. Обычно доступен через AvaloniaLocator.
            // .Open(Uri) — метод, который открывает ресурс(например, файл изображения) в виде потока(Stream).
        }

    }

    // отладка
    void FuncOtladka()
    {
        var log = new MsgLogClass();
        log.TimeStamp = DateTime.Now;
        log.SourceLog = "ot sebya";
        log.LogDetails = "eto log details";
        log.LogText = "eto logText";
        log.TypeLog = LogLevel.Warn;
        var window = new MessageWindow(log);
        window.Show();
    }

    private void DetailsCB_Click(object sender, EventArgs e)
    {
        ChangeDetailsView();
    }

    private void ChangeDetailsView()
    {
        if (DatailsCB.IsChecked == true)
        {
            DatailsTB.IsVisible = true;
        }
        else
        {
            DatailsTB.IsVisible = false;
        }
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}