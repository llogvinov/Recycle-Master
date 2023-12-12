using System;
using System.Text;
using Main.Journal;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIJournal : UIBase
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        private void OnEnable() => 
            _button.onClick.AddListener(GetJournal);

        private void OnDisable() => 
            _button.onClick.RemoveListener(GetJournal);

        private void GetJournal()
        {
            var journal = JournalManager.GetJournal();
            var stringBuilder = new StringBuilder();
            foreach (var journalKey in journal.Keys)
            {
                stringBuilder.Append(journalKey.Type);
                if (journal.TryGetValue(journalKey, out var trashDatas))
                {
                    foreach (var trashData in trashDatas)
                    {
                        stringBuilder.Append($"\n\t{trashData.Title}");
                    }
                    stringBuilder.Append("\n");
                }
            }

            _text.text = stringBuilder.ToString();
        }
    }
}