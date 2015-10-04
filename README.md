# Sandbox
Play area

**Factory** is a simple WeakReference Factory class. It solves a problem where I wanted to share Model data used across multiple ViewModels. The problem was that one ViewModel would update Model data but none of the other ViewModels would be aware that this had happened because each ViewModel had populated itself using injected Managers/Providers, thus they had the same duplicated data. I got the idea from [Microsoft Project Orleans](https://github.com/dotnet/orleans) where the only way to retrieve a handle to something is to use a pre-generated *Factory class (<Model name>Factory).
