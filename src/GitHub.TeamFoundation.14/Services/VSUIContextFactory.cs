﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Shell;
using GitHub.Services;

namespace GitHub.TeamFoundation.Services
{
    class VSUIContext : IVSUIContext
    {
        readonly UIContext context;
        readonly Dictionary<EventHandler<VSUIContextChangedEventArgs>, EventHandler<UIContextChangedEventArgs>> handlers =
            new Dictionary<EventHandler<VSUIContextChangedEventArgs>, EventHandler<UIContextChangedEventArgs>>();
        public VSUIContext(Guid contextGuid)
        {
            context = UIContext.FromUIContextGuid(contextGuid);
        }

        public bool IsActive { get { return context.IsActive; } }

        public event EventHandler<VSUIContextChangedEventArgs> UIContextChanged
        {
            add
            {
                EventHandler<UIContextChangedEventArgs> handler = null;
                if (!handlers.TryGetValue(value, out handler))
                {
                    handler = (s, e) => value.Invoke(s, new VSUIContextChangedEventArgs(e.Activated));
                    handlers.Add(value, handler);
                }
                context.UIContextChanged += handler;
            }
            remove
            {
                EventHandler<UIContextChangedEventArgs> handler = null;
                if (handlers.TryGetValue(value, out handler))
                {
                    handlers.Remove(value);
                    context.UIContextChanged -= handler;
                }
            }
        }
    }
}
