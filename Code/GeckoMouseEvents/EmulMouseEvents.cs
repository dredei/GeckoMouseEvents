#region Using

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gecko;
using Gecko.DOM;

#endregion

#region Copyright

// Пример к статье: http://www.softez.pp.ua/2015/04/27/%d1%8d%d0%bc%d1%83%d0%bb%d1%8f%d1%86%d0%b8%d1%8f-%d1%81%d0%be%d0%b1%d1%8b%d1%82%d0%b8%d0%b9-%d0%bc%d1%8b%d1%88%d0%b8-%d0%b2-geckofx/
// Автор: dredei

#endregion

namespace GeckoMouseEvents
{
    internal class EmulMouseEvents
    {
        private readonly GeckoWebBrowser _webBrowser; // ссылка на браузер
        private readonly Timer _loadingTimer; // таймер загрузки
        private bool _loading; // указывает на статус загрузки

        public EmulMouseEvents( GeckoWebBrowser webBrowser )
        {
            this._webBrowser = webBrowser;
            this._loadingTimer = new Timer { Interval = 5000 };
            this._loadingTimer.Tick += this._loadingTimer_Tick;
        }

        private void _loadingTimer_Tick( object sender, EventArgs e )
        {
            // останавливаем таймер
            this._loadingTimer.Stop();
            // считаем, что загрузка завершилась
            this._loading = false;
        }

        /// <summary>
        /// Ожидает загрузки страницы
        /// </summary>
        private async Task WaitForLoading()
        {
            while ( this._loading )
            {
                await TaskEx.Delay( 200 );
            }
        }

        /// <summary>
        /// Загружает указанную страницу и ждет завершения загрузки
        /// </summary>
        /// <param name="url">Url страницы</param>
        private async Task Navigate( string url )
        {
            this._loading = true;
            this._webBrowser.Navigate( url );
            this._loadingTimer.Start();
            await this.WaitForLoading();
        }

        /// <summary>
        /// Печатает текст
        /// </summary>
        /// <param name="element">Куда печатать</param>
        /// <param name="text">Текст</param>
        /// <returns></returns>
        private async Task WriteText( GeckoElement element, string text )
        {
            Random random = new Random();
            var htmlEl = element as GeckoHtmlElement;
            if ( htmlEl == null )
            {
                return;
            }
            for ( int i = 0; i < text.Length; i++ )
            {
                htmlEl.InnerHtml = text.Substring( 0, i + 1 );
                await TaskEx.Delay( random.Next( 100, 350 ) );
            }
        }

        /// <summary>
        /// Эмулирует движение мыши к указанному элементу (с точки 1,1)
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private async Task MoveMouseToElement( GeckoElement element )
        {
            var startPoint = new Point( 1, 1 );
            // определяем координаты конечной точки
            var endPoint = new Point( element.GetBoundingClientRect().Left, element.GetBoundingClientRect().Top );
            // примитивное движение мышью к нужной точке
            do
            {
                // эмулируем событие mousemove
                this._webBrowser.MouseEventEmulation( this._webBrowser.Document.Body, "mousemove", 0, 0, startPoint.X,
                    startPoint.Y );
                if ( startPoint.X < endPoint.X )
                {
                    startPoint.X++;
                }
                if ( startPoint.Y < endPoint.Y )
                {
                    startPoint.Y++;
                }
                // задержка
                await TaskEx.Delay( 10 );
            }
            while ( startPoint.X < endPoint.X || startPoint.Y < endPoint.Y );
        }

        /// <summary>
        /// Эмулирует нажатие левой кнопкной мыши на элемент
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private async Task MouseClickOnElement( GeckoElement element )
        {
            // эмулируем нажатие на элемент
            // вместо mousedown и mouseup можно использовать click, но он не везде работает
            this._webBrowser.MouseEventEmulation( element, "mousedown", 0, 0, 0, 0 );
            await TaskEx.Delay( 120 );
            this._webBrowser.MouseEventEmulation( element, "mouseup", 0, 0, 0, 0 );
        }

        /// <summary>
        /// Старт
        /// </summary>
        public async void Start()
        {
            // открываем страницу переводчика и ждем загрузки
            await this.Navigate( "http://translate.google.com/" );
            // получаем поле для ввода текста
            GeckoElement textElement = this._webBrowser.Document.GetElementById( "source" );
            // "плавный" набор поискового запроса
            const string query = "Hello, World!";
            // вводим поисковый запрос
            await this.WriteText( textElement, query );
            // получаем кнопку "прослушать"
            GeckoElement listenElement = this._webBrowser.Document.GetElementById( "gt-src-listen" );
            // эмулируем движение мыши к кнопке "Прослушать"
            await this.MoveMouseToElement( listenElement );
            // эмулируем нажатие на кнопку "Прослушать"
            await this.MouseClickOnElement( listenElement );
        }
    }
}