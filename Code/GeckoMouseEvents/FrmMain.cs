#region Using

using System;
using System.Windows.Forms;
using Gecko;

#endregion

#region Copyright

// Пример к статье: http://www.softez.pp.ua/2015/04/27/%d1%8d%d0%bc%d1%83%d0%bb%d1%8f%d1%86%d0%b8%d1%8f-%d1%81%d0%be%d0%b1%d1%8b%d1%82%d0%b8%d0%b9-%d0%bc%d1%8b%d1%88%d0%b8-%d0%b2-geckofx/
// Автор: dredei

#endregion

namespace GeckoMouseEvents
{
    public partial class FrmMain : Form
    {
        private readonly GeckoWebBrowser _webBrowser;

        public FrmMain()
        {
            this.InitializeComponent();

            // инициализация Xulrunner
            Xpcom.Initialize( Application.StartupPath + "\\xulrunner\\" );
            this.InitializeComponent();
            // создаем экземпляр класса GeckoWebBrowser
            this._webBrowser = new GeckoWebBrowser
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom,
                Width = this.Width,
                Height = this.Height - this.pnl1.Height - 5,
                Top = this.pnl1.Bottom + 5
            };
            // устанавливаем UserAgent браузера в Firefox 33
            GeckoPreferences.User[ "general.useragent.override" ] =
                "Mozilla/5.0 (Windows NT 6.1; rv:33.0) Gecko/20150101 Firefox/33.0";
            // добавляем контрол браузера на форму
            this.Controls.Add( this._webBrowser );
        }

        private void btnStart_Click( object sender, EventArgs e )
        {
            // создаем экземпляр класса EmulMouseEvents
            var clickClass = new EmulMouseEvents( this._webBrowser );
            // начинаем выполнение
            clickClass.Start();
        }
    }
}