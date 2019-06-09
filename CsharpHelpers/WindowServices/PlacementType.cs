namespace CsharpHelpers.WindowServices
{
    /// <summary>
    /// Specifies the type of window placement to save and restore. Excluding
    /// Default, the other values will ensure that the window won't be displayed
    /// outside the desktop area of the display.
    /// </summary>
    public enum PlacementType
    {
        /// <summary>
        /// Nothing is saved and restored.
        /// </summary>
        Default,
        /// <summary>
        /// The window size is saved and restored. WindowStartupLocation and Owner
        /// are taken into account when restoring the window placement.
        /// </summary>
        SizeOnly,
        /// <summary>
        /// The window position is saved and restored. WindowStartupLocation and
        /// Owner are not taken into account when restoring the window placement.
        /// </summary>
        PositionOnly,
        /// <summary>
        /// The window size and position are saved and restored. WindowStartupLocation
        /// and Owner are not taken into account when restoring the window placement.
        /// </summary>
        SizeAndPosition
    }
}
