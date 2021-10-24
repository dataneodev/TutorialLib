﻿using System.Threading;
using System.Windows;

namespace dataneo.TutorialLibs.WPF.Helpers
{
    public static class DispatcherHelpers
    {
        public static bool IsUIThread()
            => Thread.CurrentThread == Application.Current.Dispatcher.Thread;
    }
}