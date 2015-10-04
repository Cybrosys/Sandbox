# Sandbox
Play area

**Factory** is a simple WeakReference Factory class. It solves a problem where I wanted to share Model data used in ViewModels. The problem was that one ViewModel would update a Model and a previous ViewModel would be completely oblivious to this because each ViewModel populated itself using injected Managers/Providers.
