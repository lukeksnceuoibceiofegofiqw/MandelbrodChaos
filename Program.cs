using System;
using System.Resources;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;


//global variables
/////////////////////////////////////////////////////////////////////////

//setup variables, screen en display van de mandelbrot figuur
int xscrsize = 1000;
int yscrsize = 810;

int xdissize = 750;
int ydissize = 750;
int ydisplay = 40;

//mandelbrot variables
int maxiterations = 750;
// 1 zijn de coördinaten van de linker bovenhoek op het scherm, 2 van de rechter onderhoek
double x1 = -2;
double x2 = 2;
double y1 = -2;
double y2 = 2;

//control variables

bool pointclicked = false;
int clickx = 0;
int clicky = 0;
int color = 1;
bool boxselect = false;
bool hassupercomputer = false;

//ui 1 custom colors
int r_constant = 100;
int g_constant = 100;
int b_constant = 100;
//variabele voor de mod functie
int r_mod_amplitude = 100;
int g_mod_amplitude = 100;
int b_mod_amplitude = 100;

int r_mod = 50;
int g_mod = 50;
int b_mod = 50;

int r_mod_translation = 0;
int g_mod_translation = 0;
int b_mod_translation = 0;
//s curve variabele
double r_scurve_base = 1;
double g_scurve_base = 1;
double b_scurve_base = 1;

int r_scurve_translation = 0;
int g_scurve_translation = 0;
int b_scurve_translation = 0;

int r_scurve_amplitude = 0;
int g_scurve_amplitude = 0;
int b_scurve_amplitude = 0;

//variable
double r_wave_frequency = 2;
double g_wave_frequency = 2;
double b_wave_frequency = 2;

int r_wave_amplitude = 0;
int g_wave_amplitude = 0;
int b_wave_amplitude = 0;

int r_wave_translation = 0;
int g_wave_translation = 0;
int b_wave_translation = 0;
//ui 2
double spot1x = -0.108625;
double spot1y = 0.9014428;
double spot1scale = 4E-8;

double spot2x = -0.0495;
double spot2y = -0.67459375;
double spot2scale = 5E-6;

double spot3x = 0.3514221246;
double spot3y = -0.0638533155;
double spot3scale = 6.1E-6;
//ui3 user spots
List<UserSpot> userspots = new List<UserSpot> { };

//Global UI elements
//////////////////////////////////////////////////////////////////////////

//maakt scherm met dimensies van hierboven
Form scherm = new Form();
scherm.Text = "madelbrot";
scherm.ClientSize = new Size(xscrsize, yscrsize);

//display waarop de mandelbrot wordt gerendert
PictureBox display = new PictureBox(); scherm.Controls.Add(display);
display.Location = new Point(20, ydisplay);
display.Size = new Size(xdissize, ydissize);
Bitmap mandelbrot = new Bitmap(xdissize, ydissize);

//knop keert terug naar begin situatie
Button resetbutton = new Button(); scherm.Controls.Add(resetbutton);
resetbutton.Location = new Point(20 + (xdissize / 2) - 50, 0);
resetbutton.Text = "reset";
resetbutton.Size = new Size(100, 20);

//knoppen voor de kleur
Label colortoolbelttext = new Label(); scherm.Controls.Add(colortoolbelttext);
colortoolbelttext.Text = "color:"; colortoolbelttext.Location = new Point(0, 0);
colortoolbelttext.Size = new Size(36, 25);

ToolStrip colorselector = new ToolStrip(); scherm.Controls.Add(colorselector);

//eerste element is alleen om de toolbelt naar rechts te verplaatsen, zodat de "color:" ervoor kan :)
colorselector.Items.Add("  	");

colorselector.Items.Add("1");
colorselector.Items.Add("2");
colorselector.Items.Add("3");
colorselector.Items.Add("4");
colorselector.Items.Add("5");
colorselector.Items.Add("6");
colorselector.Items.Add("7");
colorselector.Items.Add("8");
colorselector.Items.Add("9");
colorselector.Items.Add("10");
colorselector.Size = new Size(100, 20);

//checkbox van de boxzoom
Label boxselecttext = new Label(); scherm.Controls.Add(boxselecttext);
boxselecttext.Text = "box zoom:"; boxselecttext.Location = new Point(20 + xdissize - 270, 20); boxselecttext.Size = new Size(70, 20);
CheckBox boxselectbox = new CheckBox(); scherm.Controls.Add(boxselectbox);
boxselectbox.Location = new Point(20 + xdissize - 200, 20);

//dropdown ui selector
ComboBox options = new ComboBox(); scherm.Controls.Add(options);
options.Items.Add("Text input");
options.Items.Add("Custom color");
options.Items.Add("Cool spots");
options.Items.Add("User spots");
options.Location = new Point(xdissize - 80, 20);
options.Size = new Size(100, 20);
options.Text = "Text input";

//ui selection 0 Text input
TextBox xloc = new TextBox(); scherm.Controls.Add(xloc);
xloc.Text = "0"; xloc.Location = new Point(100 + xdissize, 50);
Label xtext = new Label(); scherm.Controls.Add(xtext);
xtext.Text = "midden x:"; xtext.Location = new Point(40 + xdissize, 50);
TextBox yloc = new TextBox(); scherm.Controls.Add(yloc);
yloc.Text = "0"; yloc.Location = new Point(100 + xdissize, 80);
Label ytext = new Label(); scherm.Controls.Add(ytext);
ytext.Text = "midden y:"; ytext.Location = new Point(40 + xdissize, 80);
TextBox scale = new TextBox(); scherm.Controls.Add(scale);
scale.Text = "0,01"; scale.Location = new Point(100 + xdissize, 110);
Label stext = new Label(); scherm.Controls.Add(stext);
stext.Text = "scale:"; stext.Location = new Point(40 + xdissize, 110);
TextBox iterationsbox = new TextBox(); scherm.Controls.Add(iterationsbox);
iterationsbox.Text = $"{maxiterations}"; iterationsbox.Location = new Point(100 + xdissize, 140);
Label itext = new Label(); scherm.Controls.Add(itext);
itext.Text = "iterations:"; itext.Location = new Point(40 + xdissize, 140);
Button gobutton = new Button(); scherm.Controls.Add(gobutton);
gobutton.Text = "GO!"; gobutton.Location = new Point(40 + xdissize, 170);
Button changeiterations = new Button(); scherm.Controls.Add(changeiterations);
changeiterations.Text = "only iterations"; changeiterations.Location = new Point(140 + xdissize, 170); changeiterations.Size = new Size(100, 25);
Button getlocationbutton = new Button(); scherm.Controls.Add(getlocationbutton);
getlocationbutton.Text = "get location"; getlocationbutton.Location = new Point(40 + xdissize, 20); getlocationbutton.Size = new Size(100, 25);

//knop om de huidige bitmap van de mandelbrot in als jpeg op te slaan
Button save = new Button(); scherm.Controls.Add(save); save.Size = new Size(200, 40); save.Font = new Font("Arial", 20); save.BackColor = Color.Red;
save.Text = "Screenshot"; save.Location = new Point(40 + xdissize, 220);

//checkbox voor gebruiker bescherming max iteraties gelimiteerd naar 3000 tenzij de checkbox is gechecked
Label supercomputertext = new Label(); scherm.Controls.Add(supercomputertext);
supercomputertext.Text = "I have a Supercomputer"; supercomputertext.Location = new Point(xdissize + 40, 400); supercomputertext.Size = new Size(155, 20); supercomputertext.Font = new Font("Arial", 10);
CheckBox supercomputer = new CheckBox(); scherm.Controls.Add(supercomputer);
supercomputer.Location = new Point(xdissize + 200, 399);


//ui selection 1 Custom color
//enable button
Button activate_custom_color = new Button(); scherm.Controls.Add(activate_custom_color);
activate_custom_color.Text = "Activate Custom Color"; activate_custom_color.Size = new Size(200, 25);
activate_custom_color.Location = new Point(40 + xdissize, 560);
//color previeuw
PictureBox color_preview = new PictureBox(); scherm.Controls.Add(color_preview);
color_preview.Size = new Size(200, 40);
color_preview.Location = new Point(40 + xdissize, 510);
//mod preview
PictureBox mod_preview = new PictureBox(); scherm.Controls.Add(mod_preview);
mod_preview.Size = new Size(200, 20);
mod_preview.Location = new Point(40 + xdissize, 200);
//scurve preview
PictureBox scurve_preview = new PictureBox(); scherm.Controls.Add(scurve_preview);
scurve_preview.Size = new Size(200, 20);
scurve_preview.Location = new Point(40 + xdissize, 340);
//wave preview
PictureBox wave_preview = new PictureBox(); scherm.Controls.Add(wave_preview);
wave_preview.Size = new Size(200, 20);
wave_preview.Location = new Point(40 + xdissize, 480);

//red green blue textlabels
Label red_text = new Label(); scherm.Controls.Add(red_text);
red_text.Text = "Red"; red_text.Size = new Size(45, 25);
red_text.Location = new Point(110 + xdissize, 22);
Label green_text = new Label(); scherm.Controls.Add(green_text);
green_text.Text = "Green"; green_text.Size = new Size(50, 25);
green_text.Location = new Point(155 + xdissize, 22);
Label blue_text = new Label(); scherm.Controls.Add(blue_text);
blue_text.Text = "Blue"; blue_text.Size = new Size(50, 25);
blue_text.Location = new Point(210 + xdissize, 22);
//Alle TrackBar en Label rijen voor alle 10 catogoriën sliders om de custom kleur samen te stellen
//constant
Label constant_text = new Label(); scherm.Controls.Add(constant_text);
constant_text.Text = "Constant"; constant_text.Size = new Size(60, 25);
constant_text.Location = new Point(30 + xdissize, 40);
TrackBar slider_r_constant = new TrackBar(); scherm.Controls.Add(slider_r_constant);
slider_r_constant.Size = new Size(60, 10); slider_r_constant.SetRange(0, 255);
slider_r_constant.Location = new Point(90 + xdissize, 40);
TrackBar slider_g_constant = new TrackBar(); scherm.Controls.Add(slider_g_constant);
slider_g_constant.Size = new Size(60, 10); slider_g_constant.SetRange(0, 255);
slider_g_constant.Location = new Point(140 + xdissize, 40);
TrackBar slider_b_constant = new TrackBar(); scherm.Controls.Add(slider_b_constant);
slider_b_constant.Size = new Size(60, 10); slider_b_constant.SetRange(0, 255);
slider_b_constant.Location = new Point(190 + xdissize, 40);

//mod
Label mod_text = new Label(); scherm.Controls.Add(mod_text);
mod_text.Text = "Mod"; mod_text.Size = new Size(60, 25);
mod_text.Location = new Point(30 + xdissize, 80);
TrackBar slider_r_mod = new TrackBar(); scherm.Controls.Add(slider_r_mod);
slider_r_mod.Size = new Size(60, 10); slider_r_mod.SetRange(2, 255);
slider_r_mod.Location = new Point(90 + xdissize, 80);
TrackBar slider_g_mod = new TrackBar(); scherm.Controls.Add(slider_g_mod);
slider_g_mod.Size = new Size(60, 10); slider_g_mod.SetRange(2, 255);
slider_g_mod.Location = new Point(140 + xdissize, 80);
TrackBar slider_b_mod = new TrackBar(); scherm.Controls.Add(slider_b_mod);
slider_b_mod.Size = new Size(60, 10); slider_b_mod.SetRange(2, 255);
slider_b_mod.Location = new Point(190 + xdissize, 80);

//mod_amplitude
Label mod_amplitude_text = new Label(); scherm.Controls.Add(mod_amplitude_text);
mod_amplitude_text.Text = "Amplitude"; mod_amplitude_text.Size = new Size(65, 25);
mod_amplitude_text.Location = new Point(30 + xdissize, 120);
TrackBar slider_r_mod_amplitude = new TrackBar(); scherm.Controls.Add(slider_r_mod_amplitude);
slider_r_mod_amplitude.Size = new Size(60, 10); slider_r_mod_amplitude.SetRange(0, 255);
slider_r_mod_amplitude.Location = new Point(90 + xdissize, 120);
TrackBar slider_g_mod_amplitude = new TrackBar(); scherm.Controls.Add(slider_g_mod_amplitude);
slider_g_mod_amplitude.Size = new Size(60, 10); slider_g_mod_amplitude.SetRange(0, 255);
slider_g_mod_amplitude.Location = new Point(140 + xdissize, 120);
TrackBar slider_b_mod_amplitude = new TrackBar(); scherm.Controls.Add(slider_b_mod_amplitude);
slider_b_mod_amplitude.Size = new Size(60, 10); slider_b_mod_amplitude.SetRange(0, 255);
slider_b_mod_amplitude.Location = new Point(190 + xdissize, 120);

//mod_translation
Label mod_translation_text = new Label(); scherm.Controls.Add(mod_translation_text);
mod_translation_text.Text = "Translation"; mod_translation_text.Size = new Size(65, 25);
mod_translation_text.Location = new Point(30 + xdissize, 160);
TrackBar slider_r_mod_translation = new TrackBar(); scherm.Controls.Add(slider_r_mod_translation);
slider_r_mod_translation.Size = new Size(60, 10); slider_r_mod_translation.SetRange(0, 100);
slider_r_mod_translation.Location = new Point(90 + xdissize, 160);
TrackBar slider_g_mod_translation = new TrackBar(); scherm.Controls.Add(slider_g_mod_translation);
slider_g_mod_translation.Size = new Size(60, 10); slider_g_mod_translation.SetRange(0, 100);
slider_g_mod_translation.Location = new Point(140 + xdissize, 160);
TrackBar slider_b_mod_translation = new TrackBar(); scherm.Controls.Add(slider_b_mod_translation);
slider_b_mod_translation.Size = new Size(60, 10); slider_b_mod_translation.SetRange(0, 100);
slider_b_mod_translation.Location = new Point(190 + xdissize, 160);

//scurve_base
Label scurve_base_text = new Label(); scherm.Controls.Add(scurve_base_text);
scurve_base_text.Text = "Curvyness"; scurve_base_text.Size = new Size(65, 25);
scurve_base_text.Location = new Point(30 + xdissize, 220);
TrackBar slider_r_scurve_base = new TrackBar(); scherm.Controls.Add(slider_r_scurve_base);
slider_r_scurve_base.Size = new Size(60, 10); slider_r_scurve_base.SetRange(50, 150);
slider_r_scurve_base.Location = new Point(90 + xdissize, 220);
TrackBar slider_g_scurve_base = new TrackBar(); scherm.Controls.Add(slider_g_scurve_base);
slider_g_scurve_base.Size = new Size(60, 10); slider_g_scurve_base.SetRange(50, 150);
slider_g_scurve_base.Location = new Point(140 + xdissize, 220);
TrackBar slider_b_scurve_base = new TrackBar(); scherm.Controls.Add(slider_b_scurve_base);
slider_b_scurve_base.Size = new Size(60, 10); slider_b_scurve_base.SetRange(50, 150);
slider_b_scurve_base.Location = new Point(190 + xdissize, 220);

//scurve_amplitude
Label scurve_amplitude_text = new Label(); scherm.Controls.Add(scurve_amplitude_text);
scurve_amplitude_text.Text = "Amplitude"; scurve_amplitude_text.Size = new Size(65, 25);
scurve_amplitude_text.Location = new Point(30 + xdissize, 260);
TrackBar slider_r_scurve_amplitude = new TrackBar(); scherm.Controls.Add(slider_r_scurve_amplitude);
slider_r_scurve_amplitude.Size = new Size(60, 10); slider_r_scurve_amplitude.SetRange(0, 255);
slider_r_scurve_amplitude.Location = new Point(90 + xdissize, 260);
TrackBar slider_g_scurve_amplitude = new TrackBar(); scherm.Controls.Add(slider_g_scurve_amplitude);
slider_g_scurve_amplitude.Size = new Size(60, 10); slider_g_scurve_amplitude.SetRange(0, 255);
slider_g_scurve_amplitude.Location = new Point(140 + xdissize, 260);
TrackBar slider_b_scurve_amplitude = new TrackBar(); scherm.Controls.Add(slider_b_scurve_amplitude);
slider_b_scurve_amplitude.Size = new Size(60, 10); slider_b_scurve_amplitude.SetRange(0, 255);
slider_b_scurve_amplitude.Location = new Point(190 + xdissize, 260);

//scurve_translation
Label scurve_translation_text = new Label(); scherm.Controls.Add(scurve_translation_text);
scurve_translation_text.Text = "Translation"; scurve_translation_text.Size = new Size(65, 25);
scurve_translation_text.Location = new Point(30 + xdissize, 300);
TrackBar slider_r_scurve_translation = new TrackBar(); scherm.Controls.Add(slider_r_scurve_translation);
slider_r_scurve_translation.Size = new Size(60, 10); slider_r_scurve_translation.SetRange(0, 100);
slider_r_scurve_translation.Location = new Point(90 + xdissize, 300);
TrackBar slider_g_scurve_translation = new TrackBar(); scherm.Controls.Add(slider_g_scurve_translation);
slider_g_scurve_translation.Size = new Size(60, 10); slider_g_scurve_translation.SetRange(0, 100);
slider_g_scurve_translation.Location = new Point(140 + xdissize, 300);
TrackBar slider_b_scurve_translation = new TrackBar(); scherm.Controls.Add(slider_b_scurve_translation);
slider_b_scurve_translation.Size = new Size(60, 10); slider_b_scurve_translation.SetRange(0, 100);
slider_b_scurve_translation.Location = new Point(190 + xdissize, 300);

//wave_frequency
Label wave_frequency_text = new Label(); scherm.Controls.Add(wave_frequency_text);
wave_frequency_text.Text = "wavelength"; wave_frequency_text.Size = new Size(67, 25);
wave_frequency_text.Location = new Point(30 + xdissize, 360);
TrackBar slider_r_wave_frequency = new TrackBar(); scherm.Controls.Add(slider_r_wave_frequency);
slider_r_wave_frequency.Size = new Size(60, 10); slider_r_wave_frequency.SetRange(10, 200);
slider_r_wave_frequency.Location = new Point(90 + xdissize, 360);
TrackBar slider_g_wave_frequency = new TrackBar(); scherm.Controls.Add(slider_g_wave_frequency);
slider_g_wave_frequency.Size = new Size(60, 10); slider_g_wave_frequency.SetRange(10, 200);
slider_g_wave_frequency.Location = new Point(140 + xdissize, 360);
TrackBar slider_b_wave_frequency = new TrackBar(); scherm.Controls.Add(slider_b_wave_frequency);
slider_b_wave_frequency.Size = new Size(60, 10); slider_b_wave_frequency.SetRange(10, 200);
slider_b_wave_frequency.Location = new Point(190 + xdissize, 360);

//wave_amplitude
Label wave_amplitude_text = new Label(); scherm.Controls.Add(wave_amplitude_text);
wave_amplitude_text.Text = "amplitude"; wave_amplitude_text.Size = new Size(65, 25);
wave_amplitude_text.Location = new Point(30 + xdissize, 400);
TrackBar slider_r_wave_amplitude = new TrackBar(); scherm.Controls.Add(slider_r_wave_amplitude);
slider_r_wave_amplitude.Size = new Size(60, 10); slider_r_wave_amplitude.SetRange(0, 2000);
slider_r_wave_amplitude.Location = new Point(90 + xdissize, 400);
TrackBar slider_g_wave_amplitude = new TrackBar(); scherm.Controls.Add(slider_g_wave_amplitude);
slider_g_wave_amplitude.Size = new Size(60, 10); slider_g_wave_amplitude.SetRange(0, 2000);
slider_g_wave_amplitude.Location = new Point(140 + xdissize, 400);
TrackBar slider_b_wave_amplitude = new TrackBar(); scherm.Controls.Add(slider_b_wave_amplitude);
slider_b_wave_amplitude.Size = new Size(60, 10); slider_b_wave_amplitude.SetRange(0, 2000);
slider_b_wave_amplitude.Location = new Point(190 + xdissize, 400);

//wave_translation
Label wave_translation_text = new Label(); scherm.Controls.Add(wave_translation_text);
wave_translation_text.Text = "translation"; wave_translation_text.Size = new Size(65, 25);
wave_translation_text.Location = new Point(30 + xdissize, 440);
TrackBar slider_r_wave_translation = new TrackBar(); scherm.Controls.Add(slider_r_wave_translation);
slider_r_wave_translation.Size = new Size(60, 10); slider_r_wave_translation.SetRange(0, 100);
slider_r_wave_translation.Location = new Point(90 + xdissize, 440);
TrackBar slider_g_wave_translation = new TrackBar(); scherm.Controls.Add(slider_g_wave_translation);
slider_g_wave_translation.Size = new Size(60, 10); slider_g_wave_translation.SetRange(0, 100);
slider_g_wave_translation.Location = new Point(140 + xdissize, 440);
TrackBar slider_b_wave_translation = new TrackBar(); scherm.Controls.Add(slider_b_wave_translation);
slider_b_wave_translation.Size = new Size(60, 10); slider_b_wave_translation.SetRange(0, 100);
slider_b_wave_translation.Location = new Point(190 + xdissize, 440);

//willekeurige kleur knop
Button random_color = new Button(); scherm.Controls.Add(random_color);
random_color.Text = "RaNdOm CoLoR"; random_color.Size = new Size(200, 25);
random_color.Location = new Point(40 + xdissize, 610);

//ui selection 2 Cool spots
PictureBox spot1 = new PictureBox(); scherm.Controls.Add(spot1);
spot1.Size = new Size(200, 200);
spot1.Location = new Point(40 + xdissize, 40);

PictureBox spot2 = new PictureBox(); scherm.Controls.Add(spot2);
spot2.Size = new Size(200, 200);
spot2.Location = new Point(40 + xdissize, 260);

PictureBox spot3 = new PictureBox(); scherm.Controls.Add(spot3);
spot3.Size = new Size(200, 200);
spot3.Location = new Point(40 + xdissize, 480);

//ui selection 3 User spots (us)
Label userspottext = new Label(); scherm.Controls.Add(userspottext);
userspottext.Text = "new spot name"; userspottext.Location = new Point(40 + xdissize, 40);
TextBox userspotname = new TextBox(); scherm.Controls.Add(userspotname);
userspotname.Location = new Point(140 + xdissize, 40);
Button userspotadd = new Button(); scherm.Controls.Add(userspotadd);
userspotadd.Location = new Point(40 + xdissize, 70); userspotadd.Text = "add userspot";
userspotadd.Size = new Size(90, 20);
Button userspotremove = new Button(); scherm.Controls.Add(userspotremove);
userspotremove.Location = new Point(130 + xdissize, 70); userspotremove.Text = "remove userspot";
userspotremove.Size = new Size(110, 20);
Button gouserspot = new Button(); scherm.Controls.Add(gouserspot);
gouserspot.Location = new Point(40 + xdissize, 500); gouserspot.Text = "go to spot";
gouserspot.Size = new Size(100, 25);

ListBox userspotlist = new ListBox(); scherm.Controls.Add(userspotlist);
userspotlist.Location = new Point(40 + xdissize, 100); userspotlist.Size = new Size(200, 400);
userspotlist.SelectionMode = SelectionMode.One;

//Functies voor het renderen van de manddelbrot/////////////////////////////////////////////////////////////////

//rendert een mandelbrot van specifieke grootte en kleur
Bitmap RenderMandelbrot(int width, int height, double x1, double x2, double y1, double y2)
{
    try
    {
        maxiterations = int.Parse(iterationsbox.Text);
    }
    catch { }
    if (!hassupercomputer && maxiterations > 3000) //toets of de gebruiker daadwerkelijk veel, i.e. >3000 iteraties wilde uitrekeken
    {
        maxiterations = 3000;
        iterationsbox.Text = "3000";
    }
    Bitmap bitmap = new Bitmap(width, height);

    for (int y = 0; y < width; y++)
    {
        for (int x = 0; x < height; x++)
        {
            //setup voor while loop

            int i = 0;

            //
            double startx = Convert.ToDouble(x * (x2 - x1)) / Convert.ToDouble(width) + x1;
            double starty = Convert.ToDouble(y * (y2 - y1)) / Convert.ToDouble(height) + y1;

            //
            double newx = startx;
            double newy = starty;


            //
            bool running = newx * newx + newy * newy! < 4;

            while (running && i < maxiterations)
            {
                i++;
                double tempx = newx * newx - newy * newy + startx;
                newy = 2 * newx * newy + starty;
                newx = tempx;

                if (newx * newx + newy * newy > 4)//alles gekwadrateerd
                {
                    running = false;
                }
            }

            bitmap.SetPixel(x, y, MandColor(i, running));

        }
    }

    return bitmap;
}

void TekenDisplay(object o, PaintEventArgs pea)
{
    if (color == 9)//easter egg
    {
        if (!hassupercomputer && maxiterations > 3000) //toets of de gebruiker daadwerkelijk veel, i.e. >3000 iteraties wilde uitrekeken
        {
            maxiterations = 3000;
            iterationsbox.Text = "3000";
        }
        try
        {
            mandelbrot = new Bitmap("..\\..\\..\\bread.jpg");
        }
        catch { mandelbrot = new Bitmap(750, 750); }


        for (int y = 0; y < xdissize; y++)
        {
            for (int x = 0; x < ydissize; x++)
            {
                //setup voor while loop

                int i = 0;

                //
                double startx = Convert.ToDouble(x * (x2 - x1)) / Convert.ToDouble(xdissize) + x1;
                double starty = Convert.ToDouble(y * (y2 - y1)) / Convert.ToDouble(ydissize) + y1;

                //
                double newx = startx;
                double newy = starty;

                //
                bool running = newx * newx + newy * newy! < 4;

                while (running && i < maxiterations)
                {
                    i++;
                    double tempx = newx * newx - newy * newy + startx;
                    newy = 2 * newx * newy + starty;
                    newx = tempx;

                    if (newx * newx + newy * newy > 4)//alles gekwadrateerd
                    {
                        running = false;
                    }
                }
                if (!running)
                {
                    mandelbrot.SetPixel(x, y, Color.FromArgb(100, 120, 130));
                }
            }
        }

    }
    else
    {
        if (pointclicked) //het kader van de boxzoom
        {
            for (int m = 0; m < 50; m += 2)
            {
                mandelbrot.SetPixel(Math.Max(0, Math.Min(m + 1 + clickx, xdissize - 1)), clicky, Color.White);
                mandelbrot.SetPixel(clickx, Math.Max(0, Math.Min(m + 1 + clicky, ydissize - 1)), Color.White);

                mandelbrot.SetPixel(Math.Max(0, Math.Min(m + clickx, xdissize - 1)), clicky, Color.Black);
                mandelbrot.SetPixel(clickx, Math.Max(0, Math.Min(m + clicky, ydissize - 1)), Color.Black);
            }
        }
        else
        {
            mandelbrot = RenderMandelbrot(mandelbrot.Size.Width, mandelbrot.Size.Height, x1, x2, y1, y2);
        }
    }






    pea.Graphics.DrawImageUnscaled(mandelbrot, new Point(0, 0));

}

//wordt aangeroepen in RenderMandelbrot, voorgeprogrammeerde kleurpalleten
Color MandColor(int iterations, bool done)
{
    if (done)
    {
        return Color.Black;
    }
    if (color == 1)
    {
        double wave = (iterations / 20.0) % 2 - 1;

        return Color.FromArgb(
        Convert.ToInt32(1 / (1 + Math.Pow(1.04, -iterations + 100)) * 255),
        Convert.ToInt32(1 / (1 + Math.Pow(1.03, -iterations + 30)) * 200 + (-wave * Math.Abs(wave) + wave) * 100),
        Convert.ToInt32(Math.Max(0, 255 - 0.1 * iterations * iterations))
        );
    }
    else if (color == 2)
    {

        double wave = (iterations / 20.0) % 2 - 1;
        double wav2 = (iterations / 20.0 + 1) % 2 - 1;


        return Color.FromArgb(
        Convert.ToInt32(0),
        Convert.ToInt32((-wave * Math.Abs(wave) + wave) * 200 + 200),
        Convert.ToInt32((-wav2 * Math.Abs(wav2) + wav2) * 200 + 200));
    }
    else if (color == 3)
    {
        return Color.FromArgb(
            255 * (iterations % 7) / 6
            ,
            255 * (iterations % 33) / 32
            ,
            255 * (iterations % 2) / 1
            );
    }
    else if (color == 4)
    {
        double wave = (iterations / 20.0) % 2 - 1;
        double wav2 = (iterations / 13.0 + 1) % 2 - 1;

        return Color.FromArgb(Convert.ToInt32(
            255 - 255 / (iterations / 30.0 + 1)
            ), Convert.ToInt32(
            (-wave * Math.Abs(wave) + wave) * 200 + 200
            ), Convert.ToInt32(
            (-wav2 * Math.Abs(wav2) + wav2) * 200 + 200
            ));
    }
    else if (color == 5)
    {
        return Color.FromArgb(
            255 * (iterations % 40) / 39
            ,
            255 * (iterations % 33) / 32
            ,
            255 * (iterations % 17) / 16
            );
    }
    else if (color == 6)
    {
        double wave = (iterations / 5.0) % 2 - 1;

        return Color.FromArgb(Math.Max(0, Math.Min(255, (iterations % 10) * 255 / 9 + Convert.ToInt32(
            0
            ))), Math.Max(0, Math.Min(255, (iterations % 10) * 255 / 9 + Convert.ToInt32(
            0
            ))), Math.Max(0, Math.Min(255, (iterations % 10) * 255 / 9 + Convert.ToInt32(
            200 + wave * 200
            ))));
    }
    else if (color == 7)
    {
        return Color.FromArgb(
            255 * (iterations % 2)
            ,
            255 * (iterations % 2)
            ,
            255 * (iterations % 2)
            );
    }
    else if (color == 8)
    {
        double rwave = ((iterations + 102) / 30.0) % 2 - 1;
        double gwave = ((iterations + 180) / 23.0) % 2 - 1;
        double bwave = ((iterations + 117) / 40.0) % 2 - 1;

        return Color.FromArgb(Convert.ToInt32(
            128 + 200 * (-rwave * Math.Abs(rwave) + rwave)
            ), Convert.ToInt32(
            128 + 200 * (-gwave * Math.Abs(gwave) + gwave)
            ), Convert.ToInt32(
            128 + 200 * (-bwave * Math.Abs(bwave) + bwave)
            ));
    }
    //kleur 9 is een afbeelding in een mandelbrot en maakt geen gebruik van de kleur functie
    else //kleur 10 is de custom kleur
    {
        //eerst wordt voor r, g en b een tussenwaarde uitgerekend voor de wave functie,
        double rwave = ((iterations + r_wave_translation) / r_wave_frequency) % 2 - 1;
        double gwave = ((iterations + g_wave_translation) / g_wave_frequency) % 2 - 1;
        double bwave = ((iterations + b_wave_translation) / b_wave_frequency) % 2 - 1;


        //de waarde per kleur is samengesteld uit een constante + een modulo functie + een scurve + een golffunctie
        //de min en max functies worden gebruikt zodat de waardes altijd binnen 0-255 blijven
        //de uitkomsten van de golffunctie en de scurve moeten naar int geconverteerd worden

        return Color.FromArgb(

            //rood
            Math.Min(Math.Max(
            r_constant + ((iterations + r_mod_translation) % r_mod) * r_mod_amplitude / (r_mod - 1) + Convert.ToInt32(
            r_scurve_amplitude / (1 + Math.Pow(r_scurve_base, -iterations + r_scurve_translation)) + //de scurve is 1 bij base = 1 dalend van 1 naar 0 bij base <1 en stijgend van 0 naar 1 bij base > 1
            r_wave_amplitude * (-rwave * Math.Abs(rwave) + rwave) //de golffunctie is een herhaling van x^2-x en daarna x^2+x
            ), 0), 255),
            //groen
            Math.Min(Math.Max(
            g_constant + ((iterations + g_mod_translation) % g_mod) * g_mod_amplitude / (g_mod - 1) + Convert.ToInt32(
            g_scurve_amplitude / (1 + Math.Pow(g_scurve_base, -iterations + g_scurve_translation)) +
            g_wave_amplitude * (-gwave * Math.Abs(gwave) + gwave)
            ), 0), 255),
            //blauw
            Math.Min(Math.Max(
            b_constant + ((iterations + b_mod_translation) % b_mod) * b_mod_amplitude / (b_mod - 1) + Convert.ToInt32(
            b_scurve_amplitude / (1 + Math.Pow(b_scurve_base, -iterations + b_scurve_translation)) +
            b_wave_amplitude * (-bwave * Math.Abs(bwave) + bwave)
            ), 0), 255)
            );
    }
}

//Hoofd UI functies/////////////////////////////////////////////////////////////////////////////////////////

//box zoom checkbox eventhandler
void toggleboxselect(object o, EventArgs ea)
{
    pointclicked = false;
    boxselect = boxselectbox.Checked;
}

//activeert op muisklik op de mandelbrot
void zoom(object o, MouseEventArgs ea)
{


    double newscale = Math.Abs(x2 - x1) * 0.0025 / 2.0;

    double newclickx = ea.X * (x2 - x1) / (xdissize) + x1;
    double newclicky = ea.Y * (y2 - y1) / (ydissize) + y1;



    if (ea.Button == MouseButtons.Left)
    {
        if (boxselect)
        {
            if (pointclicked)
            {
                double newx1 = clickx * (x2 - x1) / (xdissize) + x1;
                double newx2 = ea.X * (x2 - x1) / (xdissize) + x1;
                double newy1 = clicky * (y2 - y1) / (ydissize) + y1;
                double newy2 = ea.Y * (y2 - y1) / (ydissize) + y1;

                x1 = newx1;
                x2 = newx2;
                y1 = newy1;
                y2 = newy2;

                pointclicked = false;
                display.Invalidate();
            }
            else
            {
                pointclicked = true;
                clickx = ea.X;
                clicky = ea.Y;
                display.Invalidate();

            }
        }
        else
        {
            x1 = newclickx - 200 * newscale;
            x2 = newclickx + 200 * newscale;
            y1 = newclicky - 200 * newscale;
            y2 = newclicky + 200 * newscale;
            display.Invalidate();
        }


    }
    else if (ea.Button == MouseButtons.Right)
    {
        x1 = newclickx - 200 * newscale * 4;
        x2 = newclickx + 200 * newscale * 4;
        y1 = newclicky - 200 * newscale * 4;
        y2 = newclicky + 200 * newscale * 4;
        display.Invalidate();
    }

}

//activeert als de toolbelt wordt geklikt
void newcolor(object o, ToolStripItemClickedEventArgs ea)
{
    spot1.Invalidate();
    spot2.Invalidate();
    spot3.Invalidate();

    color = int.Parse(ea.ClickedItem.Text);
    pointclicked = false;
    display.Invalidate();
}

//voor de reset knop
void ButtonClick(object o, EventArgs ea)
{
    x1 = -2;
    x2 = 2;
    y1 = -2;
    y2 = 2;
    pointclicked = false;


    display.Invalidate();

}
//verbergt alle wisselbare ui elementen
void hideUI()
{
    //ui0
    xloc.Visible = false; xtext.Visible = false; yloc.Visible = false; ytext.Visible = false; scale.Visible = false; stext.Visible = false; iterationsbox.Visible = false; itext.Visible = false; gobutton.Visible = false; changeiterations.Visible = false; getlocationbutton.Visible = false;
    save.Visible = false; activate_custom_color.Visible = false; supercomputer.Visible = false; supercomputertext.Visible = false;

    //ui1
    red_text.Visible = false; green_text.Visible = false; blue_text.Visible = false; constant_text.Visible = false; slider_r_constant.Visible = false; slider_g_constant.Visible = false; slider_b_constant.Visible = false; slider_b_constant.Visible = false;
    mod_text.Visible = false; slider_r_mod.Visible = false; slider_g_mod.Visible = false; slider_b_mod.Visible = false; mod_amplitude_text.Visible = false; slider_r_mod_amplitude.Visible = false; slider_g_mod_amplitude.Visible = false; slider_b_mod_amplitude.Visible = false; slider_r_mod_translation.Visible = false; slider_g_mod_translation.Visible = false; slider_b_mod_translation.Visible = false;
    mod_translation_text.Visible = false; slider_r_mod_translation.Visible = false; slider_g_mod_translation.Visible = false; slider_b_mod_translation.Visible = false;
    scurve_base_text.Visible = false; slider_r_scurve_base.Visible = false; slider_g_scurve_base.Visible = false; slider_b_scurve_base.Visible = false; scurve_amplitude_text.Visible = false; slider_r_scurve_amplitude.Visible = false; slider_g_scurve_amplitude.Visible = false; slider_b_scurve_amplitude.Visible = false;
    scurve_translation_text.Visible = false; slider_r_scurve_translation.Visible = false; slider_g_scurve_translation.Visible = false; slider_b_scurve_translation.Visible = false;
    wave_frequency_text.Visible = false; slider_r_wave_frequency.Visible = false; slider_g_wave_frequency.Visible = false; slider_b_wave_frequency.Visible = false; wave_amplitude_text.Visible = false; slider_r_wave_amplitude.Visible = false; slider_g_wave_amplitude.Visible = false; slider_b_wave_amplitude.Visible = false;
    wave_translation_text.Visible = false; slider_r_wave_translation.Visible = false; slider_g_wave_translation.Visible = false; slider_b_wave_translation.Visible = false;
    color_preview.Visible = false;
    random_color.Visible = false;
    mod_preview.Visible = false; scurve_preview.Visible = false; wave_preview.Visible = false;


    //ui2
    spot1.Visible = false;
    spot2.Visible = false;
    spot3.Visible = false;
    //ui3
    userspotadd.Visible = false; userspotlist.Visible = false; userspotname.Visible = false; userspotremove.Visible = false; userspottext.Visible = false; gouserspot.Visible = false;
}

//Toont ui0 elementen
void show0()
{
    xloc.Visible = true; xtext.Visible = true; yloc.Visible = true; ytext.Visible = true; scale.Visible = true; stext.Visible = true; iterationsbox.Visible = true; itext.Visible = true; gobutton.Visible = true; changeiterations.Visible = true; getlocationbutton.Visible = true;
    save.Visible = true; supercomputer.Visible = true; supercomputertext.Visible = true;
}

hideUI();
show0();

//activeert als het dropdown menu verandert, verbegt en toont de gewenste onderdelen
void newui(object o, EventArgs ea)
{
    //verstopt alle objecten in alle ui selecties


    hideUI();



    if (options.SelectedIndex == 0)
    {
        //toont alle objecten in ui selectie 1
        show0();
    }
    else if (options.SelectedIndex == 1) //toont de custom kleuren manager
    {
        red_text.Visible = true; green_text.Visible = true; blue_text.Visible = true; constant_text.Visible = true; slider_r_constant.Visible = true; slider_g_constant.Visible = true; slider_b_constant.Visible = true; slider_b_constant.Visible = true;
        mod_text.Visible = true; slider_r_mod.Visible = true; slider_g_mod.Visible = true; slider_b_mod.Visible = true; mod_amplitude_text.Visible = true; slider_r_mod_amplitude.Visible = true; slider_g_mod_amplitude.Visible = true; slider_b_mod_amplitude.Visible = true; slider_r_mod_translation.Visible = true; slider_g_mod_translation.Visible = true; slider_b_mod_translation.Visible = true;
        mod_translation_text.Visible = true; slider_r_mod_translation.Visible = true; slider_g_mod_translation.Visible = true; slider_b_mod_translation.Visible = true;
        scurve_base_text.Visible = true; slider_r_scurve_base.Visible = true; slider_g_scurve_base.Visible = true; slider_b_scurve_base.Visible = true; scurve_amplitude_text.Visible = true; slider_r_scurve_amplitude.Visible = true; slider_g_scurve_amplitude.Visible = true; slider_b_scurve_amplitude.Visible = true;
        scurve_translation_text.Visible = true; slider_r_scurve_translation.Visible = true; slider_g_scurve_translation.Visible = true; slider_b_scurve_translation.Visible = true;
        wave_frequency_text.Visible = true; slider_r_wave_frequency.Visible = true; slider_g_wave_frequency.Visible = true; slider_b_wave_frequency.Visible = true; wave_amplitude_text.Visible = true; slider_r_wave_amplitude.Visible = true; slider_g_wave_amplitude.Visible = true; slider_b_wave_amplitude.Visible = true;
        wave_translation_text.Visible = true; slider_r_wave_translation.Visible = true; slider_g_wave_translation.Visible = true; slider_b_wave_translation.Visible = true;
        activate_custom_color.Visible = true;
        color_preview.Visible = true;
        random_color.Visible = true;
        mod_preview.Visible = true; scurve_preview.Visible = true; wave_preview.Visible = true;

    }
    else if (options.SelectedIndex == 2) //toont de voorgeprogrammeerde locaties in de mandelbrot
    {
        spot1.Visible = true;
        spot2.Visible = true;
        spot3.Visible = true;
    }
    else //toont de ui om zelf locaties op te slaan/verwijderen
    {
        userspotadd.Visible = true; userspotlist.Visible = true; userspotname.Visible = true; userspotremove.Visible = true; userspottext.Visible = true; gouserspot.Visible = true;
    }

}

//ui0 Functies**************************************************************************
//verandert huidige waardes in de ingevoerde waarden als op de go knop wordt gedrukt
void gopressed(object o, EventArgs ea)
{
    try
    {
        //er is gekozen om de waarden van de hoeken op te slaan i.p.v. het midden en de schaal. Daarom wordt het hier geconverteerd
        x1 = double.Parse(xloc.Text) - 200 * double.Parse(scale.Text);
        x2 = double.Parse(xloc.Text) + 200 * double.Parse(scale.Text);
        y1 = double.Parse(yloc.Text) - 200 * double.Parse(scale.Text);
        y2 = double.Parse(yloc.Text) + 200 * double.Parse(scale.Text);
        maxiterations = int.Parse(iterationsbox.Text);
    }
    catch { }
    if (!hassupercomputer && maxiterations > 3000)
    {
        maxiterations = 3000;
        iterationsbox.Text = "3000";
    }

    pointclicked = false;
    display.Invalidate();
}

//wordt aangeroepen als op de only iterations knop wordt geklikt
void onlyiterations(object o, EventArgs ea)
{
    try
    {
        maxiterations = int.Parse(iterationsbox.Text);
    }
    catch { }
    if (!hassupercomputer && maxiterations > 3000)
    {
        maxiterations = 3000;
        iterationsbox.Text = "3000";
    }
    display.Invalidate();
}

//zet de huidige coördinaten in het text input menu als op de getlocation knop wordt gedrukt
void getlocation(object o, EventArgs ea)
{
    //verandert variabelen van hoekcoördinaten naar middencoördinaten.
    //Het midden en de breedte van het scherm blijven hierbij hetzelfde maar de hoogte verandert als de hoogte en breedte niet gelijk zijn
    xloc.Text = ((x2 + x1) / 2).ToString();
    yloc.Text = ((y2 + y1) / 2).ToString();
    scale.Text = (Math.Abs(x2 - x1) * 0.0025).ToString();
    iterationsbox.Text = maxiterations.ToString();
}

//Slaat de mandelfiguur op als een jpeg naast de executable. Aangeroepen door de screenshot knop
void saveImage(object o, EventArgs ea)
{
    string tstamp = DateTime.Now.ToString("MM.dd.yyyy HH.mm.ss");
    mandelbrot.Save(tstamp + ".jpeg", ImageFormat.Jpeg);

}

//Doet wat het zegt
void togglehassupercomputer(object o, EventArgs ea)
{
    hassupercomputer = supercomputer.Checked;
}

//ui1 Functies custom color*************************************************************
void render_colorpreview(object o, PaintEventArgs pea)
{
    //Maakt een bitmap aan die aan het einde wordt gerenderd.
    Bitmap color_preview_bit = new Bitmap(200, 40);

    //Zet voor elke kolom alle pixels in de bitmap op de kleur die overeenkomt met de costom kleur, afhangkelijk van x i.p.v. iteraties.
    for (int x = 0; x < 200; x++)
    {
        Color previeuwcolor = MandColor(x, false);
        for (int y = 0; y < 40; y++)
        {
            color_preview_bit.SetPixel(x, y, previeuwcolor);
        }
    }
    //als de color preview word gerenderd wordern ook de subpreviews gerenderd
    mod_preview.Invalidate();
    scurve_preview.Invalidate();
    wave_preview.Invalidate();


    pea.Graphics.DrawImageUnscaled(color_preview_bit, new Point(0, 0));
}
void render_modpreview(object o, PaintEventArgs pea)
{
    //Maakt een bitmap aan die aan het einde wordt gerenderd.
    Bitmap mod_preview_bit = new Bitmap(200, 40);

    //Zet voor elke kolom alle pixels in de bitmap op de kleur die overeenkomt met het modulo gedeelte van de costom kleur, afhangkelijk van x i.p.v. iteraties.
    for (int x = 0; x < 200; x++)
    {
        //kleurberekening van de complete custom kleur is te vinden in MandColor achter de laatste else
        Color previeuwcolor = Color.FromArgb(Math.Max(0, Math.Min(255, (x + r_mod_translation) % r_mod * r_mod_amplitude / (r_mod - 1))),
                                             Math.Max(0, Math.Min(255, (x + g_mod_translation) % g_mod * g_mod_amplitude / (g_mod - 1))),
                                             Math.Max(0, Math.Min(255, (x + b_mod_translation) % b_mod * b_mod_amplitude / (b_mod - 1))));
        for (int y = 0; y < 40; y++)
        {
            mod_preview_bit.SetPixel(x, y, previeuwcolor);
        }
    }

    pea.Graphics.DrawImageUnscaled(mod_preview_bit, new Point(0, 0));
}
void render_scurvepreview(object o, PaintEventArgs pea)//tekent de preview van het scurve component van de custom color
{
    //Maakt een bitmap aan die aan het einde wordt gerenderd.
    Bitmap scurve_preview_bit = new Bitmap(200, 40);

    //Zet voor elke kolom alle pixels in de bitmap op de kleur die overeenkomt met het s-curve gedeelte van de costom kleur, afhangkelijk van x i.p.v. iteraties.
    for (int x = 0; x < 200; x++)
    {
        //kleurberekening van de complete custom kleur is te vinden in MandColor achter de laatste else
        Color previeuwcolor = Color.FromArgb(Math.Max(0, Math.Min(255, Convert.ToInt32(r_scurve_amplitude / (1 + Math.Pow(r_scurve_base, -x + r_scurve_translation))))),
                                             Math.Max(0, Math.Min(255, Convert.ToInt32(g_scurve_amplitude / (1 + Math.Pow(g_scurve_base, -x + g_scurve_translation))))),
                                             Math.Max(0, Math.Min(255, Convert.ToInt32(b_scurve_amplitude / (1 + Math.Pow(b_scurve_base, -x + b_scurve_translation))))));
        for (int y = 0; y < 40; y++)
        {
            scurve_preview_bit.SetPixel(x, y, previeuwcolor);
        }
    }

    pea.Graphics.DrawImageUnscaled(scurve_preview_bit, new Point(0, 0));
}
void render_wavepreview(object o, PaintEventArgs pea)
{
    //Maakt een bitmap aan die aan het einde wordt gerenderd.
    Bitmap wave_preview_bit = new Bitmap(200, 40);

    //Zet voor elke kolom alle pixels in de bitmap op de kleur die overeenkomt met het wave gedeelte van de costom kleur, afhangkelijk van x i.p.v. iteraties.
    for (int x = 0; x < 200; x++)
    {
        //kleurberekening van de complete custom kleur is te vinden in MandColor achter de laatste else
        //het wave gedeelte heeft een tussenberekening voor overzichtelijkheid.
        double rwave = ((x + r_wave_translation) / r_wave_frequency) % 2 - 1;
        double gwave = ((x + g_wave_translation) / g_wave_frequency) % 2 - 1;
        double bwave = ((x + b_wave_translation) / b_wave_frequency) % 2 - 1;

        Color previeuwcolor = Color.FromArgb(Math.Max(0, Math.Min(255, Convert.ToInt32(r_wave_amplitude * (-rwave * Math.Abs(rwave) + rwave)))),
                                             Math.Max(0, Math.Min(255, Convert.ToInt32(g_wave_amplitude * (-gwave * Math.Abs(gwave) + gwave)))),
                                             Math.Max(0, Math.Min(255, Convert.ToInt32(b_wave_amplitude * (-bwave * Math.Abs(bwave) + bwave)))));

        for (int y = 0; y < 40; y++)
        {
            wave_preview_bit.SetPixel(x, y, previeuwcolor);
        }
    }

    pea.Graphics.DrawImageUnscaled(wave_preview_bit, new Point(0, 0));
}

void Slider_r_constant(object o, EventArgs ea) { r_constant = slider_r_constant.Value; color_preview.Invalidate(); }
void Slider_g_constant(object o, EventArgs ea) { g_constant = slider_g_constant.Value; color_preview.Invalidate(); }
void Slider_b_constant(object o, EventArgs ea) { b_constant = slider_b_constant.Value; color_preview.Invalidate(); }
void Slider_r_mod_amplitude(object o, EventArgs ea) { r_mod_amplitude = slider_r_mod_amplitude.Value; color_preview.Invalidate(); }
void Slider_g_mod_amplitude(object o, EventArgs ea) { g_mod_amplitude = slider_g_mod_amplitude.Value; color_preview.Invalidate(); }
void Slider_b_mod_amplitude(object o, EventArgs ea) { b_mod_amplitude = slider_b_mod_amplitude.Value; color_preview.Invalidate(); }
void Slider_r_mod(object o, EventArgs ea) { r_mod = slider_r_mod.Value; color_preview.Invalidate(); }
void Slider_g_mod(object o, EventArgs ea) { g_mod = slider_g_mod.Value; color_preview.Invalidate(); }
void Slider_b_mod(object o, EventArgs ea) { b_mod = slider_b_mod.Value; color_preview.Invalidate(); }
void Slider_r_mod_translation(object o, EventArgs ea) { r_mod_translation = slider_r_mod_translation.Value; color_preview.Invalidate(); }
void Slider_g_mod_translation(object o, EventArgs ea) { g_mod_translation = slider_g_mod_translation.Value; color_preview.Invalidate(); }
void Slider_b_mod_translation(object o, EventArgs ea) { b_mod_translation = slider_b_mod_translation.Value; color_preview.Invalidate(); }
void Slider_r_scurve_base(object o, EventArgs ea) { r_scurve_base = slider_r_scurve_base.Value / 100.0; color_preview.Invalidate(); }
void Slider_g_scurve_base(object o, EventArgs ea) { g_scurve_base = slider_g_scurve_base.Value / 100.0; color_preview.Invalidate(); }
void Slider_b_scurve_base(object o, EventArgs ea) { b_scurve_base = slider_b_scurve_base.Value / 100.0; color_preview.Invalidate(); }
void Slider_r_scurve_translation(object o, EventArgs ea) { r_scurve_translation = slider_r_scurve_translation.Value; color_preview.Invalidate(); }
void Slider_g_scurve_translation(object o, EventArgs ea) { g_scurve_translation = slider_g_scurve_translation.Value; color_preview.Invalidate(); }
void Slider_b_scurve_translation(object o, EventArgs ea) { b_scurve_translation = slider_b_scurve_translation.Value; color_preview.Invalidate(); }
void Slider_r_scurve_amplitude(object o, EventArgs ea) { r_scurve_amplitude = slider_r_scurve_amplitude.Value; color_preview.Invalidate(); }
void Slider_g_scurve_amplitude(object o, EventArgs ea) { g_scurve_amplitude = slider_g_scurve_amplitude.Value; color_preview.Invalidate(); }
void Slider_b_scurve_amplitude(object o, EventArgs ea) { b_scurve_amplitude = slider_b_scurve_amplitude.Value; color_preview.Invalidate(); }
void Slider_r_wave_frequency(object o, EventArgs ea) { r_wave_frequency = slider_r_wave_frequency.Value / 10.0; color_preview.Invalidate(); }
void Slider_g_wave_frequency(object o, EventArgs ea) { g_wave_frequency = slider_g_wave_frequency.Value / 10.0; color_preview.Invalidate(); }
void Slider_b_wave_frequency(object o, EventArgs ea) { b_wave_frequency = slider_b_wave_frequency.Value / 10.0; color_preview.Invalidate(); }
void Slider_r_wave_amplitude(object o, EventArgs ea) { r_wave_amplitude = slider_r_wave_amplitude.Value; color_preview.Invalidate(); }
void Slider_g_wave_amplitude(object o, EventArgs ea) { g_wave_amplitude = slider_g_wave_amplitude.Value; color_preview.Invalidate(); }
void Slider_b_wave_amplitude(object o, EventArgs ea) { b_wave_amplitude = slider_b_wave_amplitude.Value; color_preview.Invalidate(); }
void Slider_r_wave_translation(object o, EventArgs ea) { r_wave_translation = slider_r_wave_translation.Value; color_preview.Invalidate(); }
void Slider_g_wave_translation(object o, EventArgs ea) { g_wave_translation = slider_g_wave_translation.Value; color_preview.Invalidate(); }
void Slider_b_wave_translation(object o, EventArgs ea) { b_wave_translation = slider_b_wave_translation.Value; color_preview.Invalidate(); }

void Activate_Custom_Color(object o, EventArgs ea)
{
    color = 10;
    display.Invalidate();
    color_preview.Invalidate();
}

void Update_sliders()
{
    slider_r_constant.Value = r_constant;
    slider_g_constant.Value = g_constant;
    slider_b_constant.Value = b_constant;
    slider_r_mod_amplitude.Value = r_mod_amplitude;
    slider_g_mod_amplitude.Value = g_mod_amplitude;
    slider_b_mod_amplitude.Value = b_mod_amplitude;
    slider_r_mod.Value = r_mod;
    slider_g_mod.Value = g_mod;
    slider_b_mod.Value = b_mod;
    slider_r_mod_translation.Value = r_mod_translation;
    slider_g_mod_translation.Value = g_mod_translation;
    slider_b_mod_translation.Value = b_mod_translation;
    slider_r_scurve_base.Value = (int)(r_scurve_base * 100.0);
    slider_g_scurve_base.Value = (int)(g_scurve_base * 100.0);
    slider_b_scurve_base.Value = (int)(b_scurve_base * 100.0);
    slider_r_scurve_translation.Value = r_scurve_translation;
    slider_g_scurve_translation.Value = g_scurve_translation;
    slider_b_scurve_translation.Value = b_scurve_translation;
    slider_r_scurve_amplitude.Value = r_scurve_amplitude;
    slider_g_scurve_amplitude.Value = g_scurve_amplitude;
    slider_b_scurve_amplitude.Value = b_scurve_amplitude;
    slider_r_wave_frequency.Value = (int)r_wave_frequency * 10;
    slider_g_wave_frequency.Value = (int)g_wave_frequency * 10;
    slider_b_wave_frequency.Value = (int)b_wave_frequency * 10;
    slider_r_wave_amplitude.Value = r_wave_amplitude;
    slider_g_wave_amplitude.Value = g_wave_amplitude;
    slider_b_wave_amplitude.Value = b_wave_amplitude;
    slider_r_wave_translation.Value = r_wave_translation;
    slider_g_wave_translation.Value = g_wave_translation;
    slider_b_wave_translation.Value = b_wave_translation;
}

Update_sliders();

void RandomColor(object o, EventArgs ea)
{

    Random rng = new Random();

    r_constant = rng.Next(slider_r_constant.Minimum, slider_r_constant.Maximum);
    g_constant = rng.Next(slider_g_constant.Minimum, slider_g_constant.Maximum);
    b_constant = rng.Next(slider_b_constant.Minimum, slider_b_constant.Maximum);
    r_mod_amplitude = rng.Next(slider_r_mod_amplitude.Minimum, slider_r_mod_amplitude.Maximum);
    g_mod_amplitude = rng.Next(slider_g_mod_amplitude.Minimum, slider_g_mod_amplitude.Maximum);
    b_mod_amplitude = rng.Next(slider_b_mod_amplitude.Minimum, slider_b_mod_amplitude.Maximum);
    r_mod = rng.Next(slider_r_mod.Minimum, slider_r_mod.Maximum);
    g_mod = rng.Next(slider_g_mod.Minimum, slider_g_mod.Maximum);
    b_mod = rng.Next(slider_b_mod.Minimum, slider_b_mod.Maximum);
    r_mod_translation = rng.Next(slider_r_mod_translation.Minimum, slider_r_mod_translation.Maximum);
    g_mod_translation = rng.Next(slider_g_mod_translation.Minimum, slider_g_mod_translation.Maximum);
    b_mod_translation = rng.Next(slider_b_mod_translation.Minimum, slider_b_mod_translation.Maximum);
    r_scurve_base = rng.Next(50, slider_r_scurve_base.Maximum) / 100.0;
    g_scurve_base = rng.Next(50, slider_g_scurve_base.Maximum) / 100.0;
    b_scurve_base = rng.Next(50, slider_b_scurve_base.Maximum) / 100.0;
    r_scurve_translation = rng.Next(slider_r_scurve_translation.Minimum, slider_r_scurve_translation.Maximum);
    g_scurve_translation = rng.Next(slider_g_scurve_translation.Minimum, slider_g_scurve_translation.Maximum);
    b_scurve_translation = rng.Next(slider_b_scurve_translation.Minimum, slider_b_scurve_translation.Maximum);
    r_scurve_amplitude = rng.Next(slider_r_scurve_amplitude.Minimum, slider_r_scurve_amplitude.Maximum);
    g_scurve_amplitude = rng.Next(slider_g_scurve_amplitude.Minimum, slider_g_scurve_amplitude.Maximum);
    b_scurve_amplitude = rng.Next(slider_b_scurve_amplitude.Minimum, slider_b_scurve_amplitude.Maximum);
    r_wave_frequency = rng.Next(slider_r_wave_frequency.Minimum, slider_r_wave_frequency.Maximum) / 10.0;
    g_wave_frequency = rng.Next(slider_g_wave_frequency.Minimum, slider_g_wave_frequency.Maximum) / 10.0;
    b_wave_frequency = rng.Next(slider_b_wave_frequency.Minimum, slider_b_wave_frequency.Maximum) / 10.0;
    r_wave_amplitude = rng.Next(slider_r_wave_amplitude.Minimum, slider_r_wave_amplitude.Maximum);
    g_wave_amplitude = rng.Next(slider_g_wave_amplitude.Minimum, slider_g_wave_amplitude.Maximum);
    b_wave_amplitude = rng.Next(slider_b_wave_amplitude.Minimum, slider_b_wave_amplitude.Maximum);
    r_wave_translation = rng.Next(slider_r_wave_translation.Minimum, slider_r_wave_translation.Maximum);
    g_wave_translation = rng.Next(slider_g_wave_translation.Minimum, slider_g_wave_translation.Maximum);
    b_wave_translation = rng.Next(slider_b_wave_translation.Minimum, slider_b_wave_translation.Maximum);



    Update_sliders();
    color_preview.Invalidate();
}

//ui2 cool spots Functies***************************************************************************************

void renderSpot1(object o, PaintEventArgs pea)
{
    pea.Graphics.DrawImageUnscaled(RenderMandelbrot(200, 200, spot1x - 200 * spot1scale, spot1x + 200 * spot1scale, spot1y - 200 * spot1scale, spot1y + 200 * spot1scale), new Point(0, 0));
}
void renderSpot2(object o, PaintEventArgs pea)
{
    pea.Graphics.DrawImageUnscaled(RenderMandelbrot(200, 200, spot2x - 200 * spot2scale, spot2x + 200 * spot2scale, spot2y - 200 * spot2scale, spot2y + 200 * spot2scale), new Point(0, 0));
}
void renderSpot3(object o, PaintEventArgs pea)
{
    pea.Graphics.DrawImageUnscaled(RenderMandelbrot(200, 200, spot3x - 200 * spot3scale, spot3x + 200 * spot3scale, spot3y - 200 * spot3scale, spot3y + 200 * spot3scale), new Point(0, 0));
}

void SetSpot1(object o, MouseEventArgs ea)
{
    x1 = spot1x - 200 * spot1scale;
    x2 = spot1x + 200 * spot1scale;
    y1 = spot1y - 200 * spot1scale;
    y2 = spot1y + 200 * spot1scale;
    display.Invalidate();
}
void SetSpot2(object o, MouseEventArgs ea)
{
    x1 = spot2x - 200 * spot2scale;
    x2 = spot2x + 200 * spot2scale;
    y1 = spot2y - 200 * spot2scale;
    y2 = spot2y + 200 * spot2scale;
    display.Invalidate();
}
void SetSpot3(object o, MouseEventArgs ea)
{
    x1 = spot3x - 200 * spot3scale;
    x2 = spot3x + 200 * spot3scale;
    y1 = spot3y - 200 * spot3scale;
    y2 = spot3y + 200 * spot3scale;
    display.Invalidate();
}

//ui3 user spots Functies***************************************************************************************

void AddUserSpot(object o, EventArgs ea)
{
    if (userspotname.Text != "")
    {
        userspots.Add(new UserSpot(userspotname.Text, (x2 + x1) / 2, (y2 + y1) / 2, (Math.Abs(x2 - x1) * 0.0025)));
        userspotlist.Items.Add(userspotname.Text);
    }
    userspotname.Text = "";
}
void RemoveUserSpot(object o, EventArgs ea)
{
    if (userspotlist.SelectedIndex > -1)
    {
        userspots.Remove(userspots[userspotlist.SelectedIndex]);
        userspotlist.Items.Remove(userspotlist.SelectedItem);
    }
}

//wordt geactiveert als de ga naar spot knop wordt geklikt. en zet de huidige locatie naar de gekozen user spot
void LoadUserSpot(object o, EventArgs ea)
{
    if (userspotlist.SelectedIndex > -1)
    {
        x1 = userspots[userspotlist.SelectedIndex].x - 200 * userspots[userspotlist.SelectedIndex].scale;
        x2 = userspots[userspotlist.SelectedIndex].x + 200 * userspots[userspotlist.SelectedIndex].scale;
        y1 = userspots[userspotlist.SelectedIndex].y - 200 * userspots[userspotlist.SelectedIndex].scale;
        y2 = userspots[userspotlist.SelectedIndex].y + 200 * userspots[userspotlist.SelectedIndex].scale;
        display.Invalidate();
    }
}

//EVENT HANDELERS////////////////////////////////////////////////////////////////////////////////////////////////
resetbutton.Click += ButtonClick;
display.Paint += TekenDisplay;
colorselector.ItemClicked += newcolor;
options.SelectedIndexChanged += newui;
display.MouseClick += zoom;
boxselectbox.CheckedChanged += toggleboxselect;

//ui0 text input 
gobutton.Click += gopressed;
changeiterations.Click += onlyiterations;
save.Click += saveImage;
getlocationbutton.Click += getlocation;
supercomputer.CheckStateChanged += togglehassupercomputer;

//ui1 custom color events voor het custom color deel van de ui
//slider events veranderen de custom kleur en renderen de previews
slider_r_constant.ValueChanged += Slider_r_constant;
slider_g_constant.ValueChanged += Slider_g_constant;
slider_b_constant.ValueChanged += Slider_b_constant;
slider_r_mod_amplitude.ValueChanged += Slider_r_mod_amplitude;
slider_g_mod_amplitude.ValueChanged += Slider_g_mod_amplitude;
slider_b_mod_amplitude.ValueChanged += Slider_b_mod_amplitude;
slider_r_mod.ValueChanged += Slider_r_mod;
slider_g_mod.ValueChanged += Slider_g_mod;
slider_b_mod.ValueChanged += Slider_b_mod;
slider_r_mod_translation.ValueChanged += Slider_r_mod_translation;
slider_g_mod_translation.ValueChanged += Slider_g_mod_translation;
slider_b_mod_translation.ValueChanged += Slider_b_mod_translation;
slider_r_scurve_base.ValueChanged += Slider_r_scurve_base;
slider_g_scurve_base.ValueChanged += Slider_g_scurve_base;
slider_b_scurve_base.ValueChanged += Slider_b_scurve_base;
slider_r_scurve_translation.ValueChanged += Slider_r_scurve_translation;
slider_g_scurve_translation.ValueChanged += Slider_g_scurve_translation;
slider_b_scurve_translation.ValueChanged += Slider_b_scurve_translation;
slider_r_scurve_amplitude.ValueChanged += Slider_r_scurve_amplitude;
slider_g_scurve_amplitude.ValueChanged += Slider_g_scurve_amplitude;
slider_b_scurve_amplitude.ValueChanged += Slider_b_scurve_amplitude;
slider_r_wave_frequency.ValueChanged += Slider_r_wave_frequency;
slider_g_wave_frequency.ValueChanged += Slider_g_wave_frequency;
slider_b_wave_frequency.ValueChanged += Slider_b_wave_frequency;
slider_r_wave_amplitude.ValueChanged += Slider_r_wave_amplitude;
slider_g_wave_amplitude.ValueChanged += Slider_g_wave_amplitude;
slider_b_wave_amplitude.ValueChanged += Slider_b_wave_amplitude;
slider_r_wave_translation.ValueChanged += Slider_r_wave_translation;
slider_g_wave_translation.ValueChanged += Slider_g_wave_translation;
slider_b_wave_translation.ValueChanged += Slider_b_wave_translation;


random_color.Click += RandomColor;
activate_custom_color.Click += Activate_Custom_Color;
//previeuws worden live meegerenderd slider edeting
color_preview.Paint += render_colorpreview;
mod_preview.Paint += render_modpreview;
scurve_preview.Paint += render_scurvepreview;
wave_preview.Paint += render_wavepreview;



//ui2 cool spots events
spot1.Paint += renderSpot1;
spot2.Paint += renderSpot2;
spot3.Paint += renderSpot3;

//als op de afbeelding wordt geklikt verandert de huidige positie naar de geklikte cool spot
spot1.MouseClick += SetSpot1;
spot2.MouseClick += SetSpot2;
spot3.MouseClick += SetSpot3;

//ui3 user spot events
userspotadd.Click += AddUserSpot;
userspotremove.Click += RemoveUserSpot;
gouserspot.Click += LoadUserSpot;


Application.Run(scherm);

//voor opslag van userspots in een list
class UserSpot
{
    public string name;
    public double x;
    public double y;
    public double scale;
    public UserSpot(string spotname, double spotx, double spoty, double spotscale)
    {
        name = spotname;
        x = spotx;
        y = spoty;
        scale = spotscale;
    }
}
