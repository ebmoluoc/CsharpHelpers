using CsharpHelpers.Helpers;
using System;
using System.ComponentModel;
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
        private Placement _windowPlacement;


        /// <summary>
        /// Instantiate this class and set the properties before the window SourceInitialized event.
        /// </summary>
        /// <exception cref="ArgumentNullException">window cannot be null.</exception>
        public WindowPlacement(Window window)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            _window.SourceInitialized += OnSourceInitialized;
            _window.Closing += OnClosing;
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
                new BinaryFormatter().Serialize(stream, _windowPlacement);
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


        private void OnClosing(object sender, CancelEventArgs e)
        {
            // RestoreBounds is not available on closed.
            if (IsPlaceable)
                _windowPlacement = new Placement(_window);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            if (IsPlaceable)
                SavePlacement();
        }


        [Serializable]
        private class Placement
        {
            private readonly double _left;
            private readonly double _top;
            private readonly double _width;
            private readonly double _height;
            private readonly WindowState _state;

            public Placement(Window window)
            {
                var rect = window.RestoreBounds;
                _left = rect.Left;
                _top = rect.Top;
                _width = rect.Width;
                _height = rect.Height;
                _state = window.WindowState;
            }

            public void GetSizeAndPosition(Window window)
            {
                window.Left = _left;
                window.Top = _top;
                window.Width = _width;
                window.Height = _height;
                window.WindowState = GetState();
            }

            public void GetSizeOnly(Window window)
            {
                window.Width = _width;
                window.Height = _height;
            }

            public void GetPositionOnly(Window window)
            {
                window.Left = _left;
                window.Top = _top;
                window.WindowState = GetState();
            }

            private WindowState GetState()
            {
                return _state == WindowState.Minimized ? WindowState.Normal : _state;
            }
        }

    }

}
