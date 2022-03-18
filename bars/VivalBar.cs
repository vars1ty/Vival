using Eto.Drawing;
using Eto.Forms;

namespace Vival.bars;

public sealed class VivalBar : Form
{
    #region Variables
    /// <summary>
    /// The date label.
    /// </summary>
    // ? Labels that should update after startup should all be assigned their own variable.
    // ? This is because you can change the value of it directly, only redrawing the single component rather than the entire cell-set which is much more expensive.
    private readonly Label date = new()
    {
        Text = $"{DateTime.Now:D} ({DateTime.Now:HH:m:s})", TextColor = Colors.White,
        TextAlignment =
            TextAlignment.Right // ! <= Don't try and rely on this alignment logic, it hardly even works as it should.
    };

    /// <summary>
    /// The workspaces label (current / max) - max is set as '9', change it if you have more workspaces.
    /// </summary>
    private readonly Label workspaces = new()
    {
        Text = $"{LinuxUtils.GetConsoleOut("/usr/bin/bash", "-c \"expr $(xdotool get_desktop) + 1\"")} / 9",
        TextColor = Colors.Gray
    };

    /// <summary>
    /// Padding for <see cref="DrawContent"/>.
    /// </summary>
    private readonly Padding contentPadding = new(10, 1, 0, 0);

    /// <summary>
    /// Padding for <see cref="DrawDate"/>.
    /// </summary>
    private readonly Padding datePadding = new(0, 0, 10, 0);

    /// <summary>
    /// Max and Min size.
    /// </summary>
    // * Replace 1920 with your screens width and make proper adjustments if needed.
    private readonly Size defaultSize = new(1920, 22);

    /// <summary>
    /// Spacing for <see cref="DrawContent"/>.
    /// </summary>
    private readonly Size contentSpacing = new(5, 5);
    #endregion

    /// <summary>
    /// Begin drawing the bar.
    /// </summary>
    public VivalBar()
    {
        Title = "Vival";
        DrawContent();
        // * Not like xmonad gives the slightest shit about MinimumSize but yeah.
        Size = MinimumSize = defaultSize;
        AutoSize = true;
        CanFocus = Resizable = Topmost = false;
        BackgroundColor = Color.FromArgb(10, 10, 10);
        HackSyncSize();
        UpdateBar();
        SyncWorkspace();
    }

    /// <summary>
    /// Creates a new gray "|" separator.
    /// </summary>
    /// <returns></returns>
    private TableCell Separator() => new(new Label {Text = "|", TextColor = Colors.Gray});

    /// <summary>
    /// Draws all of the content.
    /// </summary>
    private void DrawContent()
    {
        // * Begin drawing the cells.
        Content = new TableLayout
        {
            Spacing = contentSpacing,
            Padding = contentPadding,
            Rows =
            {
                new TableRow(DrawText(" ", Colors.White, 0, 3),
                    Separator(),
                    DrawText(Environment.UserName, Colors.Gray),
                    Separator(),
                    DrawText(" ", Colors.White, 5, 3),
                    workspaces,
                    Separator(),
                    DrawText(" ", Colors.White, 5, 3),
                    DrawText(LinuxUtils.GetConsoleOut("uname", "-r"), Colors.Gray),
                    Separator(),
                    DrawText(" ", Colors.White, 5, 3),
                    DrawText("GeForce RTX 3070 Ti", Colors.Gray),
                    DrawDate())
            }
        };
    }

    /// <summary>
    /// Draws text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="textColor"></param>
    /// <returns></returns>
    private TableCell DrawText(string text, Color textColor) => new(new Label
    {
        Text = text, TextColor = textColor
    });

    /// <summary>
    /// Draws text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="textColor"></param>
    /// <param name="leftOffset"></param>
    /// <param name="yOffset"></param>
    /// <returns></returns>
    private TableCell DrawText(string text, Color textColor, int leftOffset, int yOffset) => new(new TableLayout
    {
        Padding = new Padding(leftOffset, yOffset, 0, 0), Rows = {new Label {Text = text, TextColor = textColor}}
    });

    /// <summary>
    /// Draws the current date and time.
    /// </summary>
    /// <returns></returns>
    private TableCell DrawDate() =>
        new(new TableLayout
        {
            Padding = datePadding,
            Rows = {date}
        });


    /// <summary>
    /// Shitty way of trying to keep the size consistent.
    /// <para>Should be reworked in the future if possible.</para>
    /// </summary>
    private async void HackSyncSize()
    {
        while (true)
        {
            Size = defaultSize;
            Location = default;
            await Task.Delay(50);
        }
        // ReSharper disable once FunctionNeverReturns
    }

    /// <summary>
    /// Syncs the bar to the currently active workspace.
    /// <para>Thanks nukistan for helping with this.</para>
    /// </summary>
    private void SyncWorkspace() =>
        new Thread(() =>
        {
            while (true)
            {
                LinuxUtils.ExecuteCommand(
                    "xdotool search --class Vival set_desktop_for_window %@ $(xdotool get_desktop)");
                Thread.Sleep(100);
            }
        }) {Priority = ThreadPriority.Lowest, IsBackground = true}.Start();

    /// <summary>
    /// Updates the bars content.
    /// </summary>
    private void UpdateBar() =>
        new Thread(() =>
        {
            while (true)
            {
                Application.Instance.Invoke(() =>
                {
                    date.Text = $"{DateTime.Now:D} ({DateTime.Now:HH:mm:ss})";
                    workspaces.Text =
                        $"{LinuxUtils.GetConsoleOut("/usr/bin/bash", "-c \"expr $(xdotool get_desktop) + 1\"")} / 9";
                });
                Thread.Sleep(1000);
                // ! GC.Collect() is needed mainly because of the allocation issues from redrawing components.
                GC.Collect();
            }
        }) {Priority = ThreadPriority.Lowest, IsBackground = true}.Start();
}