using System;
using SaveLoad.Invoker;
using VContainer;
using VContainer.Unity;

namespace Dialogue.Resolve
{
    public class OnDialogueSaver : IStartable, IDisposable
    {
        private DialogueController _dialogueController;
        
        private SaveLoadInvoker _saveLoadInvoker;

        [Inject]
        public OnDialogueSaver(DialogueController dialogueController, SaveLoadInvoker saveLoadInvoker)
        {
            _dialogueController = dialogueController;
            _saveLoadInvoker = saveLoadInvoker;
        }

        public void Start() => Bind();

        public void Dispose() => Expose();

        private void Bind()
        {
            _dialogueController.OnQuit += _saveLoadInvoker.InvokeSave;
        }

        private void Expose() => _dialogueController.OnQuit -= _saveLoadInvoker.InvokeSave;
    }
}