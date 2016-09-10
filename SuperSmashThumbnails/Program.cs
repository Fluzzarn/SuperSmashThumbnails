using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Configuration;
namespace SuperSmashBrosThumbnails
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int p1x=   int.Parse(ConfigurationManager.AppSettings["playerOneNamePositionX"]);
            int p1y=   int.Parse(ConfigurationManager.AppSettings["playerOneNamePositionY"]);

            int p2x = int.Parse(ConfigurationManager.AppSettings["playerTwoNamePositionX"]);
            int p2y = int.Parse(ConfigurationManager.AppSettings["playerTwoNamePositionY"]);


            Point playerOneNamePos = new Point(p1x, p1y);
            Point playerTwoNamePos = new Point(p2x, p2y);

            int p1ix = int.Parse(ConfigurationManager.AppSettings["playerOneImagePositionX"]);
            int p1iy = int.Parse(ConfigurationManager.AppSettings["playerOneImagePositionY"]);
                                                                            
            int p2ix = int.Parse(ConfigurationManager.AppSettings["playerTwoImagePositionX"]);
            int p2iy = int.Parse(ConfigurationManager.AppSettings["playerTwoImagePositionY"]);


            String fontName = ConfigurationManager.AppSettings["FontName"];
            int fontSize = int.Parse(ConfigurationManager.AppSettings["FontSize"]);
            Font f = new Font(fontName, fontSize);

            String[] rgbColors = ConfigurationManager.AppSettings["FontColor"].Split(',');

            Brush b = new SolidBrush(Color.FromArgb(255, byte.Parse(rgbColors[0]), byte.Parse(rgbColors[1]), byte.Parse(rgbColors[2])));

            Point playerOneImagePos = new Point(p1ix, p1iy);
            Point playerTwoImagePos = new Point(p2ix, p2iy);
            List<Matchup> matchups = new List<Matchup>();

            using (StreamReader reader = new StreamReader(File.Open(args[0], FileMode.Open)))
            {

                Console.WriteLine("Reading matchup file");
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    var brokenUpLine = line.Split(',');

                    Player left = new Player();
                    left.Name = brokenUpLine[0];
                    left.Character = brokenUpLine[1];
                    left.Facing = Direction.Left;

                    Player right = new Player();
                    right.Name = brokenUpLine[2];
                    right.Character = brokenUpLine[3];
                    right.Facing = Direction.Right;


                    Matchup mu = new Matchup();
                    mu.PlayerOne = left;
                    mu.PlayerTwo = right;

                    Console.WriteLine("Processed " + left.Name + " vs. " + right.Name);

                    matchups.Add(mu);
                }
            }

            if(!Directory.Exists("images"))
            {
                Directory.CreateDirectory("images");
            }


            foreach (Matchup match in matchups)
            {
                Bitmap image = new Bitmap(1280, 720);

                Graphics g = Graphics.FromImage(image);
                g.Clear(Color.White);
                if (File.Exists("images/background.png"))
                {
                    g.DrawImage(Image.FromFile("images/background.png"), new Point(0, 0));
                }
                else
                {
                    Console.WriteLine("Could not find images/background.png");
                }


                g.DrawString(match.PlayerOne.Name, f, b, playerOneNamePos);
                g.DrawString(match.PlayerTwo.Name, f, b, playerTwoNamePos);

                string playerOneImage = "images/" + match.PlayerOne.Character + ".png";
                

                if (File.Exists(playerOneImage))
                {
                    g.DrawImage(Image.FromFile(playerOneImage), playerOneImagePos);
                }
                else
                {
                    Console.WriteLine("Couldn't find " + playerOneImage);
                }

                string playerTwoImage = "images/" + match.PlayerTwo.Character + ".png";
                
                if (File.Exists(playerTwoImage))
                {
                    Image player2Image = Image.FromFile(playerTwoImage);
                    player2Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    g.DrawImage(player2Image, playerTwoImagePos);
                }
                else
                {
                    Console.WriteLine("Couldn't find " + playerTwoImage);
                }


                if (!Directory.Exists("output"))
                    Directory.CreateDirectory("output");

                Bitmap final = new Bitmap(image);
                final.Save("output/" + MakeValidFileName(match.PlayerOne.Name) + "_vs_" + MakeValidFileName(match.PlayerTwo.Name) + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);

                //final.Dispose();
                //image.Dispose();
                //g.Dispose();
            }

        }

        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        }
    }
}
