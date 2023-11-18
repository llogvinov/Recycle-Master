using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class MenuScreenPresenter : BasePresenter
    {
        [SerializeField] private MenuScreenView _view;

        public MenuScreenView View => _view;
    }
}