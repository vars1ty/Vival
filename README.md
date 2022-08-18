> **Warning**:
> Vival is no longer being maintained, use [Hybrid](https://github.com/dev11n/HybridBar) instead if you use Wayland.
>
> The latest, most stable and efficient release is under the [`avalonia`](https://github.com/dev11n/Vival/tree/avalonia) branch.
>
> NOTE: Vival is mainly made to work on the xmonad window manager, so for it to sync position to other workspaces, you have to adapt to the other window managers structure.
# Vival
Yet *another* minimalistic text-based status bar made for Linux
## What differs?
Not much apart from the fact that its made in C# and offers 0 configuration files apart from the source itself, which you edit in order to update the bar.
## Which file is used to render the bar?
`bars/VivalBar.cs`
## How do I run it?
Note: This is for the binary that was compiled with the Release tag, it may not always be up-to-date.

To keep it up-to-date, build it from source yourself by using the shell-script.
***
`git clone https://github.com/qqtc0/Vival.git && Vival/bin/Release/net6.0/Vival & disown && clear`
## How do I build it?
`./build-vival.sh` - .NET SDK is required
### Dependencies
* Bash
* xdotool (X11)
* dotnet-runtime - 6.0+
* dotnet-sdk - 6.0+
* Optionally: nerdfonts
## Are PRS (Pull Requests) accepted?
Yeah, as long as they satisfy IntelliSense, the code is clean and documented (much like the original code) and doesn't add additional libraries **without a solid and good reason as to why it should be added**, and I'm talking about a love-like description as to why I should even consider it.
# Known flaws
```diff
- The bar can be resized in WMs like xmonad, which shouldn't be happening but can't exactly be prevented in any good way as of right now.
  * Can be fixed by adjusting your config.
- Currently the overlay is rendered as a "Form" using Eto.Forms - GTK, which means it appears in every truly non-fullscreen application (videos and borderless-windowed games for example).
  * This can be prevented in xmonad by adding it to your window rules.
  * Setting up a keybind to start the bar is adviced so you can toggle it on/off when needed.
```
## Multi-monitor issues
This is made explicitly for xmonad.
***
If you're experiencing the bar hopping from workspace to workspace, try and make the bar load on startup automatically **and** is inside xmonad's window rules.

Config: `~/.xmonad/xmonad.hs`

![image](https://user-images.githubusercontent.com/54314240/159103939-5b2a4509-60a6-4d27-ab5f-ae73fbae21a1.png)

For starting it, add this to your startup hook: `spawnOnce "(path-to-vival) & disown"`
