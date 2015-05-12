namespace WindowScrape.Types
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using WindowScrape.Constants;
    using WindowScrape.Static;

    public class HwndObject
    {

        public HwndObject(IntPtr hwnd)
        {
            this.Hwnd = hwnd;
        }

        public void Activate()
        {
            HwndInterface.ActivateWindow(this.Hwnd);
        }

        public int GetWindowStyle()
        {
            WINDOWPLACEMENT placement = HwndInterface.GetPlacement(this.Hwnd);
            return placement.showCmd;
        }

        public void DisplayOnScreen(System.Windows.Forms.Screen target,bool maximizeOnScreen=true)
        {
            System.Diagnostics.Debug.Print("Window-Handle:{0} ({1}), Setze Sreen auf {2} -> {3},{4}:{5},{6} (Maximized: {7})", this.Hwnd,this.Title, target.DeviceName, target.WorkingArea.X, target.WorkingArea.Y, target.WorkingArea.X + target.WorkingArea.Width, target.WorkingArea.Y + target.WorkingArea.Height, maximizeOnScreen);
            WINDOWPLACEMENT placement = HwndInterface.GetPlacement(this.Hwnd);
            placement.ptMaxPosition = new System.Drawing.Point(target.WorkingArea.X, target.WorkingArea.Y);
            placement.ptMinPosition = new System.Drawing.Point(target.WorkingArea.X, target.WorkingArea.Y);
            placement.rcNormalPosition = new System.Drawing.Rectangle(target.WorkingArea.X, target.WorkingArea.Y, target.WorkingArea.X + target.WorkingArea.Width,target.WorkingArea.Y + target.WorkingArea.Height);
            //placement.showCmd = 2; // Minimized
            placement.flags = 0x0002;
            
            HwndInterface.SetWindowPlacement(this.Hwnd, ref placement);
            
            if (maximizeOnScreen)
                Maximize();
            
            //HwndInterface.MoveWindow(this.Hwnd, target.WorkingArea.X, target.WorkingArea.Y, target.WorkingArea.X + target.WorkingArea.Width, target.WorkingArea.Y + target.WorkingArea.Height,true);
            HwndInterface.MoveWindow(this.Hwnd, target.WorkingArea.X, target.WorkingArea.Y, target.WorkingArea.Width,  target.WorkingArea.Height, true);
        }

        public void Move(int X,int Y,int Width,int Height)
        {
            HwndInterface.MoveWindow(this.Hwnd, X, Y, Width, Height, true);
        }

        public WINDOWPLACEMENT GetWindowPlacement()
        {
            return HwndInterface.GetPlacement(this.Hwnd);
        }

        public void SetWindowPlacement(WINDOWPLACEMENT placement)
        {
            HwndInterface.SetWindowPlacement(this.Hwnd, ref placement);
        }
        public void Maximize()
        {
            HwndInterface.ShowWindow(this.Hwnd, 3);
        }
        public void Minimize()
        {
            HwndInterface.ShowWindow(this.Hwnd, 6);
        }
        public void ShowNormal()
        {
            HwndInterface.ShowWindow(this.Hwnd, 1);
        }
        public void Restore()
        {
            HwndInterface.ShowWindow(this.Hwnd, 9);
        }

        public void Click()
        {
            HwndInterface.ClickHwnd(this.Hwnd);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (!(obj.GetType() == typeof(HwndObject)))
            {
                return false;
            }
            return this.Equals((HwndObject) obj);
        }

        public bool Equals(HwndObject obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }
            return (object.ReferenceEquals(this, obj) || obj.Hwnd.Equals(this.Hwnd));
        }

        public HwndObject GetChild(string cls, string title)
        {
            return new HwndObject(HwndInterface.GetHwndChild(this.Hwnd, cls, title));
        }

        public List<HwndObject> GetChildren()
        {
            List<HwndObject> list = new List<HwndObject>();
            foreach (IntPtr ptr in HwndInterface.EnumChildren(this.Hwnd))
            {
                list.Add(new HwndObject(ptr));
            }
            return list;
        }

        public override int GetHashCode()
        {
            return this.Hwnd.GetHashCode();
        }

        public int GetMessageInt(WM msg)
        {
            return HwndInterface.GetMessageInt(this.Hwnd, msg);
        }

        public string GetMessageString(WM msg, uint param)
        {
            return HwndInterface.GetMessageString(this.Hwnd, msg, param);
        }

        public HwndObject GetParent()
        {
            return new HwndObject(HwndInterface.GetHwndParent(this.Hwnd));
        }

        public static HwndObject GetWindowByTitle(string title)
        {
            return new HwndObject(HwndInterface.GetHwndFromTitle(title));
        }

        public static List<HwndObject> GetWindows()
        {
            List<HwndObject> list = new List<HwndObject>();
            foreach (IntPtr ptr in HwndInterface.EnumHwnds())
            {
                list.Add(new HwndObject(ptr));
            }
            return list;
        }

        
        public static bool operator ==(HwndObject a, HwndObject b)
        {
            return (a.Hwnd == b.Hwnd);
        }

        public static bool operator !=(HwndObject a, HwndObject b)
        {
            return !(a == b);
        }
        
        public void SendMessage(WM msg, uint param1, string param2)
        {
            HwndInterface.SendMessage(this.Hwnd, msg, param1, param2);
        }

        public void SendMessage(WM msg, uint param1, uint param2)
        {
            HwndInterface.SendMessage(this.Hwnd, msg, param1, param2);
        }

        public void SetWindowPos(IntPtr hWndInsertAfter, int x, int y, int cx, int cy, UInt32 uFlags)
        {
            HwndInterface.SetWindowPos(this.Hwnd, hWndInsertAfter, x, y, cx, cy, uFlags);
        }

        public override string ToString()
        {
            Point location = this.Location;
            System.Drawing.Size size = this.Size;
            return string.Format("({0}) {1},{2}:{3}x{4} \"{5}\"", new object[] { this.Hwnd, location.X, location.Y, size.Width, size.Height, this.Title });
        }

        public string ClassName
        {
            get
            {
                return HwndInterface.GetHwndClassName(this.Hwnd);
            }
        }

        public IntPtr Hwnd { get; private set; }

        public Point Location
        {
            get
            {
                return HwndInterface.GetHwndPos(this.Hwnd);
            }
            set
            {
                HwndInterface.SetHwndPos(this.Hwnd, value.X, value.Y);
            }
        }

        public System.Drawing.Size Size
        {
            get
            {
                return HwndInterface.GetHwndSize(this.Hwnd);
            }
            set
            {
                HwndInterface.SetHwndSize(this.Hwnd, value.Width, value.Height);
            }
        }

        public string Text
        {
            get
            {
                return HwndInterface.GetHwndText(this.Hwnd);
            }
            set
            {
                HwndInterface.SetHwndText(this.Hwnd, value);
            }
        }

        public string Title
        {
            get
            {
                return HwndInterface.GetHwndTitle(this.Hwnd);
            }
            set
            {
                HwndInterface.SetHwndTitle(this.Hwnd, value);
            }
        }

    }
}

