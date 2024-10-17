
namespace PrimulMeuCursDeGrafica
{
    internal class MyGraphics
    {
        public Graphics grp;
        PictureBox display;
        Bitmap bmp;
        Color backColor = Color.AliceBlue;

        public int ResX { get { return display.Width; } }
        public int ResY { get { return display.Height; } }

        public MyGraphics(PictureBox pb)
        {
            this.display = pb;
            bmp = new Bitmap(display.Width, display.Height);
            grp = Graphics.FromImage(bmp);
            Clear();
            Refresh();
        }

        public void Clear()
        {
            grp.Clear(backColor);
        }

        public void Refresh()
        {
            display.Image = bmp;
        }
    }
}