using System;
using Utils.Extensions;
using Utils.Pooling;

namespace Popup.Pool
{
    public class PopupPoolConfig : PoolConfig<PopupElement>
    {
        public override Func<PopupElement> Factory { get; }
        public override Action<PopupElement> GetCallback { get; }
        public override Action<PopupElement> PutCallback { get; }

        public PopupPoolConfig()
        {
            Factory = () => new PopupElement();
            GetCallback = popup => popup.Show();
            PutCallback = popup => popup.Hide();
        }
    }
}