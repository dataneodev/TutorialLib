using CSharpFunctionalExtensions;
using LibVLCSharp.Shared;
using System;
using System.Threading;

namespace dataneo.TutorialLibs.WPF.UI.Player.Services
{
    public static class VLCLoader
    {
        private static volatile bool _isInit;
        private static volatile bool _isDisposed;
        private static LibVLC _libVLC;

        public static void Init()
        {
            if (_isInit)
                throw new InvalidOperationException("Already initialized");

            var thr = new Thread(new ThreadStart(InitializeInternal));
            thr.Start();
        }

        private static void InitializeInternal()
        {
            _libVLC = new LibVLC();
            _isInit = true;
        }

        public static Maybe<LibVLC> GetVLC()
            => _isInit ? _libVLC : null;

        public static bool IsInit() => _isInit;

        public static void Dispose()
        {
            if (!_isInit)
                return;

            if (_isDisposed)
                throw new InvalidOperationException();

            _libVLC.Dispose();
            _isDisposed = true;
        }
    }
}
