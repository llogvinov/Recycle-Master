using UI.Views;
using UnityEngine;

namespace UI.Presenters
{
    public class LoadingScreenPresenter : BasePresenter
    {
        [SerializeField] private LoadingScreenView _view;

        public LoadingScreenView View => _view;
    }
}