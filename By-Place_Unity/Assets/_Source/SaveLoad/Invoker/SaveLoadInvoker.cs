using System;

namespace SaveLoad.Invoker
{
    public class SaveLoadInvoker
    {
        public event Action OnSave;
        public event Action OnLoad;

        public void InvokeSave() => OnSave?.Invoke();
        public void InvokerLoad() => OnLoad?.Invoke();
    }
}