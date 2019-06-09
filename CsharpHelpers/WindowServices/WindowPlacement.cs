using CsharpHelpers.Helpers;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace CsharpHelpers.WindowServices
{

    /// <summary>
    /// Window save/restore size, position or both.
    /// </summary>
    public sealed class WindowPlacement
    {

        private const string ReadOnlyPropertyExceptionMessage = "Read-only property when the SourceInitialized event is raised.";

        private readonly Window _window;
        private IntPtr _windowHandle;


        /// <summary>
        /// Instantiate this class and set the properties before the window SourceInitialized event.
        /// </summary>
        /// <exception cref="ArgumentNullException">window cannot be null.</exception>
        public WindowPlacement(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _window.SourceInitialized += OnSourceInitialized;
            _window.Closed += OnClosed;
        }


        /// <exception cref="ArgumentException">Invalid PlacementType.</exception>
        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private PlacementType _placementType;
        public PlacementType PlacementType
        {
            get { return _placementType; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                if (!Enum.IsDefined(value.GetType(), value))
                    throw new ArgumentException("Enum value not valid.", nameof(PlacementType));

                _placementType = value;
            }
        }


        /// <exception cref="InvalidOperationException">Read-only property when the SourceInitialized event is raised.</exception>
        private FileInfo _placementPath;
        public string PlacementPath
        {
            get { return _placementPath?.FullName ?? ""; }
            set
            {
                if (IsSourceInitialized)
                    throw new InvalidOperationException(ReadOnlyPropertyExceptionMessage);

                var fileInfo = new FileInfo(value);
                fileInfo.Directory.Create();
                using (var stream = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    // Just to make sure the user has sufficient permissions to read and write to this location.
                }
                _placementPath = fileInfo;
            }
        }


        private bool IsSourceInitialized
        {
            get { return _windowHandle != IntPtr.Zero; }
        }


        private bool IsPlaceable
        {
            get { return _placementType != PlacementType.Default && _placementPath != null; }
        }


        private void SavePlacement()
        {
            if (!_placementPath.Exists)
                _placementPath.Directory.Create();

            using (var stream = _placementPath.Open(FileMode.Create, FileAccess.Write))
            {
                var placement = new Placement(_window);
                new BinaryFormatter().Serialize(stream, placement);
            }
        }


        private void LoadPlacement()
        {
            var placement = GetPlacement();
            if (placement != null)
            {
                if (_placementType == PlacementType.SizeAndPosition)
                {
                    placement.GetSizeAndPosition(_window);
                    WindowHelper.AdjustWindowPosition(_window, SystemParameters.WorkArea);
                }
                else if (_placementType == PlacementType.SizeOnly)
                {
                    placement.GetSizeOnly(_window);
                    WindowHelper.SetWindowPosition(_window);
                }
                else if (_placementType == PlacementType.PositionOnly)
                {
                    placement.GetPositionOnly(_window);
                    WindowHelper.AdjustWindowPosition(_window, SystemParameters.WorkArea);
                }
            }
        }


        private Placement GetPlacement()
        {
            try
            {
                using (var stream = _placementPath.Open(FileMode.Open, FileAccess.Read))
                    return (Placement)new BinaryFormatter().Deserialize(stream);
            }
            catch (DirectoryNotFoundException) { }
            catch (FileNotFoundException) { }
            catch (ArgumentException) { }
            catch (SerializationException) { }
            catch (InvalidCastException) { }

            return null; ;
        }


        private void OnSourceInitialized(object sender, EventArgs e)
        {
            _windowHandle = WindowHelper.GetWindowHandle(_window);

            if (IsPlaceable)
                LoadPlacement();
        }


        private void OnClosed(object sender, EventArgs e)
        {
            if (IsPlaceable)
                SavePlacement();
        }


        [Serializable]
        private class Placement
        {
            private readonly double Left;
            private readonly double Top;
            private readonly double Width;
            private readonly double Height;
            private readonly WindowState State;

            public Placement(Window window)
            {
                Left = window.Left;
                Top = window.Top;
                Width = window.Width;
                Height = window.Height;
                State = window.WindowState;
            }

            public void GetSizeAndPosition(Window window)
            {
                window.Left = Left;
                window.Top = Top;
                window.Width = Width;
                window.Height = Height;
                window.WindowState = GetState();
            }

            public void GetSizeOnly(Window window)
            {
                window.Width = Width;
                window.Height = Height;
            }

            public void GetPositionOnly(Window window)
            {
                window.Left = Left;
                window.Top = Top;
                window.WindowState = GetState();
            }

            private WindowState GetState()
            {
                return State == WindowState.Minimized ? WindowState.Normal : State;
            }
        }

    }

}
