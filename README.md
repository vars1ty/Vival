# Vival
Yet *another* minimalistic text-based status bar made for Linux
## What differs?
Not much apart from the fact that its made in C# and offers 0 configuration files apart from the source itself, which you edit in order to update the bar.
## Which file is used to render the bar?
`bars/VivalBar.cs`
## How do I run it?
Note: This is for the binary that was compiled with the Release tag, it may not be up-to-date constantly.
***
`git clone https://github.com/qqtc0/Vival.git && Vival/bin/Release/net6.0/Vival & disown && clear`
## How do I build it?
Learn C#
### Dependencies
* Bash
* xdotool (X11)
* dotnet-runtime - 6.0+
* dotnet-sdk - 6.0+
## Are PRS (Pull Requests) accepted?
Yeah, as long as they satisfy IntelliSense, the code is clean and documented (much like the original code) and doesn't add additional libraries **without a solid and good reason as to why it should be added**, and I'm talking about a love-like description as to why I should even consider it.
# Known flaws
```diff
- The bar can be resized on WMs like xmonad, which shouldn't be happening but can't exactly be prevented in any good way as of right now.
- Currently the overlay is rendered as a "Form" using Eto.Forms - GTK, which means it appears in every truly non-fullscreen application (videos and borderless-windowed games for example).
  * Setting up a keybind to start the bar is adviced so you can toggle it on/off when needed.
- Borders on WMs like xmonad may start flashing due to how Vival deals with fetching system-related data from bash.
```
