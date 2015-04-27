#region Using

using Gecko;

#endregion

#region Copyright

// Пример к статье: http://www.softez.pp.ua/2015/04/27/%d1%8d%d0%bc%d1%83%d0%bb%d1%8f%d1%86%d0%b8%d1%8f-%d1%81%d0%be%d0%b1%d1%8b%d1%82%d0%b8%d0%b9-%d0%bc%d1%8b%d1%88%d0%b8-%d0%b2-geckofx/
// Автор: dredei

#endregion

namespace GeckoMouseEvents
{
    internal static class GeckoExtensionMethods
    {
        /// <summary>
        /// Эмулирует указанное событие мыши
        /// </summary>
        /// <param name="webBrowser"></param>
        /// <param name="element">Элемент, которому будет отправляться событие</param>
        /// <param name="aTypeEvent">Тип события</param>
        /// <param name="screenX"></param>
        /// <param name="screenY"></param>
        /// <param name="clientX">Координата мыши X</param>
        /// <param name="clientY">Координата мыши Y</param>
        public static void MouseEventEmulation( this GeckoWebBrowser webBrowser, GeckoElement element, string aTypeEvent,
            int screenX, int screenY, int clientX, int clientY )
        {
            nsIDOMEventTarget target = Xpcom.QueryInterface<nsIDOMEventTarget>( element.DomObject );
            DomEventArgs evt = webBrowser.DomDocument.CreateEvent( "MouseEvent" );
            DomMouseEventArgs mouseEvent = (DomMouseEventArgs)DomEventArgs.Create( evt.DomEvent );
            mouseEvent.InitMouseEvent( aTypeEvent, true, true, webBrowser.Window, 0, screenX, screenY, clientX, clientY,
                false, false, false, false, 0, Gecko.DOM.DomEventTarget.Create( target ) );
            target.DispatchEvent( mouseEvent.DomEvent );
        }
    }
}