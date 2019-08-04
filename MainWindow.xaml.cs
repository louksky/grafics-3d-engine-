
/// <summary>
/// || simple drawer ex 2 GRAFICS           ||
/// || Names : Asaf Louk & nidal nawatha & Gal Argov Sofer ||
/// || Names : 311581144 & 308349109  & 302822622       ||
/// ||               ||
/// || Description: 
/// </summary>
using Microsoft.Win32;
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ex1Grafics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     public struct fp{
        public int x, xx, y, yy;
        }
public partial class MainWindow : Window
    {
        List<Ellipse> circle_list = new List<Ellipse>();
        List<Line> line_list = new List<Line>();
        List<System.Windows.Shapes.Path> curve_list = new List<System.Windows.Shapes.Path>();
        double[][] vers;
        int[][] polys;
        Point ppcen;
        fp FP;
        int scale;
        //DirectoryInfo dirInfo = new DirectoryInfo("c://");
        System.Windows.Point[] par;
        int i;
        static int NUMOFPOINTS = 4;
        int shapeFlag = 0; // 1 - circule , 2 - line , 3 - curve 
        public MainWindow()
        {
            InitializeComponent();

            par = new System.Windows.Point[4];
            for (int i = 0; i < par.Length; ++i) par[i] = new System.Windows.Point();

            canvas.DefaultDrawingAttributes.Color = Colors.Transparent;
            canvas.UseCustomCursor = true;


            scale = (int)this.Width / 30;
            i = 0;
            dpartinit();
            dodraw();
        }

        //האסיקס מנורמל מוחזר NAsix_sky
        //N_ נרמול צורה
        //NVec_sky ווקטור מנורמל

        private void dpartinit()
        {
            try
            {



                string parse = File.ReadAllText("ver.txt");
                string[] lines = parse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                vers = new double[lines.Length][];
                for (int k = 0; k < lines.Length; k++)
                    vers[k] = new double[3];
                int i = 0, j = 0;
                foreach (string line in lines)
                {
                    j = 0;
                    string[] split = line.Split(new[] { ",", "[", "]", " ", "" }, StringSplitOptions.None);
                    foreach (string s in split)
                    {
                        if (s != "")
                        {
                            vers[i][j++] = 6 * float.Parse(s) / 10;
                        }
                    }
                    i++;

                }


                parse = File.ReadAllText("pol.txt");
                lines = parse.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                polys = new int[lines.Length][];
                for (int k = 0; k < polys.GetLength(0); k++)
                {
                    polys[k] = new int[4];

                }

                i = j = 0;
                foreach (string line in lines)
                {
                    j = 0;
                    string[] split = line.Split(new[] { ",", "[", "]", " ", "" }, StringSplitOptions.None);
                    foreach (string s in split)
                    {
                        if (s != "")
                        {
                            polys[i][j++] = int.Parse(s);
                        }
                    }
                    i++;

                }

                i++;
            }
            catch (Exception)
            {
                return;
            }
        }
       
        public Point to2D(Point3D Point3d ,int Flag)
        {
            double NewX, NewY;

            if (Flag == 1)
            { // PRESPICTIVE
                NewX = Point3d.X;
                NewY = Point3d.Y;

                return new Point(NewX, NewY);
            }
            if (Flag == 0)
            {//PARALLEL
                NewX = Point3d.X /(1+Point3d.Z/1348);
                NewY = Point3d.Y / (1 + Point3d.Z / 1348);

                return new Point( NewX,NewY);

            }
            else
            {
                double deg = 45 / 180 * Math.PI;
                return new Point(Point3d.X + Point3d.Z * Math.Cos(deg), Point3d.Y + Point3d.Z * Math.Sin(deg));
            }
        }


        
        private void cennn()
        {
            double maxx, maxy, mix, miy;
            maxx = maxy = 0;
            mix = miy = 600;
            foreach(double [] verx in vers)
            {
                if (mix > verx[0]) mix = verx[0];
                if (maxx < verx[0]) maxx = verx[0];

                if (miy > verx[1]) miy = verx[1];
                if (maxy < verx[1]) maxy = verx[1];
            }
            mss.X = 150;
            mss.Y = 150;
        }
        private void dodraw()
        {

            cennn();

            canvas.Children.Clear();
            bolp = true;
            try
            {
                float x, y, z;
                foreach (int[] pol in polys)
                {
                    Line l;

                   

                    
                    for (int i=0; i < pol.Length-1; i++)
                    {
                        if (pol[i] == 50 || pol[i + 1] == 50) continue;



                         l = new Line();
                        Point3D a, b;
                        a = new Point3D();
                        b = new Point3D();
                        a.X = Convert.ToInt32(vers[pol[i]][0]);
                        b.X = Convert.ToInt32(vers[pol[i + 1]][0]);
                        a.Y = Convert.ToInt32(vers[pol[i]][1]);
                        b.Y = Convert.ToInt32(vers[pol[i + 1]][1]);
                        a.Z = Convert.ToInt32(vers[pol[i]][2]);
                        b.Z = Convert.ToInt32(vers[pol[i + 1]][2]);
                        Point p = to2D(a, visu);
                        Point p2 = to2D(b, visu);
                        a.X = p.X;
                        a.Y = p.Y;
                        b.X = p2.X;
                        b.Y = p2.Y;
                        l.X1 = a.X;
                        l.Y1 = a.Y;
                        l.X2 = b.X;
                        l.Y2 = b.Y;

                        l.StrokeThickness = 1;
                        l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                        canvas.Children.Add(l);


                    }
                    if (pol[i] == 50 || pol[pol.Length-1] == 50)
                    {
                        l = new Line();
                        Point3D a, b;
                        a = new Point3D();
                        b = new Point3D();
                        a.X = Convert.ToInt32(vers[pol[i]][0]);
                        b.X = Convert.ToInt32(vers[pol[i + 1]][0]);
                        a.Y = Convert.ToInt32(vers[pol[i]][1]);
                        b.Y = Convert.ToInt32(vers[pol[i + 1]][1]);
                        a.Z = Convert.ToInt32(vers[pol[i]][2]);
                        b.Z = Convert.ToInt32(vers[pol[i + 1]][2]);
                        Point p = to2D(a, visu);
                        Point p2 = to2D(b, visu);
                        a.X = p.X;
                        a.Y = p.Y;
                        b.X = p2.X;
                        b.Y = p2.Y;
                        l.X1 = a.X;
                        l.Y1 = a.Y;
                        l.X2 = b.X;
                        l.Y2 = b.Y;


                        l.StrokeThickness = 1;
                        l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                        canvas.Children.Add(l);
                    }
                    else
                    {
                        l = new Line();
                        Point3D a, b;
                        a = new Point3D();
                        b = new Point3D();
                        a.X = Convert.ToInt32(vers[pol[i]][0]);
                        b.X = Convert.ToInt32(vers[pol[i + 1]][0]);
                        a.Y = Convert.ToInt32(vers[pol[i]][1]);
                        b.Y = Convert.ToInt32(vers[pol[i + 1]][1]);
                        a.Z = Convert.ToInt32(vers[pol[i]][2]);
                        b.Z = Convert.ToInt32(vers[pol[i + 1]][2]);
                        Point p = to2D(a, visu);
                        Point p2 = to2D(b, visu);
                        a.X = p.X;
                        a.Y = p.Y;
                        b.X = p2.X;
                        b.Y = p2.Y;
                        l.X1 = a.X;
                        l.Y1 = a.Y;
                        l.X2 = b.X;
                        l.Y2 = b.Y;

                        l.StrokeThickness = 1;
                        l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                        canvas.Children.Add(l);
                    }
                    
                    //Point p =  ProjectBUS(2, pol[0], pol[1], pol[2]);
                    //draw..
                }
                
            }
            catch (Exception x)
            {
                return;
            }
        }
        
        private void hazaza(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            int rangex =0 , rangey =0;
           
           

            
            if(!(rangex<30 && rangex > -5 && rangey < 30 && rangey > -5))
            {
                MessageBox.Show("values should be 30 to -5 x and y ");
                return;
            } 
            string path = str;
            using (StreamReader sr = File.OpenText(path))
            {
                int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                string s = "";
                char[] outline = { '(', ',', ')' };
                while ((s = sr.ReadLine()) != null)
                {

                    if (s.IndexOf("#") > -1)
                    {
                        switch (s)
                        {
                            case "#Lines":
                                while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                {

                                    string[] arr = s.Split(outline);
                                    arr = func(arr);
                                    x1 = int.Parse(arr[0]) + rangex;
                                    y1 = int.Parse(arr[1]) + rangey;
                                    x2 = int.Parse(arr[2]) + rangex;
                                    y2 = int.Parse(arr[3]) + rangey;
                                    x1 *= scale; y1 *= scale; x2 *= scale; y2 *= scale;
                                    Line l = new Line();

                                    l.X1 = x1;
                                    l.Y1 = y1;
                                    l.X2 = x2;
                                    l.Y2 = y2;

                                    l.StrokeThickness = 4;
                                    l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                                    canvas.Children.Add(l);

                                }
                                break;
                            case "#Circles":
                                while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                {

                                    string[] arr = s.Split(outline);
                                    arr = func(arr);

                                    x1 = int.Parse(arr[0])+ rangex;
                                    y1 = int.Parse(arr[1])+ rangey;
                                    //x2 = int.Parse(arr[2]);
                                    y2 = int.Parse(arr[3]); // RADIUS 
                                    x1 *= scale; y1 *= scale; y2 *= scale;
                                    Ellipse ell = new Ellipse();

                                    ell.Width = 2 * y2;
                                    ell.Height = 2 * y2;
                                    InkCanvas.SetTop(ell, y1 - y2);
                                    InkCanvas.SetLeft(ell, x1 - y2);



                                    ell.StrokeThickness = 4;
                                    ell.Fill = System.Windows.Media.Brushes.Transparent;
                                    ell.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                                    canvas.Children.Add(ell);




                                }
                                break;
                            case "#Cruves":
                                while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                {

                                    string[] arr = s.Split(outline);
                                    arr = func(arr);
                                    Point p1 = new Point((int.Parse(arr[0]) + rangex) * scale , (int.Parse(arr[1]) + rangey) * scale);
                                    Point p2 = new Point((int.Parse(arr[2]) + rangex) * scale, (int.Parse(arr[3]) + rangey) * scale );
                                    Point p3 = new Point((int.Parse(arr[4]) + rangex) * scale, (int.Parse(arr[5]) + rangey) * scale );
                                    Point p4 = new Point((int.Parse(arr[6]) + rangex) * scale , (int.Parse(arr[7]) + rangey) * scale );
                                    Point[] points = new[] { p1, p2, p3, p4 };
                                    bezier bz = new bezier();
                                    var Bezier_btb = bz.CreatebezierLinepoly(points, 256);
                                    PathFigure pathDiscriptionFigure = new PathFigure(Bezier_btb.Points[0], new[] { Bezier_btb }, false);
                                    PathFigureCollection pathColletion_ = new PathFigureCollection();
                                    pathColletion_.Add(pathDiscriptionFigure);
                                    var GeomtricPath = new PathGeometry();
                                    GeomtricPath.Figures = pathColletion_;
                                    System.Windows.Shapes.Path Path_Shapepattern = new System.Windows.Shapes.Path();
                                    Path_Shapepattern.Data = GeomtricPath;
                                    Path_Shapepattern.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    canvas.Children.Add(Path_Shapepattern);


                                }
                                break;
                        }
                    }

                }
            }

        }

        private void circule_clicked(object sender, RoutedEventArgs e)
        {
            shapeFlag = 1;
        }
        private string[] func(string [] crr)
        {
            string[] nrr = new string[8];
            int j = 0;
            for(int i = 0; i < crr.Length; i++)
            {
                if (crr[i] != "" && crr[i] != " ")
                {
                    nrr[j] = crr[i];
                    j++;
                }
            }
            return nrr;
        }
        private void pharseTextFile(object sender, RoutedEventArgs e)
        {
            
            try
            {
                string path = str;
                using (StreamReader sr = File.OpenText(path))
                {
                    int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                    string s = "";
                    char[] outline = { '(', ',', ')' };
                    while ((s = sr.ReadLine()) != null)
                    {

                        if (s.IndexOf("#") > -1)
                        {
                            switch (s)
                            {
                                case "#Lines":
                                    while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                    {

                                        string[] arr = s.Split(outline);
                                        arr = func(arr);
                                        x1 = int.Parse(arr[0]);
                                        y1 = int.Parse(arr[1]);
                                        x2 = int.Parse(arr[2]);
                                        y2 = int.Parse(arr[3]);
                                        x1 *= scale; y1 *= scale; x2 *= scale; y2 *= scale;
                                        Line l = new Line();

                                        l.X1 = x1;
                                        l.Y1 = y1;
                                        l.X2 = x2;
                                        l.Y2 = y2;

                                        l.StrokeThickness = 4;
                                        l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                                        line_list.Add(l);
                                        canvas.Children.Add(l);

                                    }
                                    break;
                                case "#Circles":
                                    while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                    {

                                        string[] arr = s.Split(outline);
                                        arr = func(arr);

                                        x1 = int.Parse(arr[0]);
                                        y1 = int.Parse(arr[1]);
                                        //x2 = int.Parse(arr[2]);
                                        y2 = int.Parse(arr[3]); // RADIUS 
                                        x1 *= scale; y1 *= scale; y2 *= scale;
                                        Ellipse ell = new Ellipse();

                                        ell.Width = 2 * y2;
                                        ell.Height = 2 * y2;
                                        InkCanvas.SetTop(ell, y1 - y2);
                                        InkCanvas.SetLeft(ell, x1 - y2);



                                        ell.StrokeThickness = 4;
                                        ell.Fill = System.Windows.Media.Brushes.Transparent;
                                        ell.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                                        circle_list.Add(ell);
                                        canvas.Children.Add(ell);




                                    }
                                    break;
                                case "#Cruves":
                                    while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                    {

                                        string[] arr = s.Split(outline);
                                        arr = func(arr);
                                        Point p1 = new Point(int.Parse(arr[0]) * scale, int.Parse(arr[1]) * scale);
                                        Point p2 = new Point(int.Parse(arr[2]) * scale, int.Parse(arr[3]) * scale);
                                        Point p3 = new Point(int.Parse(arr[4]) * scale, int.Parse(arr[5]) * scale);
                                        Point p4 = new Point(int.Parse(arr[6]) * scale, int.Parse(arr[7]) * scale);
                                        Point[] points = new[] { p1, p2, p3, p4 };
                                        bezier bz = new bezier();
                                        var Bezier_btb = bz.CreatebezierLinepoly(points, 256);
                                        PathFigure pathDiscriptionFigure = new PathFigure(Bezier_btb.Points[0], new[] { Bezier_btb }, false);
                                        PathFigureCollection pathColletion_ = new PathFigureCollection();
                                        pathColletion_.Add(pathDiscriptionFigure);
                                        var GeomtricPath = new PathGeometry();
                                        GeomtricPath.Figures = pathColletion_;
                                        System.Windows.Shapes.Path Path_Shapepattern = new System.Windows.Shapes.Path();
                                        Path_Shapepattern.Data = GeomtricPath;
                                        Path_Shapepattern.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));


                                        curve_list.Add(Path_Shapepattern);
                                        canvas.Children.Add(Path_Shapepattern);


                                    }
                                    break;
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please choose file first.");
            }

        }
        private void line_clicked(object sender, RoutedEventArgs e)
        {
           
            shapeFlag = 2;
        }

        private void curve_clicked(object sender, RoutedEventArgs e)
        {
            shapeFlag = 3;
        }
        int gameclick = 0;
        private void preview_mousedown(object sender, MouseButtonEventArgs e)
        {
            if(shapeFlag < 3)
            {

                if (gameclick%2==0)
                {
                    par[0] = Mouse.GetPosition(canvas);
                    gameclick += 1;
                }
                else
                {
                    par[1] = Mouse.GetPosition(canvas);
                    gameclick -= 1;
                }
                

            }
            else
            {
               
                    if (i == 4) i = 0;
                    par[i] = Mouse.GetPosition(canvas);
                    i++;
                   
                
               
            }
        }

        private void preview_mouseup(object sender, MouseButtonEventArgs e)
        {
            if(gameclick%2==0)
            try
            {
                if (shapeFlag < 3)
                {
                    if(gameclick%2==0)
                    par[1] = Mouse.GetPosition(canvas);

                }
                else
                {

                }
                switch (shapeFlag)
                {

                    case 1:

                        Ellipse ell = new Ellipse();
                        if (par[0].X < par[1].X)
                        {
                            ell.Width = par[1].X - par[0].X;
                            ell.Height = par[1].Y - par[0].Y;
                            InkCanvas.SetTop(ell, par[0].Y);
                            InkCanvas.SetLeft(ell, par[0].X);
                        }
                        else
                        {
                            ell.Width = par[0].X - par[1].X;
                            ell.Height = par[0].Y - par[1].Y;
                            InkCanvas.SetTop(ell, par[1].Y);
                            InkCanvas.SetLeft(ell, par[1].X);
                        }
                        ell.StrokeThickness = 4;
                        ell.Fill = System.Windows.Media.Brushes.Transparent;
                        ell.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                        canvas.Children.Add(ell);

                        break;
                    case 2:
                        Line l = new Line();

                        l.X1 = par[0].X;
                        l.Y1 = par[0].Y;
                        l.X2 = par[1].X;
                        l.Y2 = par[1].Y;

                        l.StrokeThickness = 4;
                        l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                        canvas.Children.Add(l);
                        break;
                    case 3:
                        if (i == 4)
                        {
                           
                          
                            
                            Point[] points = new[] { par[0], par[1], par[2], par[3] };
                            bezier bz = new bezier();
                            var Bezier_btb = bz.CreatebezierLinepoly(points, 256);
                            PathFigure pathDiscriptionFigure = new PathFigure(Bezier_btb.Points[0], new[] { Bezier_btb }, false);
                            PathFigureCollection pathColletion_ = new PathFigureCollection();
                            pathColletion_.Add(pathDiscriptionFigure);
                            var GeomtricPath = new PathGeometry();
                            GeomtricPath.Figures = pathColletion_;
                            System.Windows.Shapes.Path Path_Shapepattern = new System.Windows.Shapes.Path();
                            Path_Shapepattern.Data = GeomtricPath;
                            Path_Shapepattern.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));


                            curve_list.Add(Path_Shapepattern);
                            canvas.Children.Add(Path_Shapepattern);
                        }
                        break;
                }
                    }catch(Exception) { return; }
        }
        
        public Line nline(System.Windows.Point p1, System.Windows.Point p2)
        {

            Line l = new Line();
            l.X1 = p1.X;
            l.Y1 = p1.Y;
            l.X2 = p2.X;
            l.Y2 = p2.Y;

            l.StrokeThickness = 4;
            l.Stroke = System.Windows.Media.Brushes.PowderBlue;
            return l; 

        }

        private void clean(object sender, RoutedEventArgs e)
        {
            
            canvas.Children.Clear();
        }
        private string str = @"C:\Users\asafl\source\repos\ex1Grafics\ex1Grafics\text.txt";
        private void openfile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                str = openFileDialog.FileName;
            }
        }
        private void silum(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
           
            double sun=0;
            try
            {
                //sun = double.Parse(silumto.Text);
                if (sun > 1)
                {
                    canvas.Width *= sun;
                    canvas.Height *= sun;
                }
                
            }
            catch (Exception)
            {
                return;
            }
            
            string path = str;
            using (StreamReader sr = File.OpenText(path))
            {
                double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                string s = "";
                char[] outline = { '(', ',', ')' };
                while ((s = sr.ReadLine()) != null)
                {
                    
                    if (s.IndexOf("#") > -1)
                    {
                        switch (s)
                        {
                            case "#Lines":
                                while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                {

                                    string[] arr = s.Split(outline);
                                    arr = func(arr);
                                    x1 = int.Parse(arr[0]) * sun;
                                    y1 = int.Parse(arr[1]) * sun;
                                    x2 = int.Parse(arr[2]) * sun;
                                    y2 = int.Parse(arr[3]) * sun;
                                    x1 *= scale; y1 *= scale; x2 *= scale; y2 *= scale;
                                    Line l = new Line();

                                    l.X1 = x1;
                                    l.Y1 = y1 ;
                                    l.X2 = x2 ;
                                    l.Y2 = y2 ;

                                    l.StrokeThickness = 4;
                                    l.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                                    canvas.Children.Add(l);

                                }
                                break;
                            case "#Circles":
                                while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                {

                                    string[] arr = s.Split(outline);
                                    arr = func(arr);

                                    x1 = int.Parse(arr[0]) * sun;
                                    y1 = int.Parse(arr[1]) * sun;
                                    //x2 = int.Parse(arr[2]);
                                    y2 = int.Parse(arr[3]) * sun; // RADIUS 
                                    x1 *= scale; y1 *= scale; y2 *= scale;
                                    Ellipse ell = new Ellipse();

                                    ell.Width = 2 * y2;
                                    ell.Height = 2 * y2;
                                    InkCanvas.SetTop(ell, y1 - y2);
                                    InkCanvas.SetLeft(ell, x1 - y2);



                                    ell.StrokeThickness = 4;
                                    ell.Fill = System.Windows.Media.Brushes.Transparent;
                                    ell.Stroke = System.Windows.Media.Brushes.LightSteelBlue;

                                    canvas.Children.Add(ell);




                                }
                                break;
                            case "#Cruves":
                                while ((s = sr.ReadLine()) != null && s != "" && s.Length > 2)
                                {

                                    string[] arr = s.Split(outline);
                                    arr = func(arr);
                                    Point p1 = new Point((int.Parse(arr[0]) * sun) * scale,( int.Parse(arr[1]) * sun) * scale);
                                    Point p2 = new Point((int.Parse(arr[2]) * sun) * scale, (int.Parse(arr[3]) * sun)*scale);
                                    Point p3 = new Point((int.Parse(arr[4]) * sun) * scale, (int.Parse(arr[5]) * sun)*scale);
                                    Point p4 = new Point((int.Parse(arr[6]) * sun) * scale, (int.Parse(arr[7]) * sun) * scale);
                                    Point[] points = new[] { p1, p2, p3, p4 };
                                    bezier bz = new bezier();
                                    var Bezier_btb = bz.CreatebezierLinepoly(points, 500);
                                    PathFigure pathDiscriptionFigure = new PathFigure(Bezier_btb.Points[0], new[] { Bezier_btb }, false);
                                    PathFigureCollection pathColletion_ = new PathFigureCollection();
                                    pathColletion_.Add(pathDiscriptionFigure);
                                    var GeomtricPath = new PathGeometry();
                                    GeomtricPath.Figures = pathColletion_;
                                    System.Windows.Shapes.Path Path_Shapepattern = new System.Windows.Shapes.Path();
                                    Path_Shapepattern.Data = GeomtricPath;
                                    Path_Shapepattern.Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                                    canvas.Children.Add(Path_Shapepattern);


                                }
                                break;
                        }
                    }

                }
            }
        }
        double cou = 0.2;
        private void sivuv(object sender, RoutedEventArgs e)
        {
            int tempo = 45;
            try
            {
                if (tempo > 0 && tempo < 360)
                    tempo = int.Parse(zavit.Text);
                else
                    tempo = 0;
            }
            catch (Exception)
            {
                return;
            }
                cou += tempo;
            RotateTransform myRotateTransform = new RotateTransform(cou);
            canvas.RenderTransform = myRotateTransform;
        }
        private void sivuv_(object sender, RoutedEventArgs e)
        {
            try
            {

                //כפץור סיבוב מוביל לכאן 
                //כל לולאה עוברת על : עיגולים קווים עקומות 
                // בכל פעם צריך לשים x y 
                // getleft -> x gettop -> y && set
                float suni = float.Parse(zavit.Text);//מתקבל מהגויי 
                foreach (Ellipse cir in circle_list)
                {
                    
                    double nx1 = Math.Cos(suni) * InkCanvas.GetLeft(cir) + Math.Sin(suni) * InkCanvas.GetTop(cir);
                    double ny1 = -Math.Sin(suni) * InkCanvas.GetLeft(cir) + Math.Cos(suni) * InkCanvas.GetTop(cir);
                    InkCanvas.SetTop(cir, nx1);//מכניס x
                    InkCanvas.SetLeft(cir, ny1);//מכניס y
                    canvas.Children.Add(cir);
                }
                foreach (Line cir in line_list)
                {
                    double nx1 = Math.Cos(suni) * InkCanvas.GetLeft(cir) + Math.Sin(suni) * InkCanvas.GetTop(cir);
                    double ny1 = -Math.Sin(suni) * InkCanvas.GetLeft(cir) + Math.Cos(suni) * InkCanvas.GetTop(cir);
                    InkCanvas.SetTop(cir, nx1);
                    InkCanvas.SetLeft(cir, ny1);
                    canvas.Children.Add(cir);
                }
                foreach (System.Windows.Shapes.Path cir in curve_list)
                {
                    double nx1 = Math.Cos(suni) * InkCanvas.GetLeft(cir) + Math.Sin(suni) * InkCanvas.GetTop(cir);
                    double ny1 = -Math.Sin(suni) * InkCanvas.GetLeft(cir) + Math.Cos(suni) * InkCanvas.GetTop(cir);
                    InkCanvas.SetTop(cir, nx1);
                    InkCanvas.SetLeft(cir, ny1);
                }

            }
            catch (Exception)
            {
                return;
            }

        }

        private void load(object sender, RoutedEventArgs e)
        {
            
        }
        private byte[] SignatureToBitmapBytes()
        {
            
            int margin = (int)this.canvas.Margin.Left;
            int width = (int)this.canvas.ActualWidth - margin;
            int height = (int)this.canvas.ActualHeight - margin;
            
            RenderTargetBitmap rtb =
            new RenderTargetBitmap(width, height, 96d, 96d, PixelFormats.Default);
            rtb.Render(canvas);
            
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            byte[] bitmapBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                
                ms.Position = 0;
                bitmapBytes = ms.ToArray();
            }
            return bitmapBytes;
        }
        int visu = 0;
        private void visu_(object sender, RoutedEventArgs e)
        {
            visu = 0;
            canvas.Children.Clear();
            dodraw();
        }
        private void visu1(object sender, RoutedEventArgs e)
        {
            
            visu = 1;
            canvas.Children.Clear();
            dodraw();

        }
        private void visu2(object sender, RoutedEventArgs e)
        {
            visu = 2;
            canvas.Children.Clear();
            dodraw();
        }

        private void Menu_Choose_byuserinput(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                
                case Key.B://curve 66 (ASCII)
                    shapeFlag = 3;
                    break;
                case Key.C: //circule 67 (ASCII)
                    shapeFlag = 1;
                    break;
                case Key.K://clear 75 (ASCII)
                    canvas.Children.Clear();
                    break;
                case Key.L: //line 76 (ASCII)
                    shapeFlag = 2;
                    break;

            }
        }

        private void about(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Creators " + Environment.NewLine + "Gal 302822622" + Environment.NewLine + " Asaf 311581144");
        }

        private void sizechaanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            label.Content = "x :" + e.GetPosition(canvas).X;
            label2.Content = "y :" + e.GetPosition(canvas).Y;
        }

        private void now(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void ddr(object sender, RoutedEventArgs e)
        {
            dodraw();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void plus(object sender, RoutedEventArgs e)
        {
            try
            {
                double t = double.Parse(godel.Text);
                t += 0.32;
                godel.Text = ""+t;
            }
            catch (Exception)
            {
                return;
            }
        }

        private void minu(object sender, RoutedEventArgs e)
        {
            try
            {
                double t = double.Parse(godel.Text);
                t -= 0.2;
                if (t <= 0) t = 0.3;
                godel.Text = "" + t;
            }
            catch (Exception)
            {
                return;
            }
        }
        bool bolp = false;
        Point mss = new Point();
        private void Scale()
        {
            try
            {
                if (bolp)
                {
                    double hagdala = double.Parse(godel.Text);
                    if (hagdala < 0.3)
                    {
                        hagdala += 0.2;
                        return;
                    }
                    if (hagdala > 5.5)
                    {
                        hagdala -= 0.2;
                        return;
                    }
                    // hagdala = (Math.PI / 180) * hagdala;
                    foreach (double[] verx in vers)
                    {
                        verx[0] = (double)(hagdala * (verx[0] - mss.X)) + mss.X;
                        verx[1] = (double)(hagdala * (verx[1] - mss.Y)) + mss.Y;
                        verx[2] *= (double)hagdala;
                    }
                    //Clear
                    dodraw();
                }

            }
            catch (Exception) { return; }
            
            

        }
        private void draw_res(object sender, DependencyPropertyChangedEventArgs e)
        {
            Scale();
        }

        private void draw_res2(object sender, TextChangedEventArgs e)
        {
            Scale();
        }
       private void  Rotate(int flag)
        {
            double zooz = double.Parse(zavit.Text);
   
            foreach (double[] verx in vers)
            {
                if (flag == 1)
                { // RotateX
                    verx[1] = (verx[1] - mss.Y * Math.Cos(zooz) -verx[2] * Math.Sin(zooz)) + mss.Y;
                    verx[2] = (verx[1] - mss.Y * Math.Sin(zooz) + verx[2] * Math.Cos(zooz));
                }
                if (flag == 2)
                {//RotateY
                    verx[0] = (verx[0] - mss.X * Math.Cos(zooz) - verx[2] * Math.Sin(zooz)) + mss.X;
                    verx[2] = (verx[0] - mss.X * Math.Sin(zooz) + verx[2] * Math.Cos(zooz));
                }
                if (flag == 3)
                {//RotateZ
                    verx[0] = (verx[0] - mss.X * Math.Cos(zooz) - verx[1] - 300 * Math.Sin(zooz)) + mss.X;
                    verx[1] = (verx[0] - mss.Y * Math.Sin(zooz) + verx[1] - 300 * Math.Cos(zooz)) + mss.Y;
                }
            }
            dodraw();
                

        }
        private void rotate(object sender, RoutedEventArgs e)
        {
            Rotate(1);
        }
        private void rotate2(object sender, RoutedEventArgs e)
        {
            Rotate(2);
        }
        private void rotatez(object sender, RoutedEventArgs e)
        {
            Rotate(3);
        }

        private void loadddd(object sender, RoutedEventArgs e)
        {
            dpartinit();
            dodraw();
        }
    }
}
