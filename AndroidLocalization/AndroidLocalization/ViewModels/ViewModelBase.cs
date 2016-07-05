using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndroidLocalization.ViewModels
{
    public abstract class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        private int _isBusyCounter;
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value)
                    ++_isBusyCounter;
                else
                {
                    --_isBusyCounter;
                    if (_isBusyCounter < 0) _isBusyCounter = 0;
                    else if (_isBusyCounter > 0) return;
                }
                if (Set(ref _isBusy, value))
                    OnIsBusyChanged();
            }
        }

        protected virtual void OnIsBusyChanged()
        {

        }

        protected class BusyContext : IDisposable
        {
            private WeakReference<ViewModelBase> _weakViewModelRef;
            public BusyContext(ViewModelBase viewModel)
            {
                _weakViewModelRef = new WeakReference<ViewModelBase>(viewModel);

                ViewModelBase target;
                if (_weakViewModelRef.TryGetTarget(out target))
                    target.IsBusy = true;
            }

            public void Dispose()
            {
                ViewModelBase target;
                if (_weakViewModelRef != null && _weakViewModelRef.TryGetTarget(out target))
                {
                    target.IsBusy = false;
                    _weakViewModelRef = null;
                }
            }
        }
    }
}
